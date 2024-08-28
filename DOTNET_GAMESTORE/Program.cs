using DOTNET_GAMESTORE.endpoints;
using DOTNET_GAMESTORE.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton(serviceProvider => {
    var configuration = serviceProvider.GetService<IConfiguration>() ?? 
    throw new ApplicationException("Configuration Could Not Be Obtained");
    var connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
    throw new ApplicationException("Connection String Null");

    return new SqlPipeline(connectionString);
});

var app = builder.Build();

app.GetEndpoints();


app.Run();
