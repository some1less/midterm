using TavernSystem.Application;
using TavernSystem.Models;
using TavernSystem.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TavernDatabase");
builder.Services.AddSingleton<ITavernService, TavernService>(TavernService => new TavernService(new TavernRepository(connectionString)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/api/adventurers", (ITavernService tavernService) =>
{
    try
    {
        var result = tavernService.GetAllAdventurers();
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/adventurers/{id}", (ITavernService tavernService, int id) =>
{
    try
    {
        var result = tavernService.GetAdventurerById(id);
        if (result == null)
            return Results.NotFound($"Adventurer with id = {id} not found");
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/api/adventurers", (ITavernService tavernService, Adventurer adventurer) =>
{
    try
    {
        var result = tavernService.CreateAdventurer(adventurer);
        return Results.Created($"/api/adventurers/{result.Id}", result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
    
});

app.Run();