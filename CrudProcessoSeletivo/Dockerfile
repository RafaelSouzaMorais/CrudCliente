# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Fase base para execução da aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Fase de build do contêiner
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CrudProcessoSeletivo/CrudProcessoSeletivo.csproj", "CrudProcessoSeletivo/"]
COPY ["CrudProcessoSeletivo.Application/CrudProcessoSeletivo.Application.csproj", "CrudProcessoSeletivo.Application/"]
COPY ["CrudProcessoSeletivo.Domain/CrudProcessoSeletivo.Domain.csproj", "CrudProcessoSeletivo.Domain/"]
COPY ["CrudProcessoSeletivo.Infrastructure/CrudProcessoSeletivo.Infrastructure.csproj", "CrudProcessoSeletivo.Infrastructure/"]
RUN dotnet restore "./CrudProcessoSeletivo/CrudProcessoSeletivo.csproj"
COPY . .
WORKDIR "/src/CrudProcessoSeletivo"
RUN dotnet build "./CrudProcessoSeletivo.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Fase de publicação do projeto
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CrudProcessoSeletivo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Fase final para produção
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CrudProcessoSeletivo.dll"]