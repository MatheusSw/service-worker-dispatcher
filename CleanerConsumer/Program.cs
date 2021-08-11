using System;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Core;

namespace CleanerConsumer
{
    class Program
    {
        private static Logger SetupLogging()
        {
            string logTemplate = "{Timestamp:dd-MM-yyyy HH:mm:ss} [{Level:u}] [Consumer] [CleanLog] {Message}{NewLine}{Exception}";
            var logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console(outputTemplate: logTemplate).CreateLogger();
            return logger;
        }

        static void Main(string[] args)
        {
            Log.Logger = SetupLogging();
            if (args.Length > 0)
            {
                Log.Error("Incorrect usage, no parameters are allowed");
                Environment.ExitCode = 1;
                return;
            }
            Log.Information("Application Started");

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string exchangeName = "topic_work";
                    channel.ExchangeDeclare(exchange: exchangeName, type: "topic");
                    Log.Debug($"Declaring exchange with name {exchangeName}");

                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                                      exchange: exchangeName,
                                      routingKey: "clean.*");
                    Log.Debug("Queue has been binded successfully");

                    Log.Information("Queue is ready and awaiting for messages...");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        bool success = Dispatcher.DispatchMessage(ea);
                        if(!success){
                            Log.Error($"Sending NACK for DeliveryTag ({ea.DeliveryTag})");
                            channel.BasicNack(ea.DeliveryTag, false, false);
                        }
                    };
                    channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);
                    Console.ReadLine();
                }
            }
        }
    }
}
