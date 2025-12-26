using GeoDistanceApi.Models;
using Microsoft.Extensions.Logging;

namespace GeoDistanceApi.Services;

/// <summary>
/// Реализация сервиса расчёта географического расстояния.
/// </summary>
public sealed class DistanceService : IDistanceService
{
    private readonly ILogger<DistanceService> _logger;

    public DistanceService(ILogger<DistanceService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Рассчитывает расстояние между двумя координатами с использованием формулы Haversine.
    /// </summary>
    /// <param name="from">Начальная точка.</param>
    /// <param name="to">Конечная точка.</param>
    /// <returns>Расстояние в километрах.</returns>
    /// <exception cref="ArgumentNullException">-если один из аргументов пуст.</exception>
    public double GetDistanceKm(Coordinate from, Coordinate to)
    {
        if (from == null)
            throw new ArgumentNullException(nameof(from), "Начальная координата не может быть нулл");
        if (to == null)
            throw new ArgumentNullException(nameof(to), "Конечная координата не может быть нулл");

        if (!from.IsValid() || !to.IsValid())
        {
            _logger.LogWarning(
                "Попытка рассчитать расстояние с невалидными координатами. From: ({FromLat}, {FromLon}), To: ({ToLat}, {ToLon})",
                from.Latitude, from.Longitude, to.Latitude, to.Longitude);
            throw new ArgumentException("Одна из координат не соответствует диапазону данных");
        }

        try
        {
            var distance = from.GetDistanceTo(to);
            _logger.LogDebug(
                "Расстояние рассчитано: {Distance} км",
                distance);
            return distance;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Ошибка при расчёте расстояния");
            throw;
        }
    }
}
