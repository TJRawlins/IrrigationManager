using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IrrigationManager.Models {
    public class Zone {
        public int Id { get; set; }
        [StringLength(80)]
        public string Name { get; set; } = "New Zone";
        [StringLength(80)]
        public string Season { get; set; } = "Summer";
        [StringLength(500)]
        public string ImagePath { get; set; } = "https://www.greenturf.com/wp-content/uploads/2016/05/sprinkler-system.jpg";
        public int RuntimeHours { get; set; } = 0;
        public int RuntimeMinutes { get; set; } = 0;
        public int RuntimePerWeek { get; set; } = 0;
        public int RuntimePerMonth { get; set; } = 0;
        public int StartHours { get; set; } = 12;
        public int StartMinutes { get; set; } = 00;
        public int EndHours { get; set; } = 12;
        public int EndMinutes { get; set; } = 00;
        public string TimeStamp { get; set; } = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        [Column(TypeName = "decimal(11,2)")]
        public decimal TotalGalPerWeek { get; set; } = 0;
        [Column(TypeName = "decimal(11,2)")]
        public decimal TotalGalPerMonth { get; set; } = 0;
        [Column(TypeName = "decimal(11,2)")]
        public decimal TotalGalPerYear { get; set; } = 0;
        public int TotalPlants { get; set; } = 0;
        public int SeasonId { get; set; }

        public virtual List<Plant>? Plants { get; set; }

        [JsonIgnore]
        public virtual Season? Seasons { get; set; }
    }
}
