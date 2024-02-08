using System.Text.Json;
using Domain.Exceptions;
using System.Net.Mime;
using Domain.Results;
using Web.Models;

namespace Web.Middlewares;

public class ErrorHandlingMiddleware(
    ILogger<ErrorHandlingMiddleware> logger
) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            string newContent;
            ServerErrorModel errorModel;
            switch (ex)
            {
                case ExceptionBase customExceptionBase:
                    context.Response.StatusCode = customExceptionBase.StatusCode;
                    errorModel = new ServerErrorModel(customExceptionBase.Errors[0]);
                    logger.LogError($"{errorModel.Error.Code}:{errorModel.Error.Description}");
                    newContent = JsonSerializer.Serialize(errorModel);
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    errorModel = new ServerErrorModel(
                        new Error(ex.GetType().ToString(), ex.Message));
                    logger.LogError($"{errorModel.Error.Code}:{errorModel.Error.Description}");
                    newContent = JsonSerializer.Serialize(errorModel);
                    break;
            }

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(newContent);
        }
    }
}