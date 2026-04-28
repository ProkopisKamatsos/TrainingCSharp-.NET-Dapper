using Dapper;
using HotelBooking.Data;
using HotelBooking.Models;

namespace HotelBooking.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public ReviewRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Review>> GetByHotelIdAsync(int hotelId)
        {
            const string sql = @"
                SELECT Id, HotelId, UserId, BookingId, Rating, Comment, CreatedAt
                FROM Reviews
                WHERE HotelId = @HotelId
                ORDER BY CreatedAt DESC";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Review>(sql, new { HotelId = hotelId });
        }

        public async Task<Review?> GetByBookingIdAsync(int bookingId)
        {
            const string sql = @"
                SELECT Id, HotelId, UserId, BookingId, Rating, Comment, CreatedAt
                FROM Reviews
                WHERE BookingId = @BookingId";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Review>(sql, new { BookingId = bookingId });
        }

        public async Task<int> CreateAsync(Review review)
        {
            const string sql = @"
                INSERT INTO Reviews (HotelId, UserId, BookingId, Rating, Comment, CreatedAt)
                VALUES (@HotelId, @UserId, @BookingId, @Rating, @Comment, GETUTCDATE());
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, review);
        }
    }
}