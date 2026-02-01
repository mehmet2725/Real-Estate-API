<div align="center">

# 🏠 Real Estate Management API

**Modern, Güvenli ve Ölçeklenebilir Emlak Yönetim Sistemi**

![Platform](https://img.shields.io/badge/.NET-10.0-purple.svg)
![Docker](https://img.shields.io/badge/docker-ready-blue.svg)
![Status](https://img.shields.io/badge/status-active-success.svg)

</div>

## 📖 İçindekiler
- [Proje Hakkında](#-proje-hakkında)
- [Temel Özellikler](#-temel-özellikler)
- [Teknoloji Stack](#-teknoloji-stack)
- [Proje Mimarisi](#-proje-mimarisi)
- [Kurulum ve Çalıştırma](#-kurulum-ve-çalıştırma)
- [API Dokümantasyonu](#-api-dokümantasyonu)
- [Test Kullanıcıları](#-test-kullanıcıları)
- [Geliştirici](#-geliştirici)

---

## 🎯 Proje Hakkında

**Real Estate Management API**, emlak ilanlarını yönetmek, gelişmiş filtreleme seçenekleriyle aramak ve emlakçılar ile müşteriler arasındaki iletişimi sağlamak amacıyla geliştirilmiş, **Clean Architecture** prensiplerine uygun, yüksek performanslı bir RESTful API projesidir.

Proje, **.NET 10**, **PostgreSQL** ve **Docker** teknolojileri kullanılarak, endüstri standartlarında (SOLID, Dependency Injection, Repository Pattern) geliştirilmiştir.

---

## ✨ Temel Özellikler

### 🔐 Güvenlik & Kimlik Doğrulama
* **JWT Authentication:** Access Token (kısa ömürlü) ve Refresh Token (uzun ömürlü) yapısı.
* **Role-Based Authorization:** `Admin`, `Agent` ve `User` rolleri ile yetkilendirme.
* **Rate Limiting:** IP bazlı istek sınırlama ile DDoS koruması.
* **Security Headers:** Helmet benzeri güvenlik başlıkları (XSS, Clickjacking koruması).
* **CORS Politikaları:** Frontend uygulamaları için güvenli erişim ayarları.

### 🚀 Performans & Optimizasyon
* **In-Memory Caching:** Sık değişmeyen veriler (örn: Emlak Tipleri) için önbellekleme.
* **Response Compression:** Gzip/Brotli ile API yanıtlarının sıkıştırılması.
* **AsNoTracking:** Okuma işlemlerinde EF Core tracking mekanizmasının devre dışı bırakılması.
* **Health Checks:** Uygulama ve veritabanı sağlığının `/health` endpoint'i ile izlenmesi.

### 📊 Veri Yönetimi
* **Gelişmiş Filtreleme:** Şehir, fiyat aralığı, oda sayısı vb. kriterlere göre dinamik sorgulama.
* **Pagination & Sorting:** Büyük veri setleri için sayfalama ve sıralama.
* **Soft Delete:** Veri güvenliği için silinen kayıtların veritabanında saklanması.
* **FluentValidation:** Gelen isteklerin iş kuralı seviyesinde doğrulanması.

---

## 🛠 Teknoloji Stack

| Kategori | Teknoloji | Açıklama |
|:---------|:----------|:---------|
| **Backend** | .NET 10 | Core Web API Framework |
| **Veritabanı** | PostgreSQL 15 | İlişkisel Veritabanı |
| **ORM** | EF Core 10 | Code-First Yaklaşımı |
| **Container** | Docker | Konteynerizasyon & Orchestration |
| **Mapping** | AutoMapper | Entity-DTO Dönüşümleri |
| **Validation** | FluentValidation | Model Doğrulama |
| **Docs** | Swagger / OpenAPI | API Dokümantasyonu |

---

## 🏗 Proje Mimarisi

Proje **Clean Architecture (Onion Architecture)** prensiplerine göre katmanlara ayrılmıştır:
```
RealEstate.API/
│
├── 📁 RealEstate.API                    # Presentation Layer (Sunum)
│   ├── Controllers/                     # (AuthController, PropertiesController...)
│   ├── Middlewares/                     # (GlobalException, SecurityHeaders...)
│   └── Tools/                           # (JwtTokenGenerator, SeedData...)
│
├── 📁 RealEstate.Business               # Application Layer (İş Mantığı)
│   ├── Abstract/                        # (IPropertyService, IInquiryService...)
│   ├── Concrete/                        # (PropertyManager, InquiryManager...)
│   ├── Dtos/                            # (AuthDtos, PropertyDtos...)
│   ├── ValidationRules/                 # (PropertyCreateValidator...)
│   └── Profiles/                        # (AutoMapper Profilleri)
│
├── 📁 RealEstate.Data                   # Infrastructure Layer (Altyapı)
│   ├── Concrete/                        # (RealEstateDbContext, GenericRepository...)
│   └── Migrations/                      # (Veritabanı Göç Dosyaları)
│
└── 📁 RealEstate.Entity                 # Domain Layer (Varlıklar)
    ├── Concrete/                        # (AppUser, Property, Inquiry, PropertyType)
    └── Abstract/                        # (BaseClass)
```

---

## 🐳 Kurulum ve Çalıştırma

Projeyi en kolay şekilde ayağa kaldırmak için Docker kullanmanızı öneririz.

### Seçenek 1: Docker (Önerilen)

1. Projeyi klonlayın:
```bash
git clone https://github.com/mehmet2725/real-estate-api.git
cd real-estate-api
```

2. Docker Compose ile başlatın:
```bash
docker-compose up -d --build
```

3. Tarayıcıda Swagger'ı açın:
👉 http://localhost:5070/swagger

### Seçenek 2: Manuel Kurulum

1. `appsettings.json` dosyasındaki veritabanı bağlantı cümlesini (Connection String) kendi local PostgreSQL sunucunuza göre düzenleyin.

2. Migrationları uygulayın:
```bash
dotnet ef database update --project RealEstate.Data --startup-project RealEstate.API
```

3. Projeyi çalıştırın:
```bash
dotnet run --project RealEstate.API
```

---

## 📡 API Dokümantasyonu

### Swagger UI
Sistemi görsel olarak test etmek için Swagger arayüzünü kullanabilirsiniz.

- **URL:** http://localhost:5070/swagger
- **Authorize:** Login endpoint'inden aldığınız Token'ı `Bearer {token}` formatında girerek kilitli endpointleri açabilirsiniz.

### Postman Collection
Proje kök dizininde bulunan `RealEstate_Postman_Collection.json` dosyasını Postman'e import ederek tüm hazır istekleri kullanabilirsiniz.

### Önemli Endpoint'ler

| Metot | Endpoint | Açıklama | Yetki |
|:------|:---------|:---------|:------|
| POST | /api/auth/login | Sisteme giriş yap ve Token al | Herkes |
| POST | /api/auth/refresh-token | Access Token yenile | Herkes |
| GET | /api/properties | Tüm ilanları listele | Herkes |
| GET | /api/properties/search | Detaylı filtreleme yap | Herkes |
| POST | /api/inquiries | İlan sahibine mesaj gönder | Herkes |
| GET | /api/inquiries | Gelen mesajları oku | Admin/Agent |
| POST | /api/propertytypes | Yeni emlak tipi ekle | Admin |

---

## 🧪 Test Kullanıcıları

Uygulama ilk kez çalıştırıldığında (Seed Data), veritabanına otomatik olarak aşağıdaki kullanıcılar eklenir:

| Rol | E-Posta | Şifre | Yetkiler |
|:----|:--------|:------|:---------|
| **Admin** | admin@test.com | Admin123! | Tam sistem erişimi. |
| **Agent** | agent@test.com | Agent123! | İlan yönetimi, mesajlaşma. |
| **User** | user@test.com | User123! | İlan görüntüleme, mesaj atma. |

---

<div align="center">

## 👨‍💻 Geliştirici

**Mehmet Sönmez**

GitHub: [@mehmet2725](https://github.com/mehmet2725)

</div>

---

