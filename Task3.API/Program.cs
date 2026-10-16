using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Task3.Application.Common.Behaviors;
using Task3.Application.Common.Interfaces;
using Task3.Application.Products.Commands;
using Task3.Application.Products.Validators;
using Task3.Infrastructure.Data;
using Task3.Infrastructure.Repositories;




// serilog configg
Log.Logger = new LoggerConfiguration().MinimumLevel.Information().Enrich.FromLogContext()
.Enrich.WithProperty("Application", "Task3ProductApi").WriteTo.Console()
.WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day).CreateLogger();



try
{
    var builder = WebApplication.CreateBuilder(args);


    // I tell .net to use serilog
    builder.Host.UseSerilog();


    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();



    // dbcotnext
    builder.Services.AddDbContext<AppDbContext>(options => 
        options.UseNpgsql("Host=localhost;Port=5432;Database=task3db;Username=mnagy;Password=mohamed123")
    );



    // repo
    builder.Services.AddScoped<IProductRepository, ProductRepository>();


    // mediatr
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssemblies(typeof(CreateProductHandler).Assembly);
    });



    // FluentValidation
    builder.Services.AddValidatorsFromAssembly(typeof(CreateProductValidator).Assembly);

    // Validation Behavior
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();



}
catch(Exception ex)
{
    Log.Fatal(ex, "App terminated unexpectedly");
}
finally{ Log.CloseAndFlush(); }



