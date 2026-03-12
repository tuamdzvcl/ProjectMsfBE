using projectDemo.DTO.Respone;
using projectDemo.Entity.Enum;
using projectDemo.Exceptions;
using System.Net;
using System.Text.Json;

namespace projectDemo.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private static Task HandleException(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            string message = exception.Message;

            switch (exception)
            {
                case UnauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;

                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case DllNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            var response = ApiResponse<object>.FailResponse(EnumStatusCode.SERVER,exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
    }
