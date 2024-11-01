# Library System API

## Table of Contents

- **[Summary](#summary)**
- **[How To Build?](#how-to-build)**
- **[How To Run Tests?](#how-to-run-tests)**
- **[Endpoints Overview](#endpoints-overview)**
- **[Test Overview](#tests-overview)**
  - **[Integration Tests](#integration-tests)**
  - **[Unit Tests](#unit-tests)**
- **[Architecture Overview](#architecture-overview)**
- **[Notes](#notes)**

## Summary

- This is a **REST DEMO API** project for a library system.
- There is ***45*** endpoints in total, with over ***80*** **unit and integration** tests
- Uses a **PostgreSQL** database with **Entity Framework**
- **JWT** authentication and authorization - (User, Employee, Admin policies)
- Project is utilizing the **Clean Architecture**
- Features **Users, Employees, Books, Authors, Genres, Borrow System, Reviews**
- Features **searching**, **recommending** and **querying** books with various filters
- Features **Email Notifications** on some endpoints.

## How To Build?

You will need to have installed:

- **Dotnet SDK 8.0.107** and higher
- **Postgres**

After that clone the git repo into the desired folder like this:

``` bash
git clone https://github.com/realtobi999/ASP.NET_LibrarySystem.git
```

Open the cloned folder and navigate to the presentation layer of the project:

```bash
cd src/LibrarySystem.Presentation
```

After that open ***appsettings.json*** in your favorite editor and modify the settings as such:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "LibrarySystem": "Host=YOUR_HOST;Username=YOUR_USERNAME;Password=YOUR_PASSWORD;Database=YOUR_DATABASE_NAME"
  },
  "Jwt": {
    "Key": "YOUR_KEY",
    "Issuer": "YOUR_ISSUER"
  },
  "SMTP": {
    "Host": "YOUR_HOST",
    "Port": "YOUR_POST",
    "Username": "YOUR_USERNAME",
    "Password": "YOUR_PASSWORD"
  }
}
```

After that you can finally go back to the **root** folder and run the project:

``` bash
cd ../.. && make run
```

**OR** run it directly from the presentation layer:

``` bash
dotnet run
```

**Expected Output:**

![successful_build](./doc/sucessful_build.png)

## How To Run Tests?

I love to hear that you care about that! It's simple if you got the app running successfull just run this command in the **root** folder:

``` bash
make test
```

**Expected Output:**

![passed_tests](./doc/passed_tests.png)

## Endpoints Overview

In total there is about **35+** endpoints

![swagger.gif](./doc/swagger.gif)

**Check out each endpoint in detail [here.](./doc/endpoints.md)**

## Tests Overview

In total there is about **80+** tests, both **integration** and **unit** tests. I've tried to cover most edge cases with the integration tests.

### **Integration Tests:**

``` bash
.
├── Endpoints
│   ├── AuthControllerTests.cs
│   ├── AuthorControllerTests.cs
│   ├── BookControllerTests.cs
│   ├── BookReviewControllerTests.cs
│   ├── BorrowControllerTests.cs
│   ├── EmployeeControllerTests.cs
│   ├── GenreControllerTests.cs
│   ├── UserControllerTests.cs
│   └── WishlistControllerTests.cs
├── Helpers
│   ├── AuthorTestExtensions.cs
│   ├── BookReviewTestExtensions.cs
│   ├── BookTextExtensions.cs
│   ├── BorrowTestExtensions.cs
│   ├── EmployeeTestExtensions.cs
│   ├── GenreTestExtensions.cs
│   ├── JwtTestExtensions.cs
│   ├── UserTestExtensions.cs
│   └── WishlistTestExtensions.cs
├── Middlewares
│   ├── EmployeeAuthenticationMiddlewareTests.cs
│   └── UserAuthenticationMiddlewareTests.cs
└── Server
    ├── TestServiceExtensions.cs
    └── WebAppFactory.cs
```

The Extensions folder is for helper extension methods that come in handy during testing. Server folder is where the testing server is defined.

### **Unit Tests:**

```bash
.
├── Books
│   ├── BookCalculatorTests.cs
│   ├── BookRecommenderTests.cs
│   └── BookSearcherTests.cs
├── Utilities
│   ├── AuthenticationMiddlewareBaseTests.cs
│   ├── HasherTests.cs
│   └── JwtTests.cs
└── Validators
    ├── BookReviewValidatorTests.cs
    ├── BookValidatorTests.cs
    ├── BorrowValidatorTests.cs
    └── WishlistValidatorTests.cs
```

## Architecture Overview

This project utilizes a **Clean Architecture** paradigm. Which is very widely known for its scalability and its perfect for bigger sized projects.

```bash
.
├── LibrarySystem.Application // application layer
│   ├── Core
│   │   ├── Attributes
│   │   ├── Configuration
│   │   ├── Emails
│   │   │   └── Services
│   │   ├── Extensions
│   │   ├── Factories
│   │   ├── Mappers
│   │   ├── Utilities
│   │   └── Validators
│   └── Services
│       ├── Authors
│       ├── Books
│       ├── Borrows
│       ├── Genres
│       ├── Pictures
│       ├── Reviews
│       ├── Staffs
│       ├── Users
│       └── Wishlists
├── LibrarySystem.Domain // domain layer
│   ├── Dtos
│   │   ├── Authors
│   │   ├── Books
│   │   ├── Borrows
│   │   ├── Email
│   │   │   └── Messages
│   │   ├── Employees
│   │   ├── Genres
│   │   ├── Responses
│   │   ├── Reviews
│   │   ├── Users
│   │   └── Wishlists
│   ├── Entities
│   ├── Enums
│   ├── Exceptions
│   │   ├── Common
│   │   └── HTTP
│   └── Interfaces
│       ├── Common
│       ├── Emails
│       │   └── Borrow
│       ├── Factories
│       ├── Managers
│       ├── Repositories
│       └── Services
│           ├── Books
│           └── Services
├── LibrarySystem.Infrastructure // infrastructure layer
│   ├── Factories
│   ├── Messages
│   │   ├── Builders
│   │   └── Html
│   └── Persistence
│       ├── Extensions
│       ├── Migrations
│       └── Repositories
└── LibrarySystem.Presentation // presentation layer
    ├── Controllers
    ├── Extensions
    ├── Middlewares
    │   ├── Filters
    │   └── Handlers
    ├── Properties
    └── Services

```

## Notes

In this project i learned a lot about ASP.NET and specifically about API's. I've implemented various authentication middlewares and authorization policies, implemented the clean architecture design, got better at writing integration tests, learned how to implement an **EMAIL** service and how to use the Razor Engine for custom email HTML. I've also got better at writing bussiness logic and implemented various features.
