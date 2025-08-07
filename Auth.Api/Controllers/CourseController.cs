using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
