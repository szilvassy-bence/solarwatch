using solar_watch_backend.Models;

namespace solar_watch_backend.Services.LatLngProvider;

public interface ILatLngJsonProcessor
{
    City Process(string json);
}