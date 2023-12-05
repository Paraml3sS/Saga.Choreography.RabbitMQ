using FlightCompany.Api;
using Saga.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddRabbitMq()
    .AddCommandHandlers();

var app = builder.Build();

app.UseCommandHandlers();

await app.RunAsync();