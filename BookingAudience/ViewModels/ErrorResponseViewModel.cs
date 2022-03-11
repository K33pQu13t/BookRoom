using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.ViewModels
{
    public class ErrorResponseViewModel
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public int StatusCode { get; set; } = 500;

        public ErrorResponseViewModel(Exception ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
            StackTrace = ex.StackTrace;
            //if (ex is HttpStatusException statusException)
            //{
            //    StatusCode = (int)statusException.Status;
            //}
        }
    }
}
