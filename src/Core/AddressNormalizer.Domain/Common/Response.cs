namespace AddressNormalizer.Domain.Common;

public class Response<T>
{
    public string? Error { get; }
    
    public T? Value { get; }

    public bool SuccessCall { get; } = false;

    private Response(T value)
    {
        Value = value;
        SuccessCall = true;
    }

    private Response(string error)
    {
        Error = error;
        SuccessCall = false;
    }

    public static Response<T> CreateSuccessObject(T value)
    {
        return new Response<T>(value);
    }
    
    public static Response<T> CreateFailObject(string error)
    {
        return new Response<T>(error);
    }
}