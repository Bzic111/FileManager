using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace FileManager
{
    class Tree
    {
        string RootPath;
        public Tree()
        {

        }
        List<string> GetRoots()
        {
            List<string> Drives = new List<string>();
            for (char c = 'A'; c < 'Z'; c++)
            {
                if (Directory.Exists(c.ToString()+":\\"))
                {
                    Drives.Add(c.ToString() + ":\\");
                }
            }
            return Drives;
        }
        List<Entry> GetEntryList(string path)
        {
            string[] dirs = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);
            Array.Sort(dirs);
            Array.Sort(files);
            List<Entry> entryes = new List<Entry>();
            for (int i = 0; i < dirs.Length; i++)
            {
                entryes.Add(new Entry(dirs[i], Entry.Type.Directory));
            }
            for (int i = 0; i < files.Length; i++)
            {
                entryes.Add(new Entry(files[i], Entry.Type.File));
            }
            return entryes;
        }
    }
}
