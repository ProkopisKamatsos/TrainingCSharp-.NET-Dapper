using System;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EfVsDapper;

public class GameCharacterRepository
{
    private readonly string connectionString = "Server=localhost\\SQLEXPRESS;Database=GameDb;Trusted_Connection=True;TrustServerCertificate=True;";
    private readonly GameContextDb _context;
    public GameCharacterRepository()
    {
        _context = new GameContextDb();
    }
    public void EF_Create()
    {
        var character = new GameCharacter
        {
            CharacterName = "Kratos",
            PowerLevel = 9001,
            Weapon = "Wrath of Olympus"
        };
        _context.GameCharacters.Add(character);
        _context.SaveChanges();
    }

    public void EF_Read()
    {
        var character = _context.GameCharacters.FirstOrDefault(c => c.CharacterName == "Kratos");
        System.Console.WriteLine($"CharacterName:{character?.CharacterName} powerLevel:{character?.PowerLevel} weapon:{character?.Weapon}");
    }
    public void EF_Update()
    {
        var character = _context.GameCharacters.FirstOrDefault(c => c.CharacterName == "Kratos");
        if (character != null)
        {
            character.Weapon = "Aragorn";
            _context.SaveChanges();
        }
    }
    public void EF_Delete()
    {
        var character = _context.GameCharacters.FirstOrDefault(c => c.CharacterName == "Aragorn");
        if (character != null)
        {
            _context.GameCharacters.Remove(character);
            _context.SaveChanges();
        }


    }
    public void Dapper_Create()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            string insertQuery = "INSERT INTO GameCharacters (CharacterName , PowerLevel , Weapon) VALUES (@CharacterName , @PowerLevel,@Weapon)";
            connection.Execute(insertQuery, new
            {
                CharacterName = "Mario",
                PowerLevel = 7000,
                Weapon = "Machine Gun"
            });
        }
    }
    public void Dapper_Read()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            string selectQuery = "SELECT * FROM GameCharacters WHERE CharacterName=@CharacterName";
            var character = connection.QueryFirstOrDefault<GameCharacter>(selectQuery, new
            {
                CharacterName = "Mario",
            });
            System.Console.WriteLine($"CharacterName:{character?.CharacterName} powerLevel:{character?.PowerLevel} weapon:{character?.Weapon}");
        }
    }
    public void Dapper_Update()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            string updateQuery = "UPDATE GameCharacters SET Weapon = @Weapon WHERE CharacterName=@CharacterName";
            connection.Execute(updateQuery, new
            {
                Weapon = "Sword Of Elendil",
                CharacterName = "Mario",
            });
        }
    }
    public void Dapper_Delete()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            string deleteQuery = "DELETE FROM GameCharacters WHERE CharacterName=@CharacterName";
            connection.Execute(deleteQuery, new
            {
                CharacterName = "Mario",
            });
        }
    }
}
