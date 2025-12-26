using System.ComponentModel.DataAnnotations;

namespace GeoDistanceApi.Models;

/// <summary>
/// Географическая координата (широта и долгота).
/// </summary>
public class Coordinate
{
    /// <summary>
    /// Широта в градусах. Диапазон: [-90, 90].
    /// </summary>
    [Range(-90, 90, ErrorMessage = "Широта должна быть в диапазоне [-90, 90]")]
    public double Latitude { get; set; }

    /// <summary>
    /// Долгота в градусах. Диапазон: [-180, 180].
    /// </summary>
    [Range(-180, 180, ErrorMessage = "Долгота должна быть в диапазоне [-180, 180]")]
    public double Longitude { get; set; }

    /// <summary>
    /// Проверяет, является ли координата корректной.
    /// </summary>
    public bool IsValid() => 
        Latitude >= -90 && Latitude <= 90 && 
        Longitude >= -180 && Longitude <= 180;

    /// <summary>
    /// Получить расстояние до другой координаты с использованием формулы Haversine.
    /// </summary>
    public double GetDistanceTo(Coordinate other)
    {
        const double earthRadiusKm = 6371.0;

        var lat1Rad = ToRadians(Latitude);
        var lat2Rad = ToRadians(other.Latitude);
        var latDiffRad = ToRadians(other.Latitude - Latitude);
        var lonDiffRad = ToRadians(other.Longitude - Longitude);

        // Формула Haversine
        var a = Math.Sin(latDiffRad / 2) * Math.Sin(latDiffRad / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(lonDiffRad / 2) * Math.Sin(lonDiffRad / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return earthRadiusKm * c;
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
}
