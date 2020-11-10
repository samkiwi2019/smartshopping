#FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
#WORKDIR /app
#
#COPY *.csproj ./
#RUN dotnet restore
#
#COPY . ./
#RUN dotnet publish -c Release -o dist
#
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
#WORKDIR /app
#COPY --from=build-env /app/dist .
#
#EXPOSE 80/tcp
#ENTRYPOINT ["dotnet", "Buyanz.WebAPI.dll"]

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY dist /app
WORKDIR /app
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "Smartshopping.dll"]

# dotnet restore
# dotnet publish --framework netcoreapp3.1 --configuration Release --output dist
# docker build . -t spider-api -f Dockerfile
# docker tag d264398931ed dockersam2019/spider-app:0.7
# docker push dockersam2019/spider-app:0.7
# docker run -d -p 5000:80 --rm --name spider-api d264398931ed