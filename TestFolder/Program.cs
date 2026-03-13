
using System.Security.Cryptography;

var customers = new List<Customer>
{
    new Customer { Id = 1, Name = "John" },
    new Customer { Id = 2, Name = "Maria" },
    new Customer { Id = 3, Name = "Nick" },
    new Customer { Id = 4, Name = "Anna" }
};
var orders = new List<Order>
{
    new Order { Id = 1, CustomerId = 1, Date = new DateTime(2025,1,10) },
    new Order { Id = 2, CustomerId = 1, Date = new DateTime(2025,1,15) },
    new Order { Id = 3, CustomerId = 2, Date = new DateTime(2025,1,20) },
    new Order { Id = 4, CustomerId = 3, Date = new DateTime(2025,2,1) },
    new Order { Id = 5, CustomerId = 4, Date = new DateTime(2025,2,5) }
};
var orderItems = new List<OrderItem>
{
    new OrderItem { OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 1200 },
    new OrderItem { OrderId = 1, ProductId = 2, Quantity = 2, UnitPrice = 30 },

    new OrderItem { OrderId = 2, ProductId = 2, Quantity = 1, UnitPrice = 30 },

    new OrderItem { OrderId = 3, ProductId = 3, Quantity = 5, UnitPrice = 3 },

    new OrderItem { OrderId = 4, ProductId = 1, Quantity = 1, UnitPrice = 1200 },
    new OrderItem { OrderId = 4, ProductId = 4, Quantity = 2, UnitPrice = 10 },

    new OrderItem { OrderId = 5, ProductId = 3, Quantity = 4, UnitPrice = 3 }
};
var products = new List<Product>
{
    new Product { Id = 1, Name = "Laptop", CategoryId = 1 },
    new Product { Id = 2, Name = "Mouse", CategoryId = 1 },
    new Product { Id = 3, Name = "Bread", CategoryId = 2 },
    new Product { Id = 4, Name = "Cheese", CategoryId = 2 }
};
var categories = new List<Category>
{
    new Category { Id = 1, Name = "Technology" },
    new Category { Id = 2, Name = "Food" }
};
var results = customers.Join(orders, c => c.Id, o => o.CustomerId, (c, o) => new
{
    cName = c.Name,
    oId = o.Id
}).Join(orderItems, o => o.oId, oi => oi.OrderId, (o, oi) => new
{
    cusName = o.cName,
    pId = oi.ProductId,
    oQuantity = oi.Quantity,
    oiUnitPrice = oi.UnitPrice
}).Join(products, oi => oi.pId, p => p.Id, (oi, p) => new
{
    customername = oi.cusName,
    oiQuantity = oi.oQuantity,
    oiUnitPrice = oi.oiUnitPrice,
    cId = p.CategoryId,
    pName = p.Name

}).Join(categories, x => x.cId, c => c.Id, (x, c) => new
{
    custoemer = x.customername,
    quantity = x.oiQuantity,
    unitprice = x.oiUnitPrice,
    cName = c.Name,

}).GroupBy(x => x.cName).Select(g => new
{
    CategoryName = g.Key,
    countCustomers = g.Select(x => x.custoemer).Distinct().Count(),
    Quantiry = g.Sum(x => x.quantity),
    Revenue = g.Sum(x => x.quantity * x.unitprice),
    Average = g.Average(x => x.unitprice * x.quantity)
}).Where(x => x.countCustomers >= 2).OrderByDescending(x => x.Revenue).ToList();
foreach (var item in results)
{
    Console.WriteLine($"Category: {item.CategoryName}, Customers: {item.countCustomers}, Quantity: {item.Quantiry}, Revenue: {item.Revenue}, Average Price: {item.Average}");
}

class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
}
class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime Date { get; set; }
}

class OrderItem
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
}

class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
}
