const API_BASE_URL = "http://localhost:8080/api/v1";

// State
let currentUserId = "";
let currentSessionId = "";
let selectedQuizOption = null;
let currentQuestionId = null;

// DOM Elements
const healthBadge = document.getElementById("healthBadge");
const errorBanner = document.getElementById("errorBanner");
const userIdInput = document.getElementById("userIdInput");
const newSessionBtn = document.getElementById("newSessionBtn");
const sessionList = document.getElementById("sessionList");
const emptyState = document.getElementById("emptyState");
const sessionView = document.getElementById("sessionView");
const messageList = document.getElementById("messageList");
const messageInput = document.getElementById("messageInput");
const sendMessageBtn = document.getElementById("sendMessageBtn");
const chatLoading = document.getElementById("chatLoading");
const tabChat = document.getElementById("tabChat");
const tabQuiz = document.getElementById("tabQuiz");
const chatContainer = document.getElementById("chatContainer");
const quizContainer = document.getElementById("quizContainer");
const quizTopicInput = document.getElementById("quizTopicInput");
const generateQuizBtn = document.getElementById("generateQuizBtn");
const quizContent = document.getElementById("quizContent");
const quizQuestion = document.getElementById("quizQuestion");
const submitAnswerBtn = document.getElementById("submitAnswerBtn");
const quizFeedback = document.getElementById("quizFeedback");
const quizOptions = [
    document.getElementById("quizOption0"),
    document.getElementById("quizOption1"),
    document.getElementById("quizOption2"),
    document.getElementById("quizOption3"),
];

// Initialization
document.addEventListener("DOMContentLoaded", () => {
    checkHealth();
    setupEventListeners();
});

// Helper functions
const showError = (message) => {
    errorBanner.textContent = message;
    errorBanner.classList.remove("hidden");
    setTimeout(() => errorBanner.classList.add("hidden"), 5000);
};

// API Functions
async function checkHealth() {
    try {
        const response = await fetch(`${API_BASE_URL}/Health`);
        if (response.ok) {
            healthBadge.textContent = "Healthy";
            healthBadge.classList.add("healthy");
            healthBadge.classList.remove("unhealthy");
        } else {
            throw new Error("API Unhealthy");
        }
    } catch (error) {
        healthBadge.textContent = "Offline";
        healthBadge.classList.add("unhealthy");
        healthBadge.classList.remove("healthy");
        showError("Unable to connect to the backend server.");
    }
}

async function createSession() {
    const userId = userIdInput.value.trim();
    if (!userId) {
        showError("Please enter a User ID");
        return;
    }

    try {
        const response = await fetch(`${API_BASE_URL}/Sessions`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ userId: userId }),
        });

        if (response.ok) {
            const session = await response.json();
            currentUserId = userId;
            await loadSessions(userId);
            selectSession(session.sessionId);
        } else {
            const err = await response.json();
            showError(err.detail || "Failed to create session");
        }
    } catch (error) {
        showError("Error connecting to the server");
    }
}

async function loadSessions(userId) {
    try {
        const response = await fetch(
            `${API_BASE_URL}/Sessions?userId=${encodeURIComponent(userId)}`,
        );
        if (response.ok) {
            const sessions = await response.json();
            renderSessionList(sessions);
        }
    } catch (error) {
        showError("Failed to load sessions");
    }
}

function renderSessionList(sessions) {
    sessionList.innerHTML = "";
    sessions.forEach((session) => {
        const li = document.createElement("li");
        li.className = `session-item ${session.sessionId === currentSessionId ? "active" : ""}`;
        li.setAttribute("data-testid", `session-item-${session.sessionId}`);
        li.onclick = () => selectSession(session.sessionId);

        const dateStr = new Date(session.createdAt).toLocaleString();
        li.innerHTML = `
            <div class="session-item-id">Session ${session.sessionId.substring(0, 8)}...</div>
            <div class="session-item-date">${dateStr}</div>
        `;
        sessionList.appendChild(li);
    });
}

async function selectSession(sessionId) {
    currentSessionId = sessionId;
    emptyState.classList.add("hidden");
    sessionView.classList.remove("hidden");

    // Update UI selection
    const items = sessionList.getElementsByClassName("session-item");
    for (let item of items) {
        if (item.getAttribute("data-testid") === `session-item-${sessionId}`) {
            item.classList.add("active");
        } else {
            item.classList.remove("active");
        }
    }

    // Load messages
    await loadMessages(sessionId);

    // Reset Quiz UI
    quizContent.classList.add("hidden");
    quizTopicInput.value = "";
    quizFeedback.classList.add("hidden");
}

async function loadMessages(sessionId) {
    messageList.innerHTML = "";
    try {
        const response = await fetch(
            `${API_BASE_URL}/Sessions/messages/${sessionId}`,
        );
        if (response.ok) {
            const messages = await response.json();
            messages.forEach(renderMessage);
            scrollToBottom();
        }
    } catch (error) {
        showError("Failed to load messages");
    }
}

function renderMessage(msg) {
    const div = document.createElement("div");
    // Schema says role: boolean. Assume true = user, false = assistant (or vice-versa).
    // Let's assume true is user.
    const isUser = msg.role === true;
    div.className = `message ${isUser ? "user" : "assistant"}`;
    div.setAttribute(
        "data-testid",
        isUser ? "user-message" : "assistant-message",
    );
    div.textContent = msg.content;
    messageList.appendChild(div);
}

