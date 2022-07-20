# academisation api
 Api for  academisation

## Development Setup

### EntityFramework and Migrations

### Generating migrations

To generate migrations use the following command from the WebApi folder:

```
dotnet ef migrations add <MIGRATION_NAME> --project ..\Dfe.Academies.Academisation.Data\Dfe.Academies.Academisation.Data.csproj --startup-project Dfe.Academies.Academisation.WebApi.csproj --context Dfe.Academies.Academisation.Data.AcademisationContext 
```

### Applying migrations
To apply a set of migrations to the database, use `dotnet ef database update`:

```
dotnet ef database update --project ..\Dfe.Academies.Academisation.Data\Dfe.Academies.Academisation.Data.csproj --startup-project Dfe.Academies.Academisation.WebApi.csproj --context Dfe.Academies.Academisation.Data.AcademisationContext
```