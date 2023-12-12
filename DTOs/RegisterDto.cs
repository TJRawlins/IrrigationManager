using System.ComponentModel.DataAnnotations;

namespace IrrigationManager.DTOs {
    public class RegisterDto {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
