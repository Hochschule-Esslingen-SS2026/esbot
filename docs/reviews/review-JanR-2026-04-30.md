# Technical Review – Jan Röhrle
**Review Type:** Technical Review  
**Date:** 2026-04-30  
**Reviewers:** Jan Röhrle 
**Scope:** `docs/esbot.md`, `docs/spec/requirements.md`, `docs/spec/spec.md`, `backend/*`  
**Tools used:** pylint 4.0.5, mypy (static type checker), manual inspection

**Severity scale:** blocking > major > minor > editorial  
**Status values:** open, accepted, rejected, deferred, fixed

---

## Table 1 – Pylint Findings



| ID | Location (file / section / module) | Summary | Type | Severity | Status | Owner | Notes / meeting decision |
|----|-------------------------------------|---------|------|----------|--------|-------|--------------------------|
| P-001 | `backend/app/db.py:14`, `backend/app/main.py:16`, `backend/app/models/base.py:121`, `backend/app/models/__init__.py:11`, `backend/app/services/answer_evaluation_service.py:49`, `backend/app/services/contextualized_response_service.py:75`, `backend/app/services/resume_learning_session_service.py:48` | Final newline missing at end of file (C0304) in all backend modules | suggestion | editorial | open | | Affects all 7 files; simple fix |
| P-002 | `backend/app/db.py:1`, `backend/app/main.py:1`, `backend/app/models/base.py:1`, `backend/app/models/__init__.py:1`, `backend/app/services/answer_evaluation_service.py:1`, `backend/app/services/contextualized_response_service.py:1`, `backend/app/services/resume_learning_session_service.py:1` | Missing module-level docstring (C0114) in all backend modules | suggestion | minor | open | | All 7 modules lack top-level documentation |
| P-003 | `backend/app/models/base.py:13,18,24,30,43,59,75,93,108`, `backend/app/services/answer_evaluation_service.py:6`, `backend/app/services/contextualized_response_service.py:6`, `backend/app/services/resume_learning_session_service.py:8` | Missing class docstring (C0115) on all model and service classes | suggestion | minor | open | | Affects all 6 domain model classes and all 3 service classes |
| P-004 | `backend/app/models/base.py:9,55,71,89,104,120`, `backend/app/main.py:15`, `backend/app/db.py:12`, `backend/app/services/answer_evaluation_service.py:11`, `backend/app/services/contextualized_response_service.py:11`, `backend/app/services/resume_learning_session_service.py:12` | Missing function/method docstring (C0116) across all modules | suggestion | minor | open | | Affects utility functions, all `create()` classmethods, service methods, and the health endpoint |
| P-005 | `backend/app/models/base.py:89` (170 chars), `base.py:90` (165 chars), `base.py:101` (101 chars), `base.py:120` (136 chars), `base.py:121` (126 chars), `backend/app/services/contextualized_response_service.py:38` (107 chars) | Line length exceeds 100 characters (C0301) | suggestion | editorial | open | | `base.py:89-90` are the worst offenders (`QuizItem.create` signature); could be split across lines |
| P-006 | `backend/app/models/base.py:89` | `QuizItem.create()` has 6 positional arguments, exceeding the recommended maximum of 5 (R0913, R0917) | suggestion | minor | open | | Consider introducing a parameter object or using keyword-only arguments |
| P-007 | `backend/app/services/answer_evaluation_service.py:6`, `backend/app/services/contextualized_response_service.py:6`, `backend/app/services/resume_learning_session_service.py:8` | Too few public methods on service classes (R0903, 1/2 each) | question | minor | open | | Each service exposes only one public method; may indicate incomplete implementation or that services should be plain functions rather than classes. Note: R0903 for model classes (`base.py`) resolved in venv – SQLModel classmethods are recognised as public methods when stubs are present |

---

## Table 2 – mypy Findings



