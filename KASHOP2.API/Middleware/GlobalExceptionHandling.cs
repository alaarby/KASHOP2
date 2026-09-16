using KASHOP2.DAL.DTOs.Responses;

namespace KASHOP2.API.Middleware
{
    // Custom middleware that wraps the rest of the request pipeline in a try/catch
    // so unhandled exceptions from any downstream middleware/controller are caught
    // in one place instead of leaking a raw 500 + stack trace to the client.
    public class GlobalExceptionHandling
    {
        // The next delegate in the ASP.NET Core request pipeline, injected by the framework.
        private readonly RequestDelegate _next;

        public GlobalExceptionHandling(RequestDelegate next)
        {
            _next = next;
        }

        // Called by the framework for every incoming request.
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Run the rest of the pipeline (routing, controllers, other middleware, etc.).
                // If nothing throws, the response is written normally and we never hit the catch block.
                await _next(context);
            }
            catch (Exception ex)
            {
                // Any exception thrown anywhere further down the pipeline bubbles up here.
                // Build a uniform error payload instead of exposing the default ASP.NET Core error page.
                var errorDetails = new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "server error",
                    StackTrace = ex.InnerException.Message
                };

                // Override the response status/body: since the pipeline already started
                // executing, we explicitly set the status code before writing the JSON response.
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(errorDetails);
            }
        }
    }
}
