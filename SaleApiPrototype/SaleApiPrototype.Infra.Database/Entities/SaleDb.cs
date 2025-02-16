namespace SaleApiPrototype.Infra.Database.Entities;

internal class SaleDb
{
    public long SaleNumber { get; set; }
    public DateTime WhenWasMade { get; set; }
    public string Customer { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public string Branch { get; set; } = default!;
    public bool IsCancelled { get; set; }
    public IEnumerable<SaleProductDb> Products { get; set; } = default!;
}
