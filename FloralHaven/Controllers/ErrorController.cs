using Microsoft.AspNetCore.Mvc;

namespace Floral_Haven.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        [Route("404")]
        public IActionResult PageNotFound()
        {
            string originalPath = "unknown";
            if (HttpContext.Items.ContainsKey("originalPath"))
            {
                originalPath = HttpContext.Items["originalPath"] as string ?? string.Empty;
            }
            return View();
        }
    }
}
