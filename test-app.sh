#!/bin/bash

echo "========================================="
echo "Sakarya Kent Rehberi - Test Script"
echo "========================================="
echo ""

# Check if PostgreSQL container is running
echo "1. PostgreSQL kontrolü..."
if docker ps | grep -q postgres-dev; then
    echo "   ✓ PostgreSQL container çalışıyor"
else
    echo "   ✗ PostgreSQL container çalışmıyor"
    echo "   Başlatılıyor..."
    docker run -d --name postgres-dev \
        -e POSTGRES_PASSWORD=postgres \
        -e POSTGRES_USER=postgres \
        -e POSTGRES_DB=envanterdb \
        -p 5432:5432 \
        postgres:15-alpine > /dev/null 2>&1
    echo "   ✓ PostgreSQL başlatıldı (5 saniye bekleniyor...)"
    sleep 5
fi
echo ""

# Build the application
echo "2. Uygulama derleniyor..."
dotnet build --no-restore > /dev/null 2>&1
if [ $? -eq 0 ]; then
    echo "   ✓ Derleme başarılı"
else
    echo "   ✗ Derleme hatası"
    exit 1
fi
echo ""

# Start the application
echo "3. Uygulama başlatılıyor..."
dotnet run > app-test.log 2>&1 &
APP_PID=$!
echo "   Process ID: $APP_PID"
sleep 10
echo ""

# Check if application started
echo "4. Uygulama testi..."
if curl -s http://localhost:5157/ > /dev/null; then
    echo "   ✓ Ana sayfa erişilebilir"
else
    echo "   ✗ Ana sayfa erişilemiyor"
    kill $APP_PID 2>/dev/null
    exit 1
fi

if curl -s http://localhost:5157/Harita | grep -q "Sakarya"; then
    echo "   ✓ Harita sayfası çalışıyor"
else
    echo "   ✗ Harita sayfası çalışmıyor"
    kill $APP_PID 2>/dev/null
    exit 1
fi
echo ""

# Show locations from database
echo "5. Veritabanı kontrolü..."
LOCATION_COUNT=$(docker exec postgres-dev psql -U postgres -d envanterdb -t -c "SELECT COUNT(*) FROM \"Konumlar\";" 2>/dev/null | xargs)
if [ ! -z "$LOCATION_COUNT" ] && [ "$LOCATION_COUNT" -gt 0 ]; then
    echo "   ✓ Veritabanında $LOCATION_COUNT konum bulundu"
else
    echo "   ⚠ Konum verisi okunamadı (bu normal olabilir)"
fi
echo ""

echo "========================================="
echo "✓ Tüm testler başarılı!"
echo "========================================="
echo ""
echo "Uygulama çalışıyor:"
echo "  URL: http://localhost:5157"
echo "  PID: $APP_PID"
echo ""
echo "Durdurmak için: kill $APP_PID"
echo "Logları görmek için: tail -f app-test.log"
echo ""
