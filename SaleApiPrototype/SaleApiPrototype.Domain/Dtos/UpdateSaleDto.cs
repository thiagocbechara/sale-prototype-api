namespace SaleApiPrototype.Domain.Dtos;

public class UpdateSaleDto
{
    public string Customer { get; set; } = default!;
    public string Branch { get; set; } = default!;
    public IEnumerable<UpdateSaleProductDto> Products { get; set; } = default!;
}
