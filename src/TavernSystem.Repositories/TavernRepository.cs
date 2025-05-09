using TavernSystem.DTO;
using Microsoft.Data.SqlClient;
using TavernSystem.Models;

namespace TavernSystem.Repositories;

public class TavernRepository : ITavernRepository
{
    private readonly string _connectionString;

    public TavernRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<AdventurerDTO> GetAllAdventurers()
    {
        
        List<AdventurerDTO> adventurers = [];
        
        const string sql = @"SELECT Id, Nickname FROM Adventurer";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var adventurer = new AdventurerDTO
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };

                        adventurers.Add(adventurer);
                    }
                }
            }
            finally
            {
                reader.Close();
            }
            return adventurers;
        }
    }
    
    public IEnumerable<PersonDTO> GetAllPersons()
    {
        
        List<PersonDTO> persons = [];
        
        const string sql = @"SELECT * FROM Person";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var person = new PersonDTO()
                        {
                            Id = reader.GetString(0),
                            HasBounty = reader.GetBoolean(1)
                        };

                        persons.Add(person);
                    }
                }
            }
            finally
            {
                reader.Close();
            }
            return persons;
        }
    }

    public AdventurerByIdDTO GetAdventurerById(int id)
    {
        
        const string sql = @"SELECT * FROM Adventurer WHERE Id = @Id";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new AdventurerByIdDTO()
                    {
                        Nickname         = reader.GetString(1),
                        RaceId               = reader.GetInt32(2),
                        ExperienceId = reader.GetInt32(3),
                        PersonId = reader.GetString(4),
                    };
                }
            }
                
            throw new InvalidOperationException($"Adventurer is not found: '{id}'");
        }
    }

    public async Task<bool> CreateAdventurer(Adventurer adventurer)
    {
        const string insertRace =
            @"INSERT INTO Race (Id, Name) VALUES (@Id, @Name)";
        const string insertExperienceLevel =
            @"INSERT INTO ExperienceLevel (Id, Name) VALUES (@Id, @Name)";
        const string insertPerson = 
            @"INSERT INTO Person (Id, FirstName, MiddleName, LastName, HasBounty) VALUES (@Id, @Name, @FirstName, @MiddleName, @LastName, @HasBounty)";
        const string insertAdventurer =
            @"INSERT INTO Adventurer (Id, Nickname, RaceId, ExperienceId, PersonId) VALUES (@Id, @Nickname, @RaceId, @ExperienceId, @PersonId)";
        
        
        int rowsAffected = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(insertPerson, connection);
            SqlTransaction transaction = connection.BeginTransaction();
            
            try
            {
                using (SqlCommand insertRaceCommand = new SqlCommand(insertRace, connection, transaction))
                {
                    
                    insertRaceCommand.Parameters.AddWithValue("@Id", adventurer.RaceId);
                    
                    rowsAffected = await insertRaceCommand.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }

                using (SqlCommand insertExperienceLevelCommand =
                       new SqlCommand(insertExperienceLevel, connection, transaction))
                {
                    insertExperienceLevelCommand.Parameters.AddWithValue("@Id", adventurer.ExperienceId);
                    rowsAffected = await insertExperienceLevelCommand.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                {
                    
                }
            }
            
            
        }
    }
}