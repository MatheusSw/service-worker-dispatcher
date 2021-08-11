using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Core;

namespace Operator
{
    public class Program
    {
        private static Logger SetupLogging()
        {
            string logTemplate = "{Timestamp:dd-MM-yyyy HH:mm:ss} [{Level:u}] [Operator] {Message}{NewLine}{Exception}";
            var logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console(outputTemplate: logTemplate).CreateLogger();
            return logger;
        }

        //This has been made to only send one message, but it can be easily adapted to send more
        static void Main(string[] args)
        {
            Log.Logger = SetupLogging();
            Log.Debug("Application started");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string exchangeName = "topic_work";
                    channel.ExchangeDeclare(exchange: exchangeName,
                        type: "topic");

                    var properties = channel.CreateBasicProperties();
                    properties.Headers = new Dictionary<string, object>();
                    //We could also go MessagePack, but why go the trouble
                    properties.ContentType = "application/json";

                    var routingKey = (args.Length > 0) ? args[0] : String.Empty;
                    properties.Headers.Add("type", routingKey.Split(".")[1]);

                    var message = (args.Length > 1) ? args[1] : String.Empty;
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: exchangeName,
                                         routingKey: routingKey,
                                         basicProperties: properties,
                                         body: body);
                    Log.Information("Message sent, exiting.");
                }
            }
        }
    }
}
