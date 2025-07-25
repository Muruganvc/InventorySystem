#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["InventorySystem_Api/InventorySystem_Api.csproj", "InventorySystem_Api/"]
COPY ["Database_Utility/Database_Utility.csproj", "Database_Utility/"]
COPY ["InventorySystem_Application/InventorySystem_Application.csproj", "InventorySystem_Application/"]
COPY ["InventorySystem_Domain/InventorySystem_Domain.csproj", "InventorySystem_Domain/"]
COPY ["InventorySystem_Infrastructure/InventorySystem_Infrastructure.csproj", "InventorySystem_Infrastructure/"]
RUN dotnet restore "Stock_Maintenance_System_Api/InventorySystem_Api.csproj"
COPY . .
WORKDIR "/src/Stock_Maintenance_System_Api"
RUN dotnet build "InventorySystem_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InventorySystem_Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventorySystem_Api.dll"]