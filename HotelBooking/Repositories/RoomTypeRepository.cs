using Dapper;
using HotelBooking.Data;
using HotelBooking.Models;

namespace HotelBooking.Repositories
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public RoomTypeRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<RoomType>> GetByHotelIdAsync(int hotelId)
        {
            const string roomTypeSql = @"
                SELECT Id, HotelId, Name, Description, BasePrice, Capacity
                FROM RoomTypes
                WHERE HotelId = @HotelId";

            const string amenitiesSql = @"
                SELECT Id, RoomTypeId, Name
                FROM RoomTypeAmenities
                WHERE RoomTypeId IN @RoomTypeIds";

            using var connection = _connectionFactory.CreateConnection();

            var roomTypes = (await connection.QueryAsync<RoomType>(roomTypeSql, new { HotelId = hotelId })).ToList();

            if (roomTypes.Any())
            {
                var roomTypeIds = roomTypes.Select(rt => rt.Id).ToList();
                var amenities = await connection.QueryAsync<RoomTypeAmenity>(amenitiesSql, new { RoomTypeIds = roomTypeIds });

                // Κάνουμε manually map τα amenities στο αντίστοιχο RoomType
                var amenitiesLookup = amenities.GroupBy(a => a.RoomTypeId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var roomType in roomTypes)
                {
                    if (amenitiesLookup.TryGetValue(roomType.Id, out var roomAmenities))
                        roomType.Amenities = roomAmenities;
                }
            }

            return roomTypes;
        }

        public async Task<RoomType?> GetByIdAsync(int id)
        {
            const string roomTypeSql = @"
                SELECT Id, HotelId, Name, Description, BasePrice, Capacity
                FROM RoomTypes
                WHERE Id = @Id";

            const string amenitiesSql = @"
                SELECT Id, RoomTypeId, Name
                FROM RoomTypeAmenities
                WHERE RoomTypeId = @RoomTypeId";

            using var connection = _connectionFactory.CreateConnection();

            var roomType = await connection.QuerySingleOrDefaultAsync<RoomType>(roomTypeSql, new { Id = id });

            if (roomType == null) return null;

            var amenities = await connection.QueryAsync<RoomTypeAmenity>(amenitiesSql, new { RoomTypeId = id });
            roomType.Amenities = amenities.ToList();

            return roomType;
        }

        public async Task<int> CreateAsync(RoomType roomType)
        {
            const string roomTypeSql = @"
                INSERT INTO RoomTypes (HotelId, Name, Description, BasePrice, Capacity)
                VALUES (@HotelId, @Name, @Description, @BasePrice, @Capacity);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            const string amenitySql = @"
                INSERT INTO RoomTypeAmenities (RoomTypeId, Name)
                VALUES (@RoomTypeId, @Name)";

            using var connection = _connectionFactory.CreateConnection();

            var roomTypeId = await connection.ExecuteScalarAsync<int>(roomTypeSql, roomType);

            if (roomType.Amenities.Any())
            {
                var amenities = roomType.Amenities.Select(a => new { RoomTypeId = roomTypeId, a.Name });
                await connection.ExecuteAsync(amenitySql, amenities);
            }

            return roomTypeId;
        }

        public async Task UpdateAsync(RoomType roomType)
        {
            const string roomTypeSql = @"
                UPDATE RoomTypes
                SET Name = @Name,
                    Description = @Description,
                    BasePrice = @BasePrice,
                    Capacity = @Capacity
                WHERE Id = @Id";

            const string deleteAmenitiesSql = @"
                DELETE FROM RoomTypeAmenities
                WHERE RoomTypeId = @RoomTypeId";

            const string insertAmenitySql = @"
                INSERT INTO RoomTypeAmenities (RoomTypeId, Name)
                VALUES (@RoomTypeId, @Name)";

            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(roomTypeSql, roomType);

            // Διαγράφουμε τα παλιά amenities και βάζουμε τα νέα
            await connection.ExecuteAsync(deleteAmenitiesSql, new { RoomTypeId = roomType.Id });

            if (roomType.Amenities.Any())
            {
                var amenities = roomType.Amenities.Select(a => new { RoomTypeId = roomType.Id, a.Name });
                await connection.ExecuteAsync(insertAmenitySql, amenities);
            }
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = @"
                DELETE FROM RoomTypes
                WHERE Id = @Id";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}