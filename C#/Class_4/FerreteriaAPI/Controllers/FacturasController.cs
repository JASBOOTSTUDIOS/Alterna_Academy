using Microsoft.AspNetCore.Mvc;



[ApiController]
[Route("api/[controller]")]
public class FacturasController : ControllerBase
{
    private readonly AppDbContext _context;
    public FacturasController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.Facturas.Where(f => !f.EsAnulada).ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var factura = await _context.Facturas.Include(f => f.Detalles).ThenInclude(d => d.Item).FirstOrDefaultAsync(f => f.Id == id);
        return factura == null ? NotFound() : Ok(factura);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Factura factura)
    {
        _context.Facturas.Add(factura);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = factura.Id }, factura);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Factura factura)
    {
        if (id != factura.Id) return BadRequest();
        _context.Entry(factura).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("anular/{id}")]
    public async Task<IActionResult> Anular(int id)
    {
        var factura = await _context.Facturas.FindAsync(id);
        if (factura == null) return NotFound();
        factura.EsAnulada = true;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
