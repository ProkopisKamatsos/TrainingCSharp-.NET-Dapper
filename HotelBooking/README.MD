# 🏨 Hotel Booking REST API

## 📌 Overview

This project is an **ASP.NET Core Web API** that implements a secure, enterprise-level **Hotel Booking Platform REST API** using **Dapper**, **SQL Server**, and **JWT Authentication**.

The platform follows a multi-tenant architecture — similar in concept to Booking.com — where multiple Hotel Managers can register and manage their hotels, and Guests can browse hotels, check room availability, make bookings, process payments, and leave reviews through authenticated API endpoints.

The application supports role-based access control, resource-level authorization, pagination, filtering, soft delete, and follows a clean layered architecture.

---

## 🎯 Purpose

The main goals of this project are to:

- Build a **multi-tenant REST API** for a hotel booking platform
- Implement **JWT Authentication** with role-based authorization
- Apply **resource-level authorization** — HotelManagers can only manage their own hotels and rooms
- Enforce **business rules** across the full booking lifecycle
- Perform secured **CRUD operations** across multiple entities
- Follow a clean layered architecture with **Controllers**, **Services**, and **Repositories**
- Practice realistic enterprise-level API development patterns

---

## 🧠 Concepts Practiced

- ASP.NET Core Web API architecture
- SQL Server integration with raw SQL
- Dapper for data access
- Repository pattern
- Service layer for business logic
- DTOs and model binding
- Data annotations validation
- JWT token generation with claims
- JWT-based authentication
- Role-based authorization with `[Authorize(Roles = "...")]`
- Resource-level authorization
- Claims-based user identification
- Pagination and filtering
- Soft delete pattern using `IsActive`
- N+1 query prevention
- Custom Dapper TypeHandler for `DateOnly`
- Async/await throughout the application
- Secure password hashing with BCrypt

---

## 🛠 Tech Stack

| Technology | Usage |
|---|---|
| **C# / .NET 10** | Core language and runtime |
| **ASP.NET Core Web API** | API framework |
| **SQL Server** | Relational database |
| **Dapper** | Lightweight ORM / data access |
| **JWT Bearer Authentication** | Stateless authentication |
| **BCrypt.Net** | Password hashing |
| **Postman** | API testing |

---

## 📂 Project Structure

```
HotelBooking/
├── Controllers/
│   ├── AuthController.cs
│   ├── HotelsController.cs
│   ├── RoomTypesController.cs
│   ├── RoomsController.cs
│   ├── BookingsController.cs
│   ├── PaymentsController.cs
│   └── ReviewsController.cs
├── Services/
│   ├── IAuthService.cs / AuthService.cs
│   ├── IHotelService.cs / HotelService.cs
│   ├── IRoomTypeService.cs / RoomTypeService.cs
│   ├── IRoomService.cs / RoomService.cs
│   ├── IBookingService.cs / BookingService.cs
│   ├── IPaymentService.cs / PaymentService.cs
│   └── IReviewService.cs / ReviewService.cs
├── Repositories/
│   ├── IUserRepository.cs / UserRepository.cs
│   ├── IHotelRepository.cs / HotelRepository.cs
│   ├── IRoomTypeRepository.cs / RoomTypeRepository.cs
│   ├── IRoomRepository.cs / RoomRepository.cs
│   ├── IBookingRepository.cs / BookingRepository.cs
│   ├── IPaymentRepository.cs / PaymentRepository.cs
│   └── IReviewRepository.cs / ReviewRepository.cs
├── Models/
│   ├── User.cs
│   ├── Hotel.cs
│   ├── HotelAmenity.cs
│   ├── RoomType.cs
│   ├── RoomTypeAmenity.cs
│   ├── Room.cs
│   ├── Booking.cs
│   ├── Payment.cs
│   └── Review.cs
├── DTOs/
│   ├── Auth/
│   ├── Hotel/
│   ├── RoomType/
│   ├── Room/
│   ├── Booking/
│   ├── Payment/
│   └── Review/
├── Data/
│   ├── SqlConnectionFactory.cs
│   └── DateOnlyTypeHandler.cs
├── Helpers/
│   └── JwtHelper.cs
├── Program.cs
└── appsettings.json
```

