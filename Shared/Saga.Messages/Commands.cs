using Saga.RabbitMQ;

namespace Saga.Messages
{
    public record PaymentCommand(string ClientName, int Amount) : Command;

    public record FlightBookingCommand(string ClientName, string FlightNumber, string From, string To, int Amount) : Command;
    public record HotelBookingCommand(string ClientName, DateTime Arrival, DateTime Departure, int Amount) : Command;
}
