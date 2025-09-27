FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

EXPOSE 80
WORKDIR /app
ENV ASPNETCORE_URLS http://+:80
ENV ASPNETCORE_ENVIRONMENT Development

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["BusinessLogic/BusinessLogic.csproj", "BusinessLogic/"]
COPY ["DataAcces/DataAcces.csproj", "DataAcces/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Validators/Validators.csproj", "Validators/"]
COPY ["test/webApplication.csproj", "test/"]

RUN dotnet restore "test/webApplication.csproj"

COPY . .

FROM build AS publish
RUN dotnet publish "test/webApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "webApplication.dll" ]