# Database Migration Guide

## Otomatik Migration (Program.cs)

Oduyo.Test projesi başlatıldığında otomatik olarak pending migration'ları kontrol eder ve uygular.

```csharp
// Program.cs içinde otomatik migration yapılıyor
- Pending migration'lar kontrol ediliyor
- Varsa otomatik olarak uygulanıyor
- Log'lar console'a yazılıyor
```

## Manuel Migration (PowerShell Script)

### Kullanım

Migration oluşturmak ve uygulamak için:

```powershell
.\migrate.ps1
```

### Script Ne Yapar?

Script interaktif çalışır ve her adımda onay ister:

1. **Timestamp ile migration adı oluşturur**
   - Format: `Migration_yyyyMMddHHmmss`
   - Örnek: `Migration_20251012153045`

2. **Migration oluşturma onayı**
   - Script migration oluşturmak için Y/N sorar
   - N seçerseniz işlem iptal olur

3. **Migration dosyalarını oluşturur**
   - Konum: `Oduyo.DataAccess/Migrations/`
   - Dosyalar: `{timestamp}_{MigrationName}.cs` ve `{timestamp}_{MigrationName}.Designer.cs`

4. **Database update onayı**
   - Script database'i güncellemek için Y/N sorar
   - ⚠️ WARNING mesajı gösterir (database schema değişecek!)
   - N seçerseniz sadece migration dosyaları oluşturulur, database'e uygulanmaz

5. **Database'i günceller** (onaylanırsa)
   - Yeni migration'ı database'e uygular
   - Schema değişikliklerini yapar

### Gereksinimler

- .NET 9.0 SDK
- dotnet-ef tools (script otomatik yükler)
- PostgreSQL bağlantısı

### İlk Kurulum

1. PostgreSQL'in çalıştığından emin olun
2. appsettings.json'da connection string'i kontrol edin:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=OduyoTestDb;Username=postgres;Password=postgres"
   }
   ```

3. İlk migration'ı oluşturun:
   ```powershell
   .\migrate.ps1
   ```

### Manuel EF Core Komutları

İsterseniz EF Core komutlarını manuel olarak da çalıştırabilirsiniz:

#### Migration Oluşturma
```bash
dotnet ef migrations add MigrationName ^
    --project Oduyo.DataAccess\Oduyo.DataAccess.csproj ^
    --startup-project Oduyo.Test\Oduyo.Test.csproj ^
    --context ApplicationDbContext
```

#### Database Güncelleme
```bash
dotnet ef database update ^
    --project Oduyo.DataAccess\Oduyo.DataAccess.csproj ^
    --startup-project Oduyo.Test\Oduyo.Test.csproj ^
    --context ApplicationDbContext
```

#### Migration Listesini Görme
```bash
dotnet ef migrations list ^
    --project Oduyo.DataAccess\Oduyo.DataAccess.csproj ^
    --startup-project Oduyo.Test\Oduyo.Test.csproj ^
    --context ApplicationDbContext
```

#### Son Migration'ı Geri Alma
```bash
dotnet ef migrations remove ^
    --project Oduyo.DataAccess\Oduyo.DataAccess.csproj ^
    --startup-project Oduyo.Test\Oduyo.Test.csproj ^
    --context ApplicationDbContext
```

#### Belirli Bir Migration'a Geri Dönme
```bash
dotnet ef database update MigrationName ^
    --project Oduyo.DataAccess\Oduyo.DataAccess.csproj ^
    --startup-project Oduyo.Test\Oduyo.Test.csproj ^
    --context ApplicationDbContext
```

### Sorun Giderme

#### EF Tools Bulunamıyor
```powershell
dotnet tool install --global dotnet-ef
```

#### Migration Uygulanamıyor
1. PostgreSQL'in çalıştığından emin olun
2. Connection string'i kontrol edin
3. Database kullanıcısının gerekli izinleri olduğundan emin olun

#### Database Sıfırlama (Dikkat: Tüm verileri siler!)
```bash
dotnet ef database drop --force ^
    --project Oduyo.DataAccess\Oduyo.DataAccess.csproj ^
    --startup-project Oduyo.Test\Oduyo.Test.csproj ^
    --context ApplicationDbContext
```

## Migration Dosya Yapısı

```
Oduyo.DataAccess/
└── Migrations/
    ├── 20251012153045_Migration_20251012153045.cs
    ├── 20251012153045_Migration_20251012153045.Designer.cs
    └── ApplicationDbContextModelSnapshot.cs
```

## Notlar

- Her migration timestamp ile adlandırılır, bu sayede sıralama ve takip kolaydır
- Migration'lar version control'e commit edilmelidir
- Production ortamında migration'lar dikkatli uygulanmalıdır
- Geri alınamaz değişiklikler yapmadan önce backup alın
