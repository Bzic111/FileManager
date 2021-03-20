using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;

namespace FileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            int Kbyte = 1024;
            double Mbyte = Math.Pow(1024, 2);
            double GByte = Math.Pow(1024, 3);

            long test = 1_000_000_000L;
            float test2 = (float)test / (float)GByte;

            Console.WriteLine(GByte);
            Console.WriteLine(Math.Round(test2,2));
        }
    }
}
