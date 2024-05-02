namespace IceSync.Middleware
{
    /// <summary>
    /// Centralized exception handler middleware.
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        #region Fields

        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        #endregion

        #region  Constructors

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,
            RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        #endregion

        #region Invoke method
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");

                int statusCode = StatusCodes.Status500InternalServerError;

                switch(ex)
                {
                    case ArgumentNullException:                    
                        statusCode = StatusCodes.Status400BadRequest;
                        break;
                    case NullReferenceException:                    
                        statusCode = StatusCodes.Status404NotFound;
                        break;
                }

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "Application/json";

                await context.Response.WriteAsync(ex.Message);
            }            
        }

        #endregion
    }
}