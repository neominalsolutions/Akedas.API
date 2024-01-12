#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# mcr.microsoft.com/dotnet/aspnet:7.0 uygulmayý bu imagedan çalýþýr.
# app
# mcr.microsoft.com/dotnet/sdk:7.0 AS build iþlemleri için sdk paket image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Akedas.API/Akedas.API.csproj", "Akedas.API/"]
RUN dotnet restore "Akedas.API/Akedas.API.csproj"
COPY . .
WORKDIR "/src/Akedas.API"
RUN dotnet build "Akedas.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Akedas.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Akedas.API.dll"]