using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace IrrigationManager.Models
{
    public class Season
    {
        public int Id { get; set; }
        [StringLength(80)]
        public string Name { get; set; } = "";
        public string TimeStamp { get; set; } = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        [Column(TypeName = "decimal(11,2)")]
        public decimal TotalGalPerWeek { get; set; } = 0;
        [Column(TypeName = "decimal(11,2)")]
        public decimal TotalGalPerMonth { get; set; } = 0;
        [Column(TypeName = "decimal(11,2)")]
        public decimal TotalGalPerYear { get; set; } = 0;
        public int TotalZones { get; set; } = 0;

        public virtual List<Zone>? Zones { get; set; }
    }
}
