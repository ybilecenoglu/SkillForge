FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# WORKDIR /src
# COPY ["CoreTestFramework.Northwind.WebMvcUI.csproj", "."]
# RUN dotnet restore "./CoreTestFramework.Northwind.WebMvcUI.csproj"
# COPY . .
# WORKDIR "/src/."
# RUN dotnet build "CoreTestFramework.Northwind.WebMvcUI.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src
COPY . . 
RUN dotnet restore "./CoreTestFramework.Northwind.WebMvcUI.csproj"
WORKDIR /src/.
RUN dotnet publish "CoreTestFramework.Northwind.WebMvcUI.csproj" -c Release -o /app 

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CoreTestFramework.Northwind.WebMvcUI.dll"]