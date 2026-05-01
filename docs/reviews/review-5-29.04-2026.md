# Review

**Project / product:** EsBot <https://github.com/ItsSami19/esbot>
**Review type:** Technical Review: As we mostly look at Architecture documents and algorithms.
**Date (planned / actual):** 2026-04-29/2026-04-29  
**Moderator:** Jan Schröter
**Author(s):** Baran Bickici, Sema Dagci, Sami Goekpinar, Lydia Karapanagiotidi
**Reviewers:** Leon Kirasic, Jan Rörhle, Benjamin Supke

## 1. General instructions
**Terminology**

| Acronym | Meaning |
|--------|---------|
| **MP** | Master Plan |
| **DS** | Data Summary |
| **LoF** | Level of Findings |
| **RR** | Review Report |

## 2. Master Plan (MP)

### 2.1 Masterplan — header

| Field | Value |
|-------|-------|
| Review No. | REV-2026-001 |
| Project | EsBot |
| Project manager | Sami Goekpinar |
| Quality expert / manager | - |
| Moderator | Jan Schröter |
| Author(s) | Baran Bickici, Sema Dagci, Sami Goekpinar, Lydia Karapanagiotidi |

### 2.2 Review objects

| # | Review objects | Abbr. |
|---|----------------|-------|
| 01 | `docs/spec/requirements.md` | REQ |
| 02 | `docs/esbot.md` | EsBot |
| 03 | `docs/spec/spec.md` | SPEC |
| 04 | `backend/app/db.py`| B-DB |
| 05 | `backend/app/main.py` | B-MAIN |
| 06 | `backend/app/models/base.py` | B-Models-Base |
| 07 | `backend/app/models/__init__.py`|B-Models-Init|
| 08 | `backend/app/services/answer_evaluation_service.py`| B-Services-Answer|
| 09 | `backend/app/services/contextualized_response_service.py`|  B-Services-Response|
| 10 | `backend/app/services/resume_learning_session_service.py`|  B-Services-Resume |
| 11 | `base.py`| Base |

### 2.3 Reference documents

Only the Review Objects

### 2.4 Checklists / scenarios

| # | Checklists / scenarios |
|---|-------------------------|
| 1 | Requirements completeness checklist (section A–D). |

### 2.5 Reviewer assignment

As this is a very small review, all Reviwers named will review all Review objects

### 2.6 Review meeting

| Date / time / location |
|------------------------|
2026-04-29 11:00 CET GMT+2

## 3. List of findings (LoF)

**Type:** defect > question > suggestion
**Severity scale:** blocking > major > minor > editorial  
**Status values:** open, accepted, rejected, deferred, fixed

### Table 1 – Pylint Findings (pylint 4.0.5)

| ID | Location (file / section / module) | Summary | Type | Severity | Status | Owner | Notes / meeting decision |
|----|-------------------------------------|---------|------|----------|--------|-------|--------------------------|
| P-001 | `B-DB:14`, `B-MAIN:16`, `B-Models-Base:121`, `B-Models-Init:11`, `B-Services-Answer:49`, `B-Services-Response:75`, `B-Services-Resume:48` | Final newline missing at end of file (C0304) in all backend modules | suggestion | editorial | open | | Affects all 7 files |
| P-002 | `B-DB:1`, `B-MAIN:1`, `B-Models-Base:1`, `B-Models-Init:1`, `B-Services-Answer:1`, `B-Services-Response:1`, `B-Services-Resume:1` | Missing module-level docstring (C0114) in all backend modules | suggestion | minor | open | | All 7 modules lack top-level documentation |
| P-003 | `B-Models-Base:13,18,24,30,43,59,75,93,108`, `B-Services-Answer:6`, `B-Services-Response:6`, `B-Services-Resume:8` | Missing class docstring (C0115) on all model and service classes | suggestion | minor | open | | Affects all 6 domain model classes and all 3 service classes |
| P-004 | `B-Models-Base:9,55,71,89,104,120`, `B-MAIN:15`, `B-DB:12`, `B-Services-Answer:11`, `B-Services-Response:11`, `B-Services-Resume:12` | Missing function/method docstring (C0116) across all modules | suggestion | minor | open | | Affects utility functions, all `create()` classmethods, service methods, and the health endpoint |
| P-005 | `B-Models-Base:89` (170 chars), `Base:90` (165 chars), `Base:101` (101 chars), `Base:120` (136 chars), `Base:121` (126 chars), `B-Services-Response:38` (107 chars) | Line length exceeds 100 characters (C0301) | suggestion | editorial | open | | `Base:89-90` are the worst offenders (`QuizItem.create` signature); could be split across lines |
| P-006 | `B-Models-Base:89` | `QuizItem.create()` has 6 positional arguments, exceeding the recommended maximum of 5 (R0913, R0917) | suggestion | minor | open | | Consider introducing a parameter object or using keyword-only arguments |
| P-007 | `B-Services-Answer:6`, `B-Services-Response:6`, `B-Services-Resume:8` | Too few public methods on service classes (R0903, 1/2 each) | question | minor | open | | Each service exposes only one public method; may indicate incomplete implementation or that services should be plain functions rather than classes. Note: R0903 for model classes (`Base`) resolved in venv – SQLModel classmethods are recognised as public methods when stubs are present |

### Table 2 – mypy Findings (mypy (static type checker))

