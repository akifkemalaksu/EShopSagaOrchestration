using MassTransit;
using Payment.API.Consumers;
using Shared.Constants;
using Shared.Settings;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<StockReservedRequestPaymentConsumer>();

    x.UsingRabbitMq((context, configure) =>
    {
        configure.Host(rabbitMqSettings.Host, hostConfigure =>
        {
            hostConfigure.Username(rabbitMqSettings.Username);
            hostConfigure.Password(rabbitMqSettings.Password);
        });

        configure.ReceiveEndpoint(RabbitMqQueues.PaymentStockReservedRequestQueue, configureEndpoint =>
        {
            configureEndpoint.ConfigureConsumer<StockReservedRequestPaymentConsumer>(context);
        });
    });
});

builder.Services.ConfigureServices();
builder.Services.ConfigureCQRSServices();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
