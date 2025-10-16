FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["lunch-roulette/lunch-roulette.csproj", "."]
RUN dotnet restore "lunch-roulette.csproj"
COPY /lunch-roulette .
RUN dotnet build "lunch-roulette.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./lunch-roulette.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

RUN mkdir -p /app/data && chmod -R 777 /app/data

ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/app.db"

WORKDIR /app

# Copy test data
COPY lunch-roulette/testData.csv . 

COPY --from=publish /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "lunch-roulette.dll"]
