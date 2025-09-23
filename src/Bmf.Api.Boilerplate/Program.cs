WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

app.UseExceptionHandler();

app.MapGet("/", () => Results.Ok(new { Service = "Bmf.Api.Boilerplate", Status = "OK" }));

app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));

app.Run();
