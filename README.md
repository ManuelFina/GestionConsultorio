# Centro Médico Toay - Sistema de Gestión Médica

Sistema de gestión médica desarrollado con **.NET**, compuesto por una **API REST** y una aplicación **Mobile/Desktop con .NET MAUI Blazor Hybrid**.

El proyecto permite administrar pacientes, médicos, recepcionistas, turnos, agenda médica, historiales clínicos y archivos adjuntos asociados a las atenciones.

---

## 📌 Descripción

**Centro Médico Toay** es una aplicación orientada a la gestión interna de un consultorio médico.

El sistema permite trabajar con diferentes roles de usuario, organizar turnos, consultar agenda, registrar pacientes, atender turnos médicos, generar historiales clínicos y adjuntar documentación médica como imágenes o archivos PDF.

El proyecto fue desarrollado como aplicación académica y de portfolio, aplicando separación de responsabilidades entre backend, frontend y capa compartida.

---

## 🧱 Estructura general del proyecto

La solución está dividida en tres proyectos principales:

```text
GestionConsultorio
├── GestionConsultorio.Api
│   └── Backend desarrollado con ASP.NET Core Web API
│
├── GestionConsultorio.Mobile
│   └── Aplicación .NET MAUI Blazor Hybrid
│
└── GestionConsultorio.Shared
    └── Modelos, DTOs, enums y respuestas compartidas
```

---

## 🧩 Estructura detallada

### GestionConsultorio.Api

Proyecto backend encargado de exponer los endpoints REST, manejar autenticación, autorización, reglas de negocio y acceso a datos.

```text
GestionConsultorio.Api
├── Controllers
│   ├── AuthController.cs
│   ├── PacientesController.cs
│   ├── MedicosController.cs
│   ├── RecepcionistasController.cs
│   ├── TurnosController.cs
│   ├── HistorialesClinicosController.cs
│   ├── ArchivosHistorialClinicoController.cs
│   ├── EspecialidadesController.cs
│   └── ConsultoriosController.cs
│
├── Data
│   └── AppDbContext.cs
│
├── Repositories
│   ├── Interfaces
│   └── Implementaciones
│
├── Services
│   ├── Interfaces
│   └── Implementaciones
│
├── Settings
│   └── CloudinarySettings.cs
│
├── Migrations
│
├── appsettings.json
├── Program.cs
└── GestionConsultorio.Api.csproj
```

#### Responsabilidades principales

- Exponer endpoints HTTP.
- Validar reglas de negocio.
- Gestionar autenticación con JWT.
- Controlar permisos por rol.
- Acceder a la base de datos con Entity Framework Core.
- Administrar archivos clínicos mediante Cloudinary.
- Marcar automáticamente turnos vencidos como ausentes.
- Aplicar baja lógica y reactivación de pacientes.

---

### GestionConsultorio.Mobile

Proyecto frontend desarrollado con **.NET MAUI Blazor Hybrid**.

Se encarga de la interfaz de usuario, navegación, consumo de la API, manejo de sesión y experiencia mobile.

```text
GestionConsultorio.Mobile
├── Components
│   ├── Layout
│   │   ├── MainLayout.razor
│   │   └── NavMenu.razor
│   │
│   └── Pages
│       ├── Auth
│       ├── Dashboard
│       ├── Pacientes
│       ├── Medicos
│       ├── Recepcionistas
│       ├── Turnos
│       ├── HistorialClinico
│       ├── Especialidades
│       └── Consultorios
│
├── Helpers
│   ├── ApiRoutes.cs
│   └── ApiErrorHelper.cs
│
├── Services
│   ├── Interfaces
│   └── Implementaciones
│
├── Resources
│   ├── AppIcon
│   ├── Splash
│   ├── Fonts
│   ├── Images
│   └── Raw
│
├── wwwroot
│   ├── css
│   ├── js
│   └── index.html
│
├── MauiProgram.cs
└── GestionConsultorio.Mobile.csproj
```

#### Responsabilidades principales

- Mostrar la interfaz del sistema.
- Consumir endpoints de la API.
- Guardar sesión del usuario.
- Controlar vistas visibles según rol.
- Permitir gestión de pacientes, turnos, agenda e historial clínico.
- Subir archivos adjuntos desde la app.
- Adaptar la experiencia a dispositivos móviles.

---

### GestionConsultorio.Shared

Proyecto compartido entre la API y la aplicación Mobile.

Contiene las clases comunes utilizadas por ambos proyectos.

