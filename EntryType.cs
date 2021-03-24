using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileManager
{
    class Entry
    {
        public enum Type
        {
            File,
            Directory
        }

        private int Kbyte = 1024;
        private double Mbyte = Math.Pow(1024, 2);
        private double Gbyte = Math.Pow(1024, 3);

        public string Name;
        public string Path;
        public Type type;
        public string Extension;
        public string Size;
        public string ShortInfo;
        public string FullInfo;
        public string LastWrite;
        public bool Visible = false;
        public string Parent;
        public List<Entry> Catalog;

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
                FileInfo fi = new FileInfo(path);
                long temp = fi.Length;
                Extension = fi.Extension;
                LastWrite = fi.LastWriteTime.ToString();
                FileAttributes fa = File.GetAttributes(path);
                FullInfo = $"{Extension} {fi.Attributes} {LastWrite}";
                if (temp < Kbyte)
                {
                    Size = temp.ToString() + " b";
                }
                else if (temp < Kbyte)
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
            else if (type == Type.Directory)
            {
                LastWrite = Directory.GetLastWriteTime(path).ToString();
                Extension = "Directory";
                Size = "".PadRight(12, ' ');
                DirectoryInfo di = new DirectoryInfo(path);
                Path = di.Parent.ToString();
            }
            ShortInfo = Name.PadRight(40).Remove(37) + Extension.PadRight(10) + Size;
            
        }
        void GetName() => Console.Write(Name);
        string GetPath() => Path;
    }
}
