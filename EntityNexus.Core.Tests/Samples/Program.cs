using EntityNexus.Tests.Core.Samples;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddDbContext<ShopDbContext>(opt =>
    opt.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ENM_Sample_11;Trusted_Connection=True;"));

var provider = services.BuildServiceProvider();

using var scope = provider.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ShopDbContext>();

await db.Database.EnsureDeletedAsync();
await db.Database.EnsureCreatedAsync();


// 1. Создаем пользователя
User user = new("John");
user.Rename("Tom");


// 2. Создаем продукт
var product = new Product("Laptop")
{
    Price = 1000,
    Stock = 5
};

db.AddRange(user, product);

user.Rename("Gary");

await db.SaveChangesAsync();


// 3. Создаем заказ
var order = new Order
{
    Parent = user,
    TotalAmount = product.Price,
    Children =
    [
        new OrderItem("Laptop")
        {
            Quantity = 1,
            Price = product.Price
        }
    ]
};

product.Stock -= 1;

db.Orders.Add(order);

product.Rename("New Name");
product.Price = 200;


await db.SaveChangesAsync();

Console.WriteLine("Done");

