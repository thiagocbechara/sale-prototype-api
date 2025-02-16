using FluentAssertions;
using Moq;
using SaleApiPrototype.Domain.Dtos;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.MessageBroker.Events;
using SaleApiPrototype.Domain.Models;
using SaleApiPrototype.Domain.Sales.Command.CancelSale;
using SaleApiPrototype.Domain.Sales.Command.CreateSale;
using SaleApiPrototype.Domain.Tests.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleApiPrototype.Domain.Tests.Sales.Commands;

public class CreateSaleCommandHandlerShould : BaseTestShould
{
    private readonly CreateSaleCommandHandler _handler;

    public CreateSaleCommandHandlerShould()
    {
        _handler = new CreateSaleCommandHandler(
            _saleRepositoryMock.Object,
            _validator,
            _mapper,
            _queuePublisherMock.Object);
    }

    [Fact]
    public async Task NotCreateSale_When_SaleWasCancelled()
    {
        //Arrange
        var command = new CreateSaleCommand();

        //Act
        var result = await _handler.Handle(command, new CancellationToken(true));

        //Assert
        AssertTokenCancellation(result);
    }

    [Fact]
    public async Task NotCreateSale_When_SaleIsNotValid()
    {
        //Arrange
        var command = new CreateSaleCommand
        {
            Sale = new CreateSaleDto
            {
                Customer = "Thiago Bechara",
                Products =
                [
                    new CreateSaleProductDto
                    {
                        ProductName = "Test",
                        Quantity = 1,
                        UnitPrice = 10
                    }
                ]
            }
        };

        //Act
        var sale = await _handler.Handle(command, CancellationToken.None);

        //Assert
        sale.Should().Satisfy<Result<long>>(r =>
        {
            r.IsSuccess.Should().BeFalse();
            r.ErrorMesage.Should().StartWith("Sale isn't valid.");
            r.Value.Should().Be(0);
        });

        _saleRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<Sale>()), Times.Never);
        _queuePublisherMock.Verify(p => p.PublishAsync<ISaleCreatedEvent>(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, 0)]
    [InlineData(3, 0)]
    [InlineData(4, 0.1)]
    [InlineData(5, 0.1)]
    [InlineData(6, 0.1)]
    [InlineData(7, 0.1)]
    [InlineData(8, 0.1)]
    [InlineData(9, 0.1)]
    [InlineData(10, 0.2)]
    [InlineData(11, 0.2)]
    [InlineData(12, 0.2)]
    [InlineData(13, 0.2)]
    [InlineData(14, 0.2)]
    [InlineData(15, 0.2)]
    [InlineData(16, 0.2)]
    [InlineData(17, 0.2)]
    [InlineData(18, 0.2)]
    [InlineData(19, 0.2)]
    [InlineData(20, 0.2)]
    public async Task CreateSale(int quantity, decimal expectedDiscount)
    {
        //Arrange
        var unitPrice = 10;
        var command = new CreateSaleCommand
        {
            Sale = new CreateSaleDto
            {
                Branch = "Branch01",
                Customer = "Thiago Bechara",
                Products =
                [
                    new CreateSaleProductDto
                    {
                        ProductName = "Test",
                        Quantity = quantity,
                        UnitPrice = unitPrice
                    }
                ]
            }
        };
        Sale savedSale = default!;
        _saleRepositoryMock
            .Setup(r => r.SaveAsync(It.IsAny<Sale>()))
            .Callback((Sale sale) =>
            {
                sale.SaleNumber = 1;
                savedSale = sale;
            })
            .ReturnsAsync((Sale sale) => sale);
        _queuePublisherMock
            .Setup(p => p.PublishAsync<ISaleCreatedEvent>(It.Is<Sale>(s => s.SaleNumber == savedSale.SaleNumber), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        //Act
        var sale = await _handler.Handle(command, CancellationToken.None);

        //Assert
        sale.Should().Satisfy<Result<long>>(r =>
        {
            r.IsSuccess.Should().BeTrue();
            r.Value.Should().BeGreaterThan(0);
        });
        savedSale.Should().Satisfy<Sale>(s =>
        {
            s.TotalAmount.Should().Be(quantity * unitPrice * (1 - expectedDiscount));
            s.Products.Should().HaveCount(1)
                .And.AllSatisfy(p =>
                {
                    p.TotalAmount.Should().Be(quantity * unitPrice * (1 - expectedDiscount));
                    p.Discount.Should().Be(expectedDiscount);
                });
        });

        _saleRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<Sale>()), Times.Once);
        _queuePublisherMock.Verify(p => p.PublishAsync<ISaleCreatedEvent>(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task NotCreateSale_When_ProductQuantityGreaterThan20()
    {
        //Arrange
        var command = new CreateSaleCommand
        {
            Sale = new CreateSaleDto
            {
                Branch = "Branch01",
                Customer = "Thiago Bechara",
                Products =
                [
                    new CreateSaleProductDto
                    {
                        ProductName = "Test",
                        Quantity = 21,
                        UnitPrice = 10
                    }
                ]
            }
        };

        //Act
        var sale = await _handler.Handle(command, CancellationToken.None);

        //Assert
        sale.Should().Satisfy<Result<long>>(r =>
        {
            r.IsSuccess.Should().BeFalse();
            r.ErrorMesage.Should().StartWith("Sale isn't valid.");
            r.Value.Should().Be(0);
        });

        _saleRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<Sale>()), Times.Never);
        _queuePublisherMock.Verify(p => p.PublishAsync<ISaleCreatedEvent>(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
