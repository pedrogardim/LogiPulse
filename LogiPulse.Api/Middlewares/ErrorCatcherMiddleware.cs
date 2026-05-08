using FluentValidation;
using LogiPulse.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LogiPulse.Api.Middlewares;

public class ErrorCatcherMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var (statusCode, message) = ex switch
            {
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, ex.Message),
                ConflictException => (StatusCodes.Status409Conflict, ex.Message),
                BusinessRuleException => (StatusCodes.Status422UnprocessableEntity, ex.Message),
                ValidationException => (StatusCodes.Status400BadRequest, ex.Message),
                _ => (StatusCodes.Status500InternalServerError, ex.Message != string.Empty ? ex.Message : "An error occurred")
            };
            
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = "Error",
                Detail = message,
                Type = ex.GetType().Name
            };
            
            context.Response.StatusCode = problemDetails.Status ?? 500;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}