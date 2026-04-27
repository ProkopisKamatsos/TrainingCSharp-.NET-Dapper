CREATE TABLE Users (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    FullName    NVARCHAR(100)   NOT NULL,
    Email       NVARCHAR(150)   NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255)  NOT NULL,
    Role        NVARCHAR(20)    NOT NULL CHECK (Role IN ('Admin', 'HotelManager', 'Guest')),
    IsActive    BIT             NOT NULL DEFAULT 1,
    CreatedAt   DATETIME2       NOT NULL DEFAULT GETUTCDATE()
);

-- =============================================
-- HOTELS
-- =============================================
CREATE TABLE Hotels (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    OwnerId     INT             NOT NULL REFERENCES Users(Id),
    Name        NVARCHAR(150)   NOT NULL,
    Description NVARCHAR(1000)  NULL,
    Address     NVARCHAR(200)   NOT NULL,
    City        NVARCHAR(100)   NOT NULL,
    Country     NVARCHAR(100)   NOT NULL,
    StarRating  INT             NOT NULL CHECK (StarRating BETWEEN 1 AND 5),
    IsActive    BIT             NOT NULL DEFAULT 1,
    CreatedAt   DATETIME2       NOT NULL DEFAULT GETUTCDATE()
);

-- =============================================
-- HOTEL AMENITIES
-- =============================================
CREATE TABLE HotelAmenities (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    HotelId     INT             NOT NULL REFERENCES Hotels(Id) ON DELETE CASCADE,
    Name        NVARCHAR(100)   NOT NULL
);

-- =============================================
-- ROOM TYPES
-- =============================================
CREATE TABLE RoomTypes (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    HotelId     INT             NOT NULL REFERENCES Hotels(Id) ON DELETE CASCADE,
    Name        NVARCHAR(50)    NOT NULL,   -- Single, Double, Suite, etc.
    Description NVARCHAR(500)   NULL,
    BasePrice   DECIMAL(10,2)   NOT NULL CHECK (BasePrice > 0),
    Capacity    INT             NOT NULL CHECK (Capacity > 0)
);

-- =============================================
-- ROOM TYPE AMENITIES
-- =============================================
CREATE TABLE RoomTypeAmenities (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    RoomTypeId  INT             NOT NULL REFERENCES RoomTypes(Id) ON DELETE CASCADE,
    Name        NVARCHAR(100)   NOT NULL
);

-- =============================================
-- ROOMS
-- =============================================
CREATE TABLE Rooms (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    HotelId     INT             NOT NULL REFERENCES Hotels(Id),
    RoomTypeId  INT             NOT NULL REFERENCES RoomTypes(Id),
    RoomNumber  NVARCHAR(10)    NOT NULL,
    Floor       INT             NOT NULL,
    IsActive    BIT             NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Room UNIQUE (HotelId, RoomNumber)
);

-- =============================================
-- BOOKINGS
-- =============================================
CREATE TABLE Bookings (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    UserId      INT             NOT NULL REFERENCES Users(Id),
    RoomId      INT             NOT NULL REFERENCES Rooms(Id),
    CheckIn     DATE            NOT NULL,
    CheckOut    DATE            NOT NULL,
    Status      NVARCHAR(20)    NOT NULL DEFAULT 'Pending'
                    CHECK (Status IN ('Pending','Confirmed','Cancelled','Completed')),
    TotalPrice  DECIMAL(10,2)   NOT NULL CHECK (TotalPrice > 0),
    CreatedAt   DATETIME2       NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT CHK_Dates CHECK (CheckOut > CheckIn)
);

-- =============================================
-- PAYMENTS
-- =============================================
CREATE TABLE Payments (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    BookingId   INT             NOT NULL UNIQUE REFERENCES Bookings(Id),
    Amount      DECIMAL(10,2)   NOT NULL CHECK (Amount > 0),
    Method      NVARCHAR(20)    NOT NULL
                    CHECK (Method IN ('CreditCard','DebitCard','PayPal','Cash')),
    Status      NVARCHAR(20)    NOT NULL DEFAULT 'Pending'
                    CHECK (Status IN ('Pending','Completed','Failed','Refunded')),
    PaidAt      DATETIME2       NULL
);

-- =============================================
-- REVIEWS
-- =============================================
CREATE TABLE Reviews (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    HotelId     INT             NOT NULL REFERENCES Hotels(Id),
    UserId      INT             NOT NULL REFERENCES Users(Id),
    BookingId   INT             NOT NULL UNIQUE REFERENCES Bookings(Id),
    Rating      INT             NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment     NVARCHAR(1000)  NULL,
    CreatedAt   DATETIME2       NOT NULL DEFAULT GETUTCDATE()
);

-- =============================================
-- INDEXES
-- =============================================
CREATE INDEX IX_Hotels_OwnerId       ON Hotels(OwnerId);
CREATE INDEX IX_Hotels_City          ON Hotels(City);
CREATE INDEX IX_Rooms_HotelId        ON Rooms(HotelId);
CREATE INDEX IX_Rooms_RoomTypeId     ON Rooms(RoomTypeId);
CREATE INDEX IX_Bookings_UserId      ON Bookings(UserId);
CREATE INDEX IX_Bookings_RoomId      ON Bookings(RoomId);
CREATE INDEX IX_Bookings_Dates       ON Bookings(CheckIn, CheckOut);
CREATE INDEX IX_Bookings_Status      ON Bookings(Status);
CREATE INDEX IX_Reviews_HotelId      ON Reviews(HotelId);
CREATE INDEX IX_Payments_BookingId   ON Payments(BookingId);