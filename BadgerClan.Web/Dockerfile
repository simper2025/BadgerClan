FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY BadgerClan.Web/BadgerClan.Web.csproj BadgerClan.Web/BadgerClan.Web.csproj
RUN dotnet restore BadgerClan.Web/BadgerClan.Web.csproj

COPY BadgerClan.Logic/BadgerClan.Logic.csproj BadgerClan.Logic/BadgerClan.Logic.csproj
RUN dotnet restore BadgerClan.Logic/BadgerClan.Logic.csproj

COPY . .

RUN dotnet publish -c Release BadgerClan.Web/BadgerClan.Web.csproj -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
CMD ["dotnet", "BadgerClan.Web.dll"]