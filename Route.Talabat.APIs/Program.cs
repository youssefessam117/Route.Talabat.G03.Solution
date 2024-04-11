
using Microsoft.EntityFrameworkCore;
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
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);

				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "an error has been occured during apply migration");
			}

			#region configure() method to configure kestrel middlewares like dot net 5 
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();


			app.MapControllers();
			#endregion

		

			app.Run();
		}
	}
}
