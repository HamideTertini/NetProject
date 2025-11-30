# Product API – .NET 8 Web API

Ky është një projekt demonstrues i ndërtuar për një test teknik/intervistë.
API ofron menaxhim produktesh me CRUD operacione, filtrime, sortime, paging dhe autentikim me JWT.

---

## 🚀 Teknologjitë e përdorura

* **.NET 8 / ASP.NET Core Web API**
* **Entity Framework Core** (InMemory Database)
* **C#**
* **JWT Authentication**
* **Swagger / OpenAPI**
* **Clean Service Layer Architecture**

---

## 📦 Funksionalitetet kryesore

## 🔐 Autentikimi (JWT)

API është i mbrojtur me JWT.

### ► 1. Login për të marrë token

**POST** `/api/auth/login`

**Body:**

```json
{
  "username": "admin",
  "password": "12345"
}
```

**Response:**

```json
{
  "token": "jwt_token_here"
}
```

### ► 2. Përdorimi i token-it

Në çdo kërkesë tjetër vendoset tek header-i:

```
Authorization: Bearer <token>
```

---

## 📘 Entiteti Product

Produktet përmbajnë:

| Fusha         | Tipi       |
| ------------- | ---------- |
| Id            | int        |
| Name          | string     |
| Category      | string     |
| Price         | decimal    |
| StockQuantity | int        |
| CreatedAt     | DateTime   |
| InStock       | bool (DTO) |

---

## 🔄 CRUD + Filtering + Sorting + Paging

### **GET /api/products**

Kthen listën e produkteve.

Mbështet filtrime:

| Parametri | Përshkrimi                       |
| --------- | -------------------------------- |
| category  | Filtron sipas kategorisë         |
| minPrice  | Filtron produktet me çmim ≥      |
| maxPrice  | Filtron produktet me çmim ≤      |
| page      | Numri i faqes (default 1)        |
| pageSize  | Madhësia e faqes (default 10)    |
| sortBy    | name, price, category, createdAt |
| sortOrder | asc / desc                       |

**Shembull:**

```
GET /api/products?minPrice=5&maxPrice=60&page=2&pageSize=10&sortBy=price&sortOrder=desc
```

---

### **GET /api/products/{id}**

Kthen një produkt të vetëm bazuar në ID.

---

### **POST /api/products**

Krijon produkt të ri.

**Body:**

```json
{
  "name": "Bluetooth Speaker",
  "category": "Electronics",
  "price": 39.99,
  "stockQuantity": 50
}
```

---

### **PUT /api/products/{id}**

Përditëson një produkt ekzistues.

---

### **DELETE /api/products/{id}**

Fshin një produkt nga sistemi.

---

## 🧱 Struktura e projektit

```
ProductApi/
 ├── Controllers/
 │    ├── ProductsController.cs
 │    └── AuthController.cs
 ├── Data/
 │    └── AppDbContext.cs
 ├── DTOs/
 │    └── ProductDtos.cs
 ├── Entities/
 │    └── Product.cs
 ├── Middleware/
 │    └── GlobalExceptionMiddleware.cs
 ├── Models/
 │    └── PagedResult.cs
 ├── Services/
 │    ├── IProductService.cs
 │    └── ProductService.cs
 ├── Program.cs
 └── README.md
```

---

## 🛠 Si të startohet projekti

### 1️⃣ Klono projektin

```
git clone https://github.com/HamideTertini/NetProject
```

### 2️⃣ Hyr në folder

```
cd NetProject
```

### 3️⃣ Starto API-n

```
dotnet run
```

### 4️⃣ Hap Swagger

Shko në:

```
https://localhost:<port>/swagger
```

---

## 🧪 Seeding (të dhëna fillestare)

Projekti krijon automatikisht disa produkte fillestare në InMemory DB në startim.

---

## ⚠️ Opsionale (Extra Features të implementuara)

✔ Global Exception Handling
✔ Paging
✔ Sorting
✔ Filtering
✔ JWT Authentication
✔ Response Object standard

---

## 👤 Autori

**Hamide Tertini**
.NET Developer
GitHub: [https://github.com/HamideTertini](https://github.com/HamideTertini)

---

