# URL Shortener with Analytics (.NET 8)

A production-ready URL shortener built with **.NET 8**, **Clean Architecture**, and **PostgreSQL**, designed for high-performance redirects, scalable analytics, and operational reliability.

---

## ✨ Features

- Short URL creation with collision-safe codes
- Fast redirects using Redis caching
- Click analytics with read-optimized queries
- Asynchronous click event publishing
- PostgreSQL persistence with EF Core
- Redis used for caching and messaging
- Global rate limiting
- Health checks for critical dependencies
- Swagger / OpenAPI documentation
- Structured logging with Serilog
- Dockerized for local and production environments

---

## 🏗 Architecture

The project follows **Clean Architecture** principles with strict separation of concerns.

```text
src/
├── UrlShortener.Api             # HTTP layer (controllers, middleware)
├── UrlShortener.Application     # Use cases, interfaces, services
├── UrlShortener.Domain          # Core entities and domain rules
├── UrlShortener.Infrastructure # Persistence, Redis, repositories
└── UrlShortener.Worker          # Background processing (analytics)

```


### Dependency Rules
- API → Application → Domain
- Infrastructure depends on Application
- Domain has no dependencies

---

## 🔁 Core Flows

### Redirect Flow
1. Client requests `/abc123`
2. Redis cache lookup
3. PostgreSQL fallback on cache miss
4. HTTP 302 redirect
5. Click event published asynchronously

### Analytics Flow
1. Click events processed asynchronously
2. Aggregated per short URL
3. Read-optimized analytics queries
4. No impact on redirect latency

---

## 📦 Tech Stack

- .NET 8
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- Redis
- Serilog
- Docker & Docker Compose
- Swagger / OpenAPI

---

## 🚀 Getting Started

### Prerequisites
- .NET SDK 8+
- Docker & Docker Compose

---

## ▶ Run with Docker

```bash
docker compose up -d
```

This starts:

API
PostgreSQL
Redis

## ▶ Run Locally (Without Docker)

- Configure appsettings.json
- Apply migrations
- Start the API

```bash
dotnet ef database update
dotnet run --project src/UrlShortener.Api

```
## 📄 API Endpoints

- Create Short URL
  POST /api/urls

- Redirect
- GET /{shortCode}
- Returns HTTP 302 redirect.

- Analytics
- GET /api/urls/{shortCode}/analytics

- Response -
- {
  "shortCode": "abc123",
  "clicks": 42
}

## 📊 Health Checks
 - GET /health

- Checks:
- PostgreSQL
- Redis

## 📘 Swagger

- Available in development:
- /swagger

## 🛡 Rate Limiting

- Fixed window rate limiter
- Configurable limits
- Applied globally to protect the API

## 🧠 Caching Strategy

- Redis cache-aside pattern
- ShortCode → LongUrl mapping cached
- Explicit TTLs
- Database used as source of truth

## 📝 Logging & Observability

- Structured logging with Serilog
- Request logging enabled
- Context enrichment (machine, thread, request)

## 🔐 Reliability & Design Considerations

- Async analytics to keep redirects fast
- Cancellation token support
- Graceful failure handling
- Dependency health monitoring

## 🧪 Verification Checklist

- Short URLs redirect correctly
- Redis cache reduces DB calls
- Analytics counts increment correctly
- Health endpoint reports healthy
- Swagger loads successfully
- Rate limiting enforced

## 📌 Notes

- Designed for horizontal scalability
- Optimized for read-heavy traffic
- Easy to extend with authentication, dashboards, or custom domains

## 👤 Author

**Abhishek Keshri**  
Backend & Systems Engineer

