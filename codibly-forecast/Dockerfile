
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG APP_UID
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["codibly-forecast.csproj", "codibly-forecast/"]
RUN dotnet restore "codibly-forecast/codibly-forecast.csproj"
COPY . .
RUN dotnet build "codibly-forecast/codibly-forecast.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "codibly-forecast/codibly-forecast.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "codibly-forecast.dll"]