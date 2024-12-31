# k6 Project for ASP.NET Core Web API

This project contains a basic setup for load testing an ASP.NET Core Web API using k6.

## Prerequisites

- [Install k6](https://k6.io/docs/getting-started/installation/)

## Project Structure

- `tests/sample_test.js`: Sample load test script.

## Running the Test

1. Start your ASP.NET Core Web API locally on `http://localhost:5000`.
2. Run the k6 test using the following command:

   ```bash
   k6 run tests/sample_test.js
   ```

## Customize

Modify the `sample_test.js` script to test different endpoints or adjust the load parameters as needed.
