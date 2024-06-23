Feature: User Management API

  Scenario: Create a new user
    Given I have a new user with Id "1", Username "testuser", Email "test@example.com", Password "ComplexPassword123!", and ConfirmPassword "ComplexPassword123!"
    When I send a request to create the user
    Then the response should be created
    And the user should be retrievable by ReferenceId "1"

  Scenario: Update an existing user
    Given I have an existing user with ReferenceId "1"
    When I send a request to update the user with ReferenceId "1", Username "updateduser", Email "updated@example.com", Password "UpdatedPassword123!", and ConfirmPassword "UpdatedPassword123!"
    Then the response should be no content

  Scenario: Deactivate a user
    Given I have an existing user with ReferenceId "1"
    When I send a request to deactivate the user
    Then the response should be no content

  Scenario: Verify that user with the same email cannot be created
    Given I have a user in database
      | Name      | Email           | Password            | ConfirmPassword      |
      | user1     | email1@email.de | ComplexPassword123! | ComplexPassword123!  |
    And I have a user data
      | Name      | Email           | Password            | ConfirmPassword      |
      | user1     | email1@email.de | ComplexPassword123! | ComplexPassword123!  |
    When I call Create user API
    Then I will receive an error code 400
    And Response message should contain "The user with the same email already exists."

  Scenario Outline: Verify that validation error is returned when required fields are not provided
    Given I have a user in database
      | Name   | Email   | Password  | ConfirmPassword |
      | <Name> | <Email> | <Password>| <ConfirmPassword> |
    When I call Create user API
    Then I should receive an error code 400
    And Response message should contain "<Error>"
    Examples:
      | Row | Name      | Email           | Password            | ConfirmPassword      | Error                      |
      | 1   |           | email1@email.de | ComplexPassword123! | ComplexPassword123!  | Name field is required     |
      | 2   | user1     |                 | ComplexPassword123! | ComplexPassword123!  | Email field is required    |
      | 3   | user1     | email1@email.de |                     | ComplexPassword123!  | Password field is required |
      | 4   | user1     | email1@email.de | ComplexPassword123! |                      | Confirm Password is required |

  Scenario Outline: Verify that validation error is returned when password does not meet complexity requirements or password size
    Given I have a user in database
      | Name      | Email           | Password  | ConfirmPassword |
      | user1 | email1@email.de | <Password> | <ConfirmPassword> |
    When I call Create user API
    Then I should receive an error code 400
    And Response message should contain "<Error>"
    Examples:
      | Row | Password           | ConfirmPassword | Error                                                                                                             |
      | 1   | abc                | abc             | Password length should be between 8 and 16.                                                                       |
      | 2   | 123                | 123             | Password length should be between 8 and 16.                                                                       |
      | 3   | abc123             | abc123          | Password length should be between 8 and 16.                                                                       |
      | 4   | 123456781235678abc | 123456781235678abc | Password length should be between 8 and 16.                                                                       |
      | 5   | 1111111            | 1111111         | Password does not meet complexity level, it should contain 1 upper case letter, 1 number and 1 special character. |
      | 6   | 1111111aA          | 1111111aA       | Password does not meet complexity level, it should contain 1 upper case letter, 1 number and 1 special character. |
      | 7   | 1111111a!          | 1111111a!       | Password does not meet complexity level, it should contain 1 upper case letter, 1 number and 1 special character. |
      | 8   | aaaaaaa!A          | aaaaaaa!A       | Password does not meet complexity level, it should contain 1 upper case letter, 1 number and 1 special character. |
      | 9   | ComplexPassword123! | ComplexPasswrd!| Password and Confirm Password do not match.                                                                       |
