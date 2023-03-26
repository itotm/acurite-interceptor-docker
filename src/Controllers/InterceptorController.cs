using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AcuriteInterceptorApi;

[EnableCors]
[Route("weatherstation/updateweatherstation")]
public class InterceptorController : ControllerBase
{
	private static readonly Dictionary<string, DateTime> _counters = new();
	private readonly WeatherDbContext _context;

	public InterceptorController(WeatherDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public IActionResult Get([FromQuery] SensorData sensorData)
	{
		if (sensorData is null)
		{
			return BadRequest();
		}

		var date = DateTime.Now.AddHours(Int32.Parse(Environment.GetEnvironmentVariable("AddHoursToUtc")!));

		sensorData.InsertDate = date;
		var pressureData = new PressureData { BaromIn = sensorData.BaromIn, InsertDate = date };

		UpdateConsole(sensorData, pressureData);

		SaveToDatabase(sensorData.Sensor, db => db.SensorDatas.Add(sensorData));
		SaveToDatabase("pressure", db => db.PressureDatas.Add(pressureData));

		return Ok("{{ \"checkversion\":\"224\" }}");
	}

	private static void UpdateConsole(SensorData data, PressureData pressure)
	{
		if (Boolean.Parse(Environment.GetEnvironmentVariable("ConsoleOutputEnabled")!))
		{
			Console.WriteLine(String.Format("{0:dd/MM/yyyy HH:mm:ss} - Sensor: {1}, H: {2} T: {3:00.0}, P: {4:0.}, Batt: {5}, Signal: {6}",
				data.InsertDate, data.Sensor, data.Humidity, data.Temperature, pressure.Pressure, data.Battery, data.Rssi));
		}
	}

	private void SaveToDatabase(string key, Action<WeatherDbContext> action)
	{
		if (Boolean.Parse(Environment.GetEnvironmentVariable("DatabaseEnabled")!))
		{
			var now = DateTime.Now;
			if (!_counters.ContainsKey(key)
				|| (now - _counters[key]).TotalSeconds > Int32.Parse(Environment.GetEnvironmentVariable("FilterOutSeconds")!))
			{
				_counters[key] = now;
				action(_context);
				_context.SaveChanges();
			}
		}
	}
}
