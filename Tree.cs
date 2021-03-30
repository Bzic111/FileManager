using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Collections;
using System.Collections.Generic;
namespace FileManager
{
    class Tree
    {
        public List<Entry> Entryes;
        public List<List<Entry>> Pages;
        public string CurrentPath;
        string CurrentDrive;
        public List<string> Roots { get; private set; }
        List<string> Drives;
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
        void GetEntryList(string path)
        {
            List<Entry> entryes = new List<Entry>();
            try
            {
                string[] dirs = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);
                string[] pather = path.Split('\\', StringSplitOptions.RemoveEmptyEntries);

                Array.Sort(dirs);
                Array.Sort(files);
                entryes.Add(new Entry(path, Entry.Type.Directory));
                entryes[0].Name = pather[^1];
                entryes[0].Path = path;
                foreach (var item in dirs)
                {
                    entryes.Add(new Entry(item, Entry.Type.Directory));
                }
                foreach (var item in files)
                {
                    entryes.Add(new Entry(item, Entry.Type.File));
                }
                CurrentPath = path;
                Entryes = entryes;
            }
            catch (UnauthorizedAccessException e)
            {
                Frame Error = new Frame(30, 30, 5, e.Message.Length + 2);
                Error.SetName("Acces Denied");
                Error.Coloring(Frame.Colorscheme.Warning);
                Error.SetColor(Frame.ColorsPreset.Normal);
                Error.Show();
                Error.Clear();
                Error.WriteName();
                Error.WriteText(e.Message);
                Console.ResetColor();
                Console.ReadKey(true);
            }
        }
        void GetPages(List<Entry> Entryes)
        {
            List<List<Entry>> pages = new List<List<Entry>>();
            for (int i = 0, counter = 0; counter < Entryes.Count; i++)
            {
                pages.Add(new List<Entry>());
                for (int j = 0; j < 40 & counter < Entryes.Count; j++, counter++)
                {
                    pages[i].Add(Entryes[counter]);
                }
            }
            Pages = pages;
        }
        public void ReFresh()
        {
            GetEntryList(CurrentPath);
            GetPages(Entryes);
        }
        public void ChangeDirectory(string path)
        {
            try
            {
                GetEntryList(path);
                GetPages(Entryes);
            }
            catch (Exception e)
            {
                Frame Error = new Frame(25, 25, 3, e.Message.Length + 2);
                Error.SetName("Error");
                Error.Coloring(Frame.Colorscheme.Warning);
                Error.SetColor(Frame.ColorsPreset.Selected);
                Error.Show();
                Error.Clear();
                Error.WriteName();
                Error.WriteText(e.Message);
                Console.ReadKey(true);
                Console.ResetColor();
            }
        }

    }
}
