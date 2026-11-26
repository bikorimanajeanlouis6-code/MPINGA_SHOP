FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Core/Domain/Domain.csproj", "Core/Domain/"]
COPY ["Core/Application/Application.csproj", "Core/Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Web/web.csproj", "Web/"]

RUN dotnet restore "Web/web.csproj"

COPY . .
WORKDIR "/src/Web"
RUN dotnet publish "web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 10000
ENTRYPOINT ["sh", "-c", "dotnet web.dll --urls http://0.0.0.0:${PORT:-10000}"]