```text
GestionConsultorio.Shared
├── DTOs
│   ├── Auth
│   ├── Medicos
│   └── Recepcionistas
│
├── Enums
│   └── EstadoTurno.cs
│
├── Models
│   ├── Usuario.cs
│   ├── Paciente.cs
│   ├── Medico.cs
│   ├── Especialidad.cs
│   ├── Consultorio.cs
│   ├── Turno.cs
│   ├── HistorialClinico.cs
│   └── ArchivoHistorialClinico.cs
│
└── Responses
    ├── ApiResponse.cs
    └── ResultadoOperacion.cs
```

#### Responsabilidades principales

- Compartir modelos entre backend y frontend.
- Evitar duplicación de DTOs.
- Centralizar enums y estructuras comunes.
- Mantener consistencia entre API y Mobile.

---

## 🚀 Tecnologías utilizadas

### Backend

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- BCrypt.Net
- Cloudinary
- Repository Pattern
- Services Layer
- Autorización por roles

### Frontend

- .NET MAUI Blazor Hybrid
- Razor Components
- HttpClient
- SecureStorage
- Preferences
- SweetAlert local
- CSS personalizado responsive

### Base de datos y servicios externos

- SQL Server
- Somee para hosting de API y base de datos
- Cloudinary para almacenamiento de archivos clínicos

---

## 👥 Roles del sistema

El sistema trabaja con tres roles principales.

### Administrador

Puede gestionar la configuración general del sistema:

- Médicos
- Recepcionistas
- Especialidades
- Consultorios

### Recepcionista

Puede gestionar la parte administrativa diaria:

- Pacientes
- Turnos
- Agenda
- Baja lógica de pacientes
- Reactivación de pacientes dados de baja

### Médico

Puede gestionar la atención médica:

- Ver agenda propia
- Ver pacientes asociados a sus turnos
- Atender turnos
- Registrar historial clínico
- Adjuntar archivos médicos

---

## ✅ Funcionalidades principales

### Autenticación

- Login con JWT.
- Contraseñas protegidas con BCrypt.
- Control de acceso por roles.
- Persistencia de sesión en el dispositivo.

### Gestión de pacientes

- Alta de pacientes.
- Edición de datos.
- Consulta de pacientes.
- Baja lógica.
- Reactivación de pacientes.
- Filtro entre pacientes activos e inactivos.
- Validación de DNI duplicado.
- Restricción para evitar turnos nuevos con pacientes dados de baja.

### Gestión de médicos

- Alta de médico con usuario de acceso.
- Edición de datos profesionales.
- Asociación con especialidad.
- Usuario médico vinculado por email.
- Acceso del médico únicamente a sus turnos y pacientes.

### Gestión de recepcionistas

- Alta de recepcionistas.
- Edición de datos.
- Eliminación de usuarios de recepción.
- Los recepcionistas se almacenan como usuarios con rol `Recepcionista`.

### Turnos y agenda

- Creación de turnos.
- Edición de turnos.
- Cancelación de turnos.
- Agenda diaria.
- Validación de superposición por médico y consultorio.
- Estados del turno:
  - Pendiente
  - Confirmado
  - Atendido
  - Cancelado
  - Reprogramado
  - Ausente
- Los turnos vencidos que quedan pendientes o confirmados se marcan automáticamente como `Ausente`.

### Atención médica

- El médico atiende un turno desde la agenda.
- Al atender, se genera un historial clínico.
- El turno pasa automáticamente a estado `Atendido`.

### Historial clínico

- Registro de diagnóstico.
- Registro de tratamiento.
- Registro de observaciones.
- Historial asociado a paciente y turno.
- Restricción para evitar más de un historial por turno.
- Consulta del historial clínico por paciente.

### Archivos clínicos

- Carga de archivos asociados al historial clínico.
- Soporte para imágenes y PDF.
- Almacenamiento externo mediante Cloudinary.
- Visualización de archivos adjuntos.
- Eliminación de archivos adjuntos.

---

## 🗃️ Base de datos

El sistema utiliza SQL Server con Entity Framework Core Code First.

Entidades principales:

```text
Usuarios
Pacientes
Medicos
Especialidades
Consultorios
Turnos
HistorialesClinicos
ArchivosHistorialClinico
```

### Aclaración sobre recepcionistas

No existe una tabla separada para recepcionistas.

Una recepcionista es un registro de la tabla `Usuarios` con:

```text
Rol = "Recepcionista"
```

### Aclaración sobre médicos

Un médico tiene dos registros relacionados conceptualmente:

