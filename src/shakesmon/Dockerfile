#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Avalentini.Shakesmon.Api/Avalentini.Shakesmon.Api.csproj", "Avalentini.Shakesmon.Api/"]
COPY ["Avalentini.Shakesmon.Core/Avalentini.Shakesmon.Core.csproj", "Avalentini.Shakesmon.Core/"]
RUN dotnet restore "Avalentini.Shakesmon.Api/Avalentini.Shakesmon.Api.csproj"
COPY . .
WORKDIR "/src/Avalentini.Shakesmon.Api"
RUN dotnet build "Avalentini.Shakesmon.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Avalentini.Shakesmon.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Avalentini.Shakesmon.Api.dll"]
