using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FYPAllocationTest.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<Student> student { get; set; }
        public DbSet<Supervisor> supervisor { get; set; }
        public DbSet<Area> supervisor_area { get; set; }
        public DbSet<Preference> student_preference { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Preference>()
                .HasKey(p => new { p.preference_id, p.student_id });
        }
        public DbSet<Allocation> allocation { get; set; }
    }
}
