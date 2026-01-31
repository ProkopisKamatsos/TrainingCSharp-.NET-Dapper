var product1 = new Product("Laptop", 1200m);
var product2 = new Product("Laptop", 1200m);
var product3 = new Product("Tablet", 600m);
Console.WriteLine($"Are product1 and product2 equal? {product1 == product2}");
Console.WriteLine($"Are product1 and product3 equal? {product1 == product3}");
public record Product(string Name, decimal Price);



