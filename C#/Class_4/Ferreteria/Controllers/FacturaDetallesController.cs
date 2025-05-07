using Microsoft.AspNetCore.Mvc;
// using Ferreteria.FacturaDetalle;
// using Ferreteria.Factura;
// using Ferreteria.Item;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/facturas/{facturaId}/detalles")]
public class FacturaDetallesController : ControllerBase
{
    private readonly AppDbContext _context;
    public FacturaDetallesController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> Get(int facturaId)
    {
        var detalles = await _context.FacturaDetalles.Include(d => d.Item).Where(d => d.FacturaId == facturaId).ToListAsync();
        return Ok(detalles);
    }

    [HttpPost]
    public async Task<IActionResult> Post(int facturaId, [FromBody] FacturaDetalle detalle)
    {
        var item = await _context.Items.FindAsync(detalle.ItemId);
        if (item == null || item.StockDisponible < detalle.Cantidad)
            return BadRequest("Stock insuficiente o item no existe");

        detalle.FacturaId = facturaId;
        _context.FacturaDetalles.Add(detalle);
        item.StockDisponible -= detalle.Cantidad;
        await _context.SaveChangesAsync();
        return Ok(detalle);
    }

    [HttpPut("{detalleId}")]
    public async Task<IActionResult> Put(int facturaId, int detalleId, [FromBody] FacturaDetalle detalleActualizado)
    {
        var detalle = await _context.FacturaDetalles.FindAsync(detalleId);
        if (detalle == null || detalle.FacturaId != facturaId) return NotFound();

        var item = await _context.Items.FindAsync(detalle.ItemId);
        if (item == null) return BadRequest();

        int diferencia = detalleActualizado.Cantidad - detalle.Cantidad;
        if (item.StockDisponible < diferencia)
            return BadRequest("Stock insuficiente para modificar la cantidad");

        detalle.Cantidad = detalleActualizado.Cantidad;
        detalle.PrecioUnitario = detalleActualizado.PrecioUnitario;
        item.StockDisponible -= diferencia;

        await _context.SaveChangesAsync();
        return Ok(detalle);
    }

    [HttpDelete("{detalleId}")]
    public async Task<IActionResult> Delete(int facturaId, int detalleId)
    {
        var detalle = await _context.FacturaDetalles.FindAsync(detalleId);
        if (detalle == null || detalle.FacturaId != facturaId) return NotFound();

        var item = await _context.Items.FindAsync(detalle.ItemId);
        if (item != null) item.StockDisponible += detalle.Cantidad;

        _context.FacturaDetalles.Remove(detalle);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}