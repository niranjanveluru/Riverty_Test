Feature: Test api functionality

    Scenario: Fetch the exchange rates with valid access key
        Given I entered the "valid" access key
        When I send GET request with base as "EUR" and symbols as "GBP"
        Then I should get response code as 200
        And I see "base" in response body as "EUR"
        And I see "rates" in response body contains key "GBP"
        And I see "success" in response body as boolean value "true"

    Scenario: Fetch the exchange rates with Invalid access key
        Given I entered the "Invalid" access key
        When I send GET request with base as "EUR" and symbols as "GBP"
        Then I should get response code as 401
        And I see "success" in response body as boolean value "false"
        And I see "invalid_access_key" error in response

    Scenario: Fetch the exchange rates for invalid symbols
        Given I entered the "valid" access key
        When I send GET request with base as "EUR" and symbols as "ABC"
        Then I should get response code as 400 
        And I see "success" in response body as boolean value "false"
        And I see "invalid_currency_codes" error in response

    Scenario: Fetch the exchange rates for multiple currencies
        Given I entered the "valid" access key
        When I send GET request with base as "EUR" and symbols as "GBP,INR"
        Then I should get response code as 200 
        And I see "base" in response body as "EUR"
        And I see "rates" in response body contains key "GBP"
	    And I see "rates" in response body contains key "INR""
        And I see "success" in response body as boolean value "true"