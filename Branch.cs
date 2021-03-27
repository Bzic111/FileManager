using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    class Branch
    {
        int StartCol;
        int StartRow;
        int Scale;
        public int rows;
        public int cols;
        public string Name;
        public List<Entry> Lines;
        public List<int> InsertIndex;
        List<List<Entry>> Pages;
        public int PageLength = 40;
        public Branch()
        {

        }
        public Branch(int startCol, int startRow, int scale, List<Entry> entries)
        {
            InsertIndex = new List<int>();
            StartCol = startCol;
            StartRow = startRow;
            Scale = scale;
            cols = Console.WindowWidth - scale - startCol;
            Name = entries[0].Path.Split('\\')[^2] + "\\";
            Lines = entries;
        }
        void ToPages()
        {
            List<List<Entry>> Pages = new List<List<Entry>>();
            for (int i = 0, counter = 0; counter < Lines.Count; i++)
            {
                Pages.Add(new List<Entry>());
                for (int j = 0; j < 40 & counter < Lines.Count; j++, counter++)
                {
                    Pages[i].Add(Lines[counter]);
                }
            }
        }
        public void InsertLines(List<Entry> entryes, int index)
        {
            InsertIndex.Add(index);
            for (int i = 0; i < entryes.Count; i++)
            {
                Lines.Insert(index + i, entryes[i]);
            }
        }
        public void Show(int Page)
        {
            Console.SetCursorPosition(StartCol + Scale, StartRow);
            Console.WriteLine(Name);
            for (int i = 0; i < Lines.Count; i++)
            {
                if (Name.Length < cols)
                {
                    if (Lines[i].type == Entry.Type.Directory)
                    {
                        Console.WriteLine("╠═" + Lines[i].Name);
                    }
                    else
                    {
                        Console.WriteLine("╟─" + Lines[i].Name);
                    }
                }
            }
        }
    }
}
