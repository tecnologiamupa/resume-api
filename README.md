# API de Hojas de Vida - Sistema de Reclutamiento

API para gestión de currículums y hojas de vida del proyecto de Feria de Empleo.

## Tecnologías

- .NET 8.0
- ASP.NET Core Web API
- MySQL
- Entity Framework Core
- JWT Authentication

## Configuración

### 1. Base de Datos

Crear las bases de datos necesarias:
- `resume`
- `recruitment_shared`
- `recruitment_companies`

### 2. Configuración Local

Crear un archivo `appsettings.Development.json` en `Resume.API` con tus credenciales:

```json
{
  "ConnectionStrings": {
    "ResumeConnection": "Host=localhost; Port=3306; Database=resume; Username=tu_usuario; Password=tu_password",
    "RecruitmentSharedConnection": "Host=localhost; Port=3306; Database=recruitment_shared; Username=tu_usuario; Password=tu_password",
    "CompanyRecruitmentConnection": "Host=localhost; Port=3306; Database=recruitment_companies; Username=tu_usuario; Password=tu_password"
  },
  "JwtSettings": {
    "Key": "tu-clave-secreta-jwt-de-al-menos-32-caracteres"
  },
  "ExternalApis": {
    "CompanyService": {
      "BaseUrl": "http://localhost:5013"
    }
  },
  "Sftp": {
    "Host": "localhost",
    "Username": "tu_usuario_sftp",
    "Password": "tu_password_sftp"
  }
}
```

### 3. Ejecutar Migraciones

```bash
cd Resume.API
dotnet ef database update
```

### 4. Ejecutar la Aplicación

```bash
dotnet run
```

La API estará disponible en `https://localhost:7xxx` o `http://localhost:5xxx`

## Estructura del Proyecto

- **Resume.API**: Capa de presentación (Controllers, Middlewares)
- **Resume.Core**: Lógica de negocio (Services, DTOs, Entities)
- **Resume.Infrastructure**: Acceso a datos (Repositories, DbContext, External Services)

## Funcionalidades

- Gestión de hojas de vida
- Carga de documentos (CV en PDF)
- Experiencia laboral y educación
- Habilidades y certificaciones
- Integración con servicio de empresas

## Seguridad

⚠️ **IMPORTANTE**: 
- Nunca commitees archivos con credenciales reales
- Usa `appsettings.Development.json` para configuración local (este archivo está en .gitignore)
- En producción, usa variables de entorno o servicios de configuración seguros
- Los archivos cargados deben ser validados y escaneados