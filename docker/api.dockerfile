# ---------- BUILD STAGE ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore "src/NovoBanco.API/NovoBanco.API.csproj"
RUN dotnet publish "src/NovoBanco.API/NovoBanco.API.csproj" -c Release -o /app/publish

# ---------- RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Crear usuario no root
RUN useradd -u 1001 -m appuser
USER appuser

COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "NovoBanco.API.dll"]
