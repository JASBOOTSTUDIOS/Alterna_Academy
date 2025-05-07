using System.ComponentModel.DataAnnotations;
// namespace FerreteriaAPI.Factura{}
public class Item
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public decimal Precio { get; set; }
    public int StockDisponible { get; set; }
}
