# Endpoints

## Auth Endpoints

### `POST - /api/auth/register`

Registers a new user into the system.

### Parameters

- None

### Example Request

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "username": "string",
  "email": "user@example.com",
  "password": "stringst"
}
```

### Response

- None

### Returns

- `201` - Created

---

### `POST - /api/auth/login`

Authenticates the user and returns a JWT.

### Parameters

- None

### Example Request

```json
{
  "email": "string",
  "password": "string"
}
```

### Response

```json
{
  "userDto": {
    "id": "14c2300b-b973-999d-d883-1aadb0735a6f",
    "username": "Zachery_Green",
    "email": "Jany31@yahoo.com",
    "profilePicture": null
  },
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpX...."
}
```

### Returns

- `200` - Success
- `401` - Not authorized

---

### `POST - /api/auth/employee/register`

Registers a new employee into the system.

### Parameters

- None

### Headers

- `Authorization` : Bearer ADMIN_TOKEN

### Example Request

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string",
  "email": "user@example.com",
  "password": "stringst"
}
```

### Response

- None

### Returns

- `201` - Created

---

### `POST - /api/auth/employee/login`

Authenticates the employee and returns a JWT.

### Parameters

- None

### Example Request

```json
{
  "email": "string",
  "password": "string"
}
```

### Response

```json
{
  "employeeDto": {
    "id": "a19b40af-0a22-9a1f-71fb-95141a876f42",
    "name": "Jarrett.Predovic48",
    "email": "Rosa.Kautzer11@gmail.com",
    "picture": null
  },
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...."
}
```

### Returns

- `200` - Success
- `401` - Not authorized

---
