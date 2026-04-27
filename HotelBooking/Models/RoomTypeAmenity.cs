using System;

namespace HotelBooking.Models;

public class RoomTypeAmenity
{
    public int Id { get; set; }
    public int RoomTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
}
