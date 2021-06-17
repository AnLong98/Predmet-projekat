#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["SmartEnergyDocuments/SmartEnergyDocuments.csproj", "SmartEnergyDocuments/"]
COPY ["SmartEnergyContracts/SmartEnergyContracts.csproj", "SmartEnergyContracts/"]
RUN dotnet restore "SmartEnergyDocuments/SmartEnergyDocuments.csproj"
COPY . .
WORKDIR "/src/SmartEnergyDocuments"
RUN dotnet build "SmartEnergyDocuments.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartEnergyDocuments.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartEnergyDocuments.dll"]
