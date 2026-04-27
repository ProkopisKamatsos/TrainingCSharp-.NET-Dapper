using System;

namespace HotelBooking.Models;

public class Room
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public int RoomTypeId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int Floor { get; set; }
    public bool IsActive { get; set; } = true;
}