---

## ⚙️ Features

### 🔑 Authentication
- Register new users (default role: `Guest`)
- Login with email and password
- Password hashing using **BCrypt**
- JWT token generation with claims: `Id`, `FullName`, `Email`, `Role`
- Protected endpoints using **Bearer Token Authentication**
- Role assignment is managed by the Admin directly in the database

### 🏨 Hotel Management
- Create, update, and soft delete hotels
- Browse all hotels with **pagination** and **filtering** by city, country, and star rating
- Get hotel details including amenities
- HotelManagers can only manage their own hotels (resource-level authorization)

### 🛏 Room Type Management
- Create, update, and delete room types per hotel
- Each room type has a name, description, base price, capacity, and amenities
- HotelManagers can only manage room types of their own hotels

### 🚪 Room Management
- Create, update, and soft delete rooms per hotel
- Check room availability based on date range
- HotelManagers can only manage rooms of their own hotels

### 📅 Booking Management
- Guests can book available rooms
- Date range overlap check before every booking
- Automatic total price calculation: `BasePrice × nights`
- Booking status lifecycle: `Pending` → `Confirmed` → `Cancelled` → `Completed`
- Guests can cancel their own bookings

### 💳 Payment Management
- One payment per booking
- Payment amount is automatically taken from the booking total
- Supported methods: `CreditCard`, `DebitCard`, `PayPal`, `Cash`
- Payment status: `Pending`, `Completed`, `Failed`, `Refunded`

### ⭐ Review Management
- Guests can review a hotel only after a `Completed` booking
- One review per booking
- Reviews are publicly visible per hotel

---

## 🗃 Database Design

### Entities

| Table | Description |
|---|---|
| `Users` | Platform users with roles |
| `Hotels` | Hotels owned by HotelManagers |
| `HotelAmenities` | Amenities per hotel (WiFi, Pool, etc.) |
| `RoomTypes` | Room categories per hotel (Single, Double, Suite) |
| `RoomTypeAmenities` | Amenities per room type (AC, TV, Minibar) |
| `Rooms` | Individual rooms per hotel |
| `Bookings` | Guest reservations |
| `Payments` | Payments linked to bookings |
| `Reviews` | Guest reviews per hotel |

### Relationships

- One **User** (HotelManager) owns many **Hotels**
- One **Hotel** has many **RoomTypes**, **Rooms**, **HotelAmenities**, and **Reviews**
- One **RoomType** has many **Rooms** and **RoomTypeAmenities**
- One **User** (Guest) has many **Bookings**
- One **Booking** belongs to one **Room** and one **User**
- One **Booking** has one **Payment**
- One **Booking** enables one **Review**

---

## ✅ Business Rules

### Authentication
- Passwords are never stored as plain text — hashed using **BCrypt**
- Only active users (`IsActive = 1`) can log in
- Duplicate emails are not allowed during registration
- New users always receive the `Guest` role by default

### Hotels & Rooms
- HotelManagers can only update, delete, or add rooms to their own hotels
- Soft delete is used for Hotels and Rooms (`IsActive = 0`)
- Room numbers must be unique within the same hotel

### Bookings
- `CheckOut` must be after `CheckIn`
- `CheckIn` cannot be in the past
- A room cannot be booked if there is an existing active booking that overlaps the requested dates
- `TotalPrice` is calculated automatically: `BasePrice × number of nights`
- A `Cancelled` or `Completed` booking cannot be cancelled again

### Payments
- A booking can only have one payment
- Payment amount is always equal to the booking `TotalPrice` — not provided by the client
- Cannot pay for a `Cancelled` or `Completed` booking

### Reviews
- A review can only be submitted for a `Completed` booking
- A booking can only have one review
- The `HotelId` in the review is resolved automatically from the booking's room — not provided by the client
- Guests can only review their own bookings

---

## 🔐 Authorization

### Roles

| Role | Description |
|---|---|
| **Admin** | Full access to all resources |
| **HotelManager** | Manages only their own hotels, room types, and rooms |
| **Guest** | Browses hotels, makes bookings, payments, and reviews |

---

### 🔑 Auth