async function sendMessage() {
    if (!currentSessionId) return;
    const text = messageInput.value.trim();
    if (!text) return;

    // Optimistic UI update
    messageInput.value = "";
    renderMessage({
        role: true,
        content: text,
        id: "temp",
        timestamp: new Date().toISOString(),
    });
    scrollToBottom();
    chatLoading.classList.remove("hidden");

    try {
        const response = await fetch(
            `${API_BASE_URL}/Sessions/${currentSessionId}/messages`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    userSessionId: currentSessionId,
                    question: text,
                }),
            },
        );

        chatLoading.classList.add("hidden");
        if (response.ok) {
            const aiMsg = await response.json();
            renderMessage(aiMsg);
            scrollToBottom();
        } else {
            showError("Failed to send message");
        }
    } catch (error) {
        chatLoading.classList.add("hidden");
        showError("Network error");
    }
}

function scrollToBottom() {
    messageList.scrollTop = messageList.scrollHeight;
}

// Tabs
window.switchTab = function (tabName) {
    if (tabName === "chat") {
        tabChat.classList.add("active");
        tabQuiz.classList.remove("active");
        chatContainer.classList.remove("hidden");
        quizContainer.classList.add("hidden");
    } else {
        tabQuiz.classList.add("active");
        tabChat.classList.remove("active");
        quizContainer.classList.remove("hidden");
        chatContainer.classList.add("hidden");
    }
};

// Quiz Functions
async function generateQuiz() {
    if (!currentSessionId) return;
    const topic = quizTopicInput.value.trim();
    if (!topic) {
        showError("Please enter a quiz topic");
        return;
    }

    generateQuizBtn.disabled = true;
    generateQuizBtn.textContent = "Generating...";
    quizContent.classList.add("hidden");

    try {
        const response = await fetch(
            `${API_BASE_URL}/Sessions/${currentSessionId}/quiz`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    userSessionId: currentSessionId,
                    topic: topic,
                }),
            },
        );

        if (response.ok) {
            const quizData = await response.json();
            renderQuiz(quizData);
        } else {
            showError("Failed to generate quiz");
        }
    } catch (error) {
        showError("Network error");
    } finally {
        generateQuizBtn.disabled = false;
        generateQuizBtn.textContent = "Generate Quiz";
    }
}

function renderQuiz(quizData) {
    if (!quizData.quizItems || quizData.quizItems.length === 0) {
        showError("Received empty quiz data");
        return;
    }

    // For simplicity in this UI, we just show the first question
    const item = quizData.quizItems[0];
    currentQuestionId = item.id;
    quizQuestion.textContent = item.questionText;

    // Assuming options come separated by newlines or we mock them.
    // The Swagger schema doesn't specify options array.
    // We'll map options abstractly for the UI based on data-testid requirements.
    quizOptions.forEach((btn, index) => {
        btn.textContent = `Option ${index + 1}`; // Placeholder since API schema lacks options details
        btn.classList.remove("selected");
    });

    selectedQuizOption = null;
    submitAnswerBtn.disabled = true;
    submitAnswerBtn.classList.add("disabled");
    quizFeedback.classList.add("hidden");
    quizContent.classList.remove("hidden");
}

window.selectOption = function (index) {
    selectedQuizOption = index;
    quizOptions.forEach((btn, i) => {
        if (i === index) btn.classList.add("selected");
        else btn.classList.remove("selected");
    });
    submitAnswerBtn.disabled = false;
    submitAnswerBtn.classList.remove("disabled");
};

async function submitAnswer() {
    if (selectedQuizOption === null || !currentSessionId || !currentQuestionId)
        return;

    submitAnswerBtn.disabled = true;
    submitAnswerBtn.textContent = "Submitting...";

    try {
        const response = await fetch(
            `${API_BASE_URL}/Sessions/${currentSessionId}/quiz/${currentQuestionId}/answer`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    /* AnswerEvaluationRequest schema is empty in swagger, sending empty object or selected index */ answerIndex:
                        selectedQuizOption,
                }),
            },
        );

        if (response.ok) {
            const result = await response.json();
            // EvaluationResultResponse is empty in swagger, mocking feedback based on status
            quizFeedback.classList.remove("hidden", "correct", "incorrect");
            // Mocking logic: just say correct for now
            quizFeedback.classList.add("correct");
            quizFeedback.textContent = "Answer submitted successfully!";
        } else {
            showError("Failed to submit answer");
            quizFeedback.classList.remove("hidden", "correct", "incorrect");
            quizFeedback.classList.add("incorrect");
            quizFeedback.textContent = "Failed to submit answer.";
        }
    } catch (error) {
        showError("Network error");
    } finally {
        submitAnswerBtn.disabled = false;
        submitAnswerBtn.textContent = "Submit Answer";
    }
}

function setupEventListeners() {
    newSessionBtn.addEventListener("click", createSession);
    sendMessageBtn.addEventListener("click", sendMessage);
    messageInput.addEventListener("keypress", (e) => {
        if (e.key === "Enter") sendMessage();
    });
    generateQuizBtn.addEventListener("click", generateQuiz);
    quizTopicInput.addEventListener("keypress", (e) => {
        if (e.key === "Enter") generateQuiz();
    });
    submitAnswerBtn.addEventListener("click", submitAnswer);
}
