using App.Domain.Entities;
using App.Orders;
using App.Products;
using Microsoft.EntityFrameworkCore;

namespace App.EntityFrameworkCore
{
    public class AppDbContext : DbContext, IDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add mapping here

            base.OnModelCreating(modelBuilder);
        }

        public new virtual DbSet<TEntity> Set<TEntity>() where TEntity : Entity => base.Set<TEntity>();

        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<OrderCustomer> OrderCustomer { get; set; }
        public virtual DbSet<Product> Product { get; set; }
    }
}