#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Simpleecom.Orders.API/Simpleecom.Orders.API/Simpleecom.Orders.API.csproj", "Simpleecom.Orders.API/Simpleecom.Orders.API/"]
COPY ["Simpleecom.Shared/Simpleecom.Shared.csproj", "Simpleecom.Shared/"]
RUN dotnet restore "./Simpleecom.Orders.API/Simpleecom.Orders.API/Simpleecom.Orders.API.csproj"
COPY . .
WORKDIR "/src/Simpleecom.Orders.API/Simpleecom.Orders.API"
RUN dotnet build "./Simpleecom.Orders.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Simpleecom.Orders.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Simpleecom.Orders.API.dll"]