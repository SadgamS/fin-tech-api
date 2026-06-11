# FinTech API

## Tecnologias Utilizadas

- .NET 10
- Entity Framework Core
- PostgreSQL
- XUnit
- Moq para mocking
- Swagger (documentción de API)

## Requisitos

- .NET 10 SDK
- PostgreSQL
- Ef Core CLI (opcional, para migraciones)

## Configuración

1. Clona el repositorio:
   ```bash
   git clone
   ```
2. Configura la cadena de conexión en `appsettings.json` o `appsettings.Development.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=FinTechDb;Username=postgres;Password=yourpassword"
   }
   ```

3. Instala las dependencias:

   ```bash
   dotnet restore
   ```

4. Aplica las migraciones para crear la base de datos:

   ```bash
   dotnet ef database update --project src/FinTech.Infrastructure --startup-project src/FinTech.API
   ```

5. Ejecuta la API:

   ```bash
   dotnet run --project src/FinTech.API
   ```

6. Accede a Swagger para probar los endpoints:
   ```
   http://localhost:5192/swagger
   ```

## Testing

Para ejecutar los tests unitarios, utiliza el siguiente comando:

```bash
dotnet test --project src/FinTech.Tests
```

## Deploy url

La API está desplegada en Railway y se puede acceder a través de la siguiente URL:

```
https://fin-tech-api-production.up.railway.app/swagger/index.html
```