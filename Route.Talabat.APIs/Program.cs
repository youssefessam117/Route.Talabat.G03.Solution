
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Route.Talabat.APIs.Errors;
using Route.Talabat.APIs.Extensions;
using Route.Talabat.APIs.Helpers;
using Route.Talabat.APIs.Middlewares;
using StackExchange.Redis;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastructure;
using Talabat.Infrastructure.Data;

namespace Route.Talabat.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			

			/// builder is the web application builder that will create the app and the privios name
			/// was builder and the new name will be webApplicationBuilder
			/// and here we are creating the app by call  WebApplication.CreateBuilder(args);
			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configure Service method from dot net 5 
			// Add services to the container.

			webApplicationBuilder.Services.AddControllers();

			webApplicationBuilder.Services.AddSwaggerServices();



			webApplicationBuilder.Services.AddDbContext<StoreContext>(option =>
			{
				option.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});

			webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
			{
				var connection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);
			});

			webApplicationBuilder.Services.AddApplicationServices();

			#endregion

			var app = webApplicationBuilder.Build();

			using var scop = app.Services.CreateScope();

			var services = scop.ServiceProvider;

			var _dbContext = services.GetRequiredService<StoreContext>();
			// ask clr for creating object from dbcontext explicitly 

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();
			try
			{
				await _dbContext.Database.MigrateAsync();
				await StoreContextSeed.SeedAsync(_dbContext);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);

				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex.StackTrace, "an error has been occured during apply migration");
			}       

			#region configure() method to configure kestrel middlewares like dot net 5 
	        
			app.UseMiddleware<ExceptionMiddleware>();
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerMiddlewares();
			}

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.MapControllers();

			#endregion

		

			app.Run();
		}
	}
}
