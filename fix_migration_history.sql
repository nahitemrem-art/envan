-- Migration history tablosuna kayıt ekle
-- Bu sayede EF Core migration'ın uygulandığını bilir

USE envanterdb;

-- Önce EkleyenKullanici kolonunu ekle (eğer yoksa)
ALTER TABLE Konumlar 
ADD COLUMN IF NOT EXISTS EkleyenKullanici VARCHAR(100) NULL;

-- Migration history tablosuna kayıt ekle
-- Bu migration'ın uygulandığını belirt
INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
VALUES ('20250120000000_AddEkleyenKullaniciToKonum', '8.0.11')
ON DUPLICATE KEY UPDATE ProductVersion = ProductVersion;

-- Kontrol et
SELECT * FROM __EFMigrationsHistory ORDER BY MigrationId DESC LIMIT 5;

SELECT 'Migration başarıyla eklendi!' AS Result;
