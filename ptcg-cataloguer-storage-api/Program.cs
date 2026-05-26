using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Cataloguer.Services;
using Cataloguer.data;
using Cataloguer.Dtos;
using Cataloguer.HelperFunctions;

// Initialises a builder for web applications and services
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=cataloguer.db"));
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CardService>();
builder.Services.AddScoped<ListService>();
builder.Services.AddScoped<Helpers>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();


// Registers the required services
builder.Services.AddOpenApi();

// Produces a request function that executes added middlewares.
var app = builder.Build();

// Conditionally add services or middleware depending on the current environment
if (app.Environment.IsDevelopment())
{
    // Adds an endpoint to generate and serve an OpenAPI document in JSON format
    app.MapOpenApi();
}

// Redirects HTTP requests to HTTPS
app.UseHttpsRedirection();


app.MapGet("/", () => "API is running!");

// Users Endpoints
// Get users
app.MapGet("/users", (UserService service) =>
{
    return service.GetAllUsers();
});

// Get user
app.MapGet("/users/{UserId}", (Guid UserId, UserService service) =>
{
    return service.GetUser(UserId);
});

// Add user
app.MapPost("/users", (UserCreateDto dto, IValidator<UserCreateDto> validator, UserService service) =>
{
    return service.CreateUser(dto, validator);
});

// Update user
app.MapPut("/users/{UserId}", (Guid UserId, UserUpdateDto dto, IValidator<UserUpdateDto> validator, UserService service) =>
{
    return service.UpdateUser(UserId, dto, validator);
});

// Delete user
app.MapDelete("/users/{UserId}", (Guid UserId, UserService service) =>
{
    return service.DeleteUser(UserId);
});

// Cards Endpoints
// Get cards
app.MapGet("/users/{UserId}/cards", (Guid UserId, CardService service) =>
{
    return service.GetCards(UserId);
});

// Get card
app.MapGet("/users/{UserId}/cards/{CardId}", (Guid UserId, string CardId, CardService service) =>
{
    return service.GetCard(UserId, CardId);
});

// Add card
app.MapPost("users/{UserId}/cards", (Guid UserId, CardCreateDto dto, IValidator<CardCreateDto> validator, CardService service) =>
{
    Console.WriteLine("POST Card");
    return service.AddCard(UserId, dto, validator);
});

// Update card
app.MapPut("/users/{UserId}/cards/{CardId}", (Guid UserId, string CardId, CardUpdateDto dto, IValidator<CardUpdateDto> validator, CardService service) =>
{
    return service.UpdateCard(UserId, CardId, dto, validator);
});

// Delete card
app.MapDelete("/users/{UserId}/cards/{CardId}", (Guid UserId, string CardId, CardService service) =>
{
    return service.DeleteCard(UserId, CardId);
});

// Lists Endpoints
// Get lists
app.MapGet("users/{UserId}/lists", (Guid UserId, ListService service) =>
{
    return service.GetLists(UserId);
});

// Get list
app.MapGet("users/{UserId}/lists/{ListId}", (Guid UserId, Guid ListId, ListService service) =>
{
    return service.GetList(UserId, ListId);
});

// Create list
app.MapPost("users/{UserId}/lists", (Guid UserId, CardsListCreateDto dto, IValidator<CardsListCreateDto> validator, ListService service) =>
{
    return service.CreateList(UserId, dto, validator);
});

// Update list name
app.MapPut("users/{UserId}/lists/{ListId}/name", (Guid UserId, Guid ListId, CardsListNameUpdateDto dto, IValidator<CardsListNameUpdateDto> validator, ListService service) =>
{
    return service.UpdateListName(UserId, ListId, dto, validator);
});

// Update list cardIds
app.MapPut("users/{UserId}/lists/{ListId}", (Guid UserId, Guid ListId, CardsListUpdateDto dto, ListService service) =>
{
    return service.UpdateList(UserId, ListId, dto);
});

// Delete list
app.MapDelete("users/{UserId}/lists/{ListId}", (Guid UserId, Guid ListId, ListService service) =>
{
    return service.DeleteList(UserId, ListId);
});

app.Run();