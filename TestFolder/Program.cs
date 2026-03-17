var customers = new List<Customer>
{
    new Customer { Id = 1, Name = "John" },
    new Customer { Id = 2, Name = "Maria" },
    new Customer { Id = 3, Name = "Nick" },
    new Customer { Id = 4, Name = "Anna" },
    new Customer { Id = 5, Name = "George" }
};
var orders = new List<Order>
{
    new Order { Id = 1, CustomerId = 1, Amount = 120 },
    new Order { Id = 2, CustomerId = 1, Amount = 80 },
    new Order { Id = 3, CustomerId = 2, Amount = 200 },
    new Order { Id = 4, CustomerId = 2, Amount = 50 },
    new Order { Id = 5, CustomerId = 3, Amount = 300 },
    new Order { Id = 6, CustomerId = 3, Amount = 100 },
    new Order { Id = 7, CustomerId = 3, Amount = 50 },
    new Order { Id = 8, CustomerId = 5, Amount = 90 }
};
var result = customers
    .GroupJoin(
        orders,
        c => c.Id,
        o => o.CustomerId,
        (c, orderGroup) => new
        {
            Name = c.Name,
            TotalAmount = orderGroup.Sum(x => x.Amount),
            OrderCount = orderGroup.Count()
        })
    .OrderByDescending(x => x.TotalAmount)
    .ToList();

foreach (var item in result)
{
    Console.WriteLine($"Customer: {item.Name}, Total Amount: {item.TotalAmount}, OrderCount: {item.OrderCount}");
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
    public decimal Amount { get; set; }
}
