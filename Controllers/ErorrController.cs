using Kursach_CorpHubPortal.Model.View;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace Kursach_CorpHubPortal.Controllers
{
        public class ErrorController : Controller
        {
            private readonly IWebHostEnvironment _environment;

            public ErrorController(IWebHostEnvironment environment)
            {
                _environment = environment;
            }

            [Route("Error")]
            [Route("Error/{statusCode}")]
            public IActionResult Index(int? statusCode = null)
            {

                var httpStatusCode = statusCode.HasValue
                    ? (HttpStatusCode)statusCode.Value
                    : HttpStatusCode.InternalServerError;

                var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionFeature?.Error;

                var model = new ErrorViewModel
                {
                    StatusCode = (int)httpStatusCode,
                    StatusDescription = httpStatusCode.ToString(),
                    Message = GetErrorMessage(httpStatusCode),
                    Exception = _environment.IsDevelopment() ? exception : null,
                    ShowDetails = _environment.IsDevelopment(),
                    EnvironmentName = _environment.EnvironmentName, 
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("Error", model);
            }

            private string GetErrorMessage(HttpStatusCode statusCode)
            {
                return statusCode switch
                {
                    HttpStatusCode.NotFound => "Страница не найдена",
                    HttpStatusCode.Forbidden => "Доступ запрещен",
                    HttpStatusCode.Unauthorized => "Требуется авторизация",
                    HttpStatusCode.BadRequest => "Неверный запрос",
                    HttpStatusCode.InternalServerError => "Внутренняя ошибка сервера",
                    _ => "Произошла ошибка"
                };
            }
    }
}
