using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Mime;

namespace Akedas.API.Middlewares
{
  public class ApiErrorModel
  {
    public int StatusCode { get; set; }
    public string Message { get; set; }

  }

  // tüm uygulama request bazlı try catch almış olduk.
  public class ExceptionHandlingMiddleware : IMiddleware
  {
    private ILogger<ExceptionHandlingMiddleware> logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware>  logger)
    {
      this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      try
      {
        await next(context);
      }
      catch(SqlException ex)
      {
        await HandleCustomError(context, "Lütfen sistem admini ile haberleşin");
      }
      catch (Exception ex)
      {
        await HandleCustomError(context, ex);

      }
    }

    private async Task HandleCustomError(HttpContext httpContext, string message)
    {
      httpContext.Response.ContentType = MediaTypeNames.Application.Json;
      httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

      var response = new ApiErrorModel()
      {
        Message = message,
        StatusCode = 500

      };

      this.logger.LogInformation(message);

      await httpContext.Response.WriteAsJsonAsync(response);
    }

    private async Task HandleCustomError(HttpContext httpContext, Exception ex)
    {
      httpContext.Response.ContentType = MediaTypeNames.Application.Json;
      httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

      var response = new ApiErrorModel()
      {
        Message = ex.Message,
        StatusCode = 500

      };

      this.logger.LogInformation(ex.Message);

      await httpContext.Response.WriteAsJsonAsync(response);
    }

  }
}
