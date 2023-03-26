using Microsoft.EntityFrameworkCore;

namespace AcuriteInterceptorApi;

public class WeatherDbContext : DbContext
{
	public DbSet<SensorData> SensorDatas { get; set; }
	public DbSet<PressureData> PressureDatas { get; set; }

	public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
	: base(options)
	{
	}
}
