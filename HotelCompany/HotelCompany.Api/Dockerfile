#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HotelCompany/HotelCompany.Api/HotelCompany.Api.csproj", "HotelCompany/HotelCompany.Api/"]
RUN dotnet restore "HotelCompany/HotelCompany.Api/HotelCompany.Api.csproj"
COPY . .
WORKDIR "/src/HotelCompany/HotelCompany.Api"
RUN dotnet build "HotelCompany.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HotelCompany.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelCompany.Api.dll"]