| ID | Location (file / section / module) | Summary | Type | Severity | Status | Owner | Notes / meeting decision |
|----|-------------------------------------|---------|------|----------|--------|-------|--------------------------|
| MY-001 | `backend/app/services/contextualized_response_service.py:26,41,59` | Argument `session_id` passed to `Message.create()` has type `int \| None`; expected `int` (`arg-type`) | defect | major | open | | `UserSession.id` is declared `Optional[int]` (base.py:31); after `db.commit()` + `db.refresh()` the id is always set, but this is not statically guaranteed; a `None`-guard or non-optional id type is needed |
| MY-002 | `backend/app/services/answer_evaluation_service.py:21` | Argument `quiz_item_id` passed to `SubmittedAnswer.create()` has type `int \| None`; expected `int` (`arg-type`) | defect | major | open | | `QuizItem.id` is `Optional[int]` (base.py:76); same root cause as MY-001 |
| MY-003 | `backend/app/services/answer_evaluation_service.py:35` | Argument `submitted_answer_id` passed to `EvaluationResult.create()` has type `int \| None`; expected `int` (`arg-type`) | defect | major | open | | `SubmittedAnswer.id` is `Optional[int]` (base.py:94); same root cause as MY-001; after `commit()` + `refresh()` the id is populated but not statically provable |
| MY-004 | `backend/app/services/resume_learning_session_service.py:40` | Argument 1 to `order_by()` has incompatible type `int`; expected a `ColumnElement` (`arg-type`) | defect | minor | open | | `Message.order` is typed as `int` in the model; mypy resolves it as a plain `int` at the call site instead of the SQLAlchemy `InstrumentedAttribute`. Fix: use `col(Message.order)` or `Message.order.asc()` to produce a proper column expression |

---

## Table 3 – Manual Review Findings (Docs vs. Code)

| ID | Location (file / section / module) | Summary | Type | Severity | Status | Owner | Notes / meeting decision |
|----|-------------------------------------|---------|------|----------|--------|-------|--------------------------|
| M-002 | `backend/app/services/answer_evaluation_service.py:28`  | `ai_provider.evaluate()` has **no try/except block**. Any exception from the AI component crashes the service. By contrast, `ContextualizedResponseService` does catch `RuntimeError`. Inconsistent error handling violates the requirement that the system "shall handle failures of external AI services gracefully". | defect | major | open | | `ContextualizedResponseService` shows the correct pattern; same pattern must be applied here |
| M-003 | `backend/app/services/contextualized_response_service.py:36` / `docs/spec/requirements.md FR4`, `docs/spec/spec.md User Story 1` | AI provider is called with `self.ai_provider.explain(question)` (only the current question). **No message history is passed**. Requirements FR4 states "The system shall maintain conversational context within a session." Without history the AI cannot maintain context, contradicting the core learning continuity feature. | defect | major | open | | Existing message history is queried (line 21-23) but not forwarded to the AI call |
| M-004 | `backend/app/models/base.py:36` / `docs/spec/spec.md §Assumptions` | `UserSession.user_identifier` is a **required, non-nullable field** (`Field(..., nullable=False)`). The spec explicitly states "No user login or account management is required for MVP; the feature is intentionally anonymous." It is unclear how an anonymous user gets a `user_identifier`. No assignment logic exists anywhere in the backend. | question | major | open | | Clarify: is `user_identifier` a device fingerprint, browser cookie, or session token? Spec and model are contradictory |
| M-005 | `backend/app/services/contextualized_response_service.py:37` / `docs/spec/requirements.md FR16`, `docs/spec/spec.md FR-006` | `ContextualizedResponseService` only catches `RuntimeError`. Network failures, `ConnectionRefusedError`, `TimeoutError`, and other AI-provider exceptions are **not caught** and will propagate as unhandled 500 errors. FR-006 requires handling "local AI model unavailability". | defect | major | open | | Broaden the except clause or catch a base `Exception`; log unexpected errors |

