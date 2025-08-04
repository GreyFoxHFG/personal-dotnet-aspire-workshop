var builder = DistributedApplication.CreateBuilder(args);

// Reference an external API service
var weatherApi = builder.AddExternalService("weather-api", "https://api.weather.gov");

var api = builder.AddProject<Projects.Api>("api")
    .WithReference(weatherApi);

var web = builder.AddProject<Projects.MyWeatherHub>("myweatherhub")
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
