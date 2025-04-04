﻿FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["ChatAnalyzer.Presentation/ChatAnalyzer.Presentation.csproj", "ChatAnalyzer.Presentation/"]
COPY ["ChatAnalyzer.Application/ChatAnalyzer.Application.csproj", "ChatAnalyzer.Application/"]
COPY ["ChatAnalyzer.Domain/ChatAnalyzer.Domain.csproj", "ChatAnalyzer.Domain/"]
COPY ["ChatAnalyzer.Infrastructure/ChatAnalyzer.Infrastructure.csproj", "ChatAnalyzer.Infrastructure/"]
RUN dotnet restore "ChatAnalyzer.Presentation/ChatAnalyzer.Presentation.csproj"
COPY . .
WORKDIR "/src/ChatAnalyzer.Presentation"
RUN dotnet build "ChatAnalyzer.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ChatAnalyzer.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatAnalyzer.Presentation.dll"]
