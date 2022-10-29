using Microsoft.EntityFrameworkCore;
using photo_gallery_api.Entities;
using System.Data.SqlClient;
using System.Net;
using System.Text.Json;

namespace photo_gallery_api.ExceptionMiddleware
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
            catch (DbUpdateConcurrencyException ex)
            {
                string message = "Some of the data you affect has changed, please refresh the page and try again";
                HttpStatusCode httpStatusCode = HttpStatusCode.Conflict;
                await HandleExceptionAsync(context, ex, message, httpStatusCode);
            }
            catch (SqlException ex)
            {
                string message = "There was a problem with the database, please refresh the page and try again";
                HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
                await HandleExceptionAsync(context, ex, message, httpStatusCode);
            }
            catch (Exception ex)
            {
                string message = "Something went wrong, please refresh the page and try again";
                HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
                await HandleExceptionAsync(context, ex, message, httpStatusCode);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception, string message, HttpStatusCode httpStatusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
