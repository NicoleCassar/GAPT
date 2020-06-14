using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace FYPAllocationTest.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> // This is the database context for the FYP Allocation System, being the primary means of communication with the database
    { // Important to note is that this class will also allow for the automatic generation of tables relating to ApplicationUser
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated(); // This construtor will checks if a database does not exist yet, if it does not, it will be created using the code first mapping provided in the maigration folder 
        }
        
        public DbSet<Student> student { get; set; } // Connection to the 'student' table using object Student for ORM (Object-relational-mapping) purposes
        public DbSet<Supervisor> supervisor { get; set; } // Linking to 'supervisor' table using the Supervisor object
        public DbSet<Area> supervisor_area { get; set; } // Setting up a connection to the 'supervisor_area' table by means of the Area object
        public DbSet<Preference> student_preference { get; set; } // Communicating with the 'student_preference' table by using the Preference object for mapping purposes
        public DbSet<Allocation> allocation { get; set; } // Gaining access to the 'allocation' table by means of mapping to the Allocation object
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { // This method allows for the specification of a composite key within the Preference table, with two Primary Keys for 'preference_id' and 'student_id'
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Preference>()
                .HasKey(p => new { p.preference_id, p.student_id });
        }
        
    }
}
