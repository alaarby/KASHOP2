using KASHOP2.DAL.DTOs.Responses;

namespace KASHOP2.API.Middleware
{
    public class GlobalExceptionHandling
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandling(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var errorDetails = new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "server error",
                    StackTrace = ex.InnerException.Message
                };
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(errorDetails);
            }
        }
    }
}
