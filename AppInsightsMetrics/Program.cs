using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AddAndConfigureApplicationInsightsTelemetry(builder.Services, builder.Configuration);

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

app.Run();

static void AddAndConfigureApplicationInsightsTelemetry(IServiceCollection services, IConfiguration configuration)
{
    services.AddApplicationInsightsTelemetry();

    services.ConfigureTelemetryModule<EventCounterCollectionModule>(
        static (module, o) =>
        {
            module.UseEventSourceNameAsMetricsNamespace = true;
            module.Counters.Add(new EventCounterCollectionRequest("System.Runtime", "gen-0-size"));
        }
    );
}
