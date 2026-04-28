using Dapper;
using HotelBooking.Data;
using HotelBooking.Models;

namespace HotelBooking.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public RoomRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Room>> GetByHotelIdAsync(int hotelId)
        {
            const string sql = @"
                SELECT Id, HotelId, RoomTypeId, RoomNumber, Floor, IsActive
                FROM Rooms
                WHERE HotelId = @HotelId AND IsActive = 1
                ORDER BY Floor, RoomNumber";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Room>(sql, new { HotelId = hotelId });
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT Id, HotelId, RoomTypeId, RoomNumber, Floor, IsActive
                FROM Rooms
                WHERE Id = @Id AND IsActive = 1";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Room>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(int hotelId, DateOnly checkIn, DateOnly checkOut)
        {
            const string sql = @"
        SELECT R.Id, R.HotelId, R.RoomTypeId, R.RoomNumber, R.Floor, R.IsActive
        FROM Rooms R
        WHERE R.HotelId = @HotelId
        AND R.IsActive = 1
        AND R.Id NOT IN (
            SELECT B.RoomId
            FROM Bookings B
            WHERE B.Status NOT IN ('Cancelled')
            AND B.CheckIn < @CheckOut
            AND B.CheckOut > @CheckIn
        )
        ORDER BY R.Floor, R.RoomNumber";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Room>(sql, new
            {
                HotelId = hotelId,
                CheckIn = checkIn.ToDateTime(TimeOnly.MinValue),
                CheckOut = checkOut.ToDateTime(TimeOnly.MinValue)
            });
        }
        public async Task<int> CreateAsync(Room room)
        {
            const string sql = @"
                INSERT INTO Rooms (HotelId, RoomTypeId, RoomNumber, Floor, IsActive)
                VALUES (@HotelId, @RoomTypeId, @RoomNumber, @Floor, 1);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, room);
        }

        public async Task UpdateAsync(Room room)
        {
            const string sql = @"
                UPDATE Rooms
                SET RoomTypeId = @RoomTypeId,
                    RoomNumber = @RoomNumber,
                    Floor = @Floor
                WHERE Id = @Id AND IsActive = 1";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, room);
        }

        public async Task SoftDeleteAsync(int id)
        {
            const string sql = @"
                UPDATE Rooms
                SET IsActive = 0
                WHERE Id = @Id";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
