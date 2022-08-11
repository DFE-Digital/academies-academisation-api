# academisation api
This is the API for persisted data related to academisation:

* Applications to Become an Academy
* Conversion Projects
* Advisory Board Decisions on Conversions

## Architecture
The architecture of this solution is based on:
* Domain Driven Design
* Hexagon Architecture

The key aspiration of this architecture is to remove any references to technical frameworks (database implementations, web interfaces) from the Domain Layer, so that this layer can focus on the pure logic.  

The following diagram shows the relationship between the projects in the solution:

![Layer Architecture](./domain-hexagon.png)

## Development Setup

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

### Applying migrations
To apply a set of migrations to the database, change to the WebApi directory and run the following command:

```
dotnet ef database update --project ..\Dfe.Academies.Academisation.Data\Dfe.Academies.Academisation.Data.csproj --startup-project Dfe.Academies.Academisation.WebApi.csproj --context Dfe.Academies.Academisation.Data.AcademisationContext
```