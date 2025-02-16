using FluentAssertions;
using Moq;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.Enums;
using SaleApiPrototype.Domain.MessageBroker.Events;
using SaleApiPrototype.Domain.Models;
using SaleApiPrototype.Domain.Sales.Command.CancelSale;
using SaleApiPrototype.Domain.Tests.Configs;

namespace SaleApiPrototype.Domain.Tests.Sales.Commands;

public class CancelSaleCommandHandlerShould : BaseTestShould
{
    private readonly CancelSaleCommandHandler _handler;

    public CancelSaleCommandHandlerShould()
    {
        _handler = new CancelSaleCommandHandler(_saleRepositoryMock.Object, _queuePublisherMock.Object);
    }

    [Fact]
    public async Task CancelSale()
    {
        //Arrange
        var saleCode = 1;
        var command = new CancelSaleCommand { SaleCode = saleCode };
        _saleRepositoryMock
            .Setup(r => r.GetByCodeAsync(saleCode))
            .ReturnsAsync(new Sale { SaleNumber = saleCode, IsCancelled = false });
        Sale saleCallback = default!;
        _saleRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Sale>()))
            .Callback((Sale sale) => { saleCallback = sale; })
            .ReturnsAsync(saleCallback);
        _queuePublisherMock
            .Setup(p => p.PublishAsync<ISaleCancelledEvent>(It.Is<Sale>(s => s.SaleNumber == saleCode), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.Should().Satisfy<Result>(r =>
        {
            r.IsSuccess.Should().BeTrue();
            r.ErrorMesage.Should().BeNullOrWhiteSpace();
        });
        saleCallback.Should()
            .NotBeNull()
            .And.Satisfy<Sale>(s =>
            {
                s.SaleNumber.Should().Be(saleCode);
                s.IsCancelled.Should().BeTrue();
            });

        _saleRepositoryMock.Verify(r => r.GetByCodeAsync(It.IsAny<long>()), Times.Once);
        _saleRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Sale>()), Times.Once);
        _queuePublisherMock.Verify(p => p.PublishAsync<ISaleCancelledEvent>(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task NotDeleteSale_When_Cancellation_WasRequested()
    {
        //Arrange
        var command = new CancelSaleCommand();

        //Act
        var result = await _handler.Handle(command, new CancellationToken(true));

        //Assert
        AssertTokenCancellation(result);
    }

    [Fact]
    public async Task NotDeleteSale_When_SaleNotFound()
    {
        //Arrange
        var saleCode = 1;
        var command = new CancelSaleCommand { SaleCode = saleCode };
        _saleRepositoryMock
            .Setup(r => r.GetByCodeAsync(saleCode))
            .ReturnsAsync((Sale?) null);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.Should().Satisfy<Result>(r =>
        {
            r.IsSuccess.Should().BeFalse();
            r.ErrorMesage.Should().Be("Sale not found.");
            r.ErrorType.Should().Be(DomainErrorType.NotFound);
        });
    }

    [Fact]
    public async Task NotDeleteSale_When_SaleWasCancelled()
    {
        //Arrange
        var saleCode = 1;
        var command = new CancelSaleCommand { SaleCode = saleCode };
        _saleRepositoryMock
            .Setup(r => r.GetByCodeAsync(saleCode))
            .ReturnsAsync(new Sale { SaleNumber = saleCode, IsCancelled = true });

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.Should().Satisfy<Result>(r =>
        {
            r.IsSuccess.Should().BeFalse();
            r.ErrorMesage.Should().Be("Cannot cancel a cancelled sale");
            r.ErrorType.Should().Be(DomainErrorType.NotValid);
        });
    }    
}
