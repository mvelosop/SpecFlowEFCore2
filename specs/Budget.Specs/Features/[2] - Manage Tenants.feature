Feature: Feature - 2 - Manage tenants
    As a service manager
    I need to manage tenants
    To keep control of the service

Scenario: Scenario - 2.1 - Add tenants

    Given these tenants don't exist:
        | Name     |
        | Tenant A |
        | Tenant B |

    When I add tenants:
        | Name     |
        | Tenant A |
        | Tenant B |

    Then the following tenants exist:
        | Name     |
        | Tenant A |
        | Tenant B |

Scenario Outline: Scenario - 2.2 - Avoid duplicate tenant name

    Given tenant "<Name>" does not exist
    When I add tenant "<Name>"
    Then I can't add another tenant "<Name>"

    Examples: 
        | Name     |
        | Tenant C |
        | Tenant D |

