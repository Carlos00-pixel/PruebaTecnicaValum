# Project Management API

API REST para la gestión de proyectos y tareas, desarrollada con .NET 8 y Entity Framework Core, siguiendo una arquitectura en capas y utilizando MediatR para separar las peticiones HTTP de la lógica de aplicación.

## Tecnologías

- .NET 8 y ASP.NET Core Web API
- Entity Framework Core 8
- SQLite
- MediatR
- FluentValidation
- xUnit
- Moq

## Arquitectura

La solución está organizada en los siguientes proyectos:

- **ProjectManagement.Domain:** entidades y enumeraciones del dominio.
- **ProjectManagement.Application:** comandos, consultas, handlers, DTOs, validadores y excepciones.
- **ProjectManagement.Infrastructure:** persistencia con Entity Framework Core y repositorios.
- **ProjectManagement.Api:** controladores REST, configuración de la aplicación y middleware global de errores.
- **ProjectManagement.Tests:** pruebas unitarias de handlers, consultas y validadores.

## Requisitos previos

- .NET 8 SDK.
- Git, si se desea clonar el repositorio.

## Instalación y ejecución

Clonar el repositorio:

git clone https://github.com/Carlos00-pixel/PruebaTecnicaValum.git
cd ProjectManagement

Restaurar dependencias y compilar:

dotnet restore
dotnet build

Ejecutar la API:

dotnet run --project src/ProjectManagement.Api

La aplicación aplica las migraciones de Entity Framework Core al arrancar. La base de datos SQLite se configura mediante la cadena de conexión de la configuración de la API.

## Pruebas automatizadas

Ejecutar todas las pruebas:

dotnet test

Las pruebas cubren reglas de negocio, actualización del estado de tareas, validación de comandos y consulta paginada con mapeo a DTOs.

## Endpoints

### Proyectos

- POST - `/api/projects` -> Crear un proyecto
- GET -`/api/projects` -> Listar proyectos con paginación y filtro por estado
- GET - `/api/projects/{id}` -> Consultar el detalle y el resumen de tareas
- PATCH - `/api/projects/{id}/archive` -> Archivar un proyecto

### Tareas

- POST - `/api/projects/{projectId}/tasks` -> Crear una tarea
- GET - `/api/projects/{projectId}/tasks` -> Listar tareas con paginación y filtros
- GET - `/api/tasks/{id}` -> Consultar una tarea
- PATCH - `/api/tasks/{id}/status` -> Cambiar el estado de una tarea
- DELETE - `/api/tasks/{id}` -> Eliminar lógicamente una tarea

## Paginación y filtros

Los endpoints de listado utilizan los siguientes valores predeterminados:

- `page`: 1.
- `pageSize`: 10.
- Tamaño máximo de página: 50.

Los proyectos pueden filtrarse por estado. Las tareas pueden filtrarse por estado y prioridad.

## Reglas de negocio

- No se permite crear tareas en proyectos archivados.
- No se permite archivar proyectos que tengan tareas en curso.
- Al completar una tarea, se establece `CompletedAt` en UTC.
- Las tareas se eliminan lógicamente mediante `IsDeleted`.
- Las tareas eliminadas no aparecen en las consultas habituales.
- La eliminación devuelve `204 No Content`, incluso si la tarea ya estaba eliminada.

## Gestión de errores

El middleware global normaliza los errores y devuelve códigos HTTP apropiados para errores de validación, recursos no encontrados, excepciones de negocio y errores inesperados.

## Decisiones de diseño

- Separación de responsabilidades mediante una arquitectura en capas.
- MediatR para desacoplar los controladores de los handlers.
- FluentValidation integrado en el pipeline de MediatR.
- Repositorios para encapsular el acceso a datos.
- Entity Framework Core y SQLite para la persistencia.
- Pruebas unitarias con xUnit y Moq.

## Uso de herramientas de IA

Se ha utilizado ChatGPT como herramienta de apoyo durante el desarrollo para revisar decisiones de arquitectura, resolver incidencias, elaborar código y preparar pruebas. Las soluciones propuestas se han integrado y validado durante el desarrollo mediante compilaciones, pruebas automatizadas y comprobaciones manuales de la API.