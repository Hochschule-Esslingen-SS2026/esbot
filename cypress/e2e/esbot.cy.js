describe('ESBot E2E Tests', () => {
  beforeEach(() => {
    // Intercept backend API calls so we don't need the actual backend running for tests.
    // If the assignment requires real backend, we can remove these cy.intercept calls.
    // But the instructions mention: "LLM mock note: For testing purpuses, you should consider using a mock LLM provider"
    // However, if we don't start the backend, we can mock via Cypress. The instructions say: "Start the backend with your mock provider"
    // That means the backend WILL be running. Let's just do standard E2E tests against the real network.
    
    // Visit the frontend page
    cy.visit('/index.html')
  })

  it('Flow 1: Creating a session and sending a chat message', () => {
    // 1. Create session
    cy.get('[data-testid="user-id-input"]').type('test-user-123')
    cy.get('[data-testid="new-session-btn"]').click()

    // 2. Wait for session list to update and verify
    cy.get('[data-testid="session-list"]')
      .find('.session-item')
      .should('have.length.at.least', 1)

    // 3. Send a message
    cy.get('[data-testid="message-input"]').type('Hello bot')
    cy.get('[data-testid="send-message-btn"]').click()

    // 4. Verify message appears (User message and Assistant message)
    cy.get('[data-testid="message-list"]')
      .find('[data-testid="user-message"]')
      .should('contain.text', 'Hello bot')

    // Wait for assistant reply (in case of mock backend, it should be fast)
    // Here we use should('be.visible') as suggested in the exercise guidance
    cy.get('[data-testid="assistant-message"]', { timeout: 10000 }).should('be.visible')
  })

  it('Flow 2: Generating a quiz and submitting an answer', () => {
    // 1. Create a session first to access quiz
    cy.get('[data-testid="user-id-input"]').type('test-user-quiz')
    cy.get('[data-testid="new-session-btn"]').click()

    // Wait for session to load
    cy.get('[data-testid="session-list"]').find('.session-item').should('have.length.at.least', 1)

    // 2. Navigate to Quiz tab
    // We didn't add a specific data-testid to the tab, but we can click by text
    cy.contains('button.tab', 'Quiz').click()

    // 3. Generate a quiz
    cy.get('[data-testid="quiz-topic-input"]').type('Software Testing')
    cy.get('[data-testid="generate-quiz-btn"]').click()

    // 4. Wait for quiz question to appear
    cy.get('[data-testid="quiz-question"]', { timeout: 10000 }).should('be.visible')

    // 5. Select an option and submit
    cy.get('[data-testid="quiz-option-0"]').click()
    cy.get('[data-testid="submit-answer-btn"]').should('not.be.disabled').click()

    // 6. Wait for feedback
    cy.get('[data-testid="quiz-feedback"]', { timeout: 10000 }).should('be.visible')
  })

  it('Negative Scenario: Trying to create a session without User ID', () => {
    // Do not type anything in user-id-input
    cy.get('[data-testid="user-id-input"]').clear()
    
    // Click new session button
    cy.get('[data-testid="new-session-btn"]').click()

    // The error banner should be displayed
    cy.get('[data-testid="error-banner"]')
      .should('be.visible')
      .and('contain.text', 'Please enter a User ID')
  })
})
