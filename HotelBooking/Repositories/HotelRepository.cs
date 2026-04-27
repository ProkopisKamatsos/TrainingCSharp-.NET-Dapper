using Dapper;
using HotelBooking.Data;
using HotelBooking.Models;

namespace HotelBooking.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public HotelRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync(int page, int pageSize, string? city, string? country, int? starRating)
        {
            var sql = @"
                SELECT H.Id, H.OwnerId, H.Name, H.Description, H.Address, 
                       H.City, H.Country, H.StarRating, H.IsActive, H.CreatedAt
                FROM Hotels H
                WHERE H.IsActive = 1
                AND (@City IS NULL OR H.City = @City)
                AND (@Country IS NULL OR H.Country = @Country)
                AND (@StarRating IS NULL OR H.StarRating = @StarRating)
                ORDER BY H.CreatedAt DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Hotel>(sql, new
            {
                City = city,
                Country = country,
                StarRating = starRating,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        }

        public async Task<Hotel?> GetByIdAsync(int id)
        {
            const string hotelSql = @"
                SELECT Id, OwnerId, Name, Description, Address, 
                       City, Country, StarRating, IsActive, CreatedAt
                FROM Hotels
                WHERE Id = @Id AND IsActive = 1";

            const string amenitiesSql = @"
                SELECT Id, HotelId, Name
                FROM HotelAmenities
                WHERE HotelId = @HotelId";

            using var connection = _connectionFactory.CreateConnection();

            var hotel = await connection.QuerySingleOrDefaultAsync<Hotel>(hotelSql, new { Id = id });

            if (hotel == null) return null;

            var amenities = await connection.QueryAsync<HotelAmenity>(amenitiesSql, new { HotelId = id });
            hotel.Amenities = amenities.ToList();

            return hotel;
        }

        public async Task<IEnumerable<Hotel>> GetByOwnerIdAsync(int ownerId)
        {
            const string sql = @"
                SELECT Id, OwnerId, Name, Description, Address,
                       City, Country, StarRating, IsActive, CreatedAt
                FROM Hotels
                WHERE OwnerId = @OwnerId AND IsActive = 1
                ORDER BY CreatedAt DESC";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Hotel>(sql, new { OwnerId = ownerId });
        }

        public async Task<int> CreateAsync(Hotel hotel)
        {
            const string hotelSql = @"
        INSERT INTO Hotels (OwnerId, Name, Description, Address, City, Country, StarRating, IsActive, CreatedAt)
        VALUES (@OwnerId, @Name, @Description, @Address, @City, @Country, @StarRating, 1, GETUTCDATE());
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            const string amenitySql = @"
        INSERT INTO HotelAmenities (HotelId, Name)
        VALUES (@HotelId, @Name)";

            using var connection = _connectionFactory.CreateConnection();

            var hotelId = await connection.ExecuteScalarAsync<int>(hotelSql, hotel);

            if (hotel.Amenities.Any())
            {
                var amenities = hotel.Amenities.Select(a => new { HotelId = hotelId, a.Name });
                await connection.ExecuteAsync(amenitySql, amenities);
            }

            return hotelId;
        }

        public async Task UpdateAsync(Hotel hotel)
        {
            const string sql = @"
                UPDATE Hotels
                SET Name = @Name,
                    Description = @Description,
                    Address = @Address,
                    City = @City,
                    Country = @Country,
                    StarRating = @StarRating
                WHERE Id = @Id AND IsActive = 1";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, hotel);
        }

        public async Task SoftDeleteAsync(int id)
        {
            const string sql = @"
                UPDATE Hotels
                SET IsActive = 0
                WHERE Id = @Id";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}