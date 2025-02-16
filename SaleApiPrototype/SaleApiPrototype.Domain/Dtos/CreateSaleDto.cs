namespace SaleApiPrototype.Domain.Dtos;

public class CreateSaleDto
{
    public string Customer { get; set; } = default!;
    public string Branch { get; set; } = default!;
    public IEnumerable<CreateSaleProductDto> Products { get; set; } = default!;
}
