using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace FileManager
{
    class Tree
    {
        public string CurrentCatalog { get; set; }
        public List<string> Roots { get; private set; }
        public List<string> Drives { get; }
        public Tree() => SetRoots();
        public void SetRoots()
        {
            List<string> Drives = new List<string>();
            for (char c = 'A'; c < 'Z'; c++)
            {
                if (Directory.Exists(c.ToString() + ":\\"))
                {
                    Drives.Add(c.ToString() + ":\\");
                }
            }
            Roots = Drives;
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
        public void ToArray(List<Entry> entryes)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < entryes.Count; i++)
            {
                if (entryes[i].type == Entry.Type.Directory)
                {
                    sb.Append($"╚╦{entryes[i].Name}".PadRight(Console.WindowWidth, ' ').Remove(Console.WindowWidth - 5));
                }
                else if (entryes[i].type == Entry.Type.File)
                {
                    sb.Append($" ╟─{entryes[i].Name}".PadRight(Console.WindowWidth, ' ').Remove(Console.WindowWidth - 5));
                }
            }
        }
        public void InsertEntryList(string path, int index, List<Entry> entryes)
        {
            string[] dirs = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);
            Array.Sort(dirs);
            Array.Sort(files);
            for (int i = 0; i < dirs.Length; i++)
            {
                entryes.Insert(index, new Entry(dirs[i], Entry.Type.Directory));
            }
            for (int i = 0; i < files.Length; i++)
            {
                entryes.Insert(index, new Entry(files[i], Entry.Type.File));
            }
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
        public List<List<Entry>> ToPages(List<Entry> Entryes)
        {
            List<List<Entry>> Pages = new List<List<Entry>>();
            for (int i = 0, counter = 0; counter < Entryes.Count; i++)
            {
                Pages.Add(new List<Entry>());
                for (int j = 0; j < 40 & counter < Entryes.Count; j++, counter++)
                {
                    Pages[i].Add(Entryes[counter]);
                }
            }
            return Pages;
        }
    }
}
