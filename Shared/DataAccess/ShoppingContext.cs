using Microsoft.EntityFrameworkCore;
using Shared.Domain.Models;

namespace Shared.DataAccess
{

    public class ShoppingDb : DbContext
    {
        public ShoppingDb(DbContextOptions<ShoppingDb> options)
               : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder

            // .UseLazyLoadingProxies()
            //.UseSqlite("Data Source=orders.db");
            // Data Source=(localdb)\mssqllocaldb;Initial Catalog=ShoppingApp.ShoppingDb;Integrated Security=True
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            // Ignore picture property in the Db build
            // Avoid loading unnecessary data that can lead to large memory footprints in the app.
            builder.Entity<Customer>()
                   .Ignore(c => c.ProfilePicture);

                   // .Ignore(c => c.ProfilePictureValueHolder);

            base.OnModelCreating(builder);
        }
    }
}
