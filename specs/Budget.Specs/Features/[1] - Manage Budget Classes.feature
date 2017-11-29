Feature: Feature - 1 - ManageBudgetClasses
    As a master user
    I need to manage budget classes
    To keep control of my budget

Background: 

    Given I'm working in a new scenario tenant context


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


Scenario: Scenario - 1.3 - Update budget classes

    Given I have added these budget classes:
        | Name    | SortOrder | TransactionType |
        | Income  | 1         | Income          |
        | Housing | 2         | Expense         |
        | Food    | 3         | Expense         |

    When I update the budget classes to this:
        | BudgetClass | Name                   | SortOrder | TransactionType |
        | Income      | Income - Name updated  | 1         | Income          |
        | Housing     | Housing - Type updated | 2         | Loan            |
        | Food        | Food - Sort update     | 4         | Expense         |

    Then I get the following budget classes
        | Name                   | SortOrder | TransactionType |
        | Income - Name updated  | 1         | Income          |
        | Housing - Type updated | 2         | Loan            |
        | Food - Sort update     | 4         | Expense         |
