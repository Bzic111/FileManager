using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileManager
{
    class Entry
    {
        private int Kbyte = 1024;
        private double Mbyte = Math.Pow(1024, 2);
        private double Gbyte = Math.Pow(1024, 3);
        public enum Type
        {
            File, Directory
        }
        public string Name;
        public string Path;
        public Type type;
        public string Extension;
        public string Size;
        public Entry()
        {

        }
        public Entry(string path, Type t)
        {
            Path = path;
            type = t;
            Name = path.Split('\\')[^1];
            if (File.Exists(path))
            {
                long temp = new FileInfo(path).Length;
                Extension = new FileInfo(path).Extension;

                if (temp < Kbyte)
                {
                    Size = temp.ToString() + " b";
                }
                else if (temp < Mbyte)
                {
                    Size = (Math.Round((float)temp / (float)Kbyte), 2).ToString() + " Kb";
                }
                else if (temp < Mbyte)
                {
                    Size = (Math.Round((float)temp / (float)Mbyte), 2).ToString() + " Mb";
                }
                else if (temp < Gbyte)
                {
                    Size = (Math.Round((float)temp / (float)Gbyte), 2).ToString() + " Gb";
                }
            }
            else if (Directory.Exists(path))
            {
                Extension = "Directory";
                Size = " ";
            }
        }
    }
}
