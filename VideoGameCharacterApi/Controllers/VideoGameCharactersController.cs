using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoGameCharacterApi.Dtos;
using VideoGameCharacterApi.Models;
using VideoGameCharacterApi.Services;

namespace VideoGameCharacterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameCharactersController : ControllerBase
    {
        private readonly IVideoGameCharacterService _service;

        public VideoGameCharactersController(IVideoGameCharacterService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult<List<CharacterResponse>>> GetCharacters()
            => Ok(await _service.GetAllCharactersAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterResponse>> GetCharacter(int id)
        {
            var character = await _service.GetCharacterByIdAsync(id);
            return character is null ? NotFound("Character with the given Id was not found.") : Ok(character);
        }

        [HttpPost]
        public async Task<ActionResult<CharacterResponse>> AddCharacter(CreateCharacterRequest character)
        {
            var createdCharacter = await _service.AddCharacterAsync(character);
            return CreatedAtAction(nameof(GetCharacter), new { id = createdCharacter.Id }, createdCharacter);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCharacter(int id, UpdateCharacterRequest character)
        {
            var updated = await _service.UpdateCharacterAsync(id, character);
            return updated ? NoContent() : NotFound("Character with the given Id was not found.");
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCharacter(int id)
        {
            var deleted = await _service.DeleteCharacterAsync(id);
            return deleted ? NoContent() : NotFound("Character with the given Id was not found.");
        }
    }
}
