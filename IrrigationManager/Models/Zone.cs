using System.ComponentModel.DataAnnotations;

namespace IrrigationManager.Models {
    public class Zone {
        public int Id { get; set; }
        [StringLength(80)]
        public string Name { get; set; } = "New Zone";
        public int RuntimeHours { get; set; } = 0;
        public int RuntimeMinutes { get; set; } = 0;
        public int RuntimePerWeek { get; set; } = 0;

        public List<Plant>? Plants { get; set; }
    }
}
