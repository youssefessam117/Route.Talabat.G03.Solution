
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Route.Talabat.APIs.Errors;
using Route.Talabat.APIs.Helpers;
using Route.Talabat.APIs.Middlewares;
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
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			webApplicationBuilder.Services.AddEndpointsApiExplorer();
			webApplicationBuilder.Services.AddSwaggerGen(); 

			webApplicationBuilder.Services.AddDbContext<StoreContext>(option =>
			{
				option.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});

			//webApplicationBuilder.Services.AddScoped<IGenaricRepository<Product>, GenericRepository<Product>>();
			//old way make u repeate the code 

			// genaric way if need of type <> gave them of type<>
			webApplicationBuilder.Services.AddScoped(typeof(IGenaricRepository<>),typeof(GenericRepository<>));

			//webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
			webApplicationBuilder.Services.AddAutoMapper(typeof(MappingProfiles));

			webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
					                                     .SelectMany(p => p.Value.Errors)
														 .Select(e => e.ErrorMessage)
					                                     .ToArray();
					var response = new ApiValidationErrorResponse()
					{
						Errors = errors
					};

					return new BadRequestObjectResult(response);
				};
			});
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
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();


			app.MapControllers();

			app.UseStaticFiles();
			#endregion

		

			app.Run();
		}
	}
}
