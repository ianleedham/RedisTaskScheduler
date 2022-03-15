FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["RedisTaskScheduler/RedisTaskScheduler.csproj", "RedisTaskScheduler/"]
RUN dotnet restore "RedisTaskScheduler/RedisTaskScheduler.csproj"
COPY . .
WORKDIR "/src/RedisTaskScheduler"
RUN dotnet build "RedisTaskScheduler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RedisTaskScheduler.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RedisTaskScheduler.dll"]
