﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 3000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["API.Casino/API.Casino.csproj", "API.Casino/"]
COPY ["API.Casino/API.Casino.csproj", "API.Casino/"]
RUN dotnet restore "API.Casino/API.Casino.csproj"
COPY . .
WORKDIR "/src/API.Casino"
RUN dotnet build "API.Casino.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.Casino.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Casino.dll"]
