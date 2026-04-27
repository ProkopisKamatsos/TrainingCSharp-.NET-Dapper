using System;

namespace HotelBooking.Models;

public class HotelAmenity
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
}
