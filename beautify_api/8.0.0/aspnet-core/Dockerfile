FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["src/SSOFT.SPA.Web.Host/SSOFT.SPA.Web.Host.csproj", "src/SSOFT.SPA.Web.Host/"]
COPY ["src/SSOFT.SPA.Web.Core/SSOFT.SPA.Web.Core.csproj", "src/SSOFT.SPA.Web.Core/"]
COPY ["src/SSOFT.SPA.Application/SSOFT.SPA.Application.csproj", "src/SSOFT.SPA.Application/"]
COPY ["src/SSOFT.SPA.Core/SSOFT.SPA.Core.csproj", "src/SSOFT.SPA.Core/"]
COPY ["src/SSOFT.SPA.EntityFrameworkCore/SSOFT.SPA.EntityFrameworkCore.csproj", "src/SSOFT.SPA.EntityFrameworkCore/"]
WORKDIR "/src/src/SSOFT.SPA.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/SSOFT.SPA.Web.Host", "src/SSOFT.SPA.Web.Host"]
COPY ["src/SSOFT.SPA.Web.Core", "src/SSOFT.SPA.Web.Core"]
COPY ["src/SSOFT.SPA.Application", "src/SSOFT.SPA.Application"]
COPY ["src/SSOFT.SPA.Core", "src/SSOFT.SPA.Core"]
COPY ["src/SSOFT.SPA.EntityFrameworkCore", "src/SSOFT.SPA.EntityFrameworkCore"]
WORKDIR "/src/src/SSOFT.SPA.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "SSOFT.SPA.Web.Host.dll"]
