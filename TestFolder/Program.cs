var categories = new List<Category>
{
    new Category { Id = 1, Name = "Tech" },
    new Category { Id = 2, Name = "Food" },
    new Category { Id = 3, Name = "Books" }
};
var products = new List<Product>
{
    new Product { Id = 1, Name = "Laptop", CategoryId = 1 },
    new Product { Id = 2, Name = "Mouse", CategoryId = 1 },
    new Product { Id = 3, Name = "Bread", CategoryId = 2 },
    new Product { Id = 4, Name = "Cheese", CategoryId = 2 },
    new Product { Id = 5, Name = "C# in Depth", CategoryId = 3 }
};
var orderItems = new List<OrderItem>
{
    new OrderItem { OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 1200 },
    new OrderItem { OrderId = 1, ProductId = 2, Quantity = 2, UnitPrice = 25 },
    new OrderItem { OrderId = 2, ProductId = 3, Quantity = 5, UnitPrice = 2 },
    new OrderItem { OrderId = 2, ProductId = 4, Quantity = 2, UnitPrice = 8 },
    new OrderItem { OrderId = 3, ProductId = 5, Quantity = 1, UnitPrice = 40 },
    new OrderItem { OrderId = 4, ProductId = 3, Quantity = 3, UnitPrice = 2 }
};
var result = categories
    .Join(products,
        c => c.Id,
        p => p.CategoryId,
        (c, p) => new
        {
            CategoryName = c.Name,
            ProductId = p.Id
        })
    .Join(orderItems,
        cp => cp.ProductId,
        oi => oi.ProductId,
        (cp, oi) => new
        {
            cp.CategoryName,
            cp.ProductId,
            oi.Quantity,
            oi.UnitPrice
        })
    .GroupBy(x => x.CategoryName)
    .Select(g => new
    {
        CategoryName = g.Key,
        SoldProductsCount = g.Select(x => x.ProductId).Distinct().Count(),
        TotalQuantity = g.Sum(x => x.Quantity),
        TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
    });

foreach (var item in result)
{
    Console.WriteLine(
        $"{item.CategoryName} - Products: {item.SoldProductsCount} - Quantity: {item.TotalQuantity} - Revenue: {item.TotalRevenue}");
}
class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
}
class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
}
class OrderItem
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}