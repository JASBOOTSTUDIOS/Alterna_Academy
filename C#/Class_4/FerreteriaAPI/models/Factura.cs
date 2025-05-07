
using System.ComponentModel.DataAnnotations;
// namespace FerreteriaAPI.Factura{

public class Factura
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public int EmpleadoId { get; set; }
    public bool EsAnulada { get; set; } = false;

    public Empleado Empleado { get; set; }
    public ICollection<FacturaDetalle> Detalles { get; set; }
}

// }