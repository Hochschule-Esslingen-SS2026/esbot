# Test Strategy

## Difference between Unit and BBD
### Scope

### Execution Time

### Dependencies

> **When should BDD/acceptance tests be executed relative to unit tests?**
> Should they always run together, or should they be treated differently
> (e.g., executed less frequently, in a separate CI stage)?

Your discussion must address:

1. The differences in execution time, scope, and purpose between unit tests and
   BDD/acceptance tests
2. A reasoned recommendation for ESBot's CI pipeline (e.g., always together, or
   unit tests on every commit and BDD tests on pull requests only)
3. How the AI mockability requirement affects this decision — specifically, why using
   mock AI providers makes acceptance tests feasible in CI even without a live model
