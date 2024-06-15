using System.Threading.Tasks;

namespace solar_watch_backend.Services.LatLngProvider;

public interface ILatLngProvider
{
    Task<string> GetLatLng(string city);
}