| ID | Location (file / section / module) | Summary | Type | Severity | Status | Owner | Notes / meeting decision |
|----|-------------------------------------|---------|------|----------|--------|-------|--------------------------|
| MY-001 | `B-Services-Response:26,41,59` | Argument `session_id` passed to `Message.create()` has type `int \| None`; expected `int` (`arg-type`) | defect | major | open | | `UserSession.id` is declared `Optional[int]` (Base:31); after `db.commit()` + `db.refresh()` the id is always set, but this is not statically guaranteed; a `None`-guard or non-optional id type is needed |
| MY-002 | `B-Services-Answer:21` | Argument `quiz_item_id` passed to `SubmittedAnswer.create()` has type `int \| None`; expected `int` (`arg-type`) | defect | major | open | | `QuizItem.id` is `Optional[int]` (Base:76); same root cause as MY-001 |
| MY-003 | `B-Services-Answer:35` | Argument `submitted_answer_id` passed to `EvaluationResult.create()` has type `int \| None`; expected `int` (`arg-type`) | defect | major | open | | `SubmittedAnswer.id` is `Optional[int]` (Base:94); same root cause as MY-001; after `commit()` + `refresh()` the id is populated but not statically provable |
| MY-004 | `B-Services-Resume:40` | Argument 1 to `order_by()` has incompatible type `int`; expected a `ColumnElement` (`arg-type`) | defect | minor | open | | `Message.order` is typed as `int` in the model; mypy resolves it as a plain `int` at the call site instead of the SQLAlchemy `InstrumentedAttribute`. Fix: use `col(Message.order)` or `Message.order.asc()` to produce a proper column expression |

### Table 3 – Manual Review Findings (Docs vs. Code)

| ID | Location (file / section / module) | Summary | Type | Severity | Status | Owner | Notes / meeting decision |
|----|-------------------------------------|---------|------|----------|--------|-------|--------------------------|
| M-001 | `B-Services-Answer:28`  | `ai_provider.evaluate()` has **no try/except block**. Any exception from the AI component crashes the service. By contrast, `ContextualizedResponseService` does catch `RuntimeError`. Inconsistent error handling violates the requirement that the system "shall handle failures of external AI services gracefully". | defect | major | open | | `ContextualizedResponseService` shows the correct pattern; same pattern must be applied here |
| M-002 | `B-Models-Base:36` / `SPEC §Assumptions` | `UserSession.user_identifier` is a **required, non-nullable field** (`Field(..., nullable=False)`). The spec explicitly states "No user login or account management is required for MVP; the feature is intentionally anonymous." It is unclear how an anonymous user gets a `user_identifier`. No assignment logic exists anywhere in the backend. | question | major | open | | Clarify: is `user_identifier` a device fingerprint, browser cookie, or session token? Spec and model are contradictory |
| M-003 | `B-Services-Response:37` / `REQ FR16`, `SPEC FR-006` | `ContextualizedResponseService` only catches `RuntimeError`. Network failures, `ConnectionRefusedError`, `TimeoutError`, and other AI-provider exceptions are **not caught** and will propagate as unhandled 500 errors. FR-006 requires handling "local AI model unavailability". | defect | major | open | | Broaden the except clause or catch a base `Exception`; log unexpected errors |**
| M-004 | `backedn/app/models/base` | On projects of larger scope on class one file rule | suggestion | minor | open |||
| M-005 | `B-Services-Answer` | The repeated usage of magic strings that are not defined in any Constant | suggestion | minor | open |||
| M-006 | config.py | Is Empty | suggestion | editorial | open|||
| M-007 |`B-Services-Answer` | Why create your own error on not rely on Http errors which are established | question | minor | open |||
| M-008 | `B-MAIN` | Corse Settings like `allow_origins=["http://localhost:3000"]` should be configurable | suggestion | minor | open |||
| M-009 | `backend/tests` | We could not findy any test actually starting a API Server which gets tested. | defect | blocking | open |||
| M-010 | esbot.md (NFR Performance) | "Normal load" is not defined | defect | major | open | Needs measurable definition |
| M-011 | NFR5 | "user-friendly" is subjective | defect | minor | open | Hard to test |
| M-012 | FR17 | "relevant data" not specified | defect | minor | open | Needs examples |
| M-013 | Edge Cases | Not linked to requirements explicitly | Improvement | minor | open | Could improve traceability |
| M-014 | FR-004 | Conflicts with "temporary or persistent" wording | Inconsistency | major | open | Needs alignment |

## 4. Data Summary (DS)

<!-- Key metrics for this review. Fill after preparation and/or after rework. -->

| Metric | Value | Notes |
|--------|-------|-------|
| Size of review object | <!-- e.g. pages, LOC, #requirements --> | <!-- --> |
| Preparation effort (hours, optional) | <!-- per role --> | <!-- --> |
| Number of findings (initial) | <!-- --> | <!-- --> |
| Number of findings after meeting | <!-- --> | <!-- --> |
| Rework effort (hours, author) | <!-- --> | <!-- --> |
| Re-inspection required? | <!-- yes / no --> | <!-- --> |

---

## 5. Review Report (RR)

### 5.1 Summary

<!-- Short executive summary: object reviewed, outcome, overall quality impression. -->

### 5.2 Review outcome

* **Review object state after review:** <!-- e.g. accepted with changes, requires re-inspection, not accepted -->
* **Major risks or themes:** <!-- bullet list -->

### 5.6 Sign-off

| Role | Name | Signature / date |
|------|------|------------------|
| Moderator | <!-- --> | <!-- --> |

`Grammtic, translation and text structure improvements with ChatGPT Version 5.3 (01.05.2026 12:20)`
