using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.Enums;
using SaleApiPrototype.Domain.MapperProfile;
using SaleApiPrototype.Domain.MessageBroker;
using SaleApiPrototype.Domain.Models;
using SaleApiPrototype.Domain.Repositories;
using SaleApiPrototype.Domain.Validations;

namespace SaleApiPrototype.Domain.Tests.Configs;

public abstract class BaseTestShould
{
    protected readonly Mock<ISaleRepository> _saleRepositoryMock;
    protected readonly Mock<IQueuePublisher> _queuePublisherMock;
    protected readonly IMapper _mapper;
    protected readonly IValidator<Sale> _validator;

    protected BaseTestShould()
    {
        _saleRepositoryMock = new Mock<ISaleRepository>(MockBehavior.Strict);
        _queuePublisherMock = new Mock<IQueuePublisher>(MockBehavior.Strict);
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new DomainProfile())));
        _validator = new SaleValidator(new SaleProductValidator());
    }

    protected virtual void AssertTokenCancellation<TCommandResult>(TCommandResult result)
        where TCommandResult : Result
    {
        result.Should().Satisfy<TCommandResult>(r =>
        {
            r.IsSuccess.Should().BeFalse();
            r.ErrorMesage.Should().Be("Cancellation was requested");
            r.ErrorType.Should().Be(DomainErrorType.CancellationToken);
        });
    }

    protected virtual void AssertTokenCancellation<TCommandResult, TValue>(TCommandResult result)
        where TCommandResult : Result<TValue>
    {
        result.Should().Satisfy<TCommandResult>(r =>
        {
            r.IsSuccess.Should().BeFalse();
            r.ErrorMesage.Should().Be("Cancellation was requested");
            r.ErrorType.Should().Be(DomainErrorType.CancellationToken);
        });
    }
}
