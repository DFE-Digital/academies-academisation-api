# Set the major version of dotnet
ARG DOTNET_VERSION=8.0

# Build the app using the dotnet SDK
FROM "mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-azurelinux3.0" AS build
WORKDIR /build

COPY ./Dfe.Academies.Academisation.Core/ ./Dfe.Academies.Academisation.Core/
COPY ./Dfe.Academies.Academisation.Data/ ./Dfe.Academies.Academisation.Data/
COPY ./Dfe.Academies.Academisation.Domain/ ./Dfe.Academies.Academisation.Domain/
COPY ./Dfe.Academies.Academisation.Domain.Core/ ./Dfe.Academies.Academisation.Domain.Core/
COPY ./Dfe.Academies.Academisation.IData/ ./Dfe.Academies.Academisation.IData/
COPY ./Dfe.Academies.Academisation.IDomain/ ./Dfe.Academies.Academisation.IDomain/
COPY ./Dfe.Academies.Academisation.IService/ ./Dfe.Academies.Academisation.IService/
COPY ./Dfe.Academies.Academisation.Service/ ./Dfe.Academies.Academisation.Service/
COPY ./Dfe.Academies.Academisation.WebApi/ ./Dfe.Academies.Academisation.WebApi/
COPY ./Dfe.Academies.Academisation.sln ./Dfe.Academies.Academisation.sln

# Mount GitHub Token as a Docker secret so that NuGet Feed can be accessed
RUN --mount=type=secret,id=github_token dotnet nuget add source --username USERNAME --password $(cat /run/secrets/github_token) --store-password-in-clear-text --name github "https://nuget.pkg.github.com/DFE-Digital/index.json"

RUN ["dotnet", "restore", "Dfe.Academies.Academisation.WebApi"]
RUN ["dotnet", "build", "Dfe.Academies.Academisation.WebApi", "--no-restore", "-c", "Release"]
RUN ["dotnet", "publish", "Dfe.Academies.Academisation.WebApi", "--no-build", "-o", "/app"]

RUN ["dotnet", "new", "tool-manifest"]
RUN ["dotnet", "tool", "install", "dotnet-ef", "--version", "8.0.11"]
RUN ["mkdir", "-p", "/app/SQL"]
RUN ["dotnet", "restore", "Dfe.Academies.Academisation.WebApi"]
RUN ["dotnet", "build", "Dfe.Academies.Academisation.WebApi", "--no-restore"]
RUN ["dotnet", "ef", "migrations", "script", "--output", "/app/SQL/DbMigrationScript.sql", "--idempotent", "-p", "/build/Dfe.Academies.Academisation.Data", "-s", "/build/Dfe.Academies.Academisation.WebApi", "-c", "AcademisationContext", "--no-build"]
RUN ["touch", "/app/SQL/DbMigrationScriptOutput.txt"]

# Install SQL tools to allow migrations to be run
FROM "mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-azurelinux3.0" AS base
RUN curl "https://packages.microsoft.com/config/rhel/9/prod.repo" | tee /etc/yum.repos.d/mssql-release.repo
ENV ACCEPT_EULA=Y
RUN ["tdnf", "update", "-y", "--security"]
RUN ["tdnf", "install", "-y", "mssql-tools18"]
RUN ["tdnf", "clean", "all"]

# Build a runtime environment
FROM base AS runtime
WORKDIR /app
LABEL org.opencontainers.image.source="https://github.com/DFE-Digital/academies-academisation-api"
COPY --from=build /app /app
COPY ./script/webapi-docker-entrypoint.sh /app/docker-entrypoint.sh
RUN ["chmod", "+x", "/app/docker-entrypoint.sh"]
RUN ["touch", "/app/SQL/DbMigrationScriptOutput.txt"]
RUN chown "$APP_UID" "/app/SQL" -R
USER $APP_UID
