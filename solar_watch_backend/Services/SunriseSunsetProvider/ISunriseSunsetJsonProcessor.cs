using System;
using solar_watch_backend.Models;

namespace solar_watch_backend.Services.SunriseSunsetProvider;

public interface ISunriseSunsetJsonProcessor
{
    SunriseSunset Process(string json, DateTime date);
}