# Tahap build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy file csproj ke dalam folder api supaya restore bisa dilakukan
COPY api/api.csproj ./api/

WORKDIR /src/api
RUN dotnet restore

# Copy semua file di folder api
COPY api/. .

# Publish project
RUN dotnet publish -c Release -o /app/publish

# Tahap runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Supaya aplikasi bisa diakses dari host semua interface
ENV ASPNETCORE_URLS=http://+:80

COPY --from=build /app/publish ./

EXPOSE 80

ENTRYPOINT ["dotnet", "api.dll"]
