# Veterinaria - Backend

Sistema de gestión básico para clínicas veterinarias. Este repositorio contiene el backend del proyecto, desarrollado como trabajo académico. El objetivo es ofrecer funcionalidades esenciales para la gestión de clientes, mascotas, citas y tratamientos en una veterinaria, siguiendo principios de separación de responsabilidades y diseño modular.

Arquitectura

Se utilizó la Arquitectura Hexagonal (Ports & Adapters) para mantener el dominio central independiente de detalles de infraestructura (bases de datos, frameworks web, etc.). Esto facilita pruebas unitarias, la evolución del sistema y el reemplazo de adaptadores sin tocar la lógica de negocio.

Estructura (resumen)

- Dominio: Entidades y reglas de negocio (propio, independiente).
- Aplicación: Casos de uso y orquestación de operaciones del dominio.
- Adaptadores / Infraestructura: Persistencia, controladores HTTP, servicios externos.
- Puertos: Interfaces que definen contratos entre el dominio y el exterior.

Tecnologías

- Lenguaje: C#
- Framework: .NET / ASP.NET Core (API REST para el backend)
- Persistencia: Implementaciones concretas en adaptadores (por ejemplo, una base de datos relacional)

Principales funcionalidades (básicas)

- Gestión de clientes (propietarios)
- Gestión de mascotas
- Agenda de citas
- Registro de tratamientos y consultas

Cómo ejecutar (rápido)

1. Abrir la solución en Visual Studio / dotnet CLI.
2. Restaurar paquetes: `dotnet restore`.
3. Configurar la cadena de conexión en appsettings (si aplica).
4. Ejecutar: `dotnet run` o desde Visual Studio.

Notas

- Proyecto académico: no se trata de un producto comercial; está pensado como base para aprendizaje y extensión.
- Para contribuciones o dudas, abrir un issue o contactar al autor.

Licencia

Este repositorio incluye un archivo LICENSE (MIT).