# LeadRouteOptimizer

## Overview

Lead Route Optimizer is a small web api built as a time‑boxed technical exercise. The goal of the system is to help ISRs (Inside Sales Representatives) optimize the order in which they visit sales leads by minimizing total travel distance.

The application allows an ISR to:

1. Upload one or more CSV lead lists (manager or personal)
2. Combine those leads into a single working set
3. Generate an ordered visit plan starting from their home location
4. Retrieve the ordered route with per‑stop and total distance information

---

## Key Design Decisions

### Time‑boxed scope

This project was created under a time constraint, so some things were done in a certain way:

* No external geocoding or mapping APIs are used
* All leads **must include latitude and longitude** in the CSV
* Distances are calculated using the Haversine formula: https://en.wikipedia.org/wiki/Haversine_formula

### Architecture

The solution follows a sort of **clean architecture**:

```
LeadRouteOptimizer
├── Api            (HTTP endpoints, controllers, Swagger)
├── Application    (use cases, handlers, abstractions)
├── Domain         (entities, routing logic)
├── Infrastructure (EF Core, SQL Server, CSV parsing)
├── Tests          (xUnit integration tests)
```

Each layer has a single responsibility:

* **Domain**: pure business logic (entities, routing algorithm)
* **Application**: orchestration and use cases
* **Infrastructure**: persistence and external concerns
* **API**: HTTP surface only
* **Tests**: black‑box validation of system behavior

---

## Technology Stack

### Backend

* **.NET 8**
* **ASP.NET Core Web API**
* **Entity Framework Core** (SQL Server)
* **CsvHelper** (CSV parsing)
* **Swagger / OpenAPI**

### Database

* **Microsoft SQL Server**
* Code‑first mapping with EF Core
* Simple relational schema (no stored procedures)

### Testing

* **xUnit**
* **Microsoft.AspNetCore.Mvc.Testing**
* **FluentAssertions**

---

## Database Model

* **UploadBatch** – represents one CSV upload (Manager or Personal)
* **Lead** – parsed rows from CSV (valid or invalid)
* **RoutePlan** – one generated visit plan
* **RouteStop** – ordered stops within a plan
* **RoutePlanUpload** – join table linking plans to their source uploads

Invalid CSV rows are stored with error messages for transparency.

---

## API Endpoints

### Upload Leads

`POST /uploads`

* Content‑Type: `multipart/form-data`
* Form fields:

  * `sourceType`: `Manager` | `Personal`
  * `file`: CSV file

**Response**:

```json
{
  "uploadBatchId": "guid",
  "totalRows": 10,
  "validRows": 8,
  "invalidRows": 2
}
```

### Create Route Plan

`POST /plans`

**Body**:

```json
{
  "homeLatitude": 41.8781,
  "homeLongitude": -87.6298,
  "uploadBatchIds": ["guid", "guid"]
}
```

**Response**:

```json
{
  "planId": "guid",
  "stops": 5,
  "totalDistanceKm": 123.45
}
```

### Get Route Plan

`GET /plans/{planId}`

Returns ordered stops, per‑leg distances, and total distance.

---

## Routing Logic

* All valid leads across selected uploads are combined
* Leads are deduplicated using a deterministic key
* Route is generated using a **Nearest‑Neighbor heuristic**
* Distances are calculated using the **Haversine formula**

---

## Running the Project Locally

### Prerequisites

* .NET 8 SDK
* SQL Server or LocalDB

### Database Setup

Run the provided SQL setup script:

```
db setup.sql
```

Update `appsettings.Development.json` with your SQL connection string:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=LeadRoutePlanner;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### Run API

```bash
dotnet run --project LeadRouteOptimizer.Api
```

Swagger UI will be available at:

```
https://localhost:{port}/swagger
```

---

## Testing

The solution includes **one integration test** that validates:

* CSV uploads (manager + personal)
* Route plan generation
* Route retrieval

Run tests with:

```bash
dotnet test
```

---

## Current Limitations

* Straight‑line distance only, so no driving distance for now
* No authentication or user accounts
* No UI, I focused on the backend for now

---

Further enhancements are documented in `roadmap.md`.
