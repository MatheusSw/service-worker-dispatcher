# About the workers dispatch queue
This is a form of study of RabbitMQ.\
The goal for this program is to provide a way for a main application (web hosted) to get some no-return work done, like the cleaning of logs or fetching of some external API for information like the weather.\
It currently takes advantage of Threading through the .NET ThreadPool, and is able to scale some-what well with some of the default RabbitMQ configuration.

## Usage
#### 🧽 CleanerConsumer
Run `dotnet run` and it'll await for messages through the queue
#### 👮 OperatorProducer
The Operator accepts two parameters, one is the routing key which basically is where you want to send the message to, the only one set up in this project is the **clean.logs**, everything else will yield errors and lost messages. The second parameter is the json message that is going into the body. The current accepted format is 
```
{
    "attributes": {
        "path": "",
        "dateFrom": "",
        "dateUntil": ""
    }
}
```
Example call: `dotnet run -- "clean.logs" {    \"attributes\": {        \"path\": \"this/is/a/path\",        \"dateFrom\": \"08/21/2021 00:00:00\",        \"dateUntil\": \"09/21/2021 00:00:00\"    }}`
*Given that this call will come from a main application, the call it's not going to look this ugly

## Media
Below is the diagram for the program.
![Program Diagram](https://svgshare.com/i/_68.svg)
