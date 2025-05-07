using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class EmpleadosController : ControllerBase
{
    private readonly AppDbContext _context;
    public EmpleadosController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.Empleados.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var empleado = await _context.Empleados.FindAsync(id);
        return empleado == null ? NotFound() : Ok(empleado);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Empleado emp)
    {
        _context.Empleados.Add(emp);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = emp.Id }, emp);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Empleado emp)
    {
        if (id != emp.Id) return BadRequest();
        _context.Entry(emp).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var emp = await _context.Empleados.FindAsync(id);
        if (emp == null) return NotFound();
        _context.Empleados.Remove(emp);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}