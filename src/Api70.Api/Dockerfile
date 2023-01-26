#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Api70.Api/Api70.Api.csproj", "src/Api70.Api/"]
COPY ["src/Api70.Application/Api70.Application.csproj", "src/Api70.Application/"]
RUN dotnet restore "src/Api70.Api/Api70.Api.csproj"
COPY . .
WORKDIR "/src/src/Api70.Api"
RUN dotnet build "Api70.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api70.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api70.Api.dll"]