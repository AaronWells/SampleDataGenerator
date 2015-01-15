using System;

namespace EdFi.SampleDataGenerator.Utility
{
    public interface ILog
    {
        void WriteLine(string format, params object[] arg);
    }

    public class ConsoleLogger : ILog
    {
        public void WriteLine(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
    }
}
