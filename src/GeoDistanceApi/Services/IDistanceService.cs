using GeoDistanceApi.Models;

namespace GeoDistanceApi.Services;

public interface IDistanceService
{
    double GetDistanceKm(Coordinate from, Coordinate to);
}
