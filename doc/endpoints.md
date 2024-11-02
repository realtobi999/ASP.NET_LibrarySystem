# Endpoints

## Table of Contents

- **[Auth Endpoints](#auth-endpoints)**
  - **[POST - /api/auth/register](#post---apiauthregister)**
  - **[POST - /api/auth/login](#post---apiauthlogin)**
  - **[POST - /api/auth/employee/register](#post---apiauthemployeeregister)**
  - **[POST - /api/auth/employee/login](#post---apiauthemployeelogin)**
- **[Author Endpoints](#author-endpoints)**
  - **[GET - /api/author](#get---apiauthor)**
  - **[GET - /api/author/{author_id}](#get---apiauthorauthorid)**
  - **[POST - /api/author](#post---apiauthor)**
  - **[PUT - /api/author/{author_id}](#put---apiauthorauthorid)**
  - **[DELETE - /api/author/{author_id}](#delete---apiauthorauthorid)**
  - **[PUT - /api/author/{author_id}/photo](#put---apiauthorauthoridphoto)**
- **[Book Endpoints](#book-endpoints)**
  - **[GET - api/book](#get---apibook)**
  - **[GET - api/book/{book_id}](#get---apibookbookid)**
  - **[GET - api/book/isbn/{isbn}](#get---apibookisbnisbn)**
  - **[GET - api/book/popular](#get---apibookpopular)**
  - **[GET - api/book/recent](#get---apibookrecent)**
  - **[GET - api/book/recommended](#get---apibookrecommended)**
  - **[GET - api/book/search/{query}](#get---apibooksearchquery)**
  - **[POST - api/book](#post---apibook)**
  - **[PUT - api/book/{book_id}](#put---apibookbookid)**
  - **[PUT - api/book/{book_id}/photo](#put---apibookbookidphoto)**
  - **[DELETE - api/book/{book_id}](#delete---apibookbookid)**
- **[Book Review Endpoints](#book-review-endpoints)**
  - **[POST - api/review](#post---apireview)**
  - **[PUT - api/review/{review_id}](#put---apireviewreviewid)**
  - **[DELETE - api/review/{review_id}](#delete---apireviewreviewid)**

## Auth Endpoints

### `POST - /api/auth/register`

Registers a new user in the system.

#### Parameters

- None

#### Example Request

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "username": "string",
  "email": "user@example.com",
  "password": "stringst"
}
```

#### Returns

- `201` - Created

---

### `POST - /api/auth/login`

Authenticates a user and returns a JWT token.

#### Parameters

- None

#### Example Request

```json
{
  "email": "string",
  "password": "string"
}
```

#### Returns

- `200` - Success
- `401` - Unauthorized

---

### `POST - /api/auth/employee/register`

Registers a new employee in the system. Requires an `Authorization` header with an Admin token.

#### Headers

- `Authorization` : Bearer ADMIN_JWT_TOKEN

#### Parameters

- None

#### Example Request

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string",
  "email": "user@example.com",
  "password": "stringst"
}
```

#### Returns

- `201` - Created

---

### `POST - /api/auth/employee/login`

Authenticates an employee and returns a JWT token.

#### Parameters

- None

#### Example Request

```json
{
  "email": "string",
  "password": "string"
}
```

#### Returns

- `200` - Success
- `401` - Unauthorized

## Author Endpoints

### `GET - /api/author`

Returns a paginated list of authors.

#### Parameters

- `limit` - Sets the maximum number of results to return. Defaults as needed.
- `offset` - Specifies the starting point in the dataset.

#### Returns

- `200` - Success

---

### `GET - /api/author/{authorId}`

Returns detailed information about an author by their unique ID.

#### Parameters

- `authorId` - The unique identifier of the author.

#### Returns

- `200` - Success
- `404` - Author not found

---

### `POST - /api/author`

Creates a new author record. Requires authentication with `Employee` role.

#### Headers

- `Authorization` : Bearer EMPLOYEE_JWT_TOKEN

#### Example Request

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string",
  "description": "string",
  "birthday": "2024-11-01T20:32:56.682Z"
}
```

#### Returns

- `201` - Author created successfully
- `401` - Unauthorized
- `403` - Forbidden

---

### `PUT - /api/author/{authorId}`

Updates an existing author record. Requires authentication with `Employee` role.

#### Headers

- `Authorization` : Bearer EMPLOYEE_JWT_TOKEN

#### Parameters

- `authorId` - The unique identifier of the author to update.

#### Example Request

```json
{
  "name": "string",
  "description": "string",
  "birthday": "2024-11-01T20:33:25.171Z"
}
```

#### Returns

- `204` - No content, update successful
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Author not found

---

### `DELETE - /api/author/{authorId}`

Deletes an author by their unique ID. Requires authentication with `Employee` role.

#### Headers

- `Authorization` : Bearer EMPLOYEE_JWT_TOKEN

#### Parameters

- `authorId` - The unique identifier of the author to delete.

#### Returns

- `204` - No content, delete successful
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Author not found

---

### `PUT - /api/author/{authorId}/photo`

Uploads a photo for the specified author. Requires authentication with `Employee` role.

#### Headers

- `Authorization` : Bearer EMPLOYEE_JWT_TOKEN

#### Parameters

- `authorId` - The unique identifier of the author to upload a photo for.
- `file` - The photo file to upload.

#### Returns

- `204` - No content, upload successful
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Author not found

## Book Endpoints

### `GET - /api/book`

Returns a list of books, optionally filtered by genre or author, with pagination support.

#### Parameters

- `limit` - Sets the maximum number of results to return. Defaults to 50 but can be adjusted as needed.
- `offset` - Specifies the starting point in the dataset for the next set of results.
- `genreId` - Filters results by the specified genre ID.
- `authorId` - Filters results by the specified author ID.

#### Returns

- `200` - Success

---

### `GET - /api/book/recent`

Returns a list of recent books, filtered by genre or author, sorted by creation date.

#### Parameters

- `limit` - Sets the maximum number of results to return.
- `offset` - Specifies the starting point in the dataset.
- `genreId` - Filters results by the specified genre ID.
- `authorId` - Filters results by the specified author ID.

#### Returns

- `200` - Success

---

### `GET - /api/book/popular`

Returns a list of popular books, filtered by genre or author, sorted by popularity.

#### Parameters

- `limit` - Sets the maximum number of results to return.
- `offset` - Specifies the starting point in the dataset.
- `genreId` - Filters results by the specified genre ID.
- `authorId` - Filters results by the specified author ID.

#### Returns

- `200` - Success

---

### `GET - /api/book/recommended`

Returns a list of books recommended for the authenticated user.

#### Parameters

- `limit` - Sets the maximum number of results to return.
- `offset` - Specifies the starting point in the dataset.

#### Returns

- `200` - Success
- `401` - Unauthorized

---

### `GET - /api/book/search/{query}`

Searches for books matching the specified query, with optional genre and author filters.

#### Parameters

- `query` - The search term.
- `limit` - Sets the maximum number of results to return.
- `offset` - Specifies the starting point in the dataset.
- `genreId` - Filters results by the specified genre ID.
- `authorId` - Filters results by the specified author ID.

#### Returns

- `200` - Success

---

### `GET - /api/book/{bookId}`

Returns detailed information about a book by its unique ID.

#### Parameters

- `bookId` - The unique identifier of the book.

#### Returns

- `200` - Success
- `404` - Book not found

---

### `GET - /api/book/isbn/{isbn}`

Returns detailed information about a book by its ISBN.

#### Parameters

- `isbn` - The ISBN of the book.

#### Returns

- `200` - Success
- `404` - Book not found

---

### `POST - /api/book`

Creates a new book record. Requires authentication with `Employee` role.

#### Headers

- `Authorization`: Bearer `EMPLOYEE_JWT_TOKEN`

#### Example Request

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "isbn": "string",
  "title": "string",
  "description": "string",
  "pagesCount": 10000,
  "publishedDate": "2024-11-01T20:31:00.134Z",
  "available": true,
  "genreIds": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  ],
  "authorIds": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  ]
}
```

#### Returns

- `201` - Book created successfully
- `401` - Unauthorized
- `403` - Forbidden

---

### `PUT - /api/book/{bookId}`

Updates an existing book record. Requires authentication with `Employee` role.

#### Headers

- `Authorization`: Bearer `EMPLOYEE_JWT_TOKEN`

#### Parameters

- `bookId` - The unique identifier of the book to update.

#### Example Request

```json
{
  "title": "string",
  "description": "string",
  "pagesCount": 10000,
  "availability": true,
  "publishedDate": "2024-11-01T20:31:22.073Z",
  "genreIds": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  ],
  "authorIds": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

#### Returns

- `204` - No content, update successful
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Book not found

---

### `DELETE - /api/book/{bookId}`

Deletes a book by its unique ID. Requires authentication with `Employee` role.

#### Headers

- `Authorization`: Bearer `EMPLOYEE_JWT_TOKEN`

#### Parameters

- `bookId` - The unique identifier of the book to delete.

#### Returns

- `204` - No content, delete successful
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Book not found

---

### `PUT - /api/book/{bookId}/photo`

Uploads a photo for the specified book. Requires authentication with `Employee` role.

#### Headers

- `Authorization`: Bearer `EMPLOYEE_JWT_TOKEN`

#### Parameters

- `bookId` - The unique identifier of the book to upload a photo for.
- `files` - Form data containing the image files.

#### Returns

- `204` - No content, upload successful
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Book not found

## Book Review Endpoints

### `POST - /api/review`

Creates a new review for a book. Requires authentication with the `User` role.

#### Headers

- `Authorization`: Bearer `USER_JWT_TOKEN`

#### Example Request

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "bookId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "rating": 10,
  "text": "string"
}

```

#### Returns

- `201` - Review created successfully
- `401` - Unauthorized
- `403` - Forbidden

---

### `DELETE - /api/review/{reviewId}`

Deletes a review by its unique ID. Requires authentication with the `User` role and verification that the user is the creator of the review.

#### Headers

- `Authorization`: Bearer `USER_JWT_TOKEN`

#### Parameters

- `reviewId` - The unique identifier of the review to delete.

#### Returns

- `204` - No content, delete successful
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Review not found

---

### `PUT - /api/review/{reviewId}`

Updates an existing review. Requires authentication with the `User` role and verification that the user is the creator of the review.

#### Headers

- `Authorization`: Bearer `USER_JWT_TOKEN`

#### Parameters

- `reviewId` - The unique identifier of the review to update.

#### Example Request

```json
{
 "text": "string",
  "rating": 10
}
```

#### Returns

- `204` - No content, update successful
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Review not found
