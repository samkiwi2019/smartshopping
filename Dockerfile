FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY Smartshopping.csproj /build/

RUN dotnet restore ./build/Smartshopping.csproj

COPY . ./build/
WORKDIR /build/
RUN dotnet publish ./Smartshopping.csproj -c $BUILDCONFIG -o dist /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

ENV TZ=Pacific/Auckland
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

WORKDIR /app
COPY --from=build-env /build/dist .

ENTRYPOINT ["dotnet", "Smartshopping.dll"]