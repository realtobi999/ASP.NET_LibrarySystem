# Endpoints

## Table of Contents

- **[Auth Endpoints](#auth-endpoints)**
  - **[POST - /api/auth/register](#post---apiauthregister)**
  - **[POST - /api/auth/login](#post---apiauthlogin)**
  - **[POST - /api/auth/employee/register](#post---apiauthemployeeregister)**
  - **[POST - /api/auth/employee/login](#post---apiauthemployeelogin)**
- **[Author Endpoints](#author-endpoints)**
  - **[GET - /api/author](#get---apiauthor)**
  - **[GET - /api/author/{author_id}](#get---apiauthorauthor_id)**
  - **[POST - /api/author](#post---apiauthor)**
  - **[PUT - /api/author/{author_id}](#put---apiauthorauthor_id)**
  - **[DELETE - /api/author/{author_id}](#delete---apiauthorauthor_id)**
  - **[PUT - /api/author/{author_id}/photo](#put---apiauthorauthor_idphoto)**

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

- `201` - created

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

- `200` - success
- `401` - not authorized

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

- `201` - created

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

- `200` - success
- `401` - not authorized

## Author Endpoints

### `GET - /api/author`

Return a list of filtered or all authors.

### Parameters

- `limit` - sets the maximum number of results to return. Defaults to 50 but can be adjusted as needed.
- `offset` - specifies the starting point in the dataset for the next set of results, enabling paged access.

### Example Request

- None

### Response

```json
[
  {
    "id": "903a392f-d09f-62e6-b751-6ee6fa1a4074",
    "name": "string",
    "description": "string",
    "birthday": "1983-04-24T14:44:32.4341948+00:00",
    "picture": null
  },
  {
    "id": "e90450c4-141d-eb7f-664f-39e849fdbac5",
    "name": "string",
    "description": "string",
    "birthday": "1956-09-03T16:51:42.7866367+00:00",
    "picture": null
  }
]
```

### Returns

- `200` - success
- `401` - not authorized
- `404` - not found

---

### `POST - /api/author`

Creates a new author entity in the database.

### Parameters

- None

### Example Request

``` json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string",
  "description": "string",
  "birthday": "2024-10-31T20:55:43.910Z"
}
```

### Response

- None

### Returns

- `201` - created
- `401` - not authorized

---

### `GET - /api/author/{author_id}`

Return a specific author entity by it's id.

### Parameters

- `author_id` - the id of the author

### Example Request

``` json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string",
  "description": "string",
  "birthday": "2024-10-31T20:55:43.910Z"
}
```

### Response

- None

### Returns

- `200` - success
- `404` - not found
- `401` - not authorized

---

### `PUT - /api/author/{author_id}`

Updates a author entity in the database.

### Parameters

- `author_id` - the id of the author

### Example Request

``` json
{
  "name": "string",
  "description": "string",
  "birthday": "2024-10-31T21:14:30.622Z"
}
```

### Response

- None

### Returns

- `204` - no content
- `404` - not found
- `401` - not authorized

---

### `DELETE - /api/author/{author_id}`

Deletes a author entity in the database.

### Parameters

- `author_id` - the id of the author

### Example Request

- None

### Response

- None

### Returns

- `204` - no content
- `404` - not found
- `401` - not authorized

---

### `PUT - /api/author/{author_id}/photo`

Updates/Creates a author pictures in the database.

### Parameters

- `author_id` - the id of the author

### Example Request

- File

### Response

- None

### Returns

- `204` - no content
- `404` - not found
- `401` - not authorized
