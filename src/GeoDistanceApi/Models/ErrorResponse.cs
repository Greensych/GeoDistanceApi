namespace GeoDistanceApi.Models;

/// <summary>
/// Ответ на ошибку.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Быстрая дескрипция ошибки.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Пдробные детали ошибки (опционально).
    /// </summary>
    public string Details { get; set; }
}
