FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app/publish
EXPOSE 8080
#EXPOSE 8081
#USER app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release

# COPY SRC
WORKDIR /src/RetroCollect
COPY ["./RetroCollect.sln", "./"]

RUN mkdir /WebApi /Domain /CrossCutting /Infrastructure /Application

COPY ["./WebApi/appsettings.Docker.json", "./Infrastructure/"]

COPY ["./WebApi/WebApi.csproj", "./WebApi/"]
COPY ["./Domain/Domain.csproj", "./Domain/"]
COPY ["./CrossCutting/CrossCutting.csproj", "./CrossCutting/"]
COPY ["./Infrastructure/Infrastructure.csproj", "./Infrastructure/"]
COPY ["./Application/Application.csproj", "./Application/"]

COPY ["./Tests/Tests.csproj", "./Tests/"]

# RESTORE
RUN dotnet restore WebApi
RUN dotnet restore Domain
RUN dotnet restore CrossCutting
RUN dotnet restore Infrastructure
RUN dotnet restore Application
RUN dotnet restore Tests
COPY . .

# BUILD & TEST
RUN dotnet build "./WebApi/WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet test ./Tests -c Debug

# MIGRATION SETUP
RUN dotnet tool install --global dotnet-ef --version 6.0.7
WORKDIR /root/.dotnet/tools
RUN ./dotnet-ef migrations add initial --project /src/RetroCollect/Infrastructure --context DataContext

#RUN PUBLISH

FROM build AS publish

ARG BUILD_CONFIGURATION=Release
RUN dotnet publish /src/RetroCollect/WebApi/WebApi.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM build AS db

WORKDIR /app

COPY --from=build /root/.dotnet/tools /root/.dotnet/tools/
COPY --from=publish /app/publish ./
COPY --from=build /src /src
COPY run-migrations.sh ./

RUN chmod 777 ./run-migrations.sh

ENV ASPNETCORE_ENVIRONMENT Docker

ENTRYPOINT ["/bin/sh", "-c", "./run-migrations.sh && dotnet WebApi.dll"]