using PaymentCompany.Api;
using Saga.Messages;
using Saga.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(serviceProvider =>
    new RabbitMqConnectionFactory(builder.Configuration).CreateConnection());
builder.Services.AddSingleton<ChannelPool>();

builder.Services.AddTransient<PaymentCommandHandler>();
builder.Services.AddTransient<RabbitConsumer<PaymentCommand, PaymentCommandHandler>>();

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{

    var rabbitConsumer = scope.ServiceProvider.GetRequiredService<RabbitConsumer<PaymentCommand, PaymentCommandHandler>>();

    rabbitConsumer.Listen();
    
}


await app.RunAsync();