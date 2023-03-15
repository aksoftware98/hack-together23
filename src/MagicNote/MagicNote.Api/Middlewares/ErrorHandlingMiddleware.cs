using MagicNote.Core.Exceptions;
using MagicNote.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Text.Json;

namespace MagicNote.Api.Middlewares
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;
		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex, _logger);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ErrorHandlingMiddleware> logger)
		{
			var code = HttpStatusCode.InternalServerError;

			var result = new ApiErrorResponse();

			switch (exception)
			{
				// TODO: Handle the other exceptoins properly 
				case DominException e:
					logger.LogWarning("Exceeded the 12 items limit");
					code = HttpStatusCode.BadRequest;
					result = new ApiErrorResponse(e.Message);
					break;
				default:
					logger.LogError(exception, "SERVER ERROR");
					code = HttpStatusCode.InternalServerError;
					result = new ApiErrorResponse("Something went wrong");
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)code;

			string jsonResponse = JsonSerializer.Serialize(result);

			await context.Response.WriteAsync(jsonResponse);
		}
	}
}
