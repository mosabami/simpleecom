#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Simpleecom.Auth/Simpleecom.Auth.csproj", "Simpleecom.Auth/"]
COPY ["Microsoft.Azure.CosmosRepository.AspNetCore/Microsoft.Azure.CosmosRepository.AspNetCore.csproj", "Microsoft.Azure.CosmosRepository.AspNetCore/"]
COPY ["Microsoft.Azure.CosmosRepository/Microsoft.Azure.CosmosRepository.csproj", "Microsoft.Azure.CosmosRepository/"]
COPY ["Simpleecom.Shared/Simpleecom.Shared.csproj", "Simpleecom.Shared/"]
RUN dotnet restore "./Simpleecom.Auth/Simpleecom.Auth.csproj"
COPY . .
WORKDIR "/src/Simpleecom.Auth"
RUN dotnet build "./Simpleecom.Auth.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Simpleecom.Auth.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Simpleecom.Auth.dll"]