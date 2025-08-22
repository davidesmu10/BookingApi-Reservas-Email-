using EcomApi.Models;

namespace EcomApi.InMemory;

public static class StaticStore
{
    public static readonly List<User> Users = new();
    public static readonly List<Category> Categories = new();
    public static readonly List<Product> Products = new();
    public static readonly List<CartItem> CartItems = new();
    public static readonly List<Order> Orders = new();

    public static int UserSeq = 1;
    public static int CategorySeq = 1;
    public static int ProductSeq = 1;
    public static int CartSeq = 1;
    public static int OrderSeq = 1;
    public static int OrderItemSeq = 1;

    public static void Seed()
    {
        if (!Users.Any())
        {
            Users.Add(new User { Id = UserSeq++, Email = "admin@shop.com", FullName = "Admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123*"), Role = "Admin" });
            Users.Add(new User { Id = UserSeq++, Email = "user@shop.com", FullName = "User Test", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123*"), Role = "Customer" });
        }
        if (!Categories.Any())
        {
            Categories.Add(new Category { Id = CategorySeq++, Name = "Electrónica", Description = "Gadgets y más" });
            Categories.Add(new Category { Id = CategorySeq++, Name = "Hogar", Description = "Casa y cocina" });
        }
        if (!Products.Any())
        {
            Products.Add(new Product { Id = ProductSeq++, Name = "Auriculares BT", Description = "Over-ear", Price = 59.9m, Stock = 50, CategoryId = 1 });
            Products.Add(new Product { Id = ProductSeq++, Name = "Cafetera", Description = "Espresso", Price = 129.9m, Stock = 15, CategoryId = 2 });
        }
    }
}
