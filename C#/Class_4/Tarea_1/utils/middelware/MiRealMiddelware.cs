public class MiRealMiddelware
{
    private readonly RequestDelegate _next;

    public MiRealMiddelware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine("La peticion entro");
        await _next(context);

        Console.WriteLine("La peticion Salio");
    }
}