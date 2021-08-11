using System;
using Newtonsoft.Json;

namespace CleanerConsumer.LogCleaner
{
    public sealed class LogMessageAttributes
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("dateFrom")]
        public DateTime DateFrom { get; set; }

        [JsonProperty("dateUntil")]
        public DateTime DateUntil { get; set; }
    }
}