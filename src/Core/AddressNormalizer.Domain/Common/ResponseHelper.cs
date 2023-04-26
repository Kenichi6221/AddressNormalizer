namespace AddressNormalizer.Domain.Common;

public static class ResponseHelper
{
    public static Response<T> Success<T>(T value)
    {
        return Response<T>.CreateSuccessObject(value);
    }

    public static Response<T> Fail<T>(string error)
    {
        return Response<T>.CreateFailObject(error);
    }
}