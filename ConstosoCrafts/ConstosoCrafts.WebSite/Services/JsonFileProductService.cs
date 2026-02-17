using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
    public class JsonFileProductService
    {
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName => Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json");

        public IEnumerable<Product> GetProducts()
        {
            using var jsonFileReader = File.OpenText(JsonFileName);
            return JsonSerializer.Deserialize<Product[]>(
                jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? Enumerable.Empty<Product>();
        }

        public void AddRating(string productId, int rating)
        {
            var products = GetProducts().ToArray();
            var product = products.First(x => x.Id == productId);

            var ratings = (product.Ratings ?? Array.Empty<int>()).ToList();
            ratings.Add(rating);
            product.Ratings = ratings.ToArray();

            using var outputStream = File.Create(JsonFileName);
            using var writer = new Utf8JsonWriter(outputStream, new JsonWriterOptions { Indented = true });

            JsonSerializer.Serialize(writer, products);
            writer.Flush();
        }


    }
}