| Endpoint | Admin | HotelManager | Guest |
|---|---|---|---|
| `POST /api/auth/register` | ✅ | ✅ | ✅ |
| `POST /api/auth/login` | ✅ | ✅ | ✅ |

---

### 🏨 Hotels

| Endpoint | Admin | HotelManager | Guest |
|---|---|---|---|
| `GET /api/hotels` | ✅ | ✅ | ✅ |
| `GET /api/hotels/{id}` | ✅ | ✅ | ✅ |
| `GET /api/hotels/my` | ❌ | ✅ Own only | ❌ |
| `POST /api/hotels` | ✅ | ✅ | ❌ |
| `PUT /api/hotels/{id}` | ✅ | ✅ Own only | ❌ |
| `DELETE /api/hotels/{id}` | ✅ | ✅ Own only | ❌ |

---

### 🛏 Room Types

| Endpoint | Admin | HotelManager | Guest |
|---|---|---|---|
| `GET /api/roomtypes/hotel/{hotelId}` | ✅ | ✅ | ✅ |
| `GET /api/roomtypes/{id}` | ✅ | ✅ | ✅ |
| `POST /api/roomtypes` | ✅ | ✅ Own hotel only | ❌ |
| `PUT /api/roomtypes/{id}` | ✅ | ✅ Own hotel only | ❌ |
| `DELETE /api/roomtypes/{id}` | ✅ | ✅ Own hotel only | ❌ |

---

### 🚪 Rooms

| Endpoint | Admin | HotelManager | Guest |
|---|---|---|---|
| `GET /api/rooms/hotel/{hotelId}` | ✅ | ✅ | ✅ |
| `GET /api/rooms/{id}` | ✅ | ✅ | ✅ |
| `GET /api/rooms/available` | ✅ | ✅ | ✅ |
| `POST /api/rooms` | ✅ | ✅ Own hotel only | ❌ |
| `PUT /api/rooms/{id}` | ✅ | ✅ Own hotel only | ❌ |
| `DELETE /api/rooms/{id}` | ✅ | ✅ Own hotel only | ❌ |

---

### 📅 Bookings

| Endpoint | Admin | HotelManager | Guest |
|---|---|---|---|
| `POST /api/bookings` | ✅ | ❌ | ✅ |
| `GET /api/bookings/my` | ❌ | ❌ | ✅ Own only |
| `GET /api/bookings/{id}` | ✅ | ❌ | ✅ Own only |
| `PUT /api/bookings/{id}/cancel` | ✅ | ❌ | ✅ Own only |

---

### 💳 Payments

| Endpoint | Admin | HotelManager | Guest |
|---|---|---|---|
| `POST /api/payments` | ✅ | ❌ | ✅ Own booking only |
| `GET /api/payments/{id}` | ✅ | ❌ | ✅ Own only |
| `GET /api/payments/booking/{bookingId}` | ✅ | ❌ | ✅ Own booking only |

---

### ⭐ Reviews

| Endpoint | Admin | HotelManager | Guest |
|---|---|---|---|
| `GET /api/reviews/hotel/{hotelId}` | ✅ | ✅ | ✅ |
| `POST /api/reviews` | ✅ | ❌ | ✅ Completed booking only |

---

## 🧪 Testing

The API is tested using **Postman**.

For protected endpoints, include the JWT token in the request header:

```http
Authorization: Bearer YOUR_TOKEN_HERE
```

### Suggested Testing Flow

1. `POST /api/auth/register` — Create a Guest account
2. `POST /api/auth/login` — Get JWT token
3. *(Set Role to HotelManager via database)*
4. `POST /api/auth/login` — Get new JWT token with updated role
5. `POST /api/hotels` — Create a hotel
6. `POST /api/roomtypes` — Add room types with amenities
7. `POST /api/rooms` — Add rooms
8. `GET /api/rooms/available` — Check availability for date range
9. *(Switch to Guest token)*
10. `POST /api/bookings` — Make a booking
11. `POST /api/payments` — Pay for the booking
12. *(Set Booking Status to Completed via database)*
13. `POST /api/reviews` — Leave a review
14. `GET /api/reviews/hotel/{hotelId}` — View hotel reviews