#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["crawler.csproj", "."]
RUN dotnet restore "./crawler.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "crawler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "crawler.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "crawler.dll"]



CMD ASPNETCORE_URLS=http://*:$PORT dotnet crawler.dll