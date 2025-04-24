FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NZWalk.Api.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
# Create Images directory before copying files
RUN mkdir -p Images
COPY --from=build /app .
ENTRYPOINT ["dotnet", "NZWalk.Api.dll"]