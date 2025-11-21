-- EkleyenKullanici kolonunu Konumlar tablosuna ekle
USE envanterdb;

ALTER TABLE Konumlar 
ADD COLUMN EkleyenKullanici VARCHAR(100) NULL;

-- Kontrol et
DESCRIBE Konumlar;
