# academisation api
 Api for  academisation

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