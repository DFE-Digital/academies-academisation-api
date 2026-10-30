#!/bin/sh
set -e

update-ca-certificates

exec su app -s /bin/sh -c "dotnet Dfe.Academies.Academisation.WebApi.dll"