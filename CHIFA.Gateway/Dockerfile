# Build context: the repository root (see deploy/gateway/docker-compose.yml and azure-pipelines-gateway.yml).
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ARG APP_UID=1000
USER ${APP_UID}

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Directory.Build.props", "."]
COPY ["CHIFA.Contract/CHIFA.Contract.csproj", "CHIFA.Contract/"]
COPY ["CHIFA.Gateway.Data/CHIFA.Gateway.Data.csproj", "CHIFA.Gateway.Data/"]
COPY ["CHIFA.Mobile/CHIFA.Mobile.csproj", "CHIFA.Mobile/"]
COPY ["CHIFA.Gateway/CHIFA.Gateway.csproj", "CHIFA.Gateway/"]
RUN dotnet restore "CHIFA.Gateway/CHIFA.Gateway.csproj"

COPY CHIFA.Contract/ CHIFA.Contract/
COPY CHIFA.Gateway.Data/ CHIFA.Gateway.Data/
COPY CHIFA.Mobile/ CHIFA.Mobile/
COPY CHIFA.Gateway/ CHIFA.Gateway/
RUN dotnet publish "CHIFA.Gateway/CHIFA.Gateway.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CHIFA.Gateway.dll"]
