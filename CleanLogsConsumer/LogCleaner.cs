using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;

namespace CleanLogsConsumer
{
    public static class LogCleaner
    {
        public static async void Clean(string path)
        {
            await Task.Run(() =>
            {
                //Hey I'm pretending to clean something
                Log.Information("Cleaning some logs on {path}, don't mind me", path);
                return Task.Delay(3000);
            });
            Log.Information("Finished cleaning logs on {path}, cya!", path);
        }
    }
}