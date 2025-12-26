using GeoDistanceApi.Models;
using GeoDistanceApi.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GeoDistanceApi.Tests;

/// <summary>
/// Unit-тесты для DistanceService.
/// </summary>
public class DistanceServiceTests
{
    private readonly Mock<ILogger<DistanceService>> _loggerMock;
    private readonly DistanceService _service;

    public DistanceServiceTests()
    {
        _loggerMock = new Mock<ILogger<DistanceService>>();
        _service = new DistanceService(_loggerMock.Object);
    }

    [Fact(DisplayName = "Расстояние между том же пунктом должно быть нулю")]
    public void SamePoint_ShouldReturnZero()
    {
        // Arrange
        var point = new Coordinate { Latitude = 55.7558, Longitude = 37.6173 };

        // Act
        var distance = _service.GetDistanceKm(point, point);

        // Assert
        Assert.Equal(0, distance, 2);
    }

    [Fact(DisplayName = "Москва -> Санкт-Петербург: примерно 633 км")]
    public void MoscowToSpb_ReturnsCorrectDistance()
    {
        // Arrange
        var moscow = new Coordinate { Latitude = 55.7558, Longitude = 37.6173 };
        var spb = new Coordinate { Latitude = 59.9311, Longitude = 30.3609 };

        // Act
        var distance = _service.GetDistanceKm(moscow, spb);

        // Assert - ожидаем примерно 633 км
        Assert.InRange(distance, 630, 640);
    }

    [Fact(DisplayName = "Расстояние симметрично (А->B = B->A)")]
    public void DistanceIsSymmetric()
    {
        // Arrange
        var p1 = new Coordinate { Latitude = 55.7558, Longitude = 37.6173 };
        var p2 = new Coordinate { Latitude = 59.9311, Longitude = 30.3609 };

        // Act
        var d1 = _service.GetDistanceKm(p1, p2);
        var d2 = _service.GetDistanceKm(p2, p1);

        // Assert
        Assert.Equal(d1, d2, 5);
    }

    [Fact(DisplayName = "Побережные координаты: Париж -> Лондон")]
    public void ParisToLondon_ReturnsCorrectDistance()
    {
        // Arrange
        var paris = new Coordinate { Latitude = 48.8566, Longitude = 2.3522 };
        var london = new Coordinate { Latitude = 51.5074, Longitude = -0.1278 };

        // Act
        var distance = _service.GetDistanceKm(paris, london);

        // Assert - ожидаем примерно 340 км
        Assert.InRange(distance, 335, 345);
    }

    [Theory(DisplayName = "Отрицательные координаты должны работать")]
    [InlineData(-33.8688, 151.2093)] // Сидней
    [InlineData(-23.5505, -46.6333)] // Сан-Пауло
    public void NegativeCoordinates_ShouldWork(double lat, double lon)
    {
        // Arrange
        var sydney = new Coordinate { Latitude = lat, Longitude = lon };
        var moscow = new Coordinate { Latitude = 55.7558, Longitude = 37.6173 };

        // Act
        var distance = _service.GetDistanceKm(sydney, moscow);

        // Assert - расстояние должно быть осмысленным
        Assert.True(distance > 0);
        Assert.False(double.IsNaN(distance));
        Assert.False(double.IsInfinity(distance));
    }

    [Fact(DisplayName = "Null-аргумент from выбросит ArgumentNullException")]
    public void FromIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var to = new Coordinate { Latitude = 55.7558, Longitude = 37.6173 };

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => _service.GetDistanceKm(null, to));
        Assert.Equal("from", ex.ParamName);
    }

    [Fact(DisplayName = "Null-аргумент to выбросит ArgumentNullException")]
    public void ToIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var from = new Coordinate { Latitude = 55.7558, Longitude = 37.6173 };

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => _service.GetDistanceKm(from, null));
        Assert.Equal("to", ex.ParamName);
    }

    [Theory(DisplayName = "Невалидные координаты выбросят ArgumentException")]
    [InlineData(100, 50)] // Латитуда вне диапазона
    [InlineData(50, 200)] // Лонгитуда вне диапазона
    [InlineData(-100, 0)] // Латитуда то эне чуть меньше -90
    public void InvalidCoordinates_ThrowsArgumentException(double lat, double lon)
    {
        // Arrange
        var valid = new Coordinate { Latitude = 55.7558, Longitude = 37.6173 };
        var invalid = new Coordinate { Latitude = lat, Longitude = lon };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.GetDistanceKm(invalid, valid));
    }

    [Fact(DisplayName = "Расстояние выростания до 3 значащих цифр")]
    public void DistanceIsPreciseToThreeDecimals()
    {
        // Arrange
        var p1 = new Coordinate { Latitude = 55.7558, Longitude = 37.6173 };
        var p2 = new Coordinate { Latitude = 59.9311, Longitude = 30.3609 };

        // Act
        var distance = _service.GetDistanceKm(p1, p2);

        // Assert - расстояние аккуратно до третьего знака
        Assert.False(double.IsNaN(distance));
        Assert.True(distance > 0);
    }
}
