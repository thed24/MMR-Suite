# MMR-Cacher

The MMR Cacher tool is primarily an AWS stack revolving around a lambda that polls Riot Games API for Summoner information, translates it to a model and feeds it into a DynamoDB table. The lambda runs against a cron-event that triggers every 15 minutes and updates the table only if a Summoners LP has changed, and if so, appends it to that entries LpLog list attribute. This allows us to accurately keep a record of hundreds of users LP gain or loss history.

# MMR-Calculator

The MMR Calculator tool is a lot more simple than the cacher, as it runs against the DynamoDB table to generate an average LP gain and LP loss amount for the players found (for reference, the players are sourced from the League that the given player is in). It then compares the players own average LP gain and LP loss amounts to gauge the players MMR relative to the league they are in.

# AWS

This project contains source code and supporting files for a serverless application that you can deploy with the SAM CLI. It includes the following files and folders.

- src/cacher - Code for the application's Lambda function.
- template.yaml - A template that defines the application's AWS resources.

The application uses several AWS resources, including Lambda functions, a DynamoDB table and a Cloudwatch Event.

# Running the Tool

To run without building, simply CD into the Calculator directory located within the src subdirectory and run ```dotnet run {SUMMONER NAME}```. Please note, you will need to have built up a database that has this user within the database first. To do so, use the supplied template to generate the appropriate AWS stack, and supply the lambda with a Riot API key and a chosen summoner name. This Stack will run the lambda every 15 minutes in-order to maintain an accurate log of LP histories.

### Sample Output:

Server average gain = 21 and average loss = 18
Your average gain = 17 and average loss = 18

# Tests

As this is just a side project I hacked together, I initially have forgone tests, which is a bad practice. Usually, I take a TDD approach to designing architecture and code. Tests will be added down the line.
