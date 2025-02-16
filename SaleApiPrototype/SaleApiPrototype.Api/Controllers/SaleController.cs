using MediatR;
using Microsoft.AspNetCore.Mvc;
using SaleApiPrototype.Domain.Dtos;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.Sales.Command.CancelSale;
using SaleApiPrototype.Domain.Sales.Command.CreateSale;
using SaleApiPrototype.Domain.Sales.Queries.GetSaleById;

namespace SaleApiPrototype.Api.Controllers;

[Route("api/sale")]
[ApiController]
public class SaleController(
    ISender mediator,
    ILogger<SaleController> logger)
    : ControllerBase
{
    private static ObjectResult InternalError() =>
        new("An error has occurred")
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

    /// <summary>
    /// Create a new sale
    /// </summary>
    /// <param name="createSaleDto">Sale data</param>
    /// <response code="200">Id for new sale</response>
    /// <response code="400">Sale data has errors</response>
    /// <response code="500">Unexpected error message</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> PostAsync([FromBody] CreateSaleDto createSaleDto)
    {
        try
        {
            if (createSaleDto is null)
            {
                return BadRequest("Sale cannot be null");
            }

            var command = new CreateSaleCommand
            {
                Sale = createSaleDto
            };
            var result = await mediator.Send(command);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.ErrorMesage);

        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An error has occurred at {0}.", nameof(PostAsync));
            return InternalError();
        }
    }

    /// <summary>
    /// Get a sale by Id
    /// </summary>
    /// <param name="sale_code">Sale's Id</param>
    /// <response code="200">Sale data for informed Id</response>
    /// <response code="404">Sale not found for informed Id</response>
    /// <response code="500">Unexpected error message</response>
    [HttpGet("{sale_code}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Sale))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetAsync([FromRoute] long sale_code)
    {
        try
        {
            var query = new GetSaleByIdQuery { SaleCode = sale_code };
            var result = await mediator.Send(query);

            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.ErrorMesage);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An error has occurred at {0}.", nameof(GetAsync));
            return InternalError();
        }
    }

    /// <summary>
    /// Cancel a sale by Id
    /// </summary>
    /// <param name="sale_code">Sale's Id</param>
    /// <response code="204">Sale was cancelled</response>
    /// <response code="400">Error message when couldn't cancel sale</response>
    /// <response code="500">Unexpected error message</response>
    [HttpPut("cancel/{sale_code}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CancelAsync([FromRoute] long sale_code)
    {
        try
        {
            var command = new CancelSaleCommand { SaleCode = sale_code };
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
}
