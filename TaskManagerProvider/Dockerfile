FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TaskManagerProvider/TaskManagerProvider.csproj", "TaskManagerProvider/"]
RUN dotnet restore "TaskManagerProvider/TaskManagerProvider.csproj"
COPY . .
WORKDIR "/src/TaskManagerProvider"
RUN dotnet build "TaskManagerProvider.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManagerProvider.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagerProvider.dll"]
