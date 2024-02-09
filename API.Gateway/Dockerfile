FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
EXPOSE 3000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["API.Gateway/API.Gateway.csproj", "API.Gateway/"]
RUN dotnet restore "API.Gateway/API.Gateway.csproj"
COPY . .
WORKDIR "/src/API.Gateway"
RUN dotnet build "API.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Gateway.dll"]
