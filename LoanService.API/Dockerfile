FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["LoanService.API/LoanService.API.csproj", "LoanService.API/"]
COPY ["LoanService.Application/LoanService.Application.csproj", "LoanService.Application/"]
COPY ["LoanService.Domain/LoanService.Domain.csproj", "LoanService.Domain/"]
COPY ["LoanService.Infrastructure/LoanService.Infrastructure.csproj", "LoanService.Infrastructure/"]


RUN dotnet restore "LoanService.API/LoanService.API.csproj"

# Копируем весь остальной код
COPY . .

WORKDIR "/src/LoanService.API"
RUN dotnet build "LoanService.API.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/LoanService.API"
RUN dotnet publish "LoanService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoanService.API.dll"]