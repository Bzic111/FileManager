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

        public string Parent;

        public string Name;
        public string Path;
        public Type type;
        DirectoryInfo DI;
        FileInfo FI;
        public string Extension;
        public string Size;
        public string ShortInfo;
        public string FullInfo;
        public string LastWrite;
        public bool Visible = false;
        public List<Entry> Catalog;

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
                FI = new FileInfo(path);
                long temp = FI.Length;
                Extension = FI.Extension;
                LastWrite = FI.LastWriteTime.ToString();
                FileAttributes fa = File.GetAttributes(path);
                FullInfo = $"{Extension} {FI.Attributes} {LastWrite}";
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
                DI = new DirectoryInfo(path);
                LastWrite = Directory.GetLastWriteTime(path).ToString();
                Extension = "Directory";
                Size = "".PadRight(12, ' ');
                if (path.Split('\\', StringSplitOptions.RemoveEmptyEntries)[0] != Name)
                {
                    Path = DI.Parent.ToString();
                }
            }
            ShortInfo = Name.PadRight(40).Remove(37) + Extension.PadRight(10) + Size;
            string[] tempS = path.Split('\\');
            //Parent = tempS[0] + '\\';
            for (int i = 0; i < tempS.Length - 1; i++)
            {
                Parent += tempS[i];
                if (i == 0 | i < tempS.Length - 2)
                {
                    Parent += '\\';
                }
            }
        }
        public Object GetInfoType()
        {
            if (this.type == Type.Directory)
            {
                return DI;
            }
            else if (this.type == Type.File)
            {
                return FI;
            }
            return null;
        }
        public void WriteName() => Console.Write(Name);
        public void WritePath() => Console.Write(Path);
        public void WriteParent() => Console.Write(Parent);
        public string GetParent() => Parent;

    }
