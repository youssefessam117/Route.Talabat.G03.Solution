
namespace Route.Talabat.APIs
{
	public class Program
	{
		public static void Main(string[] args)
		{
			/// bilder is the web application builder that will create the app and the privios name
			/// was builder and the new name will be webApplicationBuilder
			/// and here we are creating the app by call  WebApplication.CreateBuilder(args);
			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configure Service method from dot net 5 
			// Add services to the container.

			webApplicationBuilder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			webApplicationBuilder.Services.AddEndpointsApiExplorer();
			webApplicationBuilder.Services.AddSwaggerGen(); 
			#endregion

			var app = webApplicationBuilder.Build();

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
