using CleanerConsumer.LogCleaner;
using Newtonsoft.Json;

namespace CleanerConsumer.Interfaces
{
    public interface ILogMessageProperties
    {
        [JsonProperty("attributes")]
        LogMessageAttributes Attributes { get; set; }
    }
}