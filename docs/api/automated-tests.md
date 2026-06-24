# Automated API Testing — ESBot

## Overview

This document describes the automated API test suite for ESBot, including framework selection, execution instructions, backend configuration for tests, and a detailed breakdown of each test group.

---

## 1. Framework Choice & Rationale

- Test runner: xUnit (packages `xunit`, `xunit.runner.visualstudio`).
- Feature/BDD support: Reqnroll + Reqnroll.xUnit (features live under `Features/`, step definitions under `Steps/`).
- Test host: `Microsoft.AspNetCore.Mvc.Testing` via a custom `ApiFactory` (inherits `WebApplicationFactory<Program>`).
- DI fakes & assertions: `FakeItEasy` and `FluentAssertions`.
- Database: EF Core In-Memory (`Microsoft.EntityFrameworkCore.InMemory`), created per ApiFactory instance for isolation.

Rationale: This stack lets tests run an in-process ASP.NET Core host (no network port), perform real HTTP requests via `HttpClient`, and easily swap DI registrations (e.g., to fake LLMs) for deterministic test behavior.

---

## 2. How to Run the Test Suite

### Prerequisites

- **.NET SDK 6.0+** installed (check with `dotnet --version`)
- **Visual Studio 2022** (optional but recommended) or **Visual Studio Code** with C# extension
- Repository cloned to local machine

### Command-Line Execution

Run all API tests:

```bash
cd EsBot-Server
dotnet test Test.Presentation.FunctionalTests --configuration Release --verbosity normal
```

Run a specific test class:

```bash
dotnet test Test.Presentation.FunctionalTests --filter "ClassName=Test.Presentation.FunctionalTests.SessionManagementTests" --verbosity normal
```

Run a single test method:

```bash
dotnet test Test.Presentation.FunctionalTests --filter "FullyQualifiedName~SessionManagementTests.CreateSessionReturns201WithValidData" --verbosity normal
```

Generate a test report (trx format):

```bash
dotnet test Test.Presentation.FunctionalTests --logger "trx;LogFileName=test-results.trx" --configuration Release
```

### Visual Studio / Visual Studio Code

1. Open the solution: `EsBot-Server\EsBot-Server.sln`
2. **Test Explorer:** View → Test Explorer (or press `Ctrl+E, T`)
3. Click **Run All Tests** or right-click a test class/method and select **Run**
4. Review results in the Test Explorer pane

---

## 3. Backend Setup & Configuration for Tests

### Test Server Configuration

Key implementation: `Test.Presentation.FunctionalTests\Helper\ApiFactory.cs`.

- ApiFactory inherits `WebApplicationFactory<Program>` and overrides `ConfigureWebHost`.
- In `ConfigureTestServices` the factory registers an EF Core InMemory database via `options.UseInMemoryDatabase(DbName)`; DbName is a GUID per factory instance to isolate state between runs.
- The factory exposes `ConfigureTestServicesAction` (an `Action<IServiceCollection>`) which hooks or tests set to replace services in DI (for example, replacing `ILlmInterface` with a fake implementation).
- The factory sets the environment to `Testing` (`UseEnvironment("Testing")`) so the app can detect test mode.
- `SeedDataAsync<T>` is provided to seed the in-memory DB before running requests.

Notes:
- Tests obtain an `HttpClient` via `ApiFactory.CreateClient()` and exercise the in-process server.
- The setup avoids network ports and external services; DB and external integrations are replaced via DI for deterministic behavior.

---

## 4. Test Suite Structure & Coverage

### Test Structure Overview

The project contains a mixture of Gherkin feature-based scenarios (Reqnroll) and class-based xUnit tests. Key files/folders:

- `Features/` — Gherkin *.feature files (RequestaQuiz.feature, AskQuestions.feature, ResumeExistingLearningSession.feature).
- `Steps/` — step definitions for the features (e.g., `RequestQuiz.cs`, `AskQuestion.cs`, `ResumeSession.cs`, `VerificationStep.cs`, `Common.cs`).
- `ExtraTests.cs` — class-based xUnit tests covering health, sessions, messages, and quiz edge cases.
- `IsItEvenStartingTests.cs` — simple smoke test(s) (Swagger/OpenAPI accessibility).
- `Hooks/` — scenario hooks for DI overrides (e.g., `LlmHooks.cs` to mock LLM behavior by setting `ApiFactory.ConfigureTestServicesAction`).
- `Helper/ApiFactory.cs` — the test WebApplicationFactory and DB seeding utilities.

These cover health, session lifecycle, message posting/retrieval, quiz generation, error and edge cases, and LLM integration scenarios.

---

### 4.1 HealthCheckTests

**File:** `Test.Presentation.FunctionalTests\HealthCheckTests.cs`

**Purpose:** Verify the API is running and responsive.

**Test Methods:**

| Test | HTTP | Status | Assertion |
|------|------|--------|-----------|
| `HealthCheck_ReturnsOk` | `GET /api/v1/health` | 200 | Response contains `"status": "ok"` or equivalent health indicator |

**Example:**

```csharp
[Fact]
public async Task HealthCheck_ReturnsOk()
{
    // Act
    var response = await _client.GetAsync("/api/v1/health");

    // Assert
    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    var content = await response.Content.ReadAsStringAsync();
    content.Should().Contain("ok");
}
```

## Running Tests in CI/CD

### GitHub Actions Example

```yaml
# .github/workflows/api-tests.yml
name: API Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - uses: actions/setup-dotnet@v1
        with:
          dotnet-version: '6.0.x'
      - run: cd EsBot-Server && dotnet test Test.Presentation.FunctionalTests --configuration Release --logger "trx;LogFileName=results.trx"
      - uses: dorny/test-reporter@v1
        if: always()
        with:
          name: API Test Results
          path: '**/results.trx'
          reporter: 'dotnet trx'
```

---

## Coverage & Pass Criteria

### Current Coverage

- **Happy Path:** 7 tests (health, session CRUD, message, quiz)
- **Negative Cases:** 6+ tests (404, 422, validation, edge cases)
- **Total:** ~15 test methods

### Pass Criteria

- ✅ All tests pass locally with a single `dotnet test` command
- ✅ No flaky tests (tests are deterministic and order-independent)
- ✅ Error responses contain meaningful messages
- ✅ HTTP status codes match specifications
- ✅ Response bodies conform to documented JSON schemas

---

## Appendix: Quick Reference

### Run all tests

```bash
dotnet test Test.Presentation.FunctionalTests
```

### Run and generate report

```bash
dotnet test Test.Presentation.FunctionalTests --logger "trx;LogFileName=test-results.trx"
```

### Watch mode (requires dotnet-watch tool)

```bash
dotnet watch test Test.Presentation.FunctionalTests
```

### Disable test parallelization

```bash
dotnet test Test.Presentation.FunctionalTests --settings xunit.runsettings
```

`AI was used to help writing this documentation ChatGPT Version 5.3 (12.06.2026 15:45)`
