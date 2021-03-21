using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace FileManager
{
    class Tree
    {
        public List<string> Drives { get; }
        public Tree()
        {
            Drives = GetRoots();
        }
        List<string> GetRoots()
        {
            List<string> Drives = new List<string>();
            for (char c = 'A'; c < 'Z'; c++)
            {
                if (Directory.Exists(c.ToString() + ":\\"))
                {
                    Drives.Add(c.ToString() + ":\\");
                }
            }
            return Drives;
        }
        public List<Entry> GetEntryList(string path)
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
        public string[] FrameFormat(List<Entry> lst, int StartIndex)
        {
            string[] result = new string[40];
            for (int i = StartIndex; i < result.Length & i < lst.Count; i++)
            {
                if (i == lst.Count - 1 & i == result.Length)
                {
                    for (; i < result.Length; i++)
                    {
                        result[i] = "".PadRight(73, ' ');
                    }
                }
                if (lst[i].type == Entry.Type.Directory)
                {
                    result[i] = lst[i].Name.PadRight(38, ' ').Remove(37) + lst[i].Extension;
                }
                else if (lst[i].type == Entry.Type.File)
                {
                    result[i] = lst[i].Name.PadRight(38, ' ').Remove(37) + lst[i].Extension.PadRight(11, ' ').Remove(10) + lst[i].Size;
                }
            }
            return result;
        }
        
    }
}
