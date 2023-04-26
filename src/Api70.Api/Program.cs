using Api70.Application;
using Api70.Infrastructure;
using Api70.Infrastructure.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using static Microsoft.AspNetCore.Builder.WebApplication;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = CreateBuilder(args);

    builder.Host.UseSerilog((hosting, logging) =>
    {
        logging.Enrich.WithThreadId();
        logging.Enrich.WithThreadName();
        logging.Enrich.WithProperty("ApplicationId", Guid.NewGuid());
        logging.ReadFrom.Configuration(hosting.Configuration);
    });

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //Add Application Services
    builder.Services.RegisterApplication();
    builder.Services.RegisterInfrastructure(builder.Configuration);
    builder.Services.RegisterRabbitMqInfrastructure(
        builder.Configuration.GetSection(Api70.Infrastructure.RabbitMq.Module.SectionName));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.MapHealthChecks("/health");

    app.UseSerilogRequestLogging();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}