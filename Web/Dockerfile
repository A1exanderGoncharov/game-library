#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Web/Web.csproj", "Web/"]
COPY ["BLL.Infrastructure/BLL.Infrastructure.csproj", "BLL.Infrastructure/"]
COPY ["DAL.Interfaces/DAL.Interfaces.csproj", "DAL.Interfaces/"]
COPY ["DAL.Entities/DAL.Entities.csproj", "DAL.Entities/"]
COPY ["BLL.Interfaces/BLL.Interfaces.csproj", "BLL.Interfaces/"]
COPY ["BLL.DTO/BLL.DTO.csproj", "BLL.DTO/"]
COPY ["DAL.Infrastructure/DAL.Infrastructure.csproj", "DAL.Infrastructure/"]
RUN dotnet restore "Web/Web.csproj"
COPY . .
WORKDIR "/src/Web"
RUN dotnet build "Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]