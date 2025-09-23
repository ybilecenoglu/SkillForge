# [FROM] Oluşturulacak imajın hangi imajdan oluşturulacağını belirten talilamttır.
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# [WORKDIR] Komutuyla imaj içerisinde hangi klasöre geçmek yerine bu talimat kullanılarak istediğimiz klasöre geçer ve oradan çalışmaya devam ederiz.
WORKDIR /app
# [EXPOSE] İmajdan oluşturulacak containerlerin hangi portlar üstünden erişilebileceğini yani hangi portların yayınlanacağını belirtiriz.
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CoreTestFramework.Northwind.WebMvcUI/CoreTestFramework.Northwind.WebMvcUI.csproj", "CoreTestFramework.Northwind.WebMvcUI/"]
COPY ["CoreTestFramework.Northwind.Business/CoreTestFramework.Northwind.Business.csproj", "CoreTestFramework.Northwind.Business/"]
COPY ["CoreTestFramework.Northwind.DataAccess/CoreTestFramework.Northwind.DataAccess.csproj", "CoreTestFramework.Northwind.DataAccess/"]
COPY ["CoreTestFramework.Northwind.Entities/CoreTestFramework.Northwind.Entities.csproj", "CoreTestFramework.Northwind.Entities/"]
COPY ["CoreTestFramework.Core/CoreTestFramework.Core.csproj", "CoreTestFramework.Core/"]
RUN dotnet restore "CoreTestFramework.Northwind.WebMvcUI/CoreTestFramework.Northwind.WebMvcUI.csproj"
COPY . .
WORKDIR /src/.
RUN dotnet build "CoreTestFramework.Northwind.WebMvcUI/CoreTestFramework.Northwind.WebMvcUI.csproj" -c Release -o /app/build

FROM build AS publish 
WORKDIR /src
# [COPY] İmaj içine dosya veya klasör kopyalamak için kullanılırız.
COPY . . 
# [RUN] İmaj oluşturulurken shell'de bir komut çalıştırmak istersek bu talimat kullanılır.
RUN dotnet restore "CoreTestFramework.Northwind.WebMvcUI/CoreTestFramework.Northwind.WebMvcUI.csproj"
WORKDIR /src/.
RUN dotnet publish "CoreTestFramework.Northwind.WebMvcUI/CoreTestFramework.Northwind.WebMvcUI.csproj" -c Release -o /app 

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CoreTestFramework.Northwind.WebMvcUI.dll"]