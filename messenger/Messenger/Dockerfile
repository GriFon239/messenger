FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY Messenger/Messenger.csproj Messenger/
RUN dotnet restore Messenger/Messenger.csproj
COPY . .
WORKDIR /src/Messenger
RUN dotnet build Messenger.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Messenger.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Messenger.dll"]
