using System;
using System.IO;

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

    public class FileLogger : ILog
    {
        private readonly string _filename;
        
        public FileLogger(string filename)
        {
            _filename = filename;
        }

        public void WriteLine(string format, params object[] arg)
        {
            File.AppendAllLines(_filename, new[] {string.Format(format, arg)});
        }
    }
}
