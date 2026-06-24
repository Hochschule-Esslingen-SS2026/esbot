# Test Strategy

## Difference between Unit and BBD
### Scope

Unit test is a single Function
Whiel BBD are a complete feature needing multiple functions and services.
It focuses on the user journes and business results.

### Execution Time

Unit test are fast small tests
While BDD tests need longer for booting the complete system with some maybe mocked dependecies.

### Dependencies

Unit test require minimal dependencies, it is a sign for a bad test if a lot is used.

BBD need to boot the complete system with all dependencies (if not mocked)

### Additional

> **When should BDD/acceptance tests be executed relative to unit tests?**

BDD should be testes at least once before merging to main
While units tests can be run after every new line to see what changed.

> Should they always run together, or should they be treated differently

If unaccepted units test fail there is almost no need to run the BDD tests after.
as it jus takes time the programmer could already be using to fix.

> (e.g., executed less frequently, in a separate CI stage)?

There fore we suggest that the BDD is run in a later stage then the BDD Tests
and run less frequently if Hardware Server (Git Runners) are rare.
If a live AI model would be used we would waste a lot of tokens, and time as the AI needs longer to respond.
There fore long operations AI model request (external API's) in generall should be mocked. This also makes the responses deterministic and easier to test..
Test speed is also improved on a small local DB instead of a external large DB.

Test with the actuall AI or DB can be made in a staging enviroment.

Your discussion must address:

1. The differences in execution time, scope, and purpose between unit tests and
   BDD/acceptance tests
   > done
2. A reasoned recommendation for ESBot's CI pipeline (e.g., always together, or
   unit tests on every commit and BDD tests on pull requests only)
   > done
3. How the AI mockability requirement affects this decision — specifically, why using
   mock AI providers makes acceptance tests feasible in CI even without a live model
   > done
