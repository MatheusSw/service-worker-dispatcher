using CleanerConsumer.Interfaces;
using Newtonsoft.Json;

namespace CleanerConsumer.LogCleaner
{
    public sealed class LogMessage : ILogMessageProperties
    {
        [JsonProperty("attributes")]
        public LogMessageAttributes Attributes { get; set; }
    }
}