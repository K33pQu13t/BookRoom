using BookingAudience.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookingAudience.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error")]
        public IActionResult Index()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            Exception exception;
            //if (context == null)
            //{
            //    exception = new UserRoleException();
            //}
            //else
                exception = context.Error;


            // Internal Server Error поумолчанию
            HttpStatusCode code = HttpStatusCode.InternalServerError;

            //if (exception is HttpStatusException httpException)
            //{
            //    code = httpException.Status;
            //}

            Response.StatusCode = (int)code;

            return View("Error", new ErrorResponseViewModel(exception));
        }
    }
}
