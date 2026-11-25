namespace SupermarketDashboard.Models;

public class Sale
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal UnitPrice { get; set; }   // السعر اللي اتباع بيه فعلاً (ممكن يختلف عن سعر المنتج)

    public DateTime SoldAt { get; set; } = DateTime.Now;

    // الإيراد = الكمية × سعر البيع (مش بيتخزن في الداتابيز)
    public decimal Total => Quantity * UnitPrice;
}