using Dapper;
using HotelBooking.Data;
using HotelBooking.Models;

namespace HotelBooking.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public PaymentRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT Id, BookingId, Amount, Method, Status, PaidAt
                FROM Payments
                WHERE Id = @Id";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Payment>(sql, new { Id = id });
        }

        public async Task<Payment?> GetByBookingIdAsync(int bookingId)
        {
            const string sql = @"
                SELECT Id, BookingId, Amount, Method, Status, PaidAt
                FROM Payments
                WHERE BookingId = @BookingId";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Payment>(sql, new { BookingId = bookingId });
        }

        public async Task<int> CreateAsync(Payment payment)
        {
            const string sql = @"
                INSERT INTO Payments (BookingId, Amount, Method, Status, PaidAt)
                VALUES (@BookingId, @Amount, @Method, 'Completed', GETUTCDATE());
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, payment);
        }

        public async Task RefundAsync(int bookingId)
        {
            const string sql = @"
                UPDATE Payments
                SET Status = 'Refunded'
                WHERE BookingId = @BookingId";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, new { BookingId = bookingId });
        }
    }
}