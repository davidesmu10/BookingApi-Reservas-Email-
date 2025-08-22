# 📅 BookingApi – API de Reservas (Hoteles / Clínicas / Restaurantes)

**BookingApi** es una API REST construida en **ASP.NET Core 8** que permite gestionar **ubicaciones**, *slots* de disponibilidad y **reservas** con autenticación JWT y notificaciones por email (SMTP o SendGrid).  
Ideal como base para sistemas de reservas de **hoteles, clínicas médicas o restaurantes**.

Actualmente funciona en **memoria (InMemory)** para desarrollo y pruebas locales, pero está preparada para conectarse fácilmente a **SQL Server** con Entity Framework Core.

---

## ✨ Características principales
- 🔐 **Autenticación JWT**: registro y login de usuarios.
- 🏨 **Gestión de ubicaciones** (CRUD para administradores).
- 📅 **Gestión de disponibilidad** (*slots*) para reservas.
- 📌 **Reservas por usuario**: crear, consultar, cancelar.
- 📧 **Notificaciones por correo** al crear o cancelar una reserva.
- 🛠️ Estructura modular y lista para producción.

---

## 📂 Estructura del proyecto
BookingApi/
├── Program.cs
├── AppDbContext.cs
├── Models/
│ ├── User.cs
│ ├── Location.cs
│ ├── Slot.cs
│ └── Booking.cs
├── DTOs/
│ ├── AuthDtos.cs
│ └── BookingDtos.cs
├── Services/
│ ├── JwtService.cs
│ └── EmailService.cs
├── Controllers/
│ ├── AuthController.cs
│ ├── LocationsController.cs
│ ├── SlotsController.cs
│ └── BookingsController.cs
├── appsettings.json
└── BookingApi.csproj


---

## 🧭 Endpoints principales

### 🔐 Autenticación
- `POST /api/auth/register` → Crear usuario.
- `POST /api/auth/login` → Autenticación y obtención de JWT.

### 🏨 Ubicaciones
- `GET /api/locations` → Listar ubicaciones.
- `GET /api/locations/{id}` → Obtener ubicación por ID.
- `POST /api/locations` → Crear ubicación (Admin).
- `PUT /api/locations/{id}` → Editar ubicación (Admin).
- `DELETE /api/locations/{id}` → Eliminar ubicación (Admin).

### 📅 Slots (Disponibilidad)
- `GET /api/locations/{locationId}/slots?from=&to=` → Listar *slots* disponibles.
- `POST /api/locations/{locationId}/slots` → Crear slot (Admin).

### 📌 Reservas
- `GET /api/bookings` → Ver mis reservas.
- `GET /api/bookings/{id}` → Ver detalles de una reserva.
- `POST /api/bookings` → Crear nueva reserva.
- `POST /api/bookings/{id}/cancel` → Cancelar reserva (con email de confirmación).

