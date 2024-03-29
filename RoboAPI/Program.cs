// Creates the WebApplication Builder and the Web Application with pre-config defaults
// Trust the HTTPS development cert by running [dotnet dev-certs https --trust]
// Creates the WebApplication Builder and the Web Application with pre config defaults
// Trust the HTTPS development cert by running [dotnet dev-certs https --trust]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Swagger/OpenAPI https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build policy (name, token) allow services, headers, methods
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
}); // built in services


var app = builder.Build();
app.UseCors("AllowAll"); // Policy

// Pipeline HTTP requests
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "LEFT", "RIGHT", "PLACE", "MOVE"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}