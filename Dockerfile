# 1. Build Aşaması
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Proje dosyalarını kopyala
COPY . .

# Bağımlılıkları yükle (Restore)
RUN dotnet restore

# Projeyi derle (Publish)
RUN dotnet publish RealEstate.API/RealEstate.API.csproj -c Release -o /app/out

# 2. Run Aşaması (Daha hafif imaj)
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Build aşamasından çıkan dosyaları al
COPY --from=build /app/out .

# Resimlerin yükleneceği klasörü oluştur (Hata almamak için)
RUN mkdir -p wwwroot/images

# Port ayarı (Render/Heroku gibi yerler için dinamik port desteği)
ENV ASPNETCORE_URLS=http://+:8080

# Uygulamayı başlat
ENTRYPOINT ["dotnet", "RealEstate.API.dll"]