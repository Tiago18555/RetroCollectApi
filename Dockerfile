FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app/publish
EXPOSE 8080
#EXPOSE 8081
#USER app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release

# COPY SRC
WORKDIR /src
COPY RetroCollect.sln ./
COPY ["./WebApi/WebApi.csproj", "RetroCollect"]
COPY ["./Domain/Domain.csproj", "RetroCollect"]
COPY ["./CrossCutting/CrossCutting.csproj", "RetroCollect"]
COPY ["./Infrastructure/Infrastructure.csproj", "RetroCollect"]
COPY ["./Application/Application.csproj", "RetroCollect"]

COPY ["./Tests/Tests.csproj", "RetroCollect"]

# RESTORE
WORKDIR /src/RetroCollect/WebApi

RUN ls; sleep 15;

RUN dotnet restore WebApi
RUN dotnet restore Domain
RUN dotnet restore CrossCutting
RUN dotnet restore Infrastructure
RUN dotnet restore Application
RUN dotnet restore Tests
COPY . .

# BUILD & TEST
WORKDIR /src/RetroCollect
RUN dotnet build "./WebApi/WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet "./Tests/Tests.csproj" test -c Debug

# MIGRATION SETUP
RUN dotnet tool install --global dotnet-ef --version 6.0.7
WORKDIR /root/.dotnet/tools
RUN ./dotnet-ef migrations add initial --project /src/RetroCollectApi --context DataContext

FROM build AS publish

ARG BUILD_CONFIGURATION=Release
RUN dotnet publish /src/RetroCollect/WebApi.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM build AS db

WORKDIR /app

COPY --from=build /root/.dotnet/tools /root/.dotnet/tools/
COPY --from=publish /app/publish .
COPY --from=build /src /src

COPY run-migrations.sh ./

RUN chmod 777 ./run-migrations.sh

ENV ASPNETCORE_ENVIRONMENT Docker
WORKDIR /app
ENTRYPOINT ["/bin/sh", "-c", "./run-migrations.sh && dotnet RetroCollectApi.dll"]