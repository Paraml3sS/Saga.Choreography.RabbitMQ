using Saga.Messages;
using Saga.RabbitMQ;

var builder = WebApplication.CreateBuilder();

builder.Services.AddSingleton(serviceProvider => 
    new RabbitMqConnectionFactory(builder.Configuration).CreateConnection());
builder.Services.AddSingleton<ChannelPool>();
builder.Services.AddTransient<RabbitPublisher>();

var app = builder.Build();


app.MapPost("/booking", (BookTravelRequest request, RabbitPublisher rabbit) =>
{
    PaymentCommand command = new (
        ClientName: request.ClientName,
        Amount: request.Amount
    );

    rabbit.Publish(command);

    return Results.Ok("Booking accepted.");
});


await app.RunAsync();