# Set the major version of dotnet
ARG DOTNET_VERSION=8.0

# ==============================================
# .NET SDK Build Stage
# ==============================================
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-azurelinux3.0 AS build
WORKDIR /build

ARG PROJECT_NAME="Dfe.Academies.Academisation"

# Copy csproj files for restore caching (excluding test projects)
COPY ${PROJECT_NAME}.Core/${PROJECT_NAME}.Core.csproj ./${PROJECT_NAME}.Core/
COPY ${PROJECT_NAME}.Data/${PROJECT_NAME}.Data.csproj ./${PROJECT_NAME}.Data/
COPY ${PROJECT_NAME}.Domain.Core/${PROJECT_NAME}.Domain.Core.csproj ./${PROJECT_NAME}.Domain.Core/
COPY ${PROJECT_NAME}.Domain/${PROJECT_NAME}.Domain.csproj ./${PROJECT_NAME}.Domain/
COPY ${PROJECT_NAME}.IData/${PROJECT_NAME}.IData.csproj ./${PROJECT_NAME}.IData/
COPY ${PROJECT_NAME}.IDomain/${PROJECT_NAME}.IDomain.csproj ./${PROJECT_NAME}.IDomain/
COPY ${PROJECT_NAME}.IService/${PROJECT_NAME}.IService.csproj ./${PROJECT_NAME}.IService/
COPY ${PROJECT_NAME}.Seed/${PROJECT_NAME}.Seed.csproj ./${PROJECT_NAME}.Seed/
COPY ${PROJECT_NAME}.Service/${PROJECT_NAME}.Service.csproj ./${PROJECT_NAME}.Service/
COPY ${PROJECT_NAME}.WebApi/${PROJECT_NAME}.WebApi.csproj ./${PROJECT_NAME}.WebApi/

# Copy solution file
COPY ./${PROJECT_NAME}.sln .

# Mount GitHub Token and restore
RUN --mount=type=secret,id=github_token dotnet nuget add source --username USERNAME --password $(cat /run/secrets/github_token) --store-password-in-clear-text --name github "https://nuget.pkg.github.com/DFE-Digital/index.json" && \
    dotnet restore ${PROJECT_NAME}.WebApi

# Copy remaining source code (excluding test projects)
COPY ./${PROJECT_NAME}.Core/ ./${PROJECT_NAME}.Core/
COPY ./${PROJECT_NAME}.Data/ ./${PROJECT_NAME}.Data/
COPY ./${PROJECT_NAME}.Domain.Core/ ./${PROJECT_NAME}.Domain.Core/
COPY ./${PROJECT_NAME}.Domain/ ./${PROJECT_NAME}.Domain/
COPY ./${PROJECT_NAME}.IData/ ./${PROJECT_NAME}.IData/
COPY ./${PROJECT_NAME}.IDomain/ ./${PROJECT_NAME}.IDomain/
COPY ./${PROJECT_NAME}.IService/ ./${PROJECT_NAME}.IService/
COPY ./${PROJECT_NAME}.Seed/ ./${PROJECT_NAME}.Seed/
COPY ./${PROJECT_NAME}.Service/ ./${PROJECT_NAME}.Service/
COPY ./${PROJECT_NAME}.WebApi/ ./${PROJECT_NAME}.WebApi/

# Build and publish
RUN dotnet build ${PROJECT_NAME}.WebApi --no-restore -c Release && \
    dotnet publish ${PROJECT_NAME}.WebApi/${PROJECT_NAME}.WebApi.csproj \
      --no-build -c Release -o /app

# ==============================================
# Entity Framework: Migration Builder
# ==============================================
FROM build AS efbuilder
ENV PATH=$PATH:/root/.dotnet/tools

# Install dotnet-ef and create migration bundle
RUN mkdir /sql && \
    dotnet tool install --global dotnet-ef && \
    dotnet ef migrations bundle \
      -r linux-x64 \
      -p Dfe.Academies.Academisation.Data \
      -s Dfe.Academies.Academisation.WebApi \
      -c AcademisationContext \
      --configuration Release \
      --no-build \
      -o /sql/migratedb

# ==============================================
# Entity Framework: Migration Runner
# ==============================================
FROM "mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-azurelinux3.0" AS initcontainer
WORKDIR /sql
COPY --from=efbuilder /app/appsettings.json /Dfe.Academies.Academisation/
COPY --from=efbuilder /sql /sql
RUN chown "$APP_UID" "/sql" -R
USER $APP_UID

# ==============================================
# .NET: Runtime
# ==============================================
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-azurelinux3.0 AS final
WORKDIR /app
LABEL org.opencontainers.image.source="https://github.com/DFE-Digital/academies-academisation-api"
COPY --from=build /app /app
COPY ./script/webapi-docker-entrypoint.sh /app/docker-entrypoint.sh

# Set permissions and user
RUN chmod +x /app/docker-entrypoint.sh
USER $APP_UID
