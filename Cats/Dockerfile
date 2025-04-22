# Use the official .NET 8 SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything
COPY . ./

# Restore dependencies
WORKDIR /app/Cats
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET 8 ASP.NET runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Set environment variables (override appsettings.json)
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ConnectionStrings__DefaultConnection="Server=sql_server;Database=Cats;User Id=sa;Password=YourPassword123;TrustServerCertificate=True;"
ENV TheCatApi__BaseUrl="https://api.thecatapi.com/v1"
ENV TheCatApi__ApiKey="live_Oh0Ch2hH9WBHu3EMiZvBRBMfsqoVHcfH8wpjH4jKg3YtiVhIlOiyxzboN9DcDlx7"

# Expose port 80 (default for .NET applications)
EXPOSE 80

# Set the entrypoint to run the app
ENTRYPOINT ["dotnet", "Cats.dll"]
