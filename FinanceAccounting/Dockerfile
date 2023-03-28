FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ConnectionString__DefaultConnection=Host=db-app;Port=5432;Username=postgres;Password=superuser1;Database=app-db
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FinanceAccounting/FinanceAccounting.csproj", "FinanceAccounting/"]
RUN dotnet restore "FinanceAccounting/FinanceAccounting.csproj"
COPY . .
WORKDIR "/src/FinanceAccounting"
RUN dotnet build "FinanceAccounting.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinanceAccounting.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinanceAccounting.dll"]
