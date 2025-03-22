using Azure.Identity;
using SmartWaterSimulation.Worker;
using SmartWaterSimulation.Worker.Configuration;
using SmartWaterSimulation.Worker.Domain;

var builder = Host.CreateApplicationBuilder(args);

var keyVaultName = builder.Configuration["KeyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName))
{
    var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
    builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
}

bool useAzureAppConfig = builder.Configuration.GetValue<bool>("UseAzureAppConfig");
if (useAzureAppConfig)
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect(builder.Configuration["AzureAppConfigConnectionString"])
               .ConfigureRefresh(refreshOptions =>
               {
                   refreshOptions.Register("SensorSettings:IntervalMilliseconds", refreshAll: true)
                                 .SetCacheExpiration(TimeSpan.FromSeconds(30));
               });
    });
}

builder.Services.Configure<SensorSettings>(builder.Configuration.GetSection("SensorSettings"));
builder.Services.Configure<ServiceBusSettings>(builder.Configuration.GetSection("ServiceBus"));
builder.Services.AddSingleton<ISensorSimulator, SensorSimulator>();
builder.Services.AddSingleton<ISensorPublisher, ServiceBusPublisher>();
builder.Services.AddHostedService<Worker>();

// Configure logging.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var host = builder.Build();
host.Run();