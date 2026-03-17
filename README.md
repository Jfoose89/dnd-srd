D&D SRD Rules Database API
A RESTful API built with ASP.NET Core (.NET 9) that serves as a multi-edition D&D and Pathfinder SRD rules database. The API supports four tabletop RPG editions and provides full CRUD operations across monsters, spells, races, classes, and rule entries.

Supported Editions
EditionPublisherLicenseD&D 5th EditionWizards of the CoastCC BY 4.0D&D 3.5 EditionWizards of the CoastOGL 1.0aPathfinder 1ePaizoOGL 1.0aPathfinder 2ePaizoORC License

Tech Stack

Framework: ASP.NET Core Web API (.NET 9)
Database: EF Core In-Memory
Documentation: Swagger UI
HTTP Client: IHttpClientFactory with Polly retry/circuit breaker
External Data: Open5e API


Getting Started
Prerequisites

.NET 9 SDK
Visual Studio 2022 or VS Code

Running the API

Clone the repository
Open dnd-srd.sln in Visual Studio
Right-click the project and select Manage User Secrets
Add the following to secrets.json:

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
MethodEndpointDescriptionAuth RequiredGET/api/editionsReturns all editionsNoPOST/api/editionsCreates a new editionYesPUT/api/editions/{id}Updates an existing editionYesDELETE/api/editions/{id}Deletes an editionYes

Monster Types
MethodEndpointDescriptionAuth RequiredGET/api/monstertypesReturns all monster typesNoPOST/api/monstertypesCreates a new monster typeYesPUT/api/monstertypes/{id}Updates a monster typeYesDELETE/api/monstertypes/{id}Deletes a monster typeYes

Monsters
MethodEndpointDescriptionAuth RequiredGET/api/monstersReturns paginated list with filtersNoGET/api/monsters/{id}Returns full monster detailsNoPOST/api/monstersCreates a new monsterYesPUT/api/monsters/{id}Updates an existing monsterYesDELETE/api/monsters/{id}Deletes a monsterYes
Query Parameters for GET /api/monsters:
ParameterTypeDescriptionpageintPage number (default: 1)pageSizeintResults per page (default: 20)namestringFilter by name (partial match)sizestringFilter by size (e.g. Small, Medium, Large)minCRdoubleMinimum challenge ratingmaxCRdoubleMaximum challenge ratingeditionIdintFilter by editionmonsterTypeIdintFilter by monster type

Classes
MethodEndpointDescriptionAuth RequiredGET/api/classesReturns all classes and subclassesNoGET/api/classes/{id}Returns a single class by IDNoPOST/api/classesCreates a new class or subclassYesPUT/api/classes/{id}Updates an existing classYesDELETE/api/classes/{id}Deletes a classYes

Spells
MethodEndpointDescriptionAuth RequiredGET/api/spellsReturns paginated list with filtersNoGET/api/spells/{id}Returns full spell detailsNoPOST/api/spellsCreates a new spellYesPUT/api/spells/{id}Updates an existing spellYesDELETE/api/spells/{id}Deletes a spellYes
Query Parameters for GET /api/spells:
ParameterTypeDescriptionnamestringFilter by name (partial match)schoolstringFilter by school (e.g. Evocation, Illusion)levelintFilter by spell level (0 = cantrip)editionIdintFilter by editionclassIdintFilter by class

Races
MethodEndpointDescriptionAuth RequiredGET/api/racesReturns all racesNoGET/api/races/{id}Returns a single race by IDNoPOST/api/racesCreates a new raceYesPUT/api/races/{id}Updates an existing raceYesDELETE/api/races/{id}Deletes a raceYes

Rule Entries
MethodEndpointDescriptionAuth RequiredGET/api/rulesReturns all rule entries with filtersNoGET/api/rules/{id}Returns a single rule entry by IDNoPOST/api/rulesCreates a new rule entryYesPUT/api/rules/{id}Updates an existing rule entryYesDELETE/api/rules/{id}Deletes a rule entryYes
Query Parameters for GET /api/rules:
ParameterTypeDescriptioncategorystringFilter by category (Weapon, Equipment, Feat)editionIdintFilter by edition

Error Responses
All errors follow the RFC 7807 ProblemDetails format:
json{
  "type": "https://tools.ietf.org/html/rfc7807",
  "title": "An unexpected error occurred.",
  "status": 500,
  "detail": "Error detail message here.",
  "instance": "/api/monsters"
}
Status CodeMeaning200OK201Created204No Content400Bad Request — invalid input401Unauthorized — API key missing403Forbidden — invalid API key404Not Found429Too Many Requests — rate limit exceeded500Internal Server Error

Project Structure
Controllers
FileDescriptionClassesController.csCRUD endpoints for classes and subclassesEditionsController.csCRUD endpoints for editionsMonstersController.csCRUD endpoints for monsters with filteringMonsterTypesController.csCRUD endpoints for monster typesRacesController.csCRUD endpoints for racesRuleEntriesController.csCRUD endpoints for rule entriesSpellsController.csCRUD endpoints for spells with filtering
Data
FileDescriptionAppDbContext.csEF Core In-Memory database contextDbSeeder.csSeeds reference data on startup
Models
FileDescriptionMonster.csMonster entitySpell.csSpell entityRace.csRace entityClass.csClass and subclass entityRuleEntry.csRule entry entityEdition.csEdition entityMonsterType.csMonster type entityAbilityScores.csMonster ability scoresAction.csMonster actionTrait.csMonster traitEnvironment.csMonster environment
Services
FileDescriptionApiKeyMiddleware.csValidates API key on POST, PUT, DELETE requestsApiSeederService.csFetches monsters and spells from Open5e on startupExceptionMiddleware.csRFC 7807 custom exception handlingCacheExtensions.csIn-memory caching configurationCorsExtensions.csCORS policy configurationRateLimitingExtensions.csFixed window rate limiter configuration
