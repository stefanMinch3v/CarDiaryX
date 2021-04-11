using CarDiaryX.Application.Common.Exceptions;
using CarDiaryX.Domain.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            var result = string.Empty;

            switch (exception)
            {
                case ModelValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = SerializeObject(new
                    {
                        ValidationDetails = true,
                        validationException.Errors
                    });
                    break;
                //case ExampleException2 _:
                //    code = HttpStatusCode.NotFound;
                //    break;
                case BaseDomainException domainException:
                    result = SerializeObject(new[] { domainException.Message });
                    break;
                default:
                    this.logger.LogError(JsonConvert.SerializeObject(exception));
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (string.IsNullOrEmpty(result))
            {
                result = SerializeObject(new[] { exception.Message });
            }

            return context.Response.WriteAsync(result);
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
