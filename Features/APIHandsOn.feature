Feature: APIHandsOn

Testing https://fakerestapi.azurewebsites.net/

Scenario: UserCreationFlow
Given user is created with username "mkgmail" and password "mkpass"
When get the details of user with id "2"
Then delete the user details with id "5"