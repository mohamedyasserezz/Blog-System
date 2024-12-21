﻿namespace BlogSystem.Shared.Abstractions
{
    public class Result
    {
        public Result(bool isSuccess, Error error)
        {
            if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; } = default!;

        public static Result Success() => new Result(true, Error.None);
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
        public static Result Failer(Error error) => new(false, error);
        public static Result<TValue> Failer<TValue>(Error error) => new(default, false, error);
    }
    public class Result<TValue>(TValue? value, bool isSuccess, Error error) : Result(isSuccess, error)
    {
        private readonly TValue? _value = value;

        public TValue Value => IsSuccess ?
            _value! :
            throw new InvalidOperationException("Faliure results cannot have value");
    }
}