using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IrrigationManager.Models;

namespace IrrigationManager.Data
{
    public class IMSContext : DbContext {

        public DbSet<Plant> Plants { get; set; } = default!;
        public DbSet<Zone> Zones { get; set; } = default!;

        public DbSet<User> Users { get; set; } = default!;

        public IMSContext(DbContextOptions<IMSContext> options) : base(options) { }
        public IMSContext() {}

    }
}
