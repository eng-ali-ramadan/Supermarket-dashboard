namespace SupermarketDashboard.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }   // السعر الأساسي للمنتج

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}