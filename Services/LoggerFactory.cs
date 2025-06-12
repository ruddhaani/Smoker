using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoker.Services
{
    public static class LoggerFactory
    {
        public static StreamWriter CreateLogger()
        {
            var dir = "C:/SmokeLogs";
            Directory.CreateDirectory(dir);
            var file = Path.Combine(dir, DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
            return new StreamWriter(file);
        }
    }

}
