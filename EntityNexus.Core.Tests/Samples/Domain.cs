using EntityNexus.DomainModel.AbstractClasses.Core;
using EntityNexus.DomainModel.Interfaces.Core;
using EntityNexus.Infrastructure.AbstractClasses.Core;
using Microsoft.EntityFrameworkCore;

namespace EntityNexus.Tests.Core.Samples;

public class User(string name) : AEntityNamed(name);

public class Product(string name) : AEntityNamed(name)
{
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public class Order : AEntity, IHasParent<User>, IHasChild<OrderItem>
{
    public int ParentId { get; set; }
    public User? Parent { get; set; }

    public ICollection<OrderItem> Children { get; set; } = [];

    public decimal TotalAmount { get; set; }
}

public class OrderItem(string name) : AEntityNamed(name), IHasParent<Order>
{
    public int ParentId { get; set; }
    public Order? Parent { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class ShopDbContext(DbContextOptions<ShopDbContext> options)
    : ADbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
}

