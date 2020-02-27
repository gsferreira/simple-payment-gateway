#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PaymentGateway/src/PaymentGateway.Api/PaymentGateway.Api.csproj", "PaymentGateway/src/PaymentGateway.Api/"]
RUN dotnet restore "PaymentGateway/src/PaymentGateway.Api/PaymentGateway.Api.csproj"
COPY . .
WORKDIR "/src/PaymentGateway/src/PaymentGateway.Api"
RUN dotnet build "PaymentGateway.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentGateway.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentGateway.Api.dll"]