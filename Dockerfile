FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app/src

COPY . .
COPY *.sln .
COPY src/API/AddressNormalizer.Api/*.csproj ./src/Api/AddressNormalizer/
COPY src/Core/AddressNormalizer.Application/*.csproj ./src/Core/AddressNormalizer.Application/
COPY src/Core/AddressNormalizer.Domain/*.csproj ./src/Core/AddressNormalizer.Domain/
COPY src/Infrastructure/AddressNormalizer.Infrastructure/*.csproj ./src/Infrastructure/AddressNormalizer.Infrastructure/

RUN dotnet restore
RUN dotnet build -c Release -o /app/build
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN apt update && \
    apt install -y curl unzip libtool
    
RUN mkdir -p /opt && \
    cd /opt && \
    curl -sL https://github.com/rkramer1964/PKLibPostalNetData/releases/download/1.0.0/libpostal.zip -o libpostal.zip&& \
    unzip libpostal.zip -d /opt && \
    rm libpostal.zip  

ENTRYPOINT ["dotnet", "AddressNormalizer.Api.dll"]