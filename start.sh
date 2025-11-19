#!/bin/bash

echo "=========================================="
echo "  Sakarya Kent Rehberi - Başlatılıyor"
echo "=========================================="
echo ""

# MySQL kontrol
echo "🔍 MySQL kontrol ediliyor..."
if docker ps | grep -q mysql-dev; then
    echo "   ✅ MySQL container çalışıyor"
else
    echo "   ⚠️  MySQL container çalışmıyor"
    echo "   🚀 MySQL başlatılıyor..."
    
    # Eski container'ı temizle
    docker rm -f mysql-dev 2>/dev/null
    
    # Yeni container başlat
    docker run -d \
        --name mysql-dev \
        -e MYSQL_ROOT_PASSWORD=root \
        -e MYSQL_DATABASE=envanterdb \
        -p 3306:3306 \
        mysql:8.0 > /dev/null 2>&1
    
    if [ $? -eq 0 ]; then
        echo "   ✅ MySQL başlatıldı"
        echo "   ⏳ MySQL'in hazır olması için 20 saniye bekleniyor..."
        sleep 20
    else
        echo "   ❌ MySQL başlatılamadı!"
        exit 1
    fi
fi

echo ""

# Build kontrol
echo "🔨 Uygulama derleniyor..."
if dotnet build --no-restore > /dev/null 2>&1; then
    echo "   ✅ Derleme başarılı"
else
    echo "   ⚠️  Derleme başarısız, restore yapılıyor..."
    dotnet restore > /dev/null 2>&1
    if dotnet build > /dev/null 2>&1; then
        echo "   ✅ Derleme başarılı"
    else
        echo "   ❌ Derleme hatası!"
        echo ""
        echo "Detaylı hata için çalıştırın:"
        echo "  dotnet build"
        exit 1
    fi
fi

echo ""

# Uygulama başlat
echo "🚀 Uygulama başlatılıyor..."
echo ""
echo "=========================================="
echo "  Uygulama çalışıyor!"
echo "=========================================="
echo ""
echo "📍 Migration'lar otomatik uygulanacak"
echo "📍 Örnek veriler yüklenecek (ilk çalıştırma)"
echo ""
echo "Konsol çıktısında gösterilen URL'yi"
echo "tarayıcınızda açın (genellikle http://localhost:5157)"
echo ""
echo "Durdurmak için: Ctrl+C"
echo ""
echo "=========================================="
echo ""

dotnet run
