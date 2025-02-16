namespace SaleApiPrototype.Domain.Dtos;

public class UpdateSaleProductDto
{
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
