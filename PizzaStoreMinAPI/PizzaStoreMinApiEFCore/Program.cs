using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStoreMinApiEFCore.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionstring = builder.Configuration.GetConnectionString("Pizzas") ?? "Data Source = Pizzas.db";

builder.Services.AddEndpointsApiExplorer();

// The below line will be helpful for InMemory storage, but now we are going to use Sqlite, so commenting out
// builder.Services.AddDbContext<PizzaDB>(FileOptions => FileOptions.UseInMemoryDatabase("items"));
builder.Services.AddSqlite<PizzaDB>(connectionstring);

builder.Services.AddSwaggerGen( c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "PizzaStore Min EFCore",
                                        Description = " Pizzastore Min EFCore API detailed description",
                                        Version = "v1"});
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI( c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore Min EFCore");
});

app.MapGet("/", () => "Hello World!");
app.MapGet("/pizzas", async (PizzaDB db) => await db?.Pizzas?.ToListAsync());
app.MapGet("/pizzas/{id}", async (PizzaDB db, int id) => await db.Pizzas.FindAsync(id));

app.MapPost("/pizzas", async (PizzaDB db, Pizza pizza) =>
{
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();
    return Results.Created($"/pizza/{pizza.Id}", pizza);
});

app.MapPut("/pizzas/{id}", async (PizzaDB db, Pizza updatepizza, int id) =>
{
      var pizza = await db.Pizzas.FindAsync(id);
      if (pizza is null) return Results.NotFound();
      pizza.Name = updatepizza.Name;
      pizza.Description = updatepizza.Description;
      await db.SaveChangesAsync();
      return Results.NoContent();
});

app.MapDelete("/pizzas/{id}", async (PizzaDB db, int id) =>
{
   var pizza = await db.Pizzas.FindAsync(id);
   if (pizza is null)
   {
      return Results.NotFound();
   }
   db.Pizzas.Remove(pizza);
   await db.SaveChangesAsync();
   return Results.Ok();
});

app.Run();
