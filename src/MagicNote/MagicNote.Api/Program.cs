using MagicNote.Api.Extensions;
using MagicNote.Core;
using MagicNote.Core.Interfaces;
using MagicNote.Shared;
using MagicNote.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Kiota.Abstractions.Authentication;
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
	var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

	return new Microsoft.Graph.GraphServiceClient(new BaseBearerTokenAuthenticationProvider(new TokenProvider(() => Task.FromResult(token))));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/analyze-note", async ([FromBody]SubmitNoteRequest note, IPlanningService planningService) =>
{
	var result = await planningService.AnalyzeNoteAsync(note);
	return Results.Ok(result);
})
	.WithName("Analyze Note")
	.WithDescription("Submit a note to be analyzed by AI and return a plan object of the graph services to be done")
	.WithOpenApi();

app.MapPost("/submit-plan", async ([FromBody] PlanDetails plan, IPlanningService planningService) =>
{
	await planningService.SubmitPlanAsync(plan);
	return Results.Ok();
})
	.WithName("Submit Plan")
	.WithDescription("Submit a plan so the planned items inside it can be created via the Microsoft Graph")
	.WithOpenApi();

app.Run();
