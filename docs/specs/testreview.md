## Exercise 8.1 (10 Points): Unit Testing of Models

- `Session` – represents a user's learning session, including timestamps and associated messages

```text
* Not all valid attribute combinations are fully covered.

  Covered:

  * valid `ExternalUserId` with generated session
  * session creation with `CreatedAt` initialization
  * basic bidirectional relationship scenario (session ↔ messages)

  Missing valid combinations:

  * maximum-length valid `ExternalUserId` (100 chars)
  * multiple `Messages` and `QuizRequests` in the same session
  * valid session with no messages or quiz requests (empty collections as valid state)
  * updates to `LastInteractionAt` over time (state mutation scenarios)
  * multiple valid `UserSession` instances with different IDs

---

* Boundary values are only partially tested.

  Covered:

  * `ExternalUserId = null` (invalid case via validation)
  * basic timestamp initialization check for `CreatedAt`

  Missing boundary coverage:

  * `ExternalUserId = ""` (empty string)
  * whitespace-only `ExternalUserId`
  * exactly 100-character boundary for `ExternalUserId`
  * `Guid.Empty` for `Id`
  * extreme datetime scenarios (future/past manipulation of `CreatedAt` / `LastInteractionAt`)
  * large collections (`Messages`, `QuizRequests`) behavior at scale

---

* Invalid inputs and edge cases are only partially handled.

  Covered:

  * `ExternalUserId = null` validation failure
  * basic relationship integrity between `UserSession` and `Message`

  Missing invalid/edge-case scenarios:

  * empty or whitespace `ExternalUserId`
  * invalid `Guid.Empty` session IDs
  * inconsistent relationship state (message referencing different session than collection membership)
  * multiple sessions referencing same message (uniqueness constraint edge cases)
  * mutation of navigation properties after creation
  * concurrent modification scenarios of collections (if applicable)

---

* Analysis approach:

  The test suite was evaluated using structured test coverage analysis consisting of:

  1. Equivalence partitioning

     * grouping inputs into valid, invalid, and boundary categories for each property (`ExternalUserId`, `Id`, timestamps, and collections)

  2. Boundary-value analysis

     * verifying minimum, maximum, and edge-length constraints (notably `MaxLength(100)` on `ExternalUserId`)
     * checking null, empty, and whitespace conditions

  3. State and lifecycle analysis

     * reviewing timestamp initialization and mutation behavior (`CreatedAt`, `LastInteractionAt`)
     * assessing collection initialization and growth behavior

  4. Relationship integrity analysis

     * validating bidirectional navigation consistency between `UserSession`, `Message`, and `QuizRequest`

  5. Negative testing review

     * identifying missing explicit tests for invalid inputs and inconsistent entity states

  This analysis shows that the current suite focuses on basic construction and a limited set of validation rules, but lacks comprehensive coverage of boundary conditions, collection behavior, and relationship consistency under invalid or edge-case scenarios.

```

- `Message` – represents a single interaction turn (user input or ESBot response)

```text
- Not all valid attribute combinations are currently covered. The tests only verify creation with `Role = true` and valid content. Additional combinations such as `Role = false`, empty `Guid`, and different content variations are not tested.

- Boundary values are only partially tested. `null` content is validated, but other important boundaries such as empty strings, whitespace-only content, maximum message length, and invalid session states (`Guid.Empty`) are not currently covered.

- Invalid inputs and edge cases are only partially handled. The current suite checks `null` content validation, but does not explicitly test empty/whitespace content, oversized input, invalid session identifiers, or combinations of multiple invalid values.

- Test analysis approach:
  The existing tests were reviewed using boundary-value analysis and equivalence partitioning. Each entity property (`UserSessionId`, `Role`, `Content`, and `Timestamp`) was analyzed for:

  1. Valid input combinations
  2. Boundary conditions
  3. Invalid or unexpected input values

  The analysis identified that current coverage focuses mainly on successful object creation and a single validation case (`null` content), while several boundary and edge-case scenarios remain untested.

```

- `Quiz` / `Question` – represents a generated practice question with possible answers

```text
* Not all valid attribute combinations are covered. The tests currently verify only a subset of valid scenarios:

  * `Message` is tested with a valid `Guid`, `Role = true`, and normal content.
  * `QuizRequest` is tested with a valid topic and session ID.

  Missing valid combinations include:

  * `Role = false`
  * minimum valid topic/content values
  * different valid session identifiers
  * boundary-valid topic lengths (e.g., exactly 200 characters)

* Boundary values are only partially tested.
  Existing boundary coverage:

  * `QuizRequest.Topic` exceeding 200 characters

  Missing boundary tests include:

  * empty or whitespace `Content`
  * empty or whitespace `Topic`
  * maximum valid content length
  * exact boundary values (e.g., 200 characters vs. 201)
  * `Guid.Empty` as invalid session state
  * timestamp validation behavior

* Invalid inputs and edge cases are only partially handled.
  Existing invalid coverage:

  * `null` content
  * overly long topic

  Missing invalid and edge-case scenarios include:

  * empty strings
  * whitespace-only strings
  * `Guid.Empty`
  * multiple invalid fields simultaneously
  * extremely large content payloads
  * validation of required navigation/state consistency

* Analysis approach:
  The current tests were analyzed using:

  1. Equivalence partitioning

     * identifying valid and invalid input groups for each property
  2. Boundary-value analysis

     * checking minimum, maximum, and out-of-range values
  3. Negative testing

     * verifying behavior for invalid or unexpected inputs
  4. Attribute coverage review

     * ensuring each entity property and validation attribute has corresponding tests

  This review showed that the current suite validates core happy-path behavior but does not yet comprehensively cover boundary conditions, invalid states, or all valid input combinations.

```

- `Answer` – represents a user's submitted answer to a quiz question

```text
* Not all valid attribute combinations are covered. The tests currently validate:

  * a valid one-to-one relationship between `SubmittedAnswer` and `QuizItem`
  * invalid `null` value for `AnswerText`

  Missing valid combinations include:

  * valid answers with different `QuizItemId` values
  * populated vs. null `Evaluation`
  * boundary-valid answer lengths
  * verification of `SubmittedAt` initialization

* Boundary values are only partially tested.
  Existing boundary coverage:

  * `null` `AnswerText`

  Missing boundary tests include:

  * empty string `""`
  * whitespace-only answers
  * maximum answer length (if intended)
  * very large answer payloads
  * `QuizItemId = 0` or negative values (if invalid by design)
  * timestamp boundary validation (`SubmittedAt` initialization and UTC consistency)

* Invalid inputs and edge cases are only partially handled.
  Existing invalid coverage:

  * `null` `AnswerText`

  Missing invalid and edge-case scenarios include:

  * empty or whitespace answers
  * missing `QuizItem`
  * inconsistent relationship state between `QuizItemId` and `QuizItem`
  * simultaneous invalid fields
  * duplicate or invalid one-to-one mappings
  * explicit validation of optional `Evaluation`

* Analysis approach:
  The tests were analyzed using:

  1. Equivalence partitioning

     * separating valid and invalid input categories for each property
  2. Boundary-value analysis

     * identifying minimum, maximum, empty, and oversized input cases
  3. Relationship validation analysis

     * verifying consistency of one-to-one entity relationships and navigation properties
  4. Negative testing

     * reviewing whether invalid or inconsistent states are explicitly tested

  The analysis showed that the current tests validate the primary relationship behavior and one invalid input case, but additional coverage is still needed for boundary conditions, invalid states, and relationship consistency scenarios.

```
