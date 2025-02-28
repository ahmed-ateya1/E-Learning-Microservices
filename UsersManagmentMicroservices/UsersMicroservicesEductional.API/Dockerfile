# Base image with ASP.NET Core runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080 

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy and restore dependencies
COPY ["UsersMicroservicesEductional.API/UsersMicroservicesEductional.API.csproj", "UsersMicroservicesEductional.API/"]
COPY ["UsersMicroservicesEductional.Core/UsersMicroservicesEductional.Core.csproj", "UsersMicroservicesEductional.Core/"]
COPY ["UsersMicroservicesEductional.Infrastructure/UsersMicroservicesEductional.Infrastructure.csproj", "UsersMicroservicesEductional.Infrastructure/"]
RUN dotnet restore "./UsersMicroservicesEductional.API/UsersMicroservicesEductional.API.csproj"

# Copy everything and build
COPY . .
WORKDIR "/src/UsersMicroservicesEductional.API"
RUN dotnet build "./UsersMicroservicesEductional.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UsersMicroservicesEductional.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Ensure app listens on 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "UsersMicroservicesEductional.API.dll"]
