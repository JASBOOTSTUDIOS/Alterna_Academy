using Microsoft.AspNetCore.Mvc.Filters;

namespace tarea_1.Utils.CustomFilter
{

public class CustomFilter : IActionFilter
{

    public void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine("Entro Aqui");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine("Entro Aqui 2");
    }

    // public void OnActionExecuted(ActionExecutedContext context)
    // {
    //     throw new NotImplementedException();
    // }
}
}