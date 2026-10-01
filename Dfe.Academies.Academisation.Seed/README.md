# Academisation Seeder

This project seeds Academisation test data for:

- Conversion projects
- Transfer projects
- Significant change projects

## Run the seeder

Use the **Seeder** launch profile and run the project.

When prompted, enter the number of projects to create **for each project type**.

For example:

```text
Enter the number of projects to seed for each type...
5
```

This will create:

- 5 conversions
- 5 transfers
- 5 significant changes

## Database configuration

The seeder requires a database connection string in the following configuration shape:

```json
{
  "DatabaseConfig": {
    "ConnectionString": "your-connection-string"
  }
}
```

### Option 1 — User Secrets

From the `Dfe.Academies.Academisation.Seed` project directory:

```bash
dotnet user-secrets init
dotnet user-secrets set "DatabaseConfig:ConnectionString" "your-connection-string"
```

You can check the configured secrets with:

```bash
dotnet user-secrets list
```

### Option 2 — Environment variable

Set:

```text
DatabaseConfig__ConnectionString
```

For example in PowerShell:

```powershell
$env:DatabaseConfig__ConnectionString="your-connection-string"
```

Then run the project using the **Seeder** profile.

Alternatively, navigate to the seeder project and just run it

```
cd Dfe.Academies.Academisation.Seed\
dotnet run
```
