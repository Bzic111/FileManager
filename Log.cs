using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileManager
{
    class Log
    {
        string LogFile = $@"Log_{DateTime.Now:d}.txt";
        public Log() { }

        public void WriteLog(string str)
        {
            File.AppendAllText(LogFile, str + "\n");
        }
    }
}
