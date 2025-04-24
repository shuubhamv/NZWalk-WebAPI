# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY *.csproj .
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy from build stage
COPY --from=build /app .

# Expose port (adjust if your API uses a different port)
EXPOSE 80
EXPOSE 443

# Set environment variables for configuration
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Entry point
ENTRYPOINT ["dotnet", "YourApiProjectName.dll"]