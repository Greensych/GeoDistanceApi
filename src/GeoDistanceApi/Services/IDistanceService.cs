using GeoDistanceApi.Models;

namespace GeoDistanceApi.Services;

/// <summary>
/// Сервис для расчёта географического расстояния.
/// </summary>
public interface IDistanceService
{
    /// <summary>
    /// Рассчитывает расстояние между двумя координатами в километрах.
    /// Основано на формуле Haversine.
    /// </summary>
    /// <param name="from">Начальная координата.</param>
    /// <param name="to">Конечная координата.</param>
    /// <returns>Расстояние в километрах.</returns>
    double GetDistanceKm(Coordinate from, Coordinate to);
}
