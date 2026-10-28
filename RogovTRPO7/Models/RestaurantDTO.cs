using System;
using System.Collections.Generic;

namespace RogovTRPO7.Models;

public partial class RestaurantDTO
{
    public int Id { get; set; } 

    public string? Name { get; set; } = null;

    public string? Cuisine { get; set; } = null;

    public decimal? Rating { get; set; } = null;

    public decimal? AverageCheck { get; set; } = null;

    public bool? IsOpen { get; set; } = null;
}
