FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine
ARG TARGETARCH

COPY --link --from=ajovano/ea-back /app/ /app/EvidencijaAparata.Server/

WORKDIR /source/EvidencijaAparata.Tests
COPY --link ./*.csproj .
RUN dotnet restore -a $TARGETARCH
COPY --link ./. .
CMD dotnet test --no-restore
