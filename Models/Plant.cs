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
        public int Quantity { get; set; }
        public int EmittersPerPlant { get; set; } = 1;
        [Column(TypeName = "decimal(5,2)")]
        public decimal EmitterGPH { get; set; }
        public string TimeStamp { get; set; } = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        public int ZoneId { get; set; }

        [JsonIgnore]
        public virtual Zone? Zones {  get; set; }
    }
}
