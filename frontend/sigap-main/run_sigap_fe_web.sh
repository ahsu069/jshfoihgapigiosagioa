#!/bin/bash

# === Konfigurasi ===
IMAGE_NAME="sigap-fe-web"
CONTAINER_NAME="sigap-fe-web"
PORT=1234

echo "🚀 Build sigap Web..."
dotnet run

if [ $? -ne 0 ]; then
  echo "❌ Gagal build sigap Web"
  exit 1
fi

echo "📦 Build Docker Image: $IMAGE_NAME"
docker build -t $IMAGE_NAME .

if [ $? -ne 0 ]; then
  echo "❌ Gagal build Docker Image"
  exit 1
fi

echo "🧼 Stop & Remove container lama (jika ada)..."
docker rm -f $CONTAINER_NAME 2>/dev/null

echo "▶️ Jalankan container..."
docker run -d --name $CONTAINER_NAME -p $PORT:80 $IMAGE_NAME

echo "✅ Berhasil! Akses aplikasi di: http://localhost:$PORT"