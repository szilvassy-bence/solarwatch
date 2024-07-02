namespace solar_watch_backend.Exceptions;

public class CityIsNotInFavoritesException : Exception
{
    public CityIsNotInFavoritesException(string message)
        : base(message)
    {
    }

    public CityIsNotInFavoritesException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}