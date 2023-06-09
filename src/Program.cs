using Microsoft.EntityFrameworkCore;

namespace AcuriteInterceptorApi
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			if (Boolean.Parse(Environment.GetEnvironmentVariable("SwaggerEnabled")!))
			{
				builder.Services.AddSwaggerGen();
			}
			var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
			builder.Services.AddDbContext<WeatherDbContext>(options =>
				options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
			var app = builder.Build();

			if (Boolean.Parse(Environment.GetEnvironmentVariable("SwaggerEnabled")!))
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseAuthorization();

			app.UseCors();

			app.MapControllers();

			app.Run();
		}
	}
}