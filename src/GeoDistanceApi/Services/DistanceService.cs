using GeoDistanceApi.Models;

namespace GeoDistanceApi.Services;

public class DistanceService : IDistanceService
{
    private const double R = 6371; // радиус Земли в км

    public double GetDistanceKm(Coordinate from, Coordinate to)
    {
        var lat1Rad = ToRad(from.Lat);
        var lat2Rad = ToRad(to.Lat);
        var latDiff = ToRad(to.Lat - from.Lat);
        var lonDiff = ToRad(to.Lon - from.Lon);

        // формула Haversine
        var a = Math.Sin(latDiff / 2) * Math.Sin(latDiff / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(lonDiff / 2) * Math.Sin(lonDiff / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private double ToRad(double deg) => deg * Math.PI / 180.0;
}
