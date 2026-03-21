var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
List<Book> books = new List<Book>
{
    new Book { Id = 1, Name = "The Great Gatsby", Author = "F. Scott Fitzgerald" },
    new Book { Id = 2, Name = "To Kill a Mockingbird", Author = "Harper Lee" },
    new Book { Id = 3, Name = "1984", Author = "George Orwell" }
};
app.MapGet("/books", () =>
{
    return Results.Ok(books);
});
app.MapGet("/books/{id}", (int id) =>
{
    var book = books.Find(b => b.Id == id);
    if (book is null)
    {
        return Results.NotFound($"The book with {id} Id does not exist");
    }
    return Results.Ok(book);
});
app.MapPost("/books", (Book book) =>
{
    book.Id = books.Max(b => b.Id + 1);
    books.Add(book);
    return Results.Created();
});
app.MapPut("/books/{id}", (Book book, int id) =>
{
    var updateBook = books.Find(b => b.Id == id);
    if (updateBook is null)
    {
        return Results.NotFound($"The book with {id} Id does not exist");
    }
    updateBook.Name = book.Name;
    updateBook.Author = book.Author;
    return Results.NoContent();
});
app.MapDelete("/books/{id}", (int id) =>
{
    var deleteBook = books.Find(b => b.Id == id);
    if (deleteBook is null)
    {
        return Results.NotFound($"The book with {id} Id does not exist");
    }
    books.Remove(deleteBook);
    return Results.NoContent();
});
app.Run();


class Book
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
}