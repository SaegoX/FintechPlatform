FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["BillingService.API/BillingService.API.csproj", "BillingService.API/"]
COPY ["BillingService.Application/BillingService.Application.csproj", "BillingService.Application/"]
COPY ["BillingService.Domain/BillingService.Domain.csproj", "BillingService.Domain/"]
COPY ["BillingService.Infrastructure/BillingService.Infrastructure.csproj", "BillingService.Infrastructure/"]
COPY ["FintechPlatform.Contracts/FintechPlatform.Contracts.csproj","FintechPlatform.Contracts/"]

RUN dotnet restore "BillingService.API/BillingService.API.csproj"

COPY . .

WORKDIR "/src/BillingService.API"
RUN dotnet build "BillingService.API.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/BillingService.API"
RUN dotnet publish "BillingService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BillingService.API.dll"]