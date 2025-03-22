FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SmartWaterSimulation.Worker.csproj", "./"]
RUN dotnet restore "SmartWaterSimulation.Worker.csproj"

COPY . .
RUN dotnet publish "SmartWaterSimulation.Worker.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SmartWaterSimulation.Worker.dll"]