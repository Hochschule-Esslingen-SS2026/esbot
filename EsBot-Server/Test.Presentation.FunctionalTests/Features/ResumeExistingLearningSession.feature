Feature: Resume existing learning sesssion Benni

Scenario: Student wants to resume session
    Given the API is running
    When the student requests an old sesssion with session-id "session-id"
    Then the response should contain all messages with that session-id "session-id"
    
Scenario: Student want to resume a session which does not exist
    Given the API is running
    When the studen request an session with a unkown session-id "session-id"
    Then the respose status should be 404
    Then the response shoudl containt "did not find your sesssion"