using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTableTests
{
    class TestDataContext : DbContext
    {
        public string ConnectionString
        {
            get; set;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (String.IsNullOrEmpty(ConnectionString))
            {
                ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = EfCoreTests; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            }
            optionsBuilder.UseSqlServer(ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Roman" },
                new User { Id = 2, Name = "Raimund" }
                );

            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, UserId = 1, Sum = 11 },
                new Order { Id = 2, UserId = 2, Sum = 21 },
                new Order { Id = 3, UserId = 1, Sum = 11 },
                new Order { Id = 4, UserId = 2, Sum = 41 },
                new Order { Id = 5, UserId = 1, Sum = 51 },
                new Order { Id = 6, UserId = 2, Sum = 61 }
                );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }

    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        
        public virtual User User { get; set; }


        public decimal Sum { get; set; }
    }
}
