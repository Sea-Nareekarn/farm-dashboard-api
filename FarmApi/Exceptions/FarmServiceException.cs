using System;

namespace FarmApi.Exceptions;

public class FarmServiceException : Exception
{
    public string ErrorCode { get; }

    public FarmServiceException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public FarmServiceException(string errorCode, string message, Exception innerException) 
        : base(message, innerException) { ErrorCode = errorCode; }
}