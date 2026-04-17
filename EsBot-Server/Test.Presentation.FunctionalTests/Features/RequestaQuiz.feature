Feature: Request a Quiz Leon

Scenario: Student requests a Quiz
    Given the API is running
    When the Student requests a quiz on "Object Oriented Programming"
    Then the response status should be 200
    Then the System generates a list of questions
    Then the questions are send to the student
    
Scenario: Studen requests a Quiz on NSFW Content
    Given the API is running
    When the Studen requests a quiz on "NSFW"
    Then the response status should be 402