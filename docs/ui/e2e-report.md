# E2E Test Report

## 1. Test Execution Summary
- **Tests Ran:** 3
- **Passed:** 3
- **Failed:** 0
- **Total Runtime:** ~5-10 seconds
- **Framework & Version:** Cypress v15.17.0

## 2. Headless Output
Below is the terminal output from the headless run of our Cypress tests:

![Headless Output](./cypress%20headless.png)

## 3. Interactive Run Screenshot
Here is the screenshot of the Cypress Interactive runner displaying the test results:

![Interactive Run](./testspassed.png)

## 4. Flakiness Observations
We observed some potential flakiness related to asynchronous operations, especially when waiting for the backend to respond with an LLM-generated message. Currently, we use a timeout of 10,000ms (`{ timeout: 10000 }`), but network latency or a slow LLM provider could exceed this limit and cause the test to fail intermittently (timing issues). There's also the possibility of state leakage if the database isn't properly reset between sessions.

## 5. Reflection

- **What was easy about writing E2E tests compared to unit or API tests?**
  Writing Cypress tests was straightforward due to its intuitive, readable syntax (`cy.get()`, `cy.click()`). Using `data-testid` attributes made selecting elements reliable, and modeling actual user journeys felt more natural than testing isolated functions.

- **What was difficult or surprising?**
  Managing asynchronous behavior and knowing exactly when elements would appear in the DOM was challenging. The asynchronous nature of the LLM responses meant we had to carefully handle timeouts and assertions (`should('be.visible')`) to prevent tests from failing prematurely.

- **At which layer of the test pyramid (unit, API, E2E) would you detect each of the bugs your tests could catch? Why?**
  - *Missing User ID validation:* Unit test (frontend validation logic) or UI integration test.
  - *Chat/Quiz flow breakages (e.g., cannot send a message):* API level (to verify endpoints work) or E2E level (to verify the UI successfully communicates with the API). E2E is best for verifying the complete user journey and integration of all components.

- **How would these tests behave with a real (non-mock) LLM? What would you change?**
  With a real LLM, the tests would be significantly slower and much more flaky due to unpredictable generation times and varying responses (non-determinism). To fix this, I would increase the timeouts substantially or, better yet, configure the backend to use a "mock" LLM provider during E2E test runs to ensure fast, deterministic, and reliable test executions.

`Grammtic, translation and text structure improvements with ChatGPT Version 5.3 (19.06.2026 12:20)`
