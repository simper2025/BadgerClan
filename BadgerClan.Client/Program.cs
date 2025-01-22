var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/api/echo", async (HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var requestBody = await reader.ReadToEndAsync();

    // Log or process the request data
    Console.WriteLine($"Received: {requestBody}");

    // Return a response
    return Results.Json(new { message = $"You sent: {requestBody}" });
});


app.Run();
