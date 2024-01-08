using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IrrigationManager.Models {
    [Index("Username", IsUnique = true)]
    public class User {

        public int Id { get; set; }
        [StringLength(30)]
        public string Firstname { get; set; } = string.Empty;
        [StringLength(30)]
        public string Lastname { get; set; } = string.Empty;
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;
        [StringLength(30)]
        public string Username { get; set; } = string.Empty;
        [StringLength(200)]
        public string ImagePath { get; set; } = "";

        [JsonIgnore]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    }
}
