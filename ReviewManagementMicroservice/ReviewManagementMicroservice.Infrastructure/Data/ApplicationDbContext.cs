using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using ReviewManagementMicroservice.Core.Domian.Entities;

namespace ReviewManagementMicroservice.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Review> Reviews { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
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
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                .HasKey(r => r.ReviewID);

            modelBuilder.Entity<Review>()
                .Property(r => r.ReviewID)
                .ValueGeneratedNever();

            modelBuilder.Entity<Review>()
                .Property(r => r.ReviewText)
                .IsRequired();

            modelBuilder.Entity<Review>()
                .Property(r => r.ReviewDate)
                .IsRequired();

            modelBuilder.Entity<Review>()
                .Property(r => r.Rating)
                .IsRequired();

            modelBuilder.Entity<Review>()
                .HasOne(r => r.BaseReview)
                .WithMany(r => r.Replies)
                .HasForeignKey(r => r.BaseReviewID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
