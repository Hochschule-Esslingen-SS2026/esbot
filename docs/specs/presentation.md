# ESBot – Group 4
## Project Documentation

## Tech Stack [J.S.]

| Layer | Technology |
|---|---|
| **Backend** | .NET 10 (C#) |
| **Database** | PostgreSQL |
| **Frontend** | FastAPI |

## Architecture [J.S.]

### High-Level Architecture
![Image](./Architecture/BackEnd-Architecture.mermaid)

### Project Definitions & Responsibilities

Project Name | Responsibilities | Dependencies |
:--- | :--- | :--- |
**API.Presentation** | Entry point, HTTP Controllers, Middleware, DI composition. | `Core`, `Infrastructure` |
**Infrastructure** | EF DbContext, Migrations, Repository Impls, External API Clients. | `Interfaces`, `Data` |
**Core** | Pure Business Logic (Services), Use Cases, AutoMapper profiles. | `Interfaces`, `Data`, `Exceptions` |
**Core.Interfaces** | Abstractions for Repositories, Services, and External Clients. | `Core.Data` |
**Core.Data** | Database Entities (POCOs), Enums, and DTOs. | *None* |
**Core.Exceptions** | Custom domain-specific exception types (e.g., `NotFoundException`). | *None* |

## API Design [J.S.]

The prototype exposes a **RESTful HTTP API** following standard conventions:

- Resources are represented as URL segments (e.g., `/api/v1/users`, `/api/v1/sessions`)
- HTTP verbs are used semantically (`GET`, `POST`, `PUT`, `DELETE`)
- Responses use appropriate HTTP status codes (`200`, `201`, `400`, `404`, `500`)
- Data is exchanged in **JSON** format

State management is handled **server-side** via the database. The API itself is **stateless** — each request contains all information necessary to process it.

## Data Model [J.S.]

Data persistence is managed through **Entity Framework Core** using the `ApplicationDbContext`. The database runs on **PostgreSQL** and is fully accessible for inspection and testing.

Entities and their relationships are defined as C# model classes and mapped to database tables via EF Core conventions and Fluent API configuration where needed. Database schema changes are managed through **EF Core Migrations**.

### Database

The **PostgreSQL** database is fully accessible for demonstration and testing purposes. Schema inspection can be done via **JetBrains DataGrip**.

## Requirements [Leon]

### Original Requirements

The following functional and non-functional requirements were defined during the initial exercises:

#### Functional Requirements (FR)

| ID | Requirement Name |  Description | Status |
|---|---|---|---|
| **FR-1** | **Conversational Interface** |  *(as defined in exercise)* |✅ Implemented |
| **FR-2** | **Contextual Explanations** |*(as defined in exercise)* | ✅ Implemented |
| **FR-3** | **Exercise Generation** | *(as defined in exercise)* |✅ Implemented |
| **FR-4** | **Automated Evaluation** | *(as defined in exercise)* |✅ Implemented |
| **FR-5** | **Session Persistence** |*(as defined in exercise)* | ✅ Implemented |
| **FR-6** | **Deep-Dive Prompts** | *(as defined in exercise)* |❌ Not implemented |
| **FR-7** | **RESTful API Access** | *(as defined in exercise)* |✅ Implemented |
| **FR-8** | **AI Inference Integration** | *(as defined in exercise)* |✅ Implemented |

> **Note:** FR 1–8 were implemented successfully, with the exception of **FR 6**. FR 6 was never assigned as an exercise task and was therefore deprioritized and dropped from the prototype scope.

### Deviations & Changes

| Requirement | Decision | Reason |
|---|---|---|
| FR 6 | Dropped | Never assigned as an exercise; no implementation guidance was provided |

## Tools & Services [Leon]

### CI/CD – SonarQube

The project uses **SonarQube** for static code analysis and code quality reporting. This was integrated as part of the CI pipeline.

### Summary Table

| Tool / Service | Purpose |
|---|---|
| SonarQube | Static analysis, code quality, CI reports |
| PostgreSQL | Persistent data storage |
| JetBrains DataGrip | Database management & inspection |
| JetBrains Rider | Primary IDE |
| Entity Framework Core | ORM & database migrations |
| GitHub / CI Pipeline | Version control & automated builds |

## Retrospective & Lessons Learned

### Team Organization [J.R.]

The team was taken on by **Jan Schröter** but work was distributed informally. This resulted in some organizational chaos, particularly when tasks had dependencies on each other.

One exception: **Jan Röhrle** took on the role of documentation reviewer, ensuring written deliverables were consistent and correct.

**Roles / responsibilities** were not formally assigned. But came to use naturally.

### Biggest Challenges [J.R.]

The most significant challenge was **implementation uncertainty due to late clarification of requirements via exercises**. The team would form an understanding of how a feature should work, begin implementing it, and then — weeks later — receive an exercise that revealed the intended approach was fundamentally different. This led to **complete rewrites** of components, which was both time-consuming and demoralizing.

> *"Having an idea how to implement something, then 2 weeks later getting an exercise which revealed it was meant in a different way — leading to a complete redo."*

### What Worked Well [J.R.]

- **Task distribution** for independent tasks was efficient.
- The **CI pipeline** proved to be a broadly useful skill — applicable beyond this course to future projects.

### What Didn't Work Well [Benni]

- **Work distribution broke down** when tasks were dependent on each other, creating blockers.
- Frequent **refactoring due to new implementation tasks** was tedious and often fell to a single person. After each refactor, the rest of the team had to re-learn the updated system architecture.
- The last Task 11 was very intensive as a complete ui needed to be made.

### Most Valuable Exercise [Benni]

The **CI/CD integration exercise** was rated most valuable by the team, as it introduced tooling and practices directly transferable to professional and personal projects outside this course.

### What the Team Would Do Differently [Benni]

- In a real project: **communicate more closely with the customer** early on to clarify exact requirements — ideally receiving all exercises in advance to prevent rework.
- Establish **clearer team structure** — even a lightweight hierarchy helps with small, well-scoped problems.

### Suggestions for the Course [Benni]

1. **Reduce iterative refactoring overhead.** The current exercise format often forces teams to repeatedly restructure their codebase. A higher-level architectural direction given early would reduce wasted effort.

2. **Provide a pre-built UI shell.** A ready-made frontend that sends API requests — where students only need to build the API to make it work — would give teams a clearer target and reduce ambiguity about expected system behavior.

`Grammtic,translation and sorting improvements with ChatGPT Version 5.3 (10.06.2026 15:45)`
