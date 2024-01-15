namespace Akedas.API.Middlewares
{
  public class ResponseBodyReadMiddleware : IMiddleware
  {
    private ILogger<ResponseBodyReadMiddleware> logger;

    public ResponseBodyReadMiddleware(ILogger<ResponseBodyReadMiddleware> logger)
    {
      this.logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

      var originalBodyStream = context.Response.Body;

      try
      {
        var memoryBodyStream = new MemoryStream();

        context.Response.Body = memoryBodyStream;

        await next(context);

        if(context.Response.Body.CanRead)
        {
          memoryBodyStream.Seek(0, SeekOrigin.Begin);
          string body = await new StreamReader(memoryBodyStream).ReadToEndAsync();
          memoryBodyStream.Seek(0, SeekOrigin.Begin);

          await memoryBodyStream.CopyToAsync(originalBodyStream);

          logger.LogInformation(body);


        }

      }
      finally
      {
        context.Response.Body = originalBodyStream;
      }
    }
  }
}
