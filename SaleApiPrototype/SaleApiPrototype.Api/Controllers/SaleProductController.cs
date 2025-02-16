using MediatR;
using Microsoft.AspNetCore.Mvc;
using SaleApiPrototype.Domain.SaleProducts.CancelSaleProduct;

namespace SaleApiPrototype.Api.Controllers;

[Route("api/sale_product")]
[ApiController]
public class SaleProductController(
    ISender mediator,
    ILogger<SaleProductController> logger)
    : ControllerBase
{
    /// <summary>
    /// Cancel a sale product
    /// </summary>
    /// <param name="sale_code">Sale's Id</param>
    /// <param name="product_name">Product name</param>
    /// <response code="204">Product was cancelled</response>
    /// <response code="400">Error message when couldn't cancel product</response>
    /// <response code="500">Unexpected error message</response>
    [HttpPut("cancel/{sale_code}/{product_name}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CancelAsync([FromRoute] long sale_code, [FromRoute] string product_name)
    {
        try
        {
            var command = new CancelSaleProductCommand
            {
                SaleNumber = sale_code,
                ProductName = product_name
            };
            var result = await mediator.Send(command);

            return result.IsSuccess
                ? NoContent()
                : BadRequest(result.ErrorMesage);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An error has occurred at {0}.", nameof(CancelAsync));
            return InternalError();
        }
    }

    private static ObjectResult InternalError() =>
        new("An error has occurred")
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
}
