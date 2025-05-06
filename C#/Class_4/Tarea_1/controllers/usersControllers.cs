using Microsoft.AspNetCore.Mvc;
using tarea_1.Utils.CustomFilter;

namespace tarea_1.controllers
{
    [ServiceFilter(typeof(CustomFilter))]
    public class ExampleController : Controller
    {
        
        public IActionResult
        Index()
        {
            return View();
        }
    }
}