using TavernSystem.DTO;
using TavernSystem.Models;

namespace TavernSystem.Repositories;

public interface ITavernRepository
{
    IEnumerable<AdventurerDTO> GetAllAdventurers();
    IEnumerable<PersonDTO> GetAllPersons();
    AdventurerByIdDTO GetAdventurerById(int id);
    
    Task<bool> CreateAdventurer(Adventurer adventurer);
}