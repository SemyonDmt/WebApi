using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApi.Middlewares
{
    /// <summary>
    /// Central error/exception handler Middleware
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private const string JsonContentType = "application/json";
        private readonly RequestDelegate request;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.request = next;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Invoke(HttpContext context) => this.InvokeAsync(context);

        async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.request(context);
            }
            catch (ValidationException exception)
            {
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                context.Response.ContentType = JsonContentType;

                var dict = new Dictionary<string, List<string>>();

                foreach (var validationFailure in exception.Errors)
                {
                    if (dict.TryGetValue(validationFailure.PropertyName, out var list))
                        list.Add(validationFailure.ErrorMessage);
                    else
                        dict.Add(validationFailure.PropertyName, new List<string> {validationFailure.ErrorMessage});
                }
                
                await context.Response.WriteAsync(JsonConvert.SerializeObject(dict));
            }
        }
    }
}