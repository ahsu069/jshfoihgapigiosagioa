# SIGAP API

🚀 Cara Build & Run in Docker
1. Buka terminal / CLI, navigasi ke folder **backend**:
`cd path/ke/backend`

2. Build image:
`docker build --no-cache -t cognifi.sigap.dev/api .`

3. Run container:
`docker run -d -p 5000:80 -e ConnectionStrings__DefaultConnection="Server=host.docker.internal,1433;Database=SIGAP_DB;User Id=sa;Password=BroukenDev123;TrustServerCertificate=True;" -e ASPNETCORE_ENVIRONMENT=Development --name sigap.dev.api cognifi.sigap.dev/api`

4. Cek container berjalan:
`docker ps`

5. Cek logs kalau error:
`docker logs sigap.dev.api`

[API Documentation](http://localhost:5000/swagger/index.html) 
