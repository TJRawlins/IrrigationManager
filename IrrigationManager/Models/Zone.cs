﻿using System.ComponentModel.DataAnnotations;

namespace IrrigationManager.Models {
    public class Zone {
        public int Id { get; set; }
        [StringLength(80)]
        public string Name { get; set; } = "New Zone";
        [StringLength(80)]
        public string Season { get; set; } = "Summer";
        [StringLength(200)]
        public string ImagePath { get; set; } = "";
        public int RuntimeHours { get; set; } = 0;
        public int RuntimeMinutes { get; set; } = 0;
        public int RuntimePerWeek { get; set; } = 0;
        public int RuntimePerMonth { get; set; } = 0;
        public int StarHours { get; set; } = 12;
        public int StarMinutes { get; set; } = 00;
        public int EndHours { get; set; } = 12;
        public int EndMinutes { get; set; } = 00;
        public string TimeStamp { get; set; } = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

        public virtual List<Plant>? Plants { get; set; }
    }
}
