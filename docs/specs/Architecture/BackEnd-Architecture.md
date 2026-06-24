# Back End DotNet Architecture for middle sized projects

This architecture follows a **Strict Clean Architecture** pattern for .NET, optimized for scalability and decoupling. Below is the technical specification.

## Core Principle
Dependency flow is **unidirectional** and **inward** toward the Core. The `Core.Interfaces` and `Core.Data` projects act as the "Contractual Hub," allowing `Core` (Business Logic) and `Infrastructure` (Implementation) to remain entirely decoupled.

## Project Definitions & Responsibilities

Project Name | Responsibilities | Dependencies |
:--- | :--- | :--- |
**API.Presentation** | Entry point, HTTP Controllers, Middleware, DI composition. | `Core`, `Infrastructure` |
**Infrastructure** | EF DbContext, Migrations, Repository Impls, External API Clients. | `Interfaces`, `Data` |
**Core** | Pure Business Logic (Services), Use Cases, AutoMapper profiles. | `Interfaces`, `Data`, `Exceptions` |
**Core.Interfaces** | Abstractions for Repositories, Services, and External Clients. | `Core.Data` |
**Core.Data** | Database Entities (POCOs), Enums, and DTOs. | *None* |
**Core.Exceptions** | Custom domain-specific exception types (e.g., `NotFoundException`). | *None* |

---

## Implementation Constraints

1. **Zero Leakage:** `Core` must **never** reference `Infrastructure` or `Microsoft.EntityFrameworkCore`. It interacts with the DB only via `IRepository` interfaces.
2. **Explicit DI:** Each layer provides its own Service Collection extension (e.g., `AddInfrastructureServices`, `AddCoreServices`). The `Presentation` layer calls these in `Program.cs`.
3. **Data Persistence:** Configuration of entities (Fluent API) must live in `Infrastructure`. `Core.Data` entities remain "clean" of persistence-specific attributes where possible.
4. **Error Handling:** Use a Global Exception Middleware in the `Presentation` layer to catch exceptions defined in `Core.Exceptions` and map them to HTTP status codes.

## Testing Strategy

* **Unit Tests (`Tests.Unit`):** Target `Core`. Mock all `Core.Interfaces`.
* **Functional Tests (`Tests.Functional`):** Target `API.Presentation`. Use `WebApplicationFactory` with a mocked/in-memory database.
* **Integration Tests (`Tests.Integration`):** Target `Infrastructure`. Use real database instances (e.g., Testcontainers) to verify SQL/EF Core behavior.

@import "BackEnd-Architecture.mermaid"
