﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Worker.Telegram/Worker.Telegram.csproj", "Worker.Telegram/"]
RUN dotnet restore "Worker.Telegram/Worker.Telegram.csproj"
COPY . .
WORKDIR "/src/Worker.Telegram"
RUN dotnet build "Worker.Telegram.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Worker.Telegram.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Worker.Telegram.dll"]
