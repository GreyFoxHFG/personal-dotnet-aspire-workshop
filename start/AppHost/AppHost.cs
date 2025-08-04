var builder = DistributedApplication.CreateBuilder(args);

// Reference an external API service
var theWeatherApi = builder.AddExternalService("weather-api", "https://api.weather.gov");

var redisCache = builder.AddRedis("cache")
    .WithRedisInsight();

var theApi = builder.AddProject<Projects.Api>("api")
    .WithReference(theWeatherApi)
    .WithReference(redisCache);

var theWebsite = builder.AddProject<Projects.MyWeatherHub>("myweatherhub")
    .WithReference(theApi)
    .WaitFor(theApi)
    .WithExternalHttpEndpoints();

builder.Build().Run();
