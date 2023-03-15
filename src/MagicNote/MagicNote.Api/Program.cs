using MagicNote.Api.Extensions;
using MagicNote.Api.Middlewares;
using MagicNote.Core;
using MagicNote.Core.Interfaces;
using MagicNote.Shared;
using MagicNote.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Kiota.Abstractions.Authentication;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
	   .EnableTokenAcquisitionToCallDownstreamApi()
			.AddMicrosoftGraph(builder.Configuration.GetSection("DownstreamApi"))
			.AddInMemoryTokenCaches();


builder.Services.AddAuthorization(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLanguageUnderstandingService(builder.Configuration["LanguageServiceApiKey"] ?? string.Empty);

builder.Services.AddHttpContextAccessor();
builder.Services.AddPlanningService();

// Add the Graph service client and an authorized HttpClient 
builder.Services.AddAuthorizedHttpClient();
builder.Services.AddGraphServiceClient(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>(); 
app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

#region API Endpoints
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
#endregion 

app.Run();
