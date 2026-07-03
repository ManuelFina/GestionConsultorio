# GestionConsultorio

Sistema de gestión para consultorios médicos desarrollado con **ASP.NET Core Web API**, **SQL Server** y **Blazor Hybrid / .NET MAUI**.

El proyecto permite administrar pacientes, médicos, turnos, agenda diaria, historiales clínicos y archivos adjuntos asociados a cada atención médica.

---

## Tecnologías utilizadas

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- .NET MAUI Blazor Hybrid
- JWT Authentication
- BCrypt
- Cloudinary
- Repository Pattern
- Services Layer
- Dependency Injection

---

## Funcionalidades principales

- Login con autenticación JWT.
- Gestión de usuarios por roles.
- Roles: `Administrador`, `Recepcionista` y `Medico`.
- ABM de pacientes.
- ABM de médicos.
- ABM de especialidades.
- ABM de consultorios.
- Gestión de turnos médicos.
- Agenda diaria de turnos.
- Confirmación, cancelación y atención de turnos.
- Registro de historial clínico al atender un turno.
- Adjuntos médicos en historiales clínicos mediante Cloudinary.
- Validaciones de duplicados y reglas de negocio.

---

## Flujo de atención médica

```text
Agenda
↓
Seleccionar turno
↓
Atender
↓
Cargar diagnóstico, tratamiento y observaciones
↓
Guardar atención
↓
Se genera el historial clínico
↓
El turno pasa a estado Atendido
```

---

## Arquitectura del proyecto

```text
GestionConsultorio.Api
GestionConsultorio.Mobile
GestionConsultorio.Shared
```

### GestionConsultorio.Api

Contiene la **API REST**, la lógica de negocio, los repositorios, la autenticación y la integración con Cloudinary.

### GestionConsultorio.Mobile

Aplicación **Blazor Hybrid / .NET MAUI** que consume la API y permite interactuar con el sistema.

### GestionConsultorio.Shared

Proyecto compartido entre la API y la aplicación Mobile. Contiene modelos, DTOs, enums y respuestas comunes.

---

## Módulos del sistema

- Pacientes
- Médicos
- Especialidades
- Consultorios
- Turnos
- Agenda
- Historial clínico
- Archivos adjuntos
- Usuarios y autenticación

---

## Archivos adjuntos

El sistema permite adjuntar archivos a los historiales clínicos utilizando **Cloudinary**.

Tipos permitidos:

```text
JPG
PNG
WEBP
PDF
```

Cada archivo queda asociado a un historial clínico y puede visualizarse desde la aplicación.

---

## Seguridad

El sistema utiliza autenticación mediante **JWT** y almacenamiento seguro de contraseñas con **BCrypt**.

Las funcionalidades se restringen según el rol del usuario autenticado.

Roles disponibles:

```text
Administrador
Recepcionista
Medico
```

---

## Próximas mejoras

- Dashboard con estadísticas.
- Gestión avanzada de usuarios.
- Reportes por médico, fecha y especialidad.
- Exportación de datos.
- Mejoras visuales en la interfaz mobile.
- Tests unitarios para services.

---

## Autor

Desarrollado por **Manuel Fina Echarri**.

Proyecto realizado como práctica integral de desarrollo backend, frontend mobile, base de datos, autenticación y arquitectura en .NET.
