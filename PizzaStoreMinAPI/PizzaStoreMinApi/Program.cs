using Microsoft.OpenApi.Models;
using PizzaStoreMinApi.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "Todo Api", Description="Keep Track of your tasks", Version="V1"});
});
builder.Services.AddCors(Options => {});


var app = builder.Build();

app.UseCors("This is the unique string");
app.UseSwagger();
app.UseSwaggerUI(c =>
  {
     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
  });


/* Mapping API end points */

app.MapGet("/", () => "Hello World!");
app.MapGet("/pizzas", () => PizzaService.GetAll());
app.MapGet("pizzas/{id}", (int id) => PizzaService.Get(id));
app.MapPost("pizzas", (Pizza pizza) => PizzaService.Add(pizza) );
app.MapPut("/pizzas", (Pizza pizza) => PizzaService.Update(pizza) );
app.MapDelete("/pizzas/{id}", (int id) => PizzaService.Delete(id));


app.Run();
