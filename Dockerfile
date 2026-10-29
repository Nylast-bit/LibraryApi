# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Se copian primero los .csproj para que el restore quede en caché
COPY Directory.Build.props ./
COPY src/LibraryApi.Domain/LibraryApi.Domain.csproj src/LibraryApi.Domain/
COPY src/LibraryApi.Application/LibraryApi.Application.csproj src/LibraryApi.Application/
COPY src/LibraryApi.Infrastructure/LibraryApi.Infrastructure.csproj src/LibraryApi.Infrastructure/
COPY src/LibraryApi.Api/LibraryApi.Api.csproj src/LibraryApi.Api/
RUN dotnet restore src/LibraryApi.Api/LibraryApi.Api.csproj

COPY src/ src/
RUN dotnet publish src/LibraryApi.Api/LibraryApi.Api.csproj -c Release -o /app/publish --no-restore

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "LibraryApi.Api.dll"]