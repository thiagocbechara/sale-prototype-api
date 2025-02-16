namespace SaleApiPrototype.Domain.Entities;

public class Sale
{
    public Sale()
    {
        IsCancelled = false;
    }

    public long SaleNumber { get; set; }
    public DateTime WhenWasMade { get; set; }
    public string Customer { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public string Branch { get; set; } = default!;
    public bool IsCancelled { get; set; }
    public IList<SaleProduct> Products { get; set; } = default!;

    public void ApplyDiscountRule()
    {
        TotalAmount = 0;
        for (var i = 0; i < Products.Count; i++)
        {
            Products[i].ApplyDiscountRule();
            TotalAmount += Products[i].TotalAmount;
        }
    }
}
