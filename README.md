# 🛒 EcomApi

**EcomApi** es una API RESTful desarrollada en **.NET 9** que permite gestionar información relacionada con un sistema de e-commerce.  
El proyecto está diseñado para funcionar tanto en **memoria interna** (ideal para pruebas rápidas) como con conexión a una **base de datos** (para entornos productivos).

---

## 🚀 Características principales
- API REST desarrollada en **ASP.NET Core 9**.
- Arquitectura **modular y escalable**.
- Funciona en **modo en memoria** (sin necesidad de BD) o conectado a **SQL Server**.
- Endpoints para consultar y gestionar información de productos, órdenes y clientes.
- Documentación con **Swagger/OpenAPI**.

---

## 📂 Estructura del Proyecto
EcomApi/
│── Controllers/ # Controladores con los endpoints
│── Models/ # Modelos de datos
│── Services/ # Servicios de negocio (en memoria o BD)
│── Program.cs # Configuración principal
│── appsettings.json # Configuración de la aplicación


Endpoints principales
Productos

GET /api/products → Lista todos los productos

GET /api/products/{id} → Obtiene un producto por ID

POST /api/products → Crea un nuevo producto

PUT /api/products/{id} → Actualiza un producto

DELETE /api/products/{id} → Elimina un producto

Órdenes

GET /api/orders → Lista todas las órdenes

GET /api/orders/{id} → Obtiene una orden por ID

POST /api/orders → Crea una nueva orden

Clientes

GET /api/customers → Lista todos los clientes

GET /api/customers/{id} → Obtiene un cliente por ID

POST /api/customers → Crea un nuevo cliente
