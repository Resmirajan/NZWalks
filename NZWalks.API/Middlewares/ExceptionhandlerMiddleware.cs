using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionhandlerMiddleware
    {
        private readonly ILogger<ExceptionhandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionhandlerMiddleware(ILogger<ExceptionhandlerMiddleware>logger,
            RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {

                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                //Log this eception
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                //Return a custom Error response 
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    id = errorId,
                    ErrorMessage = "Something went wrong ! we are resolving it ASAP"
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
