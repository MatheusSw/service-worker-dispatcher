using System.Threading.Tasks;
using Serilog;

namespace CleanerConsumer.LogCleaner
{
    public static class LogCleaner
    {
        public static async void Clean(LogMessage message)
        {
            await Task.Run(() =>
            {
                //Hey I'm pretending to clean something
                Log.ForContext("Attributes", message.Attributes, true).Information("Cleaning logs");
                return Task.Delay(3000);
            });
            Log.Information("Finished cleaning logs on {path}, cya!", message.Attributes.Path);
        }
    }
}