FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TaskRunnerService/TaskRunnerService.csproj", "TaskRunnerService/"]
RUN dotnet restore "TaskRunnerService/TaskRunnerService.csproj"
COPY . .
WORKDIR "/src/TaskRunnerService"
RUN dotnet build "TaskRunnerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskRunnerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskRunnerService.dll"]
