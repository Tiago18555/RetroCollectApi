FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app/publish
EXPOSE 8080
#EXPOSE 8081
#USER app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release

# COPY SRC
WORKDIR /src
COPY RetroCollectApi.sln ./
COPY ["RetroCollectApi/RetroCollectApi.csproj", "RetroCollectApi/"]
COPY ["RetroCollectApiTests/RetroCollectApiTests.csproj", "RetroCollectApiTests/"]

# RESTORE
RUN dotnet restore "./RetroCollectApi/./RetroCollectApi.csproj"
RUN dotnet restore "./RetroCollectApiTests/./RetroCollectApiTests.csproj"
COPY . .

# BUILD & TEST
WORKDIR /src/RetroCollectApi
RUN dotnet build "./RetroCollectApi.csproj" -c $BUILD_CONFIGURATION -o /app/build
WORKDIR /src/RetroCollectApiTests
#RUN dotnet test -c Debug --logger "trx;LogFileName=test_results.trx"

# MIGRATION SETUP
RUN dotnet tool install --global dotnet-ef --version 6.0.7
WORKDIR /root/.dotnet/tools
RUN ./dotnet-ef migrations add initial --project /src/RetroCollectApi --context DataContext

FROM build AS publish

ARG BUILD_CONFIGURATION=Release
RUN dotnet publish /src/RetroCollectApi/RetroCollectApi.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM build AS db

WORKDIR /app

COPY --from=build /root/.dotnet/tools /root/.dotnet/tools/
COPY --from=publish /app/publish .
COPY --from=build /src /src

COPY RetroCollectApi/Data/run-migrations.sh ./

RUN chmod 777 ./run-migrations.sh

ENV ASPNETCORE_ENVIRONMENT Docker
WORKDIR /app
ENTRYPOINT ["/bin/sh", "-c", "./run-migrations.sh && dotnet RetroCollectApi.dll"]