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

# Generate an Entity Framework bundle
FROM build AS efbuilder
ENV PATH=$PATH:/root/.dotnet/tools
RUN ["mkdir", "/sql"]
RUN ["dotnet", "tool", "install", "--global", "dotnet-ef"]
RUN ["dotnet", "ef", "migrations", "bundle", "-r", "linux-x64", "-p", "Dfe.Academies.Academisation.Data", "-s", "Dfe.Academies.Academisation.WebApi", "-c", "AcademisationContext", "--configuration", "Release", "--no-build", "-o", "/sql/migratedb"]

# Create a runtime environment for Entity Framework
FROM "mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-azurelinux3.0" AS initcontainer
WORKDIR /sql
COPY --from=efbuilder /app/appsettings.json /Dfe.Academies.Academisation/
COPY --from=efbuilder /sql /sql
RUN chown "$APP_UID" "/sql" -R
USER $APP_UID

# Build a runtime environment
FROM "mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-azurelinux3.0" AS final
WORKDIR /app
LABEL org.opencontainers.image.source="https://github.com/DFE-Digital/academies-academisation-api"
COPY --from=build /app /app
COPY ./script/webapi-docker-entrypoint.sh /app/docker-entrypoint.sh
RUN ["chmod", "+x", "/app/docker-entrypoint.sh"]
USER $APP_UID
