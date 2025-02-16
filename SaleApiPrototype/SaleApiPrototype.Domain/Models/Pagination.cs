namespace SaleApiPrototype.Domain.Models;

public class Pagination<TEntity>
{
    public int CurrentPage { get; set; }
    public int ResultsPerPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalResults { get; set; }

    public IEnumerable<TEntity> Results { get; set; } = default!;
}
