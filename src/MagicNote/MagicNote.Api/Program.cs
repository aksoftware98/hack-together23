using MagicNote.Core;
using MagicNote.Core.Interfaces;
using MagicNote.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLanguageUnderstandingService(builder.Configuration["LanguageServiceApiKey"] ?? string.Empty);
builder.Services.AddSingleton(sp => new HttpClient());
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(sp =>
{
	var context = sp.GetRequiredService<IHttpContextAccessor>();
	var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");

	return new Microsoft.Graph.GraphServiceClient(new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
	{
		requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
		return Task.FromResult(0);
	}));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
	"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapPost("/submit-note", async ([FromBody]SubmitNoteRequest note, ILanguageUnderstnadingService languageService, Microsoft.Graph.GraphServiceClient graphClient) =>
{
	var result = await languageService.AnalyzeTextAsync(note.Query);
	return Results.Ok(result);
});
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

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}