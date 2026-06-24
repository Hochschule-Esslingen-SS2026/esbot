# API Reference

## Overview

**API Version:** 1.0.0
**OpenAPI Version:** 3.1.1
**Base URL:** `http://localhost:8080`

---

# Setup Instructions

## Prerequisites

* .NET 8 SDK (or the version used by the project)
* Database configured for the application
* Required application configuration in `appsettings.json` or environment variables

## Installation

Clone the repository:

```bash
git clone <repository-url>
cd <repository-name>
```

Restore dependencies:

```bash
dotnet restore
```

Build the application:

```bash
dotnet build
```

Run the application:

```bash
dotnet run
```

The API should start on:

```text
http://localhost:8080
```

## Verify Application Health

```bash
curl http://localhost:8080/v1/Health
```

Example response:

```json
{
  "status": "Healthy",
  "major": "1",
  "minor": "0",
  "patch": "0",
  "commitHash": "abc123"
}
```

---

# Common Request Headers

All endpoints that accept JSON should include:

```http
Content-Type: application/json
Accept: application/json
```

---

# Endpoints

## Health

### GET `/v1/Health`

Returns application health and version information.

| Property       | Value  |
| -------------- | ------ |
| Method         | GET    |
| Authentication | None   |
| Request Body   | N/A    |
| Success Status | 200 OK |

#### Example Response

```json
{
  "status": "Healthy",
  "major": "1",
  "minor": "0",
  "patch": "0",
  "commitHash": "abc123"
}
```

---

## Question

### POST `/v1/Question`

Submits a question to the AI service.

| Property       | Value           |
| -------------- | --------------- |
| Method         | POST            |
| Authentication | None            |
| Request Body   | QuestionRequest |
| Success Status | 200 OK          |

#### Request Body

```json
{
  "userSessionId": "3d9773e7-5249-4e43-a94b-0f1ef33dca16",
  "question": "What is dependency injection?"
}
```

#### Success Response

```json
{
  "id": "d290f1ee-6c54-4b01-90e6-d701748f0851",
  "userSessionId": "3d9773e7-5249-4e43-a94b-0f1ef33dca16",
  "role": true,
  "content": "Dependency injection is a design pattern used to provide dependencies to classes.",
  "timestamp": "2026-01-01T12:00:00Z"
}
```

#### Possible Error Responses

* `408` - LLM timeout

Example:

```json
{
  "error": "LLM timeout"
}
```

---

## Quiz

### POST `/v1/Quiz`

Generates a quiz for a topic.

| Property       | Value             |
| -------------- | ----------------- |
| Method         | POST              |
| Authentication | None              |
| Request Body   | CreateQuizRequest |
| Success Status | 200 OK            |

#### Request Body

```json
{
  "userSessionId": "3d9773e7-5249-4e43-a94b-0f1ef33dca16",
  "topic": "ASP.NET Core"
}
```

#### Success Response

```json
{
  "id": "a8a9c4b8-c78e-4f84-ae4d-7dd55a7d791d",
  "topic": "ASP.NET Core",
  "quizItems": [
    {
      "id": "44a0e9ca-35d4-4669-a3d5-a0afde5a37a2",
      "questionText": "What is middleware?"
    }
  ]
}
```

#### Possible Error Responses

* `402` - Restricted topic

Example:

```json
{
  "error": "Topic not allowed"
}
```

---

## Sessions

### POST `/v1/Sessions`

Creates a new learning session.

| Property       | Value                |
| -------------- | -------------------- |
| Method         | POST                 |
| Request Body   | CreateSessionRequest |
| Success Status | 201 Created          |

#### Request Body

```json
{
  "userId": "user-123"
}
```

#### Success Response

```json
{
  "sessionId": "d290f1ee-6c54-4b01-90e6-d701748f0851",
  "userId": "user-123",
  "createdAt": "2026-01-01T12:00:00Z"
}
```

---

### GET `/v1/Sessions?userId={userId}`

Returns all sessions for a user.

| Property        | Value  |
| --------------- | ------ |
| Method          | GET    |
| Query Parameter | userId |
| Success Status  | 200 OK |

#### Example Response

```json
[
  {
    "sessionId": "d290f1ee-6c54-4b01-90e6-d701748f0851",
    "userId": "user-123",
    "createdAt": "2026-01-01T12:00:00Z"
  }
]
```

