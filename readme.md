# Consumer
TourAPI
WeatherAPiTests (powinno byc TourApiTests)

Consumer assumes the expected request and response for the request (e.g. using provider's http client)
Registers it in PACT DSL

Consumer code fires a real request to mock provider (Created by Pact)
Mock provider compares actual request with the expected request and emits expected response if comparison is succesfull


https://www.agilepartner.net/en/consumer-driven-contract-testing-made-simple-with-pact-part-2/

TODO:
- Add POST request (fix models, lower / upper case)

# Provider
WeatherForecastApi
TourAPITests (powinno byc WeatherAPITests)


Completely driven by Pact
Requests from file are sent to the provider, and actual response it generates is compared with the 
mininal expected resoponse described in consumer test

