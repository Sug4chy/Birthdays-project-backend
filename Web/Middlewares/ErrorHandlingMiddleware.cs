using System.Text.Json;
using Domain.Exceptions;
using System.Net.Mime;
using Domain.Results;
using FluentValidation;
using Web.Models;

namespace Web.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
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
            switch (ex)
            {
                case ValidationException validationException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    newContent = JsonSerializer.Serialize(new ServerErrorModel(
                        validationException.Errors
                        .Select(Error.FromValidationFailure).First()));
                    break;
                case CustomExceptionBase customExceptionBase:
                    context.Response.StatusCode = customExceptionBase.StatusCode;
                    newContent = JsonSerializer.Serialize(new ServerErrorModel(
                        customExceptionBase.Errors.First()));
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    newContent = JsonSerializer.Serialize(new ServerErrorModel(
                        new Error(ex.GetType().ToString(), ex.Message)));
                    break;
            }

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(newContent);
        }
    }
}