using TavernSystem.DTO;
using TavernSystem.Models;

namespace TavernSystem.Application;

public interface ITavernService
{
    IEnumerable<AdventurerDTO> GetAllAdventurers();
    AdventurerByIdDTO GetAdventurerById(int id);
    
    Task<bool> CreateAdventurer(Adventurer adventurer);
}