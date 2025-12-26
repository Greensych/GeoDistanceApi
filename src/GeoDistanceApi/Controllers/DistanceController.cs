using GeoDistanceApi.Models;
using GeoDistanceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoDistanceApi.Controllers;

/// <summary>
/// API контроллер для расчёта географического расстояния.
/// </summary>
[ApiController]
[Route("api/distance")]
[Produces("application/json")]
public class DistanceController : ControllerBase
{
    private readonly IDistanceService _distanceService;
    private readonly ILogger<DistanceController> _logger;

    public DistanceController(
        IDistanceService distanceService,
        ILogger<DistanceController> logger)
    {
        _distanceService = distanceService ?? throw new ArgumentNullException(nameof(distanceService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Рассчитывает расстояние между двумя географическими точками.
    /// </summary>
    /// <param name="request">Объект с конечными и начальными координатами.</param>
    /// <returns>Ответ с полар расстоянию в километрах.</returns>
    /// <response code="200">Расстояние успешно рассчитано.</response>
    /// <response code="400">Ошибка валидации данных или некорректные координаты.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    [HttpPost("calculate")]
    [ProducesResponseType(typeof(DistanceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public IActionResult CalculateDistance([FromBody] DistanceRequest request)
    {
        try
        {
            if (request == null)
            {
                _logger.LogWarning("Получен null реквест");
                return BadRequest(new ErrorResponse 
                { 
                    Message = "Тело реквеста не может быть пустым" 
                });
            }

            // Модельстет от аннотаций на моделях
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Ошибка валидации: {Errors}", string.Join(", ", errors));
                return BadRequest(new ErrorResponse 
                { 
                    Message = "Координаты должны находиться в следующих диапазонах: Lat[-90,90], Lon[-180,180]"
                });
            }

            if (request.From == null || request.To == null)
            {
                _logger.LogWarning("Одна из координат нуль в реквесте");
                return BadRequest(new ErrorResponse 
                { 
                    Message = "Оба поля 'from' и 'to' обязательны"
                });
            }

            var distanceKm = _distanceService.GetDistanceKm(request.From, request.To);

            return Ok(new DistanceResponse
            {
                DistanceKm = Math.Round(distanceKm, 3),
                From = request.From,
                To = request.To
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Некорректные аргументы");
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неочиданная ошибка");
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ErrorResponse 
                { 
                    Message = "Ошибка сервера при расчёте расстояния",
                    Details = ex.Message
                });
        }
    }
}
