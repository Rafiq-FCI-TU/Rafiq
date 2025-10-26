# ====== BUILD STAGE ======
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY Rafiq.slnx ./Rafiq.slnx

COPY src/Core/Rafiq.Application/Rafiq.Application.csproj src/Core/Rafiq.Application/
COPY src/Core/Rafiq.Domain/Rafiq.Domain.csproj src/Core/Rafiq.Domain/
COPY src/Infrastructure/Rafiq.Infrastructure/Rafiq.Infrastructure.csproj src/Infrastructure/Rafiq.Infrastructure/
COPY src/Infrastructure/Rafiq.Persistence/Rafiq.Persistence.csproj src/Infrastructure/Rafiq.Persistence/
COPY src/Web/Rafiq.API/Rafiq.API.csproj src/Web/Rafiq.API/

RUN dotnet restore Rafiq.slnx

COPY . .

RUN dotnet publish src/Web/Rafiq.API/Rafiq.API.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app .

ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "Rafiq.API.dll"]
