# ---------------- Build Stage ----------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1) traga TODO o código do contexto (incluindo Program.cs e todos os projetos)
COPY . .

# 2) vá para a pasta do projeto de API
WORKDIR /src/FastTech.Pedido/FastTech.Pedido.Api

# 3) restaure e publique o executável, agora com entrypoint enxergando o Program.cs
RUN dotnet restore \
 && dotnet publish -c Release -o /app/publish

# ---------------- Runtime Stage ----------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# 4) copie apenas o resultado do publish
COPY --from=build /app/publish .

# 5) execute
ENTRYPOINT ["dotnet", "FastTech.Pedido.Api.dll"]

