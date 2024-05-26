using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions;
using Shared.Settings;
using Stock.API.Consumers;
using Stock.API.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedEventConsumer>();

    x.UsingRabbitMq((context, configure) =>
    {
        configure.Host(rabbitMqSettings.Host, hostConfigure =>
        {
            hostConfigure.Username(rabbitMqSettings.Username);
            hostConfigure.Password(rabbitMqSettings.Password);

        });

        configure.ConfigureEndpoints(context);

        configure.ReceiveEndpoint(RabbitMqQueues.StockOrderCreatedEventQueue, configureEndpoint =>
        {
            configureEndpoint.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
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
