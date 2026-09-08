@api @regression
Feature: Create bookings with different deposit values

  Background:
    Given the booking service is available
    And I have a valid admin token

  Scenario Outline: Create a booking with depositpaid set to different values
    When I create a booking with depositpaid set to "<depositPaid>"
    Then the booking should be created with depositpaid set to "<depositPaid>"
    When I delete the booking
    Then retrieving the deleted booking should return 404

    Examples:
      | depositPaid |
      | true        |
      | false       |