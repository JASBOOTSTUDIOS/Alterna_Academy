using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;



public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Empleado> Empleado { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Factura> Facturas { get; set; }
    public DbSet<FacturaDetalle> FacturaDetalles { get; set; }
}
