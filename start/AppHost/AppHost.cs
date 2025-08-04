var builder = DistributedApplication.CreateBuilder(args);

// Reference an external API service
var theWeatherApi = builder.AddExternalService("weather-api", "https://api.weather.gov");

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);  // Container persists across app restarts;  

var theWeatherDb = postgres.AddDatabase("weatherdb");

var redisCache = builder.AddRedis("cache")
    .WithRedisInsight();

var theApi = builder.AddProject<Projects.Api>("api")
    .WithReference(theWeatherApi)
    .WithReference(redisCache);

var theWebsite = builder.AddProject<Projects.MyWeatherHub>("myweatherhub")
    .WithReference(theApi)
    .WithReference(theWeatherDb)
    .WaitFor(theApi)
    .WaitFor(postgres)  // Ensures database is ready before app starts
    .WithExternalHttpEndpoints();

builder.Build().Run();
