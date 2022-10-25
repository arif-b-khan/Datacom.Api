#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8081
EXPOSE 8081
EXPOSE 4431

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/Payroll/Payroll.csproj", "Payroll/"]
COPY ["/Payroll.Core/Payroll.Core.csproj", "Payroll.Core/"]
COPY ["/Payroll.Domain/Payroll.Domain.csproj", "Payroll.Domain/"]
# WORKDIR /src/Payroll.Domain
RUN dotnet restore "/src/Payroll.Domain/Payroll.Domain.csproj"
RUN dotnet restore "/src/Payroll.Core/Payroll.Core.csproj"
RUN dotnet restore "/src/Payroll/Payroll.csproj"
COPY . .
WORKDIR "/src/Payroll"
RUN dotnet build "Payroll.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payroll.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payroll.dll"]

# docker build --pull --rm -f "Dockerfile" -t datacom-api:latest . --progress=plain
# docker build --no-cache --pull --rm -f "Dockerfile" -t datacom-api:latest "Payroll" --progress=plain