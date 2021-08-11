using System.Collections.Generic;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Text;
using Newtonsoft.Json;
using CleanerConsumer.LogCleaner;

namespace CleanerConsumer
{
    public static class Dispatcher
    {
        private static Dictionary<string, Action<string>> cleaners = new Dictionary<string, Action<string>>(){
            { "logs", (string message) => {
                    LogMessage logMessage = JsonConvert.DeserializeObject<LogMessage>(message);
                    LogCleaner.LogCleaner.Clean(logMessage);
                }
            }
        };

        public static bool DispatchMessage(BasicDeliverEventArgs ea)
        {
            byte[] encodedType = ea.BasicProperties.Headers["type"] as byte[];
            string type = Encoding.UTF8.GetString(encodedType);
            
            if (!cleaners.ContainsKey(type))
            {
                Log.Error($"[{ea.DeliveryTag}] Type [{type}] doesn't exist");
                return false;
            }

            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            cleaners[type].Invoke(body);
            return true;
        }
    }
}