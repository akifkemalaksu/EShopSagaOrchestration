using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Contexts;
using Shared.Constants;
using Shared.Extensions;
using Shared.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderRequestCompletedEventConsumer>();

    x.UsingRabbitMq((context, configure) =>
    {
        configure.Host(rabbitMqSettings.Host, hostConfigure =>
        {
            hostConfigure.Username(rabbitMqSettings.Username);
            hostConfigure.Password(rabbitMqSettings.Password);

        });

        configure.ConfigureEndpoints(context);

        configure.ReceiveEndpoint(RabbitMqQueues.OrderRequestCompletedEventQueue, configureEndpoint =>
        {
            configureEndpoint.ConfigureConsumer<OrderRequestCompletedEventConsumer>(context);
        });
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.ConfigureCQRSServices();
builder.Services.ConfigureServices();

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

