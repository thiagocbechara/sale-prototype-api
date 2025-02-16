namespace SaleApiPrototype.Infra.Database.Entities;

internal class SaleProductDb
{
    public long Id { get; set; }
    public long SaleNumber { get; set; }
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }

    public SaleDb Sale { get; set; } = default!;
}
