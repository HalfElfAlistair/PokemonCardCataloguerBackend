# Pokémon Card Cataloguer — Experimental ASP.NET Core API

An experimental backend API built with ASP.NET Core and EF Core. This repo was created as a learning project, to get to grips with using C# and .NET, but doubles up as a potential future replacement for the Firebase backend.

## Status:  
Experimental — not currently used in production.

## Purpose

- Learn C# and ASP.NET Core through practical application

- Explore replacing Firebase with a custom backend

- Experiment with EF Core, validation, and clean architecture

## Tech Stack

- .NET 10

- ASP.NET Core Web API

- EF Core (SQLite). Current work in progress

- FluentValidation

- DTOs for request/response shaping

- Validators for input safety

## Endpoints

### GET

- /users (temporary, will be removed)

- /users/{id}

- /cards

- /cards/{id}

- /lists

- /lists/{id}

### POST

- Create user

- Create card

- Create list

### PUT

- Update user

- Update card

- Update list name

- Update list content

### DELETE

- Remove user

- Remove card

- Remove list

### Data Layer

- SQLite used for local development

- EF Core migrations exist but are not part of a documented workflow

- DTOs and validators ensure clean, safe data flow

### Running Locally

#### Clone the repo:

git clone <repo-url>
cd <repo-folder>

#### Run the API:

dotnet run

#### The API will start on the default Kestrel port.

## Notes on Migrations

- Migrations were created during development

- They were replaced frequently during experimentation

- No formal database update workflow is included

## Status & Future Direction

This repo is not currently used by the Pokémon Card Cataloguer project.
It remains a useful reference for:

- C# learning

- API design experimentation

- Potential future backend replacement
