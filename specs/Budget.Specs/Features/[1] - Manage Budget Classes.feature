Feature: Feature - 1 - ManageBudgetClasses
    As a master user
    I need to manage budget classes
    To keep control of my budget

Background: 

    Given there are no BudgetClasses


Scenario: Scenario - 1.1 - Add budget classes

    When I add budget classes:
        | Name           | SortOrder | TransactionType |
        | Income         | 1         | Income          |
        | Housing        | 2         | Expense         |
        | Food           | 3         | Expense         |
        | Transportation | 4         | Expense         |
        | Entertainment  | 5         | Expense         |

    Then I get the following budget classes
        | Name           | SortOrder | TransactionType |
        | Income         | 1         | Income          |
        | Housing        | 2         | Expense         |
        | Food           | 3         | Expense         |
        | Transportation | 4         | Expense         |
        | Entertainment  | 5         | Expense         |


Scenario Outline: Scenario - 1.2 - Avoid duplicate budget class name

    When I add budget class "<Name>"
    Then I can't add another class "<Name>"

    Examples: 
        | Name    |
        | Income  |
        | Housing |
        | Food    |

