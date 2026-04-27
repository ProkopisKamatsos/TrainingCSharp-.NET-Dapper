using System;

namespace HotelBooking.Models;

public class RoomType
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public int Capacity { get; set; }

    public List<RoomTypeAmenity> Amenities { get; set; } = new();
}