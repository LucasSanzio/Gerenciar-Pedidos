# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copia os arquivos da solution
COPY ProjetoRevendedores.sln ./
COPY Catalogo.Domain/ Catalogo.Domain/
COPY Catalogo.Application/ Catalogo.Application/
COPY Catalogo.Infrastructure/ Catalogo.Infrastructure/
COPY Catalogo.Api/ Catalogo.Api/
COPY Pedidos.Domain/ Pedidos.Domain/
COPY Pedidos.Application/ Pedidos.Application/
COPY Pedidos.Infrastructure/ Pedidos.Infrastructure/
COPY Pedidos.Api/ Pedidos.Api/
COPY Reporting.Projections/ Reporting.Projections/
COPY Reporting.Api/ Reporting.Api/

RUN dotnet restore ProjetoRevendedores.sln
RUN dotnet publish Catalogo.Api/Catalogo.Api.csproj -c Release -o /app/publish

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "Catalogo.Api.dll"]
