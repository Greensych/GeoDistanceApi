namespace GeoDistanceApi.Models;

/// <summary>
/// Ответ на реквест рассчёта расстояния.
/// </summary>
public class DistanceResponse
{
    /// <summary>
    /// Расстояние в километрах.
    /// </summary>
    public double DistanceKm { get; set; }

    /// <summary>
    /// Начальная точка.
    /// </summary>
    public Coordinate From { get; set; }

    /// <summary>
    /// Конечная точка.
    /// </summary>
    public Coordinate To { get; set; }
}
