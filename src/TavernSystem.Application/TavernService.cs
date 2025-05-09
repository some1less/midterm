using TavernSystem.DTO;
using TavernSystem.Models;
using TavernSystem.Repositories;

namespace TavernSystem.Application;

public class TavernService : ITavernService
{
    
    private readonly ITavernRepository _repository;

    public TavernService(ITavernRepository repository)
    {
        _repository = repository;
    }


    public IEnumerable<AdventurerDTO> GetAllAdventurers()
    {
        var adventurers = _repository.GetAllAdventurers();
        return adventurers;
    }
    
    

    public AdventurerByIdDTO GetAdventurerById(int id)
    {
        var adventurer = _repository.GetAdventurerById(id);
        return adventurer;
    }

    public Task<bool> CreateAdventurer(Adventurer adventurer)
    {
        var persons = _repository.GetAllPersons();
        foreach (var person in persons)
        {
            if (adventurer.PersonId == person.Id)
            {
                if (!person.HasBounty)
                {
                    var result = _repository.CreateAdventurer(adventurer);
                    return result;
                }
            }
            else
            {
                throw new Exception("Cannot find person id");
            }
        }

        return null;
    }
}