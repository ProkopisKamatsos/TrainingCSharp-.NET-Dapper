var products = new[] {
    new { Name = "Laptop", Price = 1200 },
    new { Name = "Tablet", Price = 600 }
};

var filteredProducts = from p in products
                       where p.Price > 1000
                       select new { p.Name, p.Price };

foreach (var p in filteredProducts)
{
    Console.WriteLine($"Name: {p.Name}, Price: {p.Price}");
}
// Output:
// Name: Laptop, Price: 1200