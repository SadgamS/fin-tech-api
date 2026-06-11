# Arquitectura del proyecto

Esta diseñado siguiendo la arquitectura limpia (Clean Architecture) para mantener una separación clara entre las diferentes capas de la aplicación. Las capas principales son:

- **Domain**: Contiene las entidades, interfaces y reglas de negocio. Es independiente de cualquier framework o tecnología.
- **Application**: Contiene los casos de uso y la lógica de aplicación. Depende de la capa Domain pero no de la capa Infrastructure.
- **Infrastructure**: Contiene la implementación de acceso a datos utilizando Entity Framework Core. Depende de la capa Domain.
- **API**: Contiene el proyecto de la API RESTful. Depende de la capa Application y de la capa Infrastructure para manejar las solicitudes HTTP y la lógica de negocio.

## Estructura de carpetas

- `src/FinTech.Api`: Contiene el proyecto de la API RESTful.
- `src/FinTech.Domain`: Contiene las entidades, interfaces y lógica de negocio.
- `src/FinTech.Infrastructure`: Contiene la implementación de acceso a datos con Entity Framework Core.
- `src/FinTech.Tests`: Contiene los tests unitarios para la lógica de negocio.

## Patrones utilizados

- **Repository Pattern**: Para abstraer el acceso a datos.
- **Strategy Pattern**: Para manejar diferentes tipos de cálculos de préstamos.
- **Factory Pattern**: Para crear instancias de estrategias de cálculo de préstamos.
- **Dependency Injection**: Para gestionar las dependencias entre clases.

## Decisiones de diseño

- EF Core es el ORM estándar del ecosistema .NET y tiene integración nativa con PostgreSQL a través de Npgsql. 
- Se utiliza Strategy y Factory Pattern para permitir la flexibilidad de agregar nuevos tipos de cálculos de préstamos sin modificar el código existente.

## Trade-offs

- Se optó por una arquitectura limpia para mantener una separación clara de responsabilidades, aunque esto puede aumentar la complejidad inicial del proyecto.
- Sin autenticación ni autorización para simplificar el proyecto, aunque esto no es recomendable para aplicaciones en producción.
- Sin paginación ni filtros en los endpoints para mantener la simplicidad, aunque esto puede afectar el rendimiento con grandes volúmenes de datos.
- Migraciones automáticas para facilitar el desarrollo, aunque esto puede ser riesgoso en entornos de producción.