using CarDiaryX.Application.Common;
using CarDiaryX.Application.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarDiaryX.Web.Middlewares
{
    public class ValidationExceptionHandlerMiddleware
    {
        private readonly ILogger logger;
        private readonly RequestDelegate next;

        public ValidationExceptionHandlerMiddleware(RequestDelegate next, ILogger<ValidationExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(context, ex);
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            ErrorResult errorResult = null;

            switch (exception)
            {
                case ModelValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    errorResult = new ErrorResult(validationException.Errors.SelectMany(e => e.Value));
                    break;
                default:
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    if (environment == Environments.Development)
                    {
                        this.logger.LogError(JsonConvert.SerializeObject(exception));
                    }
                    else
                    {
                        var userId = context
                            .User
                            ?.Claims
                            ?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                            ?.Value ?? "Anonymous request";

                        this.logger.LogError(
                            exception,
                            "An unexpected error has occurred. {UserId} - {Environment}; {0} - {1}",
                            userId,
                            environment,
                            nameof(userId),
                            nameof(environment));
                    }

                    errorResult = new ErrorResult(new[] { "An unexpected error has occurred. Please try again later." });
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(SerializeObject(errorResult));
        }

        private static string SerializeObject(object obj)
            => JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(true, true)
                }
            });
    }

    public static class ValidationExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseValidationExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ValidationExceptionHandlerMiddleware>();
    }
}
