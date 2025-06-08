FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["B3.QuotationHistories.WebApi/B3.QuotationHistories.WebApi.csproj", "B3.QuotationHistories.WebApi/"]
COPY ["B3.QuotationHistories.Domain/B3.QuotationHistories.Domain.csproj", "B3.QuotationHistories.Domain/"]
COPY ["B3.QuotationHistories.Application/B3.QuotationHistories.Application.csproj", "B3.QuotationHistories.Application/"]
COPY ["B3.QuotationHistories.Infrastructure/B3.QuotationHistories.Infrastructure.csproj", "B3.QuotationHistories.Infrastructure/"]
RUN dotnet restore "B3.QuotationHistories.WebApi/B3.QuotationHistories.WebApi.csproj"
COPY . .
WORKDIR "/src/B3.QuotationHistories.WebApi"
RUN dotnet build "B3.QuotationHistories.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "B3.QuotationHistories.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "B3.QuotationHistories.WebApi.dll"]
