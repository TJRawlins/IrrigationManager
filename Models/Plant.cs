using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IrrigationManager.Models {
    public class Plant {
        public int Id { get; set; }
        [StringLength(10)]
        public string Type { get; set; } = "Tree";
        [StringLength(50)]
        public string Name { get; set;} = string.Empty;
        [Column(TypeName = "decimal(5,2)")]
        public decimal GalsPerWk { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal GalsPerWkCalc { get; set; }
        public int Quantity { get; set; }
        public int EmittersPerPlant { get; set; } = 1;
        [Column(TypeName = "decimal(5,2)")]
        public decimal EmitterGPH { get; set; }
        public string TimeStamp { get; set; } = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        [StringLength(500)]
        public string ImagePath { get; set; } = "https://img.freepik.com/free-vector/light-gray-seamless-leaf-patterned-background-vector_53876-166104.jpg";
        public string Notes { get; set; } = string.Empty;
        public int Age { get; set; } = 0;
        public int HardinessZone { get; set; } = 1;
        [StringLength(11)]
        public string HarvestMonth { get; set; } = string.Empty;
        [StringLength(11)]
        public string Exposure { get; set; } = "Full Sun";
        public int ZoneId { get; set; }

        [JsonIgnore]
        public virtual Zone? Zones {  get; set; }
    }
}
