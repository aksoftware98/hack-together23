namespace MagicNote.Api.Extensions
{
	public static class HttpExtensions
	{

		public static string ExtractToken(this IHttpContextAccessor context)
		{
			return context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
		}
	}
}
