using System.ComponentModel.DataAnnotations;

namespace IrrigationManager.Models {
    public class Plant {
        public int Id { get; set; }
        [StringLength(10)]
        public string Type { get; set; } = "Tree";
        [StringLength(50)]
        public string Name { get; set;} = string.Empty;
        public float GalsPerWk { get; set; }
        public int Quantity { get; set; }
        public int EmittersPerPlant { get; set; } = 1;
        public float EmitterGPH { get; set; }

        public int ZoneId { get; set; }
        public virtual Zone? Zone {  get; set; }
    }
}
