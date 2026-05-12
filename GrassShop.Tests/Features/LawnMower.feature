Feature: LawnMower CRUD
  As a store manager
  I want to manage lawn mowers in the system
  So that customers can view and purchase products

  Scenario Outline: Create a new lawn mower
    Given no lawn mowers exist
    When I create a lawn mower with name "<name>", brand "<brand>", description "<description>", price <price> and stock <stock>
    Then the created lawn mower should have name "<name>"
    And the created lawn mower should have price <price>

    Examples:
      | name             | brand    | description      | price   | stock |
      | GreenMaster 3000 | GreenCo  | Powerful mower   | 4999.99 | 10    |
      | TurboMow 500     | TurboInc | Budget mower     | 1299.00 | 25    |

  Scenario: Get all lawn mowers when none exist
    Given no lawn mowers exist
    When I get all lawn mowers
    Then the result should be an empty list

  Scenario: Get all lawn mowers with data
    Given a lawn mower with name "Alpha Mower" exists
    And a lawn mower with name "Beta Mower" exists
    When I get all lawn mowers
    Then the result should contain 2 lawn mowers

  Scenario: Get lawn mower by ID when it exists
    Given a lawn mower with name "Alpha Mower" exists
    When I get the lawn mower by its ID
    Then the lawn mower should be found with name "Alpha Mower"

  Scenario: Get lawn mower by ID when it does not exist
    Given no lawn mowers exist
    When I get the lawn mower with ID 999
    Then the lawn mower should not be found

  Scenario: Update an existing lawn mower
    Given a lawn mower with name "Old Name" exists
    When I update the lawn mower name to "New Name"
    Then the updated lawn mower should have name "New Name"

  Scenario: Update a non-existing lawn mower
    Given no lawn mowers exist
    When I update the lawn mower with ID 999
    Then the update result should be null

  Scenario: Delete an existing lawn mower
    Given a lawn mower with name "Mower To Delete" exists
    When I delete the lawn mower by its ID
    Then the deletion should succeed
    And the lawn mower should no longer exist

  Scenario: Delete a non-existing lawn mower
    Given no lawn mowers exist
    When I delete the lawn mower with ID 999
    Then the deletion should fail
