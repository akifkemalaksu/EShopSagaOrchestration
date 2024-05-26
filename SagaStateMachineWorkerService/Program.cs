using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaStateMachineWorkerService;
using SagaStateMachineWorkerService.Contexts;
using SagaStateMachineWorkerService.Machines;
using SagaStateMachineWorkerService.Models;
using Shared.Settings;
using System.Reflection;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(configure =>
{
    var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

    configure.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
        .EntityFrameworkRepository(options =>
        {
            options.AddDbContext<DbContext, OrderStateDbContext>((serviceProvider, dbContextBuilder) =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                dbContextBuilder.UseSqlServer(connectionString, migrationOptions =>
                {
                    migrationOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                });
            });
        });

    configure.UsingRabbitMq((context, busConfigure) =>
    {
        busConfigure.Host(rabbitMqSettings.Host, hostConfigure =>
        {
            hostConfigure.Username(rabbitMqSettings.Username);
            hostConfigure.Password(rabbitMqSettings.Password);
        });

        busConfigure.ReceiveEndpoint(RabbitMqQueues.OrderSaga, configureEndpoint =>
        {
            configureEndpoint.UseMessageRetry(retry => retry.Immediate(5));

            configureEndpoint.ConfigureSaga<OrderStateInstance>(context);
        });
    });
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
