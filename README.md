D&D SRD Rules Database API
A RESTful API built with ASP.NET Core (.NET 9) that serves as a multi-edition D&D and Pathfinder SRD rules database. The API supports four tabletop RPG editions and provides full CRUD operations across monsters, spells, races, classes, and rule entries.
Supported Editions
EditionPublisherLicenseD&D 5th EditionWizards of the CoastCC BY 4.0D&D 3.5 EditionWizards of the CoastOGL 1.0aPathfinder 1ePaizoOGL 1.0aPathfinder 2ePaizoORC License

Tech Stack

Framework: ASP.NET Core Web API (.NET 9)
Database: EF Core In-Memory
Documentation: Swagger UI
HTTP Client: IHttpClientFactory with Polly retry/circuit breaker
External Data: Open5e API (https://api.open5e.com/v1/)


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
GET /api/editions
Returns all editions.
Response 200 OK:
json[
  {
    "id": 1,
    "name": "D&D 5th Edition",
    "releaseYear": 2014,
    "publisher": "Wizards of the Coast",
    "licenseType": "CC BY 4.0"
  }
]
POST /api/editions
Creates a new edition. Requires API key.
Request body:
json{
  "name": "D&D 4th Edition",
  "releaseYear": 2008,
  "publisher": "Wizards of the Coast",
  "licenseType": "Proprietary"
}
Response 201 Created
PUT /api/editions/{id}
Updates an existing edition. Requires API key.
Response 200 OK
DELETE /api/editions/{id}
Deletes an edition. Requires API key.
Response 204 No Content

Monster Types
GET /api/monstertypes
Returns all monster types.
Response 200 OK:
json[
  {
    "id": 1,
    "name": "Dragon",
    "description": "Large reptilian creatures of ancient power"
  }
]
POST /api/monstertypes
Creates a new monster type. Requires API key.
Request body:
json{
  "name": "Fey",
  "description": "Magical creatures tied to the Feywild"
}
Response 201 Created
PUT /api/monstertypes/{id}
Updates an existing monster type. Requires API key.
Response 200 OK
DELETE /api/monstertypes/{id}
Deletes a monster type. Requires API key.
Response 204 No Content

Monsters
GET /api/monsters
Returns a paginated list of monsters. Supports filtering via query parameters.
Query parameters:
ParameterTypeDescriptionpageintPage number (default: 1)pageSizeintResults per page (default: 20)namestringFilter by name (partial match)sizestringFilter by size (e.g. Small, Medium, Large)minCRdoubleMinimum challenge ratingmaxCRdoubleMaximum challenge ratingeditionIdintFilter by editionmonsterTypeIdintFilter by monster type
Response 200 OK:
json{
  "data": [
    {
      "id": 1,
      "name": "Goblin",
      "challengeRating": 0.25,
      "size": "Small",
      "alignment": "Neutral Evil",
      "monsterTypeName": "Humanoid",
      "editionName": "D&D 5th Edition",
      "sourceUrl": "/api/monsters/1"
    }
  ],
  "page": 1,
  "pageSize": 20,
  "totalCount": 8
}
GET /api/monsters/{id}
Returns full details for a single monster including ability scores, actions, traits and environments.
Response 200 OK:
json{
  "id": 1,
  "name": "Goblin",
  "description": "Small, black-hearted humanoids that lair in caves and forests.",
  "hitPoints": 7,
  "armorClass": 15,
  "walkSpeed": 30,
  "flySpeed": null,
  "swimSpeed": null,
  "burrowSpeed": null,
  "challengeRating": 0.25,
  "size": "Small",
  "alignment": "Neutral Evil",
  "monsterTypeName": "Humanoid",
  "editionName": "D&D 5th Edition",
  "abilityScores": {
    "strength": 8,
    "dexterity": 14,
    "constitution": 10,
    "intelligence": 10,
    "wisdom": 8,
    "charisma": 8,
    "strengthModifier": -1,
    "dexterityModifier": 2,
    "constitutionModifier": 0,
    "intelligenceModifier": 0,
    "wisdomModifier": -1,
    "charismaModifier": -1
  },
  "actions": [
    {
      "id": 1,
      "name": "Scimitar",
      "description": "Melee Weapon Attack",
      "attackBonus": 4,
      "damageDice": "1d6+2",
      "damageType": "Slashing",
      "actionType": "Action"
    }
  ],
  "traits": [
    {
      "id": 1,
      "name": "Nimble Escape",
      "description": "The goblin can take the Disengage or Hide action as a bonus action."
    }
  ],
  "environments": [
    { "id": 1, "name": "Forest" },
    { "id": 2, "name": "Cave" }
  ]
}
Response 404 Not Found if monster does not exist.
POST /api/monsters
Creates a new monster. Requires API key.
Request body:
json{
  "name": "Owlbear",
  "description": "A magical beast with the body of a bear and head of an owl.",
  "hitPoints": 59,
  "armorClass": 13,
  "walkSpeed": 40,
  "flySpeed": null,
  "swimSpeed": null,
  "burrowSpeed": null,
  "challengeRating": 3,
  "size": "Large",
  "alignment": "Unaligned",
  "editionId": 1,
  "monsterTypeId": 3
}
Response 201 Created
PUT /api/monsters/{id}
Updates an existing monster. Requires API key.
Response 200 OK or 404 Not Found
DELETE /api/monsters/{id}
Deletes a monster. Requires API key.
Response 204 No Content or 404 Not Found

Classes
GET /api/classes
Returns all classes and subclasses.
Response 200 OK:
json[
  {
    "id": 1,
    "name": "Wizard",
    "description": "A scholarly magic-user capable of manipulating the structures of reality.",
    "hitDie": "d6",
    "primaryAbility": "Intelligence",
    "savingThrows": "Intelligence, Wisdom",
    "editionName": "D&D 5th Edition",
    "parentClassName": null
  }
]
GET /api/classes/{id}
Returns a single class by ID.
Response 200 OK or 404 Not Found
POST /api/classes
Creates a new class or subclass. Requires API key.
Request body (base class):
json{
  "name": "Paladin",
  "description": "A holy warrior bound by a sacred oath.",
  "hitDie": "d10",
  "primaryAbility": "Strength and Charisma",
  "savingThrows": "Wisdom, Charisma",
  "armorProficiencies": "All armor, shields",
  "weaponProficiencies": "Simple weapons, martial weapons",
  "editionId": 1,
  "parentClassId": null
}
Response 201 Created
PUT /api/classes/{id}
Updates an existing class. Requires API key.
Response 200 OK
DELETE /api/classes/{id}
Deletes a class. Requires API key.
Response 204 No Content

Spells
GET /api/spells
Returns a paginated list of spells. Supports filtering via query parameters.
Query parameters:
ParameterTypeDescriptionnamestringFilter by name (partial match)schoolstringFilter by school (e.g. Evocation, Illusion)levelintFilter by spell level (0 = cantrip)editionIdintFilter by editionclassIdintFilter by class
Response 200 OK:
json{
  "data": [
    {
      "id": 1,
      "name": "Fireball",
      "level": 3,
      "school": "Evocation",
      "castingTime": "1 action",
      "range": "150 feet",
      "duration": "Instantaneous",
      "concentration": false,
      "ritual": false,
      "editionName": "D&D 5th Edition",
      "className": "Wizard"
    }
  ],
  "page": 1,
  "pageSize": 20,
  "totalCount": 8
}
GET /api/spells/{id}
Returns full details for a single spell.
Response 200 OK or 404 Not Found
POST /api/spells
Creates a new spell. Requires API key.
Request body:
json{
  "name": "Lightning Bolt",
  "description": "A stroke of lightning forming a line 100 feet long blasts out from you.",
  "level": 3,
  "school": "Evocation",
  "castingTime": "1 action",
  "range": "Self (100-foot line)",
  "duration": "Instantaneous",
  "concentration": false,
  "ritual": false,
  "components": "V, S, M",
  "editionId": 1,
  "classId": 1
}
Response 201 Created
PUT /api/spells/{id}
Updates an existing spell. Requires API key.
Response 200 OK
DELETE /api/spells/{id}
Deletes a spell. Requires API key.
Response 204 No Content

Races
GET /api/races
Returns all races.
Response 200 OK:
json[
  {
    "id": 1,
    "name": "Elf",
    "description": "Elves are a magical people of otherworldly grace.",
    "speed": 30,
    "size": "Medium",
    "abilityScoreBonus": "+2 Dexterity",
    "traits": "Darkvision, Keen Senses, Fey Ancestry, Trance",
    "languages": "Common, Elvish",
    "editionName": "D&D 5th Edition"
  }
]
GET /api/races/{id}
Returns a single race by ID.
Response 200 OK or 404 Not Found
POST /api/races
Creates a new race. Requires API key.
Request body:
json{
  "name": "Tiefling",
  "description": "Tieflings are derived from human bloodlines infused with the power of Asmodeus.",
  "speed": 30,
  "size": "Medium",
  "abilityScoreBonus": "+1 Intelligence, +2 Charisma",
  "traits": "Darkvision, Hellish Resistance, Infernal Legacy",
  "languages": "Common, Infernal",
  "editionId": 1
}
Response 201 Created
PUT /api/races/{id}
Updates an existing race. Requires API key.
Response 200 OK
DELETE /api/races/{id}
Deletes a race. Requires API key.
Response 204 No Content

Rule Entries
GET /api/rules
Returns all rule entries. Supports filtering via query parameters.
Query parameters:
ParameterTypeDescriptioncategorystringFilter by category (Weapon, Equipment, Feat)editionIdintFilter by edition
Response 200 OK:
json[
  {
    "id": 1,
    "name": "Longsword",
    "category": "Weapon",
    "description": "A versatile martial melee weapon.",
    "cost": "15 gp",
    "weight": 3,
    "damageDice": "1d8",
    "damageType": "Slashing",
    "editionName": "D&D 5th Edition"
  }
]
GET /api/rules/{id}
Returns a single rule entry by ID.
Response 200 OK or 404 Not Found
POST /api/rules
Creates a new rule entry. Requires API key.
Request body:
json{
  "name": "Greatsword",
  "category": "Weapon",
  "description": "A large two-handed martial melee weapon.",
  "cost": "50 gp",
  "weight": 6,
  "damageDice": "2d6",
  "damageType": "Slashing",
  "editionId": 1
}
Response 201 Created
PUT /api/rules/{id}
Updates an existing rule entry. Requires API key.
Response 200 OK
DELETE /api/rules/{id}
Deletes a rule entry. Requires API key.
Response 204 No Content

Project Structure
dnd-srd/
├── Controllers/
│   ├── ClassesController.cs
│   ├── EditionsController.cs
│   ├── MonstersController.cs
│   ├── MonsterTypesController.cs
│   ├── RacesController.cs
│   ├── RuleEntriesController.cs
│   └── SpellsController.cs
├── Data/
│   ├── AppDbContext.cs
│   └── DbSeeder.cs
├── DTOs/
│   ├── Requests/
│   └── Responses/
├── Models/
│   ├── AbilityScores.cs
│   ├── Action.cs
│   ├── Class.cs
│   ├── Edition.cs
│   ├── Environment.cs
│   ├── Monster.cs
│   ├── MonsterType.cs
│   ├── Race.cs
│   ├── RuleEntry.cs
│   ├── Spell.cs
│   └── Trait.cs
├── Services/
│   ├── ApiKeyMiddleware.cs
│   ├── ApiSeederService.cs
│   ├── CacheExtensions.cs
│   ├── CorsExtensions.cs
│   ├── ExceptionMiddleware.cs
│   └── RateLimitingExtensions.cs
└── Program.cs

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
