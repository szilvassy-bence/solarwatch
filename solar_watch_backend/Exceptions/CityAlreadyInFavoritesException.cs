namespace solar_watch_backend.Exceptions;

public class CityAlreadyInFavoritesException : Exception
{
    public CityAlreadyInFavoritesException(string message)
        : base(message)
    {
    }

    public CityAlreadyInFavoritesException(string message, Exception inner)
        : base(message, inner)
    {
    }
}