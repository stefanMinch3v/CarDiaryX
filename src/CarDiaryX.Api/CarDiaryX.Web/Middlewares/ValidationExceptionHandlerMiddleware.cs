using CarDiaryX.Application.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

            ProblemDetailsError result = null;

            switch (exception)
            {
                case ModelValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = new ProblemDetailsError
                    {
                        Status = (int)code,
                        Detail = MergeErrors(validationException.Errors.SelectMany(e => e.Value)),
                        Instance = context.Request.Path
                    };
                    break;
                //case ExampleException2 _:
                //    code = HttpStatusCode.NotFound;
                //    break;
                case BaseDomainException domainException:
                    code = HttpStatusCode.BadRequest;
                    result = new ProblemDetailsError
                    {
                        Status = (int)code,
                        Detail = domainException.Message,
                        Instance = context.Request.Path
                    };
                    break;
                default:
                    this.logger.LogError(JsonConvert.SerializeObject(exception));
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (result is null)
            {
                result = new ProblemDetailsError
                {
                    Status = (int)code,
                    Detail = "An unexpected error has occured. Please try again later.",
                    Instance = context.Request.Path
                };
            }

            return context.Response.WriteAsync(SerializeObject(result));
        }

        private static string SerializeObject(object obj)
            => JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(true, true)
                }
            });

        private static string MergeErrors(IEnumerable<string> errors)
        {
            var sb = new StringBuilder();

            foreach (var error in errors)
            {
                sb.AppendLine(error);
            }

            return sb.ToString();
        }

        private class ProblemDetailsError
        {
            public string Detail { get; set; }
            public int Status { get; set; }
            public string Instance { get; set; }
        }
    }

    public static class ValidationExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseValidationExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ValidationExceptionHandlerMiddleware>();
    }
}
