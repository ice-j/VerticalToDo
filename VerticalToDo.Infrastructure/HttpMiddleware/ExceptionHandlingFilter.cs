using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using VerticalToDo.Abstractions.Exceptions;

namespace VerticalToDo.Infrastructure.HttpMiddleware
{
    public class ExceptionHandlingFilter
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingFilter(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
                if (context.Response.StatusCode == 404)
                    throw new CustomException($"The call you made was not found. For all available endpoints, check <a href=\"/swagger\">swaggegr</a>");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            MaxDepth = 255,
            AllowTrailingCommas = true
        };
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            string message = "";
            object jContent = null;

            if (exception is CustomException ce)
            {
                code = HttpStatusCode.BadRequest;
                message = ce.Message;
            }
            else if (exception is UnauthorizedException ue)
            {
                code = HttpStatusCode.Unauthorized;
                message = ue.Message;
            }
            else if (exception is FluentValidation.ValidationException ve)
            {
                code = HttpStatusCode.BadRequest;
                jContent = new ErrorsResponse(ve.Errors);
            }
            else
            {
                message = exception.InnerException != null ? JsonSerializer.Serialize(exception.InnerException, _jsonOptions) : exception.Message;
            }

            var result = JsonSerializer.Serialize(new { Error = message }, _jsonOptions);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(JsonSerializer.Serialize(jContent, _jsonOptions).Equals("null") ? result : JsonSerializer.Serialize(jContent, _jsonOptions));
        }

        public class ErrorsResponse
        {
            public ErrorsResponse(IEnumerable<FluentValidation.Results.ValidationFailure> validationFailures)
            {
                Errors = validationFailures.Select(x => new Error
                {
                    PropertyName = x.PropertyName,
                    ErrorMessage = x.ErrorMessage,
                    AttemptedValue = x.AttemptedValue
                }).ToList();
            }
            public class Error
            {
                public string PropertyName { get; set; }
                public string ErrorMessage { get; set; }
                public object AttemptedValue { get; set; }
            }

            public IList<Error> Errors { get; set; }
        }
    }
}
