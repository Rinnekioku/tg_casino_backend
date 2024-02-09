# Use the official .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR ~/tg_casino_backend

# Copy csproj and restore as distinct layers
COPY ["API.Casino/API.Casino.csproj", "API.Casino/"]
COPY ["Common.CasinoServices/Common.CasinoServices.csproj", "Common.CasinoServices/"]
RUN dotnet restore "API.Casino/API.Casino.csproj"

# Copy everything else and build
COPY . ./
WORKDIR /app/API.Casino
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/API.Casino/out .
ENTRYPOINT ["dotnet", "API.Casino.dll"]

# Expose the port the app runs on
EXPOSE 80
