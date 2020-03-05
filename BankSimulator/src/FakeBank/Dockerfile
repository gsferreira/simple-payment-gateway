#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BankSimulator/src/FakeBank/FakeBank.csproj", "BankSimulator/src/FakeBank/"]
RUN dotnet restore "BankSimulator/src/FakeBank/FakeBank.csproj"
COPY . .
WORKDIR "/src/BankSimulator/src/FakeBank"
RUN dotnet build "FakeBank.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FakeBank.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FakeBank.dll"]