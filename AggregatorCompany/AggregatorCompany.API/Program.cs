using Saga.Infrastructure;
using Saga.Messages;

var builder = WebApplication.CreateBuilder();

builder.AddRabbitMq();

var app = builder.Build();


app.MapPost("/booking", (BookTravelRequest request, RabbitPublisher rabbit) =>
{
    FlightBookingCommand flightBookingCommand = new (
        ClientName: request.ClientName,
        FlightNumber: request.FlightNumber,
        From: request.From,
        To: request.To,
        Amount: request.FlightCost
    );

    HotelBookingCommand hotelBookingCommand = new (
        ClientName: request.ClientName,
        Arrival: request.Arrival,
        Departure: request.Departure,
        Amount: request.HotelCost
    );

    rabbit.Publish(flightBookingCommand);
    rabbit.Publish(hotelBookingCommand);

    return Results.Ok("Booking accepted.");
});


await app.RunAsync();