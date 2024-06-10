using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TP_FINAL_GRUPO_C.Models;

namespace TP_FINAL_GRUPO_C.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("/Error")]
        public IActionResult Error(string errorMessage)
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = HttpContext?.TraceIdentifier ?? "<RequestId no disponible>",
                ErrorMessage = errorMessage
            };
            return View(errorViewModel);
        }
    }

}
