using System;
using Microsoft.EntityFrameworkCore;

namespace EfVsDapper;

public class GameContextDb() : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=GameDb;Trusted_Connection=True;TrustServerCertificate=True;");
    }
    public DbSet<GameCharacter> GameCharacters { get; set; }
}
