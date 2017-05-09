using PowerArgs;
using PowerArgs.Cli;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace speak
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConsoleArgs parsed = null;
            try
            {
                Console.WriteLine();
                Args.InvokeMain<ConsoleArgs>(args);
            }
            catch(ArgException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<ConsoleArgs>());
            }

            // exit if help is requested
            if (parsed == null || parsed.Help)
            {
                return;
            }
        }
    }
}
