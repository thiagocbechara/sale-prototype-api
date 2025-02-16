using SaleApiPrototype.Domain.Enums;

namespace SaleApiPrototype.Domain.Models;

public class Result
{
    public bool IsSuccess { get; set; }
    public string? ErrorMesage { get; set; }
    public DomainErrorType ErrorType { get; set; }

    public static Result Success() =>
        new()
        {
            IsSuccess = true
        };

    public static Result Error(string errorMesage, DomainErrorType type) =>
       new()
       {
           IsSuccess = false,
           ErrorMesage = errorMesage,
           ErrorType = type
       };

    public static Result<TValue> Error<TValue>(string errorMesage, DomainErrorType type) =>
        new()
        {
            IsSuccess = false,
            ErrorMesage = errorMesage,
            ErrorType = type
        };

    public static Result<TValue> NotFoundError<TValue>() =>
        new()
        {
            IsSuccess = false,
            ErrorMesage = $"{typeof(TValue).Name} not found.",
            ErrorType = DomainErrorType.NotFound
        };

    public static Result NotFoundError(string errorMessage) =>
        Error(errorMessage, DomainErrorType.NotFound);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new()
        {
            Value = value,
            IsSuccess = true
        };
}

public class Result<TValue> : Result
{
    public TValue Value { get; set; } = default!;
}
