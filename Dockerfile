FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["APIRestaurante.API/APIRestaurante.API.csproj", "APIRestaurante.API/"]
COPY ["APIRestaurante.Application/APIRestaurante.Application.csproj", "APIRestaurante.Application/"]
COPY ["APIRestaurante.Domain/APIRestaurante.Domain.csproj", "APIRestaurante.Domain/"]
COPY ["APIRestaurante.Infrastructure/APIRestaurante.Infrastructure.csproj", "APIRestaurante.Infrastructure/"]

RUN dotnet restore "APIRestaurante.API/APIRestaurante.API.csproj"

COPY . .

WORKDIR "/src/APIRestaurante.API"

RUN dotnet build "APIRestaurante.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "APIRestaurante.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "APIRestaurante.API.dll"]
