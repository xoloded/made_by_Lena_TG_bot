using made_by_Lena_TG_bot.Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace made_by_Lena_TG_bot.DataBase
{
    //это БД (образно)
    public class DatabaseContext : DbContext
    {

        private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=EFCore;Trusted_Connection=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(q => q.Products)
                .WithOne(q => q.Category)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasMany(q => q.Reviews)
                .WithOne(q => q.Category)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(q => q.Reviews)
                .WithOne(q => q.Product)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(q => q.Category)
                .WithMany(q => q.Products)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Product>()
            //   .HasOne(q => q.Photo)
            //   .WithMany()
            //   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
               .HasOne(q => q.Photo)
               .WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
               .HasOne(q => q.Category)
               .WithMany(q => q.Reviews)
               .OnDelete(DeleteBehavior.Restrict);
        }

        //это таблицы
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}
