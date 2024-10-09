FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG TARGETARCH
WORKDIR /source

COPY --link ./*.csproj .
RUN dotnet restore -a $TARGETARCH

COPY --link ./. .
RUN dotnet publish --no-restore -a $TARGETARCH -o /app

# FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
# EXPOSE 8080
# WORKDIR /app
# COPY --link --from=build /app .
# USER $APP_UID
# ENTRYPOINT ["./EvidencijaAparata.Server"]