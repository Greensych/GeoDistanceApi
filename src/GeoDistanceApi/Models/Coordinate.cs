using System.ComponentModel.DataAnnotations;

namespace GeoDistanceApi.Models;

public class Coordinate
{
    [Range(-90, 90)]
    public double Lat { get; set; }

    [Range(-180, 180)]
    public double Lon { get; set; }
}
