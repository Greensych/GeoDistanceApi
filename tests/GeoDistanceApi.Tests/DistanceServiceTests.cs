using GeoDistanceApi.Models;
using GeoDistanceApi.Services;
using Xunit;

namespace GeoDistanceApi.Tests;

public class DistanceServiceTests
{
    private readonly DistanceService _service;

    public DistanceServiceTests()
    {
        _service = new DistanceService();
    }

    [Fact]
    public void SamePoint_ShouldReturnZero()
    {
        var point = new Coordinate { Lat = 55.7558, Lon = 37.6173 };

        var distance = _service.GetDistanceKm(point, point);

        Assert.Equal(0, distance, 1);
    }

    [Fact]
    public void MoscowToSpb_ReturnsCorrectDistance()
    {
        var moscow = new Coordinate { Lat = 55.7558, Lon = 37.6173 };
        var spb = new Coordinate { Lat = 59.9311, Lon = 30.3609 };

        var distance = _service.GetDistanceKm(moscow, spb);

        // ожидаем примерно 633 км
        Assert.True(distance > 630 && distance < 640);
    }

    [Fact]
    public void DistanceIsSymmetric()
    {
        var p1 = new Coordinate { Lat = 55.7558, Lon = 37.6173 };
        var p2 = new Coordinate { Lat = 59.9311, Lon = 30.3609 };

        var d1 = _service.GetDistanceKm(p1, p2);
        var d2 = _service.GetDistanceKm(p2, p1);

        Assert.Equal(d1, d2, 5);
    }
}
