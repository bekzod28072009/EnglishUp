using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }


    }
}
