using KASHOP2.DAL.DTOs.Responses;
using Microsoft.AspNetCore.Diagnostics;

namespace KASHOP2.API
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            var errorDetails = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "server error",
                //StackTrace = ex.InnerException.Message
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(errorDetails);

            return true;
        }
    }
}
