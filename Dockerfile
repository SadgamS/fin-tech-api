FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY FinTechApi.slnx ./
COPY src/FinTech.API/FinTech.API.csproj src/FinTech.API/
COPY src/FinTech.Application/FinTech.Application.csproj src/FinTech.Application/
COPY src/FinTech.Domain/FinTech.Domain.csproj src/FinTech.Domain/
COPY src/FinTech.Infrastructure/FinTech.Infrastructure.csproj src/FinTech.Infrastructure/
COPY src/FinTech.Tests/FinTech.Tests.csproj src/FinTech.Tests/

RUN dotnet restore

COPY src/ src/
RUN dotnet publish src/FinTech.API/FinTech.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "FinTech.API.dll"]
