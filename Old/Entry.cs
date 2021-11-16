using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Collections;
using System.Collections.Generic;

namespace FileManager.Old
{
    [Serializable]
    public class Entry
    {
        public enum Type
        {
            File,
            Directory
        }

        private int Kbyte = 1024;
        private double Mbyte = Math.Pow(1024, 2);
        private double Gbyte = Math.Pow(1024, 3);

        public string Parent;
        public string Name;
        public string Path;
        public string Extension;
        public string Size;
        public string ShortInfo;
        public string FullInfo;
        public string LastWrite;
        public bool Visible = false;
        public Type type;

        public Entry()
        {

        }
        public Entry(string path, Type t)
        {
            Path = path;
            type = t;
            Name = path.Split('\\', StringSplitOptions.RemoveEmptyEntries)[^1];
            if (type == Type.File)
            {
                FileInfo FI = new FileInfo(path);
                long temp = FI.Length;
                Extension = FI.Extension;
                LastWrite = FI.LastWriteTime.ToString();
                FullInfo = $"{Extension} {FI.Attributes} {LastWrite}";
                if (temp < Kbyte)
                {
                    Size = temp.ToString() + " b";
                }
                else if (temp < Kbyte)
                {
                    Size = (Math.Round(temp / (float)Kbyte), 2).ToString() + " Kb";
                }
                else if (temp < Mbyte)
                {
                    Size = (Math.Round(temp / (float)Mbyte), 2).ToString() + " Mb";
                }
                else if (temp < Gbyte)
                {
                    Size = (Math.Round(temp / (float)Gbyte), 2).ToString() + " Gb";
                }
            }
            else if (type == Type.Directory)
            {
                DirectoryInfo DI = new DirectoryInfo(path);
                LastWrite = Directory.GetLastWriteTime(path).ToString();
                Extension = "Directory";
                Size = "".PadRight(12, ' ');
                if (path.Split('\\', StringSplitOptions.RemoveEmptyEntries)[0] != Name)
                {
                    Path = DI.Parent.ToString();
                }
            }
            ShortInfo = Extension.PadRight(10) + Size;
            string[] tempS = path.Split('\\');
            for (int i = 0; i < tempS.Length - 1; i++)
            {
                Parent += tempS[i];
                if (i == 0 | i < tempS.Length - 2)
                {
                    Parent += '\\';
                }
            }
        }
    }
}
