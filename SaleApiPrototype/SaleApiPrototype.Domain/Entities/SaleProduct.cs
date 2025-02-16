namespace SaleApiPrototype.Domain.Entities;

public class SaleProduct
{
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }

    public void ApplyDiscountRule()
    {
        Discount = 0M;
        if (Quantity is >= 10 and <= 20)
        {
            Discount = .2M;
        }
        else if (Quantity is >= 4)
        {
            Discount = .1M;
        }

        TotalAmount = Quantity * UnitPrice * (1 - Discount);
    }
}
