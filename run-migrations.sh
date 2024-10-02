#!/bin/bash

echo Updating migrations
dotnet tool install --global dotnet-ef --version 6.0.7
cd /root/.dotnet/tools
echo now on directory $(pwd)
./dotnet-ef database update --project /src/RetroCollect/Infrastructure/Infrastructure.csproj --context DataContext
echo Run migrations OK