# Hotel Booking REST API
 
## 📌 Overview
 
This project is an **ASP.NET Core Web API** that implements a secure **Hotel Booking Platform REST API** using **Dapper**, **SQL Server**, and **JWT Authentication**.
 
It was developed as a multi-tenant hotel booking platform — similar in concept to Booking.com — where multiple Hotel Managers can register their hotels, and Guests can browse and book rooms through authenticated API endpoints.
 
The application supports role-based access control, resource-level authorization, pagination, filtering, and follows a clean layered architecture.
 
---
 
## 🎯 Purpose
 
The main goals of this project are to:
 
- Build a **multi-tenant REST API** for a hotel booking platform
- Implement **JWT Authentication** with role-based authorization
- Apply **resource-level authorization** — HotelManagers can only manage their own hotels
- Perform secured **CRUD operations** across multiple entities
- Follow a clean layered architecture with **Controllers**, **Services**, and **Repositories**
- Practice realistic enterprise API development patterns
---
 
## 🧠 Concepts Practiced
 
- ASP.NET Core Web API architecture
- SQL Server integration
- Dapper for data access
- Repository pattern
- Service layer for business logic
- DTOs and model binding
- Data annotations validation
- JWT token generation
- JWT-based authentication
- Role-based authorization with `[Authorize(Roles = "...")]`
- Resource-level authorization
- Claims-based user identification
- Pagination and filtering
- Soft delete pattern
- Async/await throughout the application
- Secure password hashing with BCrypt
---
 
## 🛠 Tech Stack
 
- **C#**
- **ASP.NET Core Web API**
- **.NET 9**
- **SQL Server**
- **Dapper**
- **JWT Bearer Authentication**
- **BCrypt**
- **Postman**
---
 
## 📂 Project Structure
 
- **Controllers** – API endpoints for authentication, hotels, rooms, bookings, payments and reviews
- **Models** – Core domain models
- **DTOs** – Request and response models per feature
- **Repositories** – Data access layer with Dapper and raw SQL
- **Services** – Business logic layer
- **Data** – SQL connection factory
- **Helpers** – JWT token generation
- **Program.cs** – Application startup and dependency injection
- **appsettings.json** – Configuration and connection strings
---
 
## ⚙️ Features
 
### Authentication
- Register new users (default role: Guest)
- Login with email and password
- Password hashing using **BCrypt**
- JWT token generation with claims (Id, FullName, Email, Role)
- Protected endpoints using **Bearer Token Authentication**
### Hotel Management
- Create, update, soft delete hotels
- Browse all hotels with **pagination** and **filtering** (city, country, star rating)
- Get hotel details including amenities
- HotelManagers can only manage their own hotels
### Room Management
- Create and manage room types with amenities
- Create and manage individual rooms per hotel
### Booking Management
- Guests can book available rooms
- Availability check based on date range overlap
- Automatic total price calculation (BasePrice × nights)
- Booking status lifecycle: Pending → Confirmed → Cancelled → Completed
### Payment Management
- One payment per booking
- Payment methods: CreditCard, DebitCard, PayPal, Cash
- Payment status: Pending, Completed, Failed, Refunded
- Booking cancellation automatically refunds the payment
### Reviews
- Guests can review a hotel only after a Completed booking
- One review per booking
---
 
## 🔐 Authorization
 
The API includes a full authentication and role-based authorization system:
 
- **Admin** – Full access to everything
- **HotelManager** – Manages only their own hotels and rooms
- **Guest** – Makes bookings, payments, and reviews
| Endpoint | Admin | HotelManager | Guest |
|---|---|---|---|
| Register / Login | ✅ | ✅ | ✅ |
| GET Hotels | ✅ | ✅ | ✅ |
| POST Hotel | ✅ | ✅ | ❌ |
| PUT / DELETE Hotel | ✅ | Own only | ❌ |
| POST Room | ✅ | Own hotel only | ❌ |
| POST Booking | ✅ | ❌ | ✅ |
| GET My Bookings | ✅ | ❌ | Own only |
| POST Payment | ✅ | ❌ | Own booking only |
| POST Review | ✅ | ❌ | Completed booking only |
 
---
 
## 🗃 Database Design
 
The system is based on the following core entities:
 
- **Users**
- **Hotels**
- **HotelAmenities**
- **RoomTypes**
- **RoomTypeAmenities**
- **Rooms**
- **Bookings**
- **Payments**
- **Reviews**
### Relationships
 
- One **User** (HotelManager) owns many **Hotels**
- One **Hotel** has many **RoomTypes** and **Rooms**
- One **Hotel** has many **HotelAmenities**
- One **RoomType** has many **RoomTypeAmenities**
- One **Room** belongs to one **Hotel** and one **RoomType**
- One **User** (Guest) has many **Bookings**
- One **Booking** belongs to one **Room** and one **User**
- One **Booking** has one **Payment**
- One **Booking** enables one **Review**
---
 
## ✅ Business Rules Implemented
 
- Passwords are never stored as plain text — hashed using **BCrypt**
- Only active users can log in
- Role assignment is managed by the Admin directly in the database
- HotelManagers can only update or delete their own hotels
- Room availability is checked before every booking (date range overlap)
- TotalPrice is calculated automatically: `BasePrice × nights`
- A review can only be submitted after a **Completed** booking
- One review per booking
- One payment per booking
- Booking cancellation sets payment status to **Refunded**
- Soft delete is implemented for Users, Hotels, and Rooms using `IsActive`
---
 
## 🌐 API Endpoints
 
### Auth
- `POST /api/auth/register`
- `POST /api/auth/login`
### Hotels
- `GET /api/hotels`
- `GET /api/hotels/{id}`
- `GET /api/hotels/my`
- `POST /api/hotels`
- `PUT /api/hotels/{id}`
- `DELETE /api/hotels/{id}`
### Rooms
- `GET /api/rooms/{id}`
- `GET /api/rooms/hotel/{hotelId}`
- `POST /api/rooms`
- `PUT /api/rooms/{id}`
- `DELETE /api/rooms/{id}`
### Bookings
- `POST /api/bookings`
- `GET /api/bookings/my`
- `GET /api/bookings/{id}`
- `PUT /api/bookings/{id}/cancel`
### Payments
- `POST /api/payments`
- `GET /api/payments/{id}`
### Reviews
- `POST /api/reviews`
- `GET /api/reviews/hotel/{hotelId}`
---
 
## 🧪 Testing
 
The API is tested using **Postman**.
 
For protected endpoints, include the JWT token in the request header:
 
```http
Authorization: Bearer YOUR_TOKEN_HERE
```
