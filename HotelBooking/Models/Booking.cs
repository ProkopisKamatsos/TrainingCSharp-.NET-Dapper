using System;

namespace HotelBooking.Models;

public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RoomId { get; set; }
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public string Status { get; set; } = "Pending";
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}