---

### POST `/v1/Sessions/{sessionId}/messages`

Submits a message to a session.

| Property       | Value  |
| -------------- | ------ |
| Method         | POST   |
| Success Status | 200 OK |

#### Request Body

```json
{
  "userSessionId": "3d9773e7-5249-4e43-a94b-0f1ef33dca16",
  "question": "Explain middleware."
}
```

#### Success Response

```json
{
  "id": "d290f1ee-6c54-4b01-90e6-d701748f0851",
  "userSessionId": "3d9773e7-5249-4e43-a94b-0f1ef33dca16",
  "role": true,
  "content": "Middleware processes HTTP requests and responses.",
  "timestamp": "2026-01-01T12:00:00Z"
}
```

#### Possible Error Responses

* `422` Validation failed
* `503` AI service unavailable

---

### GET `/v1/Sessions/messages/{sessionId}`

Returns session message history.

| Property       | Value  |
| -------------- | ------ |
| Method         | GET    |
| Success Status | 200 OK |

#### Example Response

```json
[
  {
    "id": "d290f1ee-6c54-4b01-90e6-d701748f0851",
    "userSessionId": "3d9773e7-5249-4e43-a94b-0f1ef33dca16",
    "role": true,
    "content": "Hello",
    "timestamp": "2026-01-01T12:00:00Z"
  }
]
```

---

### POST `/v1/Sessions/{sessionId}/quiz`

Generates a quiz within a session.

| Property       | Value  |
| -------------- | ------ |
| Method         | POST   |
| Success Status | 200 OK |

#### Request Body

```json
{
  "userSessionId": "3d9773e7-5249-4e43-a94b-0f1ef33dca16",
  "topic": "ASP.NET Core"
}
```

#### Success Response

```json
{
  "id": "a8a9c4b8-c78e-4f84-ae4d-7dd55a7d791d",
  "topic": "ASP.NET Core",
  "quizItems": [
    {
      "id": "44a0e9ca-35d4-4669-a3d5-a0afde5a37a2",
      "questionText": "What is middleware?"
    }
  ]
}
```

#### Possible Error Responses

* `422` Validation failed
* `503` Service unavailable

---

### POST `/v1/Sessions/{sessionId}/quiz/{questionId}/answer`

Evaluates a quiz answer.

| Property       | Value  |
| -------------- | ------ |
| Method         | POST   |
| Success Status | 200 OK |

#### Request Body

```json
{}
```

#### Success Response

```json
{}
```

---

### DELETE `/v1/Sessions/{sessionId}`

Deletes a session and all associated data.

| Property       | Value          |
| -------------- | -------------- |
| Method         | DELETE         |
| Success Status | 204 No Content |

#### Success Response

No response body.

---

# Error Responses

## 404 Not Found

```json
{
  "error": "Session with ID {sessionId} not found."
}
```

## 422 Unprocessable Entity

```json
{
  "type": "https://tools.ietf.org/html/rfc4918#section-11.2",
  "title": "One or more validation errors occurred.",
  "status": 422,
  "detail": "Validation failed."
}
```

## 500 Internal Server Error

```json
{
  "error": "An unexpected error occurred."
}
```

## 503 Service Unavailable

```json
{
  "error": "AI service is currently unavailable."
}
```

## 524 Timeout

```json
{
  "error": "LLM timeout"
}
```

---

# DTO Schemas

## CreateSessionRequest

```json
{
  "userId": "string"
}
```

## QuestionRequest

```json
{
  "userSessionId": "uuid",
  "question": "string"
}
```

## CreateQuizRequest

```json
{
  "userSessionId": "uuid",
  "topic": "string"
}
```

## SessionResponse

```json
{
  "sessionId": "uuid",
  "userId": "string",
  "createdAt": "datetime"
}
```

## MessageResponse

```json
{
  "id": "uuid",
  "userSessionId": "uuid",
  "role": true,
  "content": "string",
  "timestamp": "datetime"
}
```

## QuizRequestResponse

```json
{
  "id": "uuid",
  "topic": "string",
  "quizItems": [
    {
      "id": "uuid",
      "questionText": "string"
    }
  ]
}
```
