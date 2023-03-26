using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcuriteInterceptorApi;

[Table("PressureData")]
public class PressureData
{
	private const decimal MIN2Millibar = 33.8637526m;

	[Key] public int PressureDataID { get; set; }
	public int Pressure { get; set; }
	[NotMapped]
	public decimal BaromIn
	{
		get
		{
			return default;
		}
		set
		{
			this.Pressure = (int)Math.Round(value * MIN2Millibar, MidpointRounding.ToEven);
		}
	}
	public DateTime InsertDate { get; set; }
}
