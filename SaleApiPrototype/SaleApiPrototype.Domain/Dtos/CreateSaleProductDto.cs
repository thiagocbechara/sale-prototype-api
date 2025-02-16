namespace SaleApiPrototype.Domain.Dtos;

public class CreateSaleProductDto
{
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
