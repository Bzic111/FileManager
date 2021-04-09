using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileManager
{
    class Log
    {
        string LogFile = $"Log_{DateTime.Now}.txt";
        public FileStream logger;
        public Log()
        {
            FileStream logger = new FileStream(LogFile, FileMode.OpenOrCreate);
        }
    }
}
