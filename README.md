# Simple Payment Gateway

 

Simple Payment Gateway is an example of how to approach a real-world problem using:

 - Event Sourcing
 - CQRS
 - Mediator Pattern
 - Hexagonal/Clean architecture

This is an exploration project. Please don't expect a production-ready code.

The solution includes a Payment Gateway Service and a Bank Simulator. The Bank Simulator is a Fake service for development purposes. 

The system is currently storing the information in memory.

## Prerequisites

Before you begin, ensure you have met the following requirements:

* You have installed .net core 3.1

## Building

To build simple-payment-gateway, follow these steps:

Linux and macOS:
```
./build.sh
```

Windows:
```
.\build.ps1
```

## Using simple-payment-gateway

To use simple-payment-gateway, follow these steps:


 1. Configure the FakeBank service endpoint at the PaymentGateway.Api `appsettings.json` file
 2. Start the FakeBank Service
 3. Start the PaymentGateway.Api Service
    1. You can interact with it using the REST API or accessing the Swagger page at `/swagger`


### Testing payments

Send a POST request to the endpoint `/api/Payments`. 

JSON payload example:

```JSON
{
  "amount": 200,
  "currency": "EUR",
  "card": {
    "type": "VISA",
    "name": "GF",
    "number": "1111222233334444",
    "expireMonth": 12,
    "expireYear": 2020,
    "cvv": 123
  }
}
```

## What's next / Improvement ideas

  - Implement a Database
  - Idempotency
  - Logging
  - Use Correlation Id for tracking requests
  - Implement an enumeration with the accepted Card Types
  - Unit tests - Implement the Builder pattern to simplify data initialization
  - Implement Integration Tests
  - Ensure that Personal Identifiable Information is anonymized