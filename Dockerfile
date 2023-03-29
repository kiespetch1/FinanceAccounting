FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

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
ENV ASPNETCORE_URLS http://*:80
EXPOSE 80
ENTRYPOINT ["dotnet", "FinanceAccounting.dll"]
