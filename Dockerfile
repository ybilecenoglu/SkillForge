# [FROM] Oluşturulacak imajın hangi imajdan oluşturulacağını belirten talilamttır.
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# [WORKDIR] Komutuyla imaj içerisinde hangi klasöre geçmek yerine bu talimat kullanılarak istediğimiz klasöre geçer ve oradan çalışmaya devam ederiz.
WORKDIR /app
# [EXPOSE] İmajdan oluşturulacak containerlerin hangi portlar üstünden erişilebileceğini yani hangi portların yayınlanacağını belirtiriz.
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SkillForge.WebMvcUI/SkillForge.WebMvcUI.csproj", "SkillForge.WebMvcUI/"]
COPY ["SkillForge.Business/SkillForge.Business.csproj", "SkillForge.Business/"]
COPY ["SkillForge.DataAccess/SkillForge.DataAccess.csproj", "SkillForge.DataAccess/"]
COPY ["SkillForge.Entities/SkillForge.Entities.csproj", "SkillForge.Entities/"]
COPY ["SkillForge.Core/SkillForge.Core.csproj", "SkillForge.Core/"]
RUN dotnet restore "SkillForge.WebMvcUI/SkillForge.WebMvcUI.csproj"
COPY . .
WORKDIR /src/.
RUN dotnet build "SkillForge.WebMvcUI/SkillForge.WebMvcUI.csproj" -c Release -o /app/build

FROM build AS publish 
WORKDIR /src
# [COPY] İmaj içine dosya veya klasör kopyalamak için kullanılırız.
COPY . . 
# [RUN] İmaj oluşturulurken shell'de bir komut çalıştırmak istersek bu talimat kullanılır.
RUN dotnet restore "SkillForge.WebMvcUI/SkillForge.WebMvcUI.csproj"
WORKDIR /src/.
RUN dotnet publish "SkillForge.WebMvcUI/SkillForge.WebMvcUI.csproj" -c Release -o /app 

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SkillForge.WebMvcUI.dll"]