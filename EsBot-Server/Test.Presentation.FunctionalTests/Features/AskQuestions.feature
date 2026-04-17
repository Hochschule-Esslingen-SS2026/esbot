Feature: Ask a Question

@mock-llm
Scenario: Ask a Question with LLm responding
    Given the API is running
    When the Student sends a question
    Then the response status should be 200
    Then the question should be saved with the session-id "" in the DataBase
    Then the respone should contain "mockedanswer"
    
Scenario: Ask a Question with real LLm responding
    Given the API is running
    When the Student sends a question
    Then the response status should be 200
    Then the question should be saved with the session-id "" in the DataBase
    Then the respone should contain "real LLm Answer"
    
@llm-timeout  
Scenario: Ask a Question with LLM timeout
    Given the API is running
    When the Student sends a question
    Then the response status should be 524