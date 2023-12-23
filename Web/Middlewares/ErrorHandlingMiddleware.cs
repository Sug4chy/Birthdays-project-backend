using System.Text.Json;
using Domain.Exceptions;
using Domain.Models;
using Domain.Responses;

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
            string newContent = string.Empty;
            switch (ex)
            {
                case PasswordValidationException passwordValidationException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    newContent = JsonSerializer.Serialize(new WrapperResponseDto<IResponse>
                    {
                        Response = null,
                        Errors = passwordValidationException.Errors.ToList(),
                        Links = null
                    });
                    break;
                case DuplicateUsernameException duplicateUsernameException:
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    newContent = JsonSerializer.Serialize(new WrapperResponseDto<IResponse>
                    {
                        Response = null,
                        Errors = new List<Error> { duplicateUsernameException.Error },
                        Links = null
                    });
                    break;
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(newContent);
        }
    }
}