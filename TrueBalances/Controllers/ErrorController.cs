using Microsoft.AspNetCore.Mvc;

namespace TrueBalances.Controllers;

public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        switch (statusCode)
        {
            case 404:
                return View("404");
            default:
                return View("404");
                return View("Error");
        }
    }
}