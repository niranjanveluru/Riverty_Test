Feature: Test api functionality

    Scenario: Fetch the exchange rates with valid access key
        Given I entered the valid access key
        When I send GET request with base as "EUR" and symbols as "GBP"
        Then I should get response code as 200
        And I see "base" in response body as "EUR"
        And I see "rates" in response body contains key "GBP"
        And I see "success" in response body as boolean value "true"