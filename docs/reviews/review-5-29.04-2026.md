* The ESBot-oriented **introductory / system description** (equivalent to `docs/esbot.md` or the team’s merged variant)
* **Functional requirements and specification** (e.g. `docs/spec/requirements.md`, `docs/spec/spec.md`, or Spec Kit output paths used by that team)
* **Relevant implementation** to date (e.g. backend domain model, unit tests, BDD tests, etc.)

### Tasks

1. Assign **review roles** inside your team (e.g. moderator, expert reviewers, recorder/note-taker, author liaison if applicable).
2. Choose a **review type** (e.g. walkthrough, technical review, inspection) and justify why it fits the artefacts and timebox.
3. Hold a **preparation** phase: reviewers study the materials; record questions and assumptions. If required, you can ask the other team for clarifications or schedule a meeting to discuss the artefacts and to understand the project.
4. Run the **review session(s)** using the course review template: log defects, inconsistencies, ambiguities, and improvement suggestions with clear references (file, section, or module).
5. **Document** the review type, participants, scope, and outcomes in your **own** repository (e.g. `docs/reviews/review-<team-reviewed>-<date>.md` or as a filled template uploaded in the same form as required by Moodle).

**Expected deliverables:**
* Written evidence of planning (roles, review type, scope, schedule) in your repository.
* Completed review protocol / issue list using the course template in your repository.
* Brief summary of major findings and agreed follow-up (if any) in your repository.

<https://github.com/ItsSami19/esbot>

# Review template (inspection / technical review)

**Project / product:** EsBot
**Review object(s):** ``docs/esbot.md`` ``docs/spec/requirements.md`` ``docs/spec/spec.md`` ``backend/*``
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
| Project manager | Sami Goekpinar #TODO |
| Quality expert / manager | - |
| Moderator | Jan Schröter |
| Author(s) | Baran Bickici, Sema Dagci, Sami Goekpinar, Lydia Karapanagiotidi |

### 2.2 Review objects

| # | Review objects | Abbr. |
|---|----------------|-------|
| 1 | docs/spec/requirements.md | REQ |
| 2 | docs/esbot.md | EsBot |
| 3 | docs/spec/spec.md | SPEC |
| 4 | | |
| 5 | | |

# TODO add when done

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

Use one row per finding. Extend the table if your course requires extra columns.

Suggested values: **Type** — defect, question, suggestion; **Severity** — blocking, major, minor, editorial; **Status** — open, accepted, rejected, deferred, fixed (update through meeting and rework).

| ID | Location (file / section / module) | Summary | Type | Severity | Status | Notes / meeting decision |
|----|-------------------------------------|---------|------|----------|--------|--------------------------|
| F-001 | backedn/app/models/base| On projects of larger scope on class one file | suggestion | minor | open | <!-- --> |
| F-002 | backend/app/services/answer_evaluation_service.py | The repeated usage of magic strings that are not defined in any Constant | suggestion | minor | open ||
| F-003 | config.py | Is Empty | suggestion | editorial | open||

backend/app/services/answer_evaluation_service.py why not return a http error

allow_origins=["http://localhost:3000"], can be configureble
in no test is the real api started and setup not even with mocked components

---

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

### 5.3 Decisions and follow-up

| Topic | Decision | Responsible | Due date |
|-------|----------|-------------|----------|
| <!-- --> | <!-- --> | <!-- --> | <!-- --> |

### 5.4 Positive observations (optional)

<!-- What was done well; good practices worth keeping. -->

### 5.5 Lessons learned (optional)

<!-- Process improvements for the next review. -->

### 5.6 Sign-off

| Role | Name | Signature / date |
|------|------|------------------|
| Moderator | <!-- --> | <!-- --> |
| Author | <!-- --> | <!-- --> |

---

<!-- End of template -->