```text
Tabla Medicos
└── Datos profesionales: matrícula, teléfono, especialidad

Tabla Usuarios
└── Datos de acceso: email, contraseña hasheada, rol
```

El vínculo se realiza mediante el email.

---

## 🔐 Configuración de secretos

El archivo `appsettings.json` del repositorio no debe contener credenciales reales.

Ejemplo seguro:

```json
{
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "Jwt": {
    "Audience": "GestionConsultorio.Mobile",
    "ExpiresInMinutes": 120,
    "Issuer": "GestionConsultorio.Api",
    "Key": ""
  },
  "Cloudinary": {
    "CloudName": "",
    "ApiKey": "",
    "ApiSecret": ""
  }
}
```

Para desarrollo o producción se deben utilizar archivos locales no versionados:

```text
appsettings.Development.json
appsettings.Production.json
```

Estos archivos deben estar ignorados por Git:

```gitignore
**/appsettings.Development.json
**/appsettings.Production.json
```

---

## ▶️ Ejecución local

### 1. Clonar el repositorio

```bash
git clone https://github.com/usuario/repositorio.git
cd GestionConsultorio
```

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Configurar la API

Crear o completar el archivo:

```text
GestionConsultorio.Api/appsettings.Development.json
```

con la cadena de conexión, configuración JWT y datos de Cloudinary.

### 4. Ejecutar migraciones

```bash
dotnet ef database update --project GestionConsultorio.Api --startup-project GestionConsultorio.Api
```

### 5. Ejecutar la API

```bash
dotnet run --project GestionConsultorio.Api
```

Endpoint de prueba:

```text
http://localhost:5186/health
```

### 6. Ejecutar la aplicación Mobile

Desde Visual Studio:

1. Seleccionar el proyecto `GestionConsultorio.Mobile`.
2. Elegir dispositivo Android, emulador o Windows.
3. Ejecutar la aplicación.

---

## 🌐 Configuración de URL de la API en Mobile

La URL base de la API se configura en:

```text
GestionConsultorio.Mobile/MauiProgram.cs
```

Ejemplo para desarrollo local:

```csharp
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://192.168.0.2:5186/")
});
```

Ejemplo para producción:

```csharp
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://tu-sitio.somee.com/")
});
```

Las rutas específicas se centralizan en:

```text
GestionConsultorio.Mobile/Helpers/ApiRoutes.cs
```

Ejemplo:

```csharp
public const string Auth = "api/auth";
public const string Pacientes = "api/pacientes";
public const string Turnos = "api/turnos";
```

---

## 📦 Publicación

### Publicar API

```bash
dotnet publish GestionConsultorio.Api/GestionConsultorio.Api.csproj -c Release -o publish/api
```

Luego subir el contenido de:

```text
publish/api
```

al hosting correspondiente.

### Publicar APK Android

```bash
dotnet publish GestionConsultorio.Mobile/GestionConsultorio.Mobile.csproj -f net10.0-android -c Release -p:AndroidPackageFormat=apk
```


## 🧠 Decisiones técnicas

- Se utiliza JWT para autenticación y autorización.
- Las contraseñas se almacenan hasheadas con BCrypt.
- Se aplica autorización por roles.
- Los pacientes no se eliminan físicamente: se utiliza baja lógica.
- Los pacientes dados de baja pueden reactivarse.
- Los historiales clínicos se conservan por trazabilidad.
- Los archivos clínicos se almacenan externamente en Cloudinary.
- Los turnos vencidos se marcan automáticamente como ausentes.
- El médico solo puede visualizar y atender turnos propios.
- La recepcionista gestiona pacientes, turnos y agenda.
- El administrador gestiona la configuración general del sistema.
- La capa `Shared` permite reutilizar modelos, DTOs y enums entre API y Mobile.

---

## 📌 Estado del proyecto

Proyecto finalizado como sistema académico y de portfolio.

Funcionalidades implementadas:

- API REST.
- Aplicación Mobile/Desktop.
- Login con JWT.
- Roles.
- Gestión de pacientes.
- Baja lógica y reactivación.
- Gestión de médicos.
- Gestión de recepcionistas.
- Gestión de especialidades.
- Gestión de consultorios.
- Agenda.
- Turnos.
- Atención médica.
- Historial clínico.
- Archivos adjuntos.
- Hosting de API.
- Base de datos SQL Server en hosting externo.

---

## 👨‍💻 Autor

Desarrollado por **Manuel Fina Echarri**.

Proyecto académico y de portfolio desarrollado con tecnologías .NET.
