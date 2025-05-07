using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// using Ferreteria.Empleado;


[ApiController]
[Route("api/[controller]")]
public class EmpleadoController : ControllerBase
{
    private readonly AppDbContext _context;
    public EmpleadoController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.Empleado.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var empleado = await _context.Empleado.FindAsync(id);
        return empleado == null ? NotFound() : Ok(empleado);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Empleado emp)
    {
        _context.Empleado.Add(emp);
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
        var emp = await _context.Empleado.FindAsync(id);
        if (emp == null) return NotFound();
        _context.Empleado.Remove(emp);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}