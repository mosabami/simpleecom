#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Simpleecom.Cart.API/Simpleecom.Carts.API.csproj", "Simpleecom.Cart.API/"]
COPY ["Microsoft.Azure.CosmosRepository.AspNetCore/Microsoft.Azure.CosmosRepository.AspNetCore.csproj", "Microsoft.Azure.CosmosRepository.AspNetCore/"]
COPY ["Microsoft.Azure.CosmosRepository/Microsoft.Azure.CosmosRepository.csproj", "Microsoft.Azure.CosmosRepository/"]
COPY ["Simpleecom.Shared/Simpleecom.Shared.csproj", "Simpleecom.Shared/"]
RUN dotnet restore "./Simpleecom.Cart.API/Simpleecom.Carts.API.csproj"
COPY . .
WORKDIR "/src/Simpleecom.Cart.API"
RUN dotnet build "./Simpleecom.Carts.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Simpleecom.Carts.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Simpleecom.Carts.API.dll"]