# CI Pipeline Documentation

## Overview

The CI pipeline is defined in [`.github/workflows/ci.yml`](../../.github/workflows/ci.yml) and runs on every push and pull request. It mirrors one of the local verification steps documented in [`local-verification.md`](./local-verification.md).

## Triggers

```yaml
on:
  push:
  pull_request:
```

**Why `push`?** Every commit pushed to any branch is verified immediately, so broken builds are caught before a pull request is even opened.

**Why `pull_request`?** Pull request events trigger an additional check on the merge commit, which can differ from the head commit. This ensures that merged code also passes.

**`workflow_dispatch` (not added)?** It is not added because as it is executed with every push no code changed can be introduced in between pushs.
For rarly used manual test executions or staging scripts we can use this dispatch system.

## Runner

```yaml
runs-on: ubuntu-latest
```

`ubuntu-latest` is the standard GitHub-hosted runner. It is free for public repositories, well-maintained, and sufficient for building and testing a .NET backend. There is no platform-specific code in ESBot that would require Windows or macOS runners.

## Environment Setup

```yaml
- name: Set up .NET
  uses: actions/setup-dotnet@v4
  with:
    dotnet-version: "10.0.x"
```

The project targets **net10.0** (as declared in each `.csproj` file). `actions/setup-dotnet@v4` installs the exact SDK version and also caches the NuGet global packages folder automatically via the built-in cache support.

No database service is needed: the integration tests use `Microsoft.EntityFrameworkCore.InMemory`, and the functional tests use `Microsoft.AspNetCore.Mvc.Testing` with an in-process test server. No live LLM is required because all LLM calls are covered by mocks/fakes (as established in Exercise 8).

## Jobs and Steps

| Step | Purpose |
|------|---------|
| `actions/checkout@v4` | Checks out the repository at the commit that triggered the workflow |
| `dotnet restore` | Restores NuGet packages; separated from build so cache misses are visible in the log |
| `dotnet build --no-restore` | Compiles all projects in the solution; fails the job if any project does not compile |
| `dotnet test --no-build` | Runs all three test projects (`Test.Application.UnitTests`, `Test.Infrastructure.IntegrationTests`, `Test.Presentation.FunctionalTests`) and fails the job if any test fails |
| `actions/upload-artifact@v4` | Uploads `.trx` test result files as a build artifact for inspection, even when tests fail (`if: always()`) |

### Intentionally excluded from CI

- **Live LLM inference** (Ollama, vLLM, LM Studio) — no hosted runner has a GPU or a running model server; LLM calls are mocked in tests.
- **Production database** — all database interactions use the in-memory EF Core provider in tests.
- **Frontend build** — ESBot currently has no separate frontend build artifact that requires CI validation.

## Parity with Local Verification

| Local command | CI step |
|---------------|---------|
| `dotnet restore EsBot-Server/EsBot.sln` | `dotnet restore EsBot-Server/EsBot.sln` |
| `dotnet build EsBot-Server/EsBot.sln` | `dotnet build EsBot-Server/EsBot.sln --no-restore --configuration Release` |
| `dotnet test EsBot-Server/EsBot.sln` | `dotnet test EsBot-Server/EsBot.sln --no-build --configuration Release` |

The CI build runs in `Release` mode to match what would be deployed, while local development typically uses `Debug`. If a test passes locally but fails in CI (or vice versa), check:

1. **SDK version** — confirm `dotnet --version` locally matches the `10.0.x` range.
2. **Configuration-conditional code** — some code paths differ between `Debug` and `Release` (e.g., `#if DEBUG` guards).
3. **Environment variables** — CI has no `.env` file or user secrets; confirm tests do not depend on local configuration.

## Exercise 9.2 enhancements

- What action or tool was added
  - Added **OWASP Dependency-Check** to the GitHub Actions pipeline.
  - It scans project dependencies for known public vulnerabilities.

- Added value vs. cost
  - Added value: better security visibility and an artifact report in CI.
    - Added value: The check improves security by detecting vulnerable libraries in the actual application stack.
  - Cost: longer pipeline runtime and occasional maintenance for scan configuration or false positives.

- Whether the same check still runs locally
  - This can be done with the Dependency-Check CLI or a small wrapper script using the same scan arguments as CI.

`Grammtic and sorting improvements with Claude Sonnet Version 4.6 (05.06.2026 13:53)`
