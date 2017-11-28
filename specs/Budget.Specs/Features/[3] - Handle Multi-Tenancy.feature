Feature: Feature - 3 - Handle Multi-Tenancy
    As a site owner
    I need to handle multi-tenancy
    To optimize resource usage

Scenario: Scenario - 3.1 - Allow same budget classes for different tenants

    Given I have a new tenant "Multi-Tenant A"

    And I have the following budget class for "Multi-Tenant A":
        | Name           | SortOrder | TransactionType |
        | Budget Class A | 1         | Income          |
        | Bduget Class B | 2         | Expense         |

    When I have a new tenant "Multi-Tenant B"

    Then I can also have the following budget class for "Multi-Tenant B":
        | Name           | SortOrder | TransactionType |
        | Budget Class A | 1         | Income          |
        | Bduget Class B | 2         | Expense         |
