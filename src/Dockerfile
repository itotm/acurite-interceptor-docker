#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine-arm64v8 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AcuriteInterceptorApi.csproj", "."]
RUN dotnet restore "./AcuriteInterceptorApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "AcuriteInterceptorApi.csproj" -c Release -o /app/build -r linux-arm64 --no-self-contained

FROM build AS publish
RUN dotnet publish "AcuriteInterceptorApi.csproj" -c Release -o /app/publish /p:UseAppHost=false -r linux-arm64 --no-self-contained

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ConnectionString="Data Source=.;Initial Catalog=Weather;Integrated Security=True"
ENV ConsoleOutputEnabled=false
ENV DatabaseEnabled=true
ENV FilterOutSeconds=300
ENV AddHoursToUtc=0
ENV SwaggerEnabled=false

RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENTRYPOINT ["dotnet", "AcuriteInterceptorApi.dll"]