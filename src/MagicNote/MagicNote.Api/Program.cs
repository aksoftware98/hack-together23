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
builder.Services.AddPlanningService();
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

app.MapPost("/submit-note", async ([FromBody]SubmitNoteRequest note, IPlanningService planningService) =>
{
	var result = await planningService.AnalyzeNoteAsync(note);
	return Results.Ok(result);
})
	.WithName("Submit Note")
	.WithDescription("Submit a note to be analyzed by AI and return a plan object of the graph services to be done")
	.WithOpenApi();


app.Run();
