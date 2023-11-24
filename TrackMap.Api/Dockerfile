FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TrackMap.Api/TrackMap.Api.csproj", "TrackMap.Api/"]
RUN dotnet restore "TrackMap.Api/TrackMap.Api.csproj"
COPY . .
WORKDIR "/src/TrackMap.Api"
RUN dotnet build "TrackMap.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrackMap.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrackMap.Api.dll"]