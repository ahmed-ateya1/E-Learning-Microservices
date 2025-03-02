using EnrollmentManagementMicroservice.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EnrollmentManagementMicroservice.DataAccessLayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Enrollment> Enrollments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>()
                .HasKey(e => e.EnrollmentID);
            modelBuilder.Entity<Enrollment>()
                .Property(e => e.UserID)
                .IsRequired();
            modelBuilder.Entity<Enrollment>()
                .Property(e => e.CourseID)
                .IsRequired();
            modelBuilder.Entity<Enrollment>()
                .Property(e => e.EnrollmentDate)
                .IsRequired();
            modelBuilder.Entity<Enrollment>()
                .Property(e => e.Progress)
                .HasDefaultValue(50);
            modelBuilder.Entity<Enrollment>()
                .HasIndex(e => new { e.UserID, e.CourseID })
                .IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
