# Test Case 1
Test case ID (e.g. TC-UI-01)
1. Date and Testername:
- 19.06.2026 Jan Schröter
1. Environment: OS, browser, LLM mock used for the test:
- Windows 11 Firefox LLm mock
1. Steps performed
- Entered a Session id
- Clicked New Session Button
- Entered a Question "hallo?"
- Pressed Send Button
1.
- LLM answers wwitth Really from a  real LLm Answer hallo?
1. Pass / Fail
- Pass
1. Screenshot for any failed or unexpected outcome (if necessary)
- None nneeded

## Test Case 2
1. Test case ID (e.g. TC-UI-02)
2. Date and tester name
- 19.06.2026 Leon Kirasic
1. Environment: OS, browser, LLM mock used for the test
- Windows 11, Google Chrome, LLM Mock
1. Steps performed
- Start Frontend and Backend -> Create New Session -> Switch from Chat to Quiz -> Requests a quiz on "Object Oriented Programming"
1. Expected vs. actual result
- We receive the expected result: A multiple Choice Quiz Question is generated and displayed
1. Pass / Fail
- Pass
1. Screenshot for any failed or unexpected outcome (if necessary)
- none

## Summary

The manual testing was straight forward for these two simple test cases. We did not encounter any problems and we would almost argue that manual testing is more effecient than automated testing, as it is faster then implementing an automation. Running automations would make sense on every change to the code base to ensure stability of the application, or if it is mandatory to run e.g. a test every 2 hours for compliance reasons.
