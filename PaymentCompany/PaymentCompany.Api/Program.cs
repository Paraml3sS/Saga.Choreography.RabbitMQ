using PaymentCompany.Api;
using Saga.Infrastructure;
using Saga.Messages;
using Saga.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.AddRabbitMq();

builder.Services.AddTransient<PaymentCommandHandler>();
builder.Services.AddTransient<RabbitConsumer<PaymentCommand, PaymentCommandHandler>>();

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{

    var rabbitConsumer = scope.ServiceProvider.GetRequiredService<RabbitConsumer<PaymentCommand, PaymentCommandHandler>>();

    rabbitConsumer.Listen();
    
}


await app.RunAsync();