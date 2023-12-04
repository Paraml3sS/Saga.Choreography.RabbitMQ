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
    PaymentCommand paymentCommand = new (
        ClientName: request.ClientName,
        Amount: request.Amount
    );

    return Results.Ok("Booking accepted.");
});


app.Run();
