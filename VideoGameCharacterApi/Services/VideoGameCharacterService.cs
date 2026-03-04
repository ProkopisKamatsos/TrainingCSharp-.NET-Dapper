using Microsoft.EntityFrameworkCore;
using VideoGameCharacterApi.Data;
using VideoGameCharacterApi.Dtos;
using VideoGameCharacterApi.Models;

namespace VideoGameCharacterApi.Services
{
    public class VideoGameCharacterService : IVideoGameCharacterService
    {
        private readonly AppDbContext _dbContext;
        public VideoGameCharacterService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<CharacterResponse> AddCharacterAsync(CreateCharacterRequest character)
        {
            var newCharacter = new Character
            {
                Name = character.Name,
                Game = character.Game,
                Role = character.Role
            };

            _dbContext.Characters.Add(newCharacter);
            await _dbContext.SaveChangesAsync();

            return new CharacterResponse
            {
                Id = newCharacter.Id,
                Name = newCharacter.Name,
                Game = newCharacter.Game,
                Role = newCharacter.Role
            };
        }

        public async Task<bool> DeleteCharacterAsync(int id)
        {
            var characterToDelete = await _dbContext.Characters.FindAsync(id);
            if (characterToDelete is null)
                return false;

            _dbContext.Characters.Remove(characterToDelete);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<CharacterResponse>> GetAllCharactersAsync()
        {
            return await _dbContext.Characters
                .Select(c => new CharacterResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Game = c.Game,
                    Role = c.Role
                })
                .ToListAsync();
        }

        public async Task<CharacterResponse?> GetCharacterByIdAsync(int id)
        {
            var character = await _dbContext.Characters.Where(c => c.Id == id)
                                                        .Select(c => new CharacterResponse
                                                        {

                                                            Name = c.Name,
                                                            Game = c.Game,
                                                            Role = c.Role
                                                        }).FirstOrDefaultAsync();

            return character;
        }

        public async Task<bool> UpdateCharacterAsync(int id, UpdateCharacterRequest character)
        {
            var existingCharacter = await _dbContext.Characters.FindAsync(id);
            if (existingCharacter is null)
                return false;

            existingCharacter.Name = character.Name;
            existingCharacter.Game = character.Game;
            existingCharacter.Role = character.Role;

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
