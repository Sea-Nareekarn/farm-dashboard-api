FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["FarmApi/FarmApi.csproj", "FarmApi/"]
RUN dotnet restore "FarmApi/FarmApi.csproj"

COPY . .
WORKDIR /src/FarmApi
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:5022
EXPOSE 5022

ENTRYPOINT ["dotnet", "FarmApi.dll"]
