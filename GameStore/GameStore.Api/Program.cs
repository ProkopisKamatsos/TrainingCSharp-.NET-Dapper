using System.Reflection.Metadata.Ecma335;
using GameStore.Api.Dtos;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
    new(
        1,
        "Street Fighter ||",
        "Fighting",
        19.99m,
        new DateOnly(1992,7,15)
        ),
        new(
        2,
        "Final Fantasy",
        "RPG",
        69.99m,
        new DateOnly(2024,2,29)
        ),
        new(
        3,
        "Astro Bot",
        "Platform",
        59.99m,
        new DateOnly(2024,9,6)
        )
];
//GET /games
app.MapGet("/games", () => games);

const string GetNameEndpointName = "GetGame";

//GET /games/id
app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id)).WithName(GetNameEndpointName);
//POST/games
app.MapPost("/games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);
    games.Add(game);
    return Results.CreatedAtRoute(GetNameEndpointName, new { id = game.Id }, game);
});
//PUT/games/id
app.MapPut("/games/{id}", (int id,UpdateGameDto updateGame) =>
{
    var index = games.FindIndex(game=>game.Id==id);
    games[index]= new GameDto(
        id,
        updateGame.Name,
        updateGame.Genre,
        updateGame.Price,
        updateGame.ReleaseDate
    );
    return Results.NoContent();
});
//DELETE/games/id
app.MapDelete("/games/{id}",(int id) =>
{
   games.RemoveAll(game=>game.Id==id) ;
   return Results.NoContent();
});
app.Run();
