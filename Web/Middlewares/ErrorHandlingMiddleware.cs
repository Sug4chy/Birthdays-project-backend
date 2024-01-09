using System.Text.Json;
using Domain.Exceptions;
using Domain.Models;
using System.Net.Mime;
using Domain.DTO.Responses;
using FluentValidation;

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
                    newContent = JsonSerializer.Serialize(new WrapperResponseDto<IResponse>
                    {
                        Response = null,
                        Errors = validationException.Errors
                            .Select(vf => new Error
                            {
                                Code = vf.ErrorCode,
                                Message = vf.ErrorMessage
                            }).ToArray(),
                        Links = null
                    });
                    break;
                case CustomExceptionBase customExceptionBase:
                    context.Response.StatusCode = customExceptionBase.StatusCode;
                    newContent = JsonSerializer.Serialize(new WrapperResponseDto<IResponse>
                    {
                        Response = null,
                        Errors = customExceptionBase.Errors.ToArray(),
                        Links = null
                    });
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    newContent = JsonSerializer.Serialize(new WrapperResponseDto<IResponse>
                    {
                        Response = null,
                        Errors = new[] { new Error { Code = ex.GetType().ToString(), Message = ex.Message } },
                        Links = null
                    });
                    break;
            }

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(newContent);
        }
    }
}