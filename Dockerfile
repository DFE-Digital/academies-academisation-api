# Stage 1
ARG ASPNET_IMAGE_TAG=8.0-bookworm-slim
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /build

ENV DEBIAN_FRONTEND=noninteractive

COPY ./Dfe.Academies.Academisation.Core/ ./Dfe.Academies.Academisation.Core/
COPY ./Dfe.Academies.Academisation.Data/ ./Dfe.Academies.Academisation.Data/
COPY ./Dfe.Academies.Academisation.Domain/ ./Dfe.Academies.Academisation.Domain/
COPY ./Dfe.Academies.Academisation.Domain.Core/ ./Dfe.Academies.Academisation.Domain.Core/
COPY ./Dfe.Academies.Academisation.IData/ ./Dfe.Academies.Academisation.IData/
COPY ./Dfe.Academies.Academisation.IDomain/ ./Dfe.Academies.Academisation.IDomain/
COPY ./Dfe.Academies.Academisation.IService/ ./Dfe.Academies.Academisation.IService/
COPY ./Dfe.Academies.Academisation.Service/ ./Dfe.Academies.Academisation.Service/
COPY ./Dfe.Academies.Academisation.WebApi/ ./Dfe.Academies.Academisation.WebApi/
COPY ./Dfe.Academies.Academisation.sln/ ./Dfe.Academies.Academisation.sln

RUN --mount=type=secret,id=github_token dotnet nuget add source --username USERNAME --password $(cat /run/secrets/github_token) --store-password-in-clear-text --name github "https://nuget.pkg.github.com/DFE-Digital/index.json"
RUN dotnet restore Dfe.Academies.Academisation.WebApi
RUN dotnet build Dfe.Academies.Academisation.WebApi

RUN dotnet new tool-manifest
RUN dotnet tool install dotnet-ef --version 7.0.14

RUN dotnet ef migrations script --output /app/SQL/DbMigrationScript.sql --idempotent -p /build/Dfe.Academies.Academisation.Data -s /build/Dfe.Academies.Academisation.WebApi -c AcademisationContext

RUN dotnet publish Dfe.Academies.Academisation.WebApi -c Release -o /app

COPY ./script/webapi-docker-entrypoint.sh /app/docker-entrypoint.sh

ARG ASPNET_IMAGE_TAG
FROM "mcr.microsoft.com/dotnet/aspnet:${ASPNET_IMAGE_TAG}" AS final

RUN apt-get update
RUN apt-get install unixodbc curl gnupg -y
RUN curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
RUN curl https://packages.microsoft.com/config/debian/11/prod.list | tee /etc/apt/sources.list.d/msprod.list
RUN apt-get update
RUN ACCEPT_EULA=Y apt-get install msodbcsql18 mssql-tools18 -y

COPY --from=build /app /app
WORKDIR /app
RUN chmod +x ./docker-entrypoint.sh
ENV ASPNETCORE_HTTP_PORTS=80
EXPOSE 80/tcp
