# academisation api
This is the API for persisted data related to academisation:

* Applications to Become an Academy
* Conversion Projects
* Advisory Board Decisions on Conversions

## Context

This API is intended to own the business logic and 
store data related to Preparing for Academisation: the process by which a Maintained School becomes an Academy. This fits within the responsibilities owned by the Regional Services Division:

![Domain Contexts](./domain-contexts.png)

## Architecture
The architecture of this solution is based on:
* Domain Driven Design
* Hexagon Architecture

The key aspiration of this architecture is to remove any references to technical frameworks (database implementations, web interfaces) from the Domain Layer, so that this layer can focus on the pure logic.  

The following diagram shows the relationship between the projects in the solution:

![Domain/Hexagon Architecture](./domain-hexagon.png)

Each of the layers in this architecture has a distinct set of responsibilities:

| Layer   | Responsibility |
|---------|----------------|
| Web     | Mapping to HTTP |
| Service | Co-ordination of a complete operation |
| Domain  | Validation and Mutation |
| Data    | Persistence |

## Development Setup

### Authentication

When running locally the webapp does not require authentication.  Once deployed the
authentication keys should be provided as a configuration setting.  For best practice
this key should be provided as a UUID:

```
"AuthenticationConfig__ApiKeys__0" -> "<Key>"
"AuthenticationConfig__ApiKeys__1" -> "<Key>"
```

### EntityFramework and Migrations

Set your database connection string in user secrets:

"AcademiesDatabaseConnectionString": "connection string here"

Install the Entity Framework Core CLI Tools:

```
dotnet tool install --global dotnet-ef
```

### Generating migrations

To generate migrations, change to the WebApi directory and run the following command:

```
dotnet ef migrations add <MIGRATION_NAME> --project ..\Dfe.Academies.Academisation.Data\Dfe.Academies.Academisation.Data.csproj --startup-project Dfe.Academies.Academisation.WebApi.csproj --context Dfe.Academies.Academisation.Data.AcademisationContext 
```

OR the following within Visual Studio package manager console with the the 'WebApi' project selected as a startup:-
```
dotnet ef migrations add WantConversionToHappenOnSelectedDate --project Dfe.Academies.Academisation.Data --startup-project Dfe.Academies.Academisation.WebApi --context Dfe.Academies.Academisation.Data.AcademisationContext
```


### Applying migrations
To apply a set of migrations to the database, change to the WebApi directory and run the following command:

```
dotnet ef database update --project ..\Dfe.Academies.Academisation.Data\Dfe.Academies.Academisation.Data.csproj --startup-project Dfe.Academies.Academisation.WebApi.csproj --context Dfe.Academies.Academisation.Data.AcademisationContext
```

## Getting Started
### Code commit comment rules
Nothing formal, but been using the following pattern:
AB#105435 - conversion target date - store whether target date has been chosen - 'description of this commit'
