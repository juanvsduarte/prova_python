using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSB.App.Log
{
    public class LogUtil
    {
        public static void InitializeLogs()
        {
            Serilog.Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log/", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
