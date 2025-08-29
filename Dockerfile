# Use the official .NET 8 SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/*.csproj ./src/
RUN dotnet restore ./src/*.csproj

# Copy everything else and build
COPY src/. ./src/
WORKDIR /app/src
RUN dotnet publish -c Release -o /out

# Use the official .NET 8 ASP.NET runtime image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /out ./

# Expose the default port (change if your app uses a different one)
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "TestesFOILMinimalApi.dll"]