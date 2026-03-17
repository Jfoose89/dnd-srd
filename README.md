D&D SRD Rules Database API
A RESTful API built with ASP.NET Core (.NET 9) that serves as a multi-edition D&D and Pathfinder SRD rules database. The API supports four tabletop RPG editions and provides full CRUD operations across monsters, spells, races, classes, and rule entries.

Supported Editions

-D&D 5th Edition — Wizards of the Coast (CC BY 4.0)
-D&D 3.5 Edition — Wizards of the Coast (OGL 1.0a)
-Pathfinder 1e — Paizo (OGL 1.0a)
-Pathfinder 2e — Paizo (ORC License)


Tech Stack

-Framework: ASP.NET Core Web API (.NET 9)
-Database: EF Core In-Memory
-Documentation: Swagger UI
-HTTP Client: IHttpClientFactory with Polly retry/circuit breaker
-External Data: Open5e API


Getting Started
Prerequisites

-.NET 9 SDK
-Visual Studio 2022 or VS Code

Running the API

-Clone the repository
-Open dnd-srd.sln in Visual Studio
-Right-click the project and select Manage User Secrets
-Add the following to secrets.json:

json{
  "ApiKeys": {
    "InternalApiKey": "dnd-srd-secret-key-2024"
  }
}

Press F5 or run dotnet run
The API will be available at https://localhost:7120
Swagger UI is available at https://localhost:7120/swagger


Database Seeding
The database is populated automatically on startup via two seeders:

DbSeeder — manually seeds reference data including editions, monster types, races, classes, spells, and rule entries
ApiSeederService — fetches additional monsters and spells from the Open5e API on startup


Authentication
POST, PUT and DELETE endpoints require an API key passed in the request header:
X-Api-Key: dnd-srd-secret-key-2024
GET endpoints are publicly accessible and do not require authentication.

Rate Limiting
The API uses a Fixed Window rate limiter allowing 30 requests per minute. Exceeding this limit returns 429 Too Many Requests.

Endpoints
Editions

-GET /api/editions — Returns all editions
-POST /api/editions — Creates a new edition (requires API key)
-PUT /api/editions/{id} — Updates an existing edition (requires API key)
-DELETE /api/editions/{id} — Deletes an edition (requires API key)


Monster Types

-GET /api/monstertypes — Returns all monster types
-POST /api/monstertypes — Creates a new monster type (requires API key)
-PUT /api/monstertypes/{id} — Updates a monster type (requires API key)
-DELETE /api/monstertypes/{id} — Deletes a monster type (requires API key)


Monsters

-GET /api/monsters — Returns a paginated list of monsters with optional filters
-GET /api/monsters/{id} — Returns full details for a single monster
-POST /api/monsters — Creates a new monster (requires API key)
-PUT /api/monsters/{id} — Updates an existing monster (requires API key)
-DELETE /api/monsters/{id} — Deletes a monster (requires API key)

Query parameters for GET /api/monsters:

-page — Page number (default: 1)
-pageSize — Results per page (default: 20)
-name — Filter by name (partial match)
-size — Filter by size (e.g. Small, Medium, Large)
-minCR — Minimum challenge rating
-maxCR — Maximum challenge rating
-editionId — Filter by edition
-monsterTypeId — Filter by monster type


Classes

-GET /api/classes — Returns all classes and subclasses
-GET /api/classes/{id} — Returns a single class by ID
-POST /api/classes — Creates a new class or subclass (requires API key)
-PUT /api/classes/{id} — Updates an existing class (requires API key)
-DELETE /api/classes/{id} — Deletes a class (requires API key)


Spells

-GET /api/spells — Returns a paginated list of spells with optional filters
-GET /api/spells/{id} — Returns full details for a single spell
-POST /api/spells — Creates a new spell (requires API key)
-PUT /api/spells/{id} — Updates an existing spell (requires API key)
-DELETE /api/spells/{id} — Deletes a spell (requires API key)

Query parameters for GET /api/spells:

-name — Filter by name (partial match)
-school — Filter by school (e.g. Evocation, Illusion)
-level — Filter by spell level (0 = cantrip)
-editionId — Filter by edition
-classId — Filter by class


Races

-GET /api/races — Returns all races
-GET /api/races/{id} — Returns a single race by ID
-POST /api/races — Creates a new race (requires API key)
-PUT /api/races/{id} — Updates an existing race (requires API key)
-DELETE /api/races/{id} — Deletes a race (requires API key)


Rule Entries

-GET /api/rules — Returns all rule entries with optional filters
-GET /api/rules/{id} — Returns a single rule entry by ID
-POST /api/rules — Creates a new rule entry (requires API key)
-PUT /api/rules/{id} — Updates an existing rule entry (requires API key)
-DELETE /api/rules/{id} — Deletes a rule entry (requires API key)

Query parameters for GET /api/rules:

-category — Filter by category (Weapon, Equipment, Feat)
-editionId — Filter by edition


Error Responses
All errors follow the RFC 7807 ProblemDetails format:
json{
  "type": "https://tools.ietf.org/html/rfc7807",
  "title": "An unexpected error occurred.",
  "status": 500,
  "detail": "Error detail message here.",
  "instance": "/api/monsters"
}
Status codes:

-200 — OK
-201 — Created
-204 — No Content
-400 — Bad Request (invalid input)
-401 — Unauthorized (API key missing)
-403 — Forbidden (invalid API key)
-404 — Not Found
-429 — Too Many Requests (rate limit exceeded)
-500 — Internal Server Error


Project Structure
Controllers

-ClassesController.cs — CRUD endpoints for classes and subclasses
-EditionsController.cs — CRUD endpoints for editions
-MonstersController.cs — CRUD endpoints for monsters with filtering
-MonsterTypesController.cs — CRUD endpoints for monster types
-RacesController.cs — CRUD endpoints for races
-RuleEntriesController.cs — CRUD endpoints for rule entries
-SpellsController.cs — CRUD endpoints for spells with filtering

Data

-AppDbContext.cs — EF Core In-Memory database context
-DbSeeder.cs — Seeds reference data on startup

Models

-Monster.cs — Monster entity
-Spell.cs — Spell entity
-Race.cs — Race entity
-Class.cs — Class and subclass entity
-RuleEntry.cs — Rule entry entity
-Edition.cs — Edition entity
-MonsterType.cs — Monster type entity
-AbilityScores.cs — Monster ability scores
-Action.cs — Monster action
-Trait.cs — Monster trait
-Environment.cs — Monster environment

Services

-ApiKeyMiddleware.cs — Validates API key on POST, PUT, DELETE requests
-ApiSeederService.cs — Fetches monsters and spells from Open5e on startup
-ExceptionMiddleware.cs — RFC 7807 custom exception handling
-CacheExtensions.cs — In-memory caching configuration
-CorsExtensions.cs — CORS policy configuration
-RateLimitingExtensions.cs — Fixed window rate limiter configuration
