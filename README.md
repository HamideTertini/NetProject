# 📄 **Product Management API**

RESTful Web API built with **.NET 8**, **ASP.NET Core**, **Entity Framework Core**, supporting full product CRUD, filtering, sorting, paging, JWT authentication, global exception handling, and seed data.

---

## 📌 **Features**

### ✅ Core Features

* Full CRUD for Product entity
* Filtering by:

  * Category
  * MinPrice
  * MaxPrice
* Automatic **InStock** field (true/false based on StockQuantity)
* DTOs based on CQRS pattern
* Service layer architecture
* EF Core with **InMemory Database**
* JWT Authentication (Login → Token → Access secured endpoints)

### 🌟 Optional Extras (Implemented)

* Global Exception Middleware
* Paging
* Sorting
* Seed data
* Consistent JSON API responses

---

## 📂 **Project Structure**

```
ProductApi/
 ├── Controllers/
 │    ├── ProductsController.cs
 ├── Services/
 │    ├── IProductService.cs
 │    ├── ProductService.cs
 ├── Entities/
 │    ├── Product.cs
 ├── Dtos/
 │    ├── ProductDtos.cs
 ├── Data/
 │    ├── AppDbContext.cs
 ├── Auth/
 │    ├── AuthController.cs
 ├── Middleware/
 │    ├── GlobalExceptionMiddleware.cs
 ├── Program.cs
 ├── README.md
```

---

## 🚀 **How to Run the Project**

### **1️⃣ Clone the repository**

```bash
git clone https://github.com/HamideTertini/NetProject.git
cd NetProject
```

### **2️⃣ Run the API**

```bash
dotnet run
```

### **3️⃣ Access Swagger UI**

```
https://localhost:5237/swagger
```

---

## 🔐 **Authentication (JWT)**

### **Login endpoint**

```
POST /api/auth/login
```

### Example Request

```json
{
  "username": "admin",
  "password": "12345"
}
```

### Example Response

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6..."
}
```

### **How to use the token**

Në Swagger:

1. Kliko **Authorize**
2. Shkruaj:

```
Bearer <your-token>
```

---

## 🧪 **API Documentation**

### 📍 **1. GET /api/products**

Returns all products with filtering, paging & sorting.

**Query Parameters**

* category
* minPrice
* maxPrice
* page
* pageSize
* sortBy (name, price, category, createdAt)
* sortOrder (asc/desc)

### 📍 **2. GET /api/products/{id}**

Get single product.

### 📍 **3. POST /api/products**

Create a product
**Required:** Name, Category, Price

### 📍 **4. PUT /api/products/{id}**

Update product.

### 📍 **5. DELETE /api/products/{id}**

Delete product.

### 📍 **6. POST /api/auth/login**

Returns JWT token.

---

## 🌱 **Seed Data**

Seed data is loaded automatically when the project starts.

Example entries include:

* Mouse
* Keyboard
* Headphones
* Monitor
* etc.

---

## ⚠️ **Global Exception Handling**

Every unhandled exception returns standard JSON:

```json
{
  "status": 500,
  "message": "An unexpected error occurred.",
  "traceId": "0HLQ123ABCDEF"
}
```

---

## 📦 **Technologies Used**

* .NET 8 / C#
* ASP.NET Core Web API
* Entity Framework Core (InMemory)
* JWT Authentication
* Swagger / OpenAPI
* LINQ, DTOs, Services

---

## 🧑‍💻 **Author**

**Hamide Tertini**
GitHub: [https://github.com/HamideTertini](https://github.com/HamideTertini)

