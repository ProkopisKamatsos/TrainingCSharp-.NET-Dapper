using Dapper;
using HotelBooking.Data;
using HotelBooking.Models;

namespace HotelBooking.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public BookingRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT Id, UserId, RoomId, CheckIn, CheckOut, Status, TotalPrice, CreatedAt
                FROM Bookings
                WHERE Id = @Id";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Booking>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId)
        {
            const string sql = @"
                SELECT Id, UserId, RoomId, CheckIn, CheckOut, Status, TotalPrice, CreatedAt
                FROM Bookings
                WHERE UserId = @UserId
                ORDER BY CreatedAt DESC";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Booking>(sql, new { UserId = userId });
        }

        public async Task<bool> HasOverlapAsync(int roomId, DateOnly checkIn, DateOnly checkOut)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM Bookings
                WHERE RoomId = @RoomId
                AND Status NOT IN ('Cancelled')
                AND CheckIn < @CheckOut
                AND CheckOut > @CheckIn";

            using var connection = _connectionFactory.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(sql, new
            {
                RoomId = roomId,
                CheckIn = checkIn.ToDateTime(TimeOnly.MinValue),
                CheckOut = checkOut.ToDateTime(TimeOnly.MinValue)
            });

            return count > 0;
        }

        public async Task<int> CreateAsync(Booking booking)
        {
            const string sql = @"
                INSERT INTO Bookings (UserId, RoomId, CheckIn, CheckOut, Status, TotalPrice, CreatedAt)
                VALUES (@UserId, @RoomId, @CheckIn, @CheckOut, 'Pending', @TotalPrice, GETUTCDATE());
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                booking.UserId,
                booking.RoomId,
                CheckIn = booking.CheckIn.ToDateTime(TimeOnly.MinValue),
                CheckOut = booking.CheckOut.ToDateTime(TimeOnly.MinValue),
                booking.TotalPrice
            });
        }

        public async Task CancelAsync(int id)
        {
            const string sql = @"
                UPDATE Bookings
                SET Status = 'Cancelled'
                WHERE Id = @Id";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
