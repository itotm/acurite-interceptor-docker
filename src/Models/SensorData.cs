using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcuriteInterceptorApi;

[Table("SensorData")]
public class SensorData
{
	[Key] public int SensorDataID { get; set; }

	[NotMapped] public string DateUtc { get; set; } = null!;
	[NotMapped] public string Action { get; set; } = null!;
	[NotMapped] public string Realtime { get; set; } = null!;
	[NotMapped] public string Id { get; set; } = null!;
	[NotMapped] public string Mt { get; set; } = null!;
	public string Sensor { get; set; } = null!;
	public int Humidity { get; set; }
	[NotMapped]
	public decimal TempF
	{
		get => default;
		set => this.Temperature = Math.Round((value - 32) * 5 / 9, 1, MidpointRounding.ToEven);
	}
	[NotMapped] public decimal BaromIn { get; set; }
	public string Battery { get; set; } = null!;
	public int Rssi { get; set; }

	[Column(TypeName = "decimal(18,2)")]
	public decimal Temperature { get; set; }
	public DateTime InsertDate { get; set; }
}
