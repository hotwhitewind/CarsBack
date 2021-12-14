using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCars.Model;

namespace TestCars.DB
{
    public class CarsDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }

        public CarsDbContext(DbContextOptions<CarsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
