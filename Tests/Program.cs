using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Collections;
using System.Collections.Generic;


namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ResetColor();
            Console.Clear();
            bool Cycle = true;
            int index = 0;
            int page = 0;
            int liner = 0;
            List<string> memory = new List<string>();
            StringBuilder consoleReader = new StringBuilder();

            Tree tree = new Tree();
            Frame frame = new Frame(0, 0, 41, 74);
            Frame warn = new Frame(30, 30, 5, 60);
            Frame question = new Frame(30, 30, 5, 60);
            Frame readConsole = new Frame(30, 30, 5, 60);
            Frame info = new Frame(30, 10, 20, 40);

            Comands comand = new Comands();

            FileInfo fi;
            DirectoryInfo di;

            question.Coloring(Frame.Colorscheme.BIOS);
            readConsole.Coloring(Frame.Colorscheme.BIOS);
            info.Coloring(Frame.Colorscheme.BIOS);
            warn.Coloring(Frame.Colorscheme.Warning);
            info.SetName("Information");

            tree.ChangeDirectory(tree.Roots[0]);

            frame.Coloring(Frame.Colorscheme.Default);
            frame.SetName(tree.Roots[0] + $"Page {page + 1}/{tree.Pages.Count}");
            frame.Show();
            foreach (var item in tree.Pages[page])
            {
                frame.WriteText(item.Name, 0, liner++);
            }
            do
            {
                liner = 0;
                frame.SetName($"╣{tree.Pages[page][index].Parent} | Page {page + 1}/{tree.Pages.Count}╠");
                frame.WriteName();
                Console.CursorVisible = false;
                frame.SetColor(Frame.ColorsPreset.Selected);
                frame.WriteText(tree.Pages[page][index].Name, 0, index);
                var key = Console.ReadKey();
                frame.SetColor(Frame.ColorsPreset.Normal);
                switch (key.Key)
                {
                    case ConsoleKey.Backspace:
                        tree.ChangeDirectory(tree.Pages[0][0].Parent);
                        page = 0;
                        index = 0;
                        frame.Clear();
                        foreach (var item in tree.Pages[page])
                        {
                            frame.WriteText(item.Name, 0, liner++);
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (page == 0 & index == 0)
                        {
                            tree.ChangeDirectory(tree.Pages[page][index].Parent);
                        }
                        else
                        {
                            tree.ChangeDirectory(tree.Pages[page][index].Path + '\\' + tree.Pages[page][index].Name);
                        }
                        page = 0;
                        index = 0;
                        frame.Clear();
                        foreach (var item in tree.Pages[page])
                        {
                            frame.WriteText(item.Name, 0, liner++);
                        }
                        break;
                    case ConsoleKey.Escape:
                        Cycle = false;
                        break;

                    case ConsoleKey.PageUp:
                        if (page > 0)
                        {
                            page--;
                            index = 0;
                        }
                        frame.Clear();
                        foreach (var item in tree.Pages[page])
                        {
                            frame.WriteText(item.Name, 0, liner++);
                        }
                        break;
                    case ConsoleKey.PageDown:
                        if (page < tree.Pages.Count - 1)
                        {
                            page++;
                            index = 0;
                        }
                        frame.Clear();
                        foreach (var item in tree.Pages[page])
                        {
                            frame.WriteText(item.Name, 0, liner++);
                        }
                        break;
                    case ConsoleKey.End:
                        page = tree.Pages.Count - 1;
                        index = tree.Pages[page].Count - 1;
                        frame.Clear();
                        foreach (var item in tree.Pages[page])
                        {
                            frame.WriteText(item.Name, 0, liner++);
                        }
                        break;
                    case ConsoleKey.Home:
                        page = 0;
                        index = 0;
                        frame.Clear();
                        foreach (var item in tree.Pages[page])
                        {
                            frame.WriteText(item.Name, 0, liner++);
                        }
                        break;

                    case ConsoleKey.UpArrow:
                        frame.WriteText(tree.Pages[page][index].Name, 0, index);
                        if (index > 0)
                        {
                            index--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        frame.WriteText(tree.Pages[page][index].Name, 0, index);
                        if (index < tree.Pages[page].Count - 1)
                        {
                            index++;
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        break;
                    case ConsoleKey.RightArrow:
                        break;

                    case ConsoleKey.Insert:
                        question.Show();
                        question.SetName("Creating");
                        question.WriteName();
                        question.WriteText($"Create Directory or File? [D/F] ?");
                        var q = Console.ReadKey(true);
                        string name;
                        switch (q.Key)
                        {
                            case ConsoleKey.D:
                                readConsole.Show();
                                readConsole.SetName("Input Name");
                                readConsole.SetColor(Frame.ColorsPreset.ContextNormal);
                                readConsole.WriteText("".PadRight(readConsole.cols - 2, ' '));
                                readConsole.SetCursorPosition(0, 0);
                                name = Console.ReadLine();
                                if (!Directory.Exists(tree.Pages[page][index].Path + '\\' + name))
                                {
                                    try
                                    {
                                        di = new DirectoryInfo(tree.Pages[page][index].Path + '\\' + name);
                                        di.Create();
                                    }
                                    catch (Exception e)
                                    {
                                        warn.Show();
                                        warn.WriteText(e.Message);
                                        Console.ReadKey(true);
                                    }

                                }
                                else
                                {
                                    warn.Show();
                                    warn.WriteText("Directory already exist.");
                                    Console.ReadKey(true);
                                }
                                break;
                            case ConsoleKey.F:
                                readConsole.Show();
                                readConsole.SetName("Input Name");
                                readConsole.SetColor(Frame.ColorsPreset.ContextNormal);
                                readConsole.WriteText("".PadRight(readConsole.cols - 2, ' '));
                                readConsole.SetCursorPosition(0, 0);
                                name = Console.ReadLine();
                                if (!File.Exists(tree.Pages[page][index].Path + '\\' + name))
                                {
                                    try
                                    {
                                        FileStream fs = File.Create(tree.Pages[page][index].Path + '\\' + name);
                                        fs.Close();
                                    }
                                    catch (Exception e)
                                    {
                                        warn.Show();
                                        warn.WriteText(e.Message);
                                        Console.ReadKey(true);
                                    }
                                }
                                else
                                {
                                    warn.Show();
                                    warn.WriteText("File already exist.");
                                    Console.ReadKey(true);
                                }
                                break;
                            default: break;
                        }
                        tree.ReFresh();
                        page = 0;
                        index = 0;
                        frame.Refresh();
                        foreach (var item in tree.Pages[page])
                        {
                            frame.WriteText(item.Name, 0, liner++);
                        }
                        break;
                    case ConsoleKey.Delete:
                        question.Show();
                        question.SetName("Deleting");
                        question.WriteName();
                        question.WriteText($"Delete {tree.Pages[page][index].Name} Y/N ?");
                        q = Console.ReadKey(true);
                        switch (q.Key)
                        {
                            case ConsoleKey.Y:
                                if (tree.Pages[page][index].type == Entry.Type.Directory)
                                {
                                    try
                                    {
                                        di = (DirectoryInfo)tree.Pages[page][index].GetInfoType();
                                        di.Delete(true);
                                    }
                                    catch (Exception e)
                                    {
                                        warn.Show();
                                        warn.WriteText(e.Message);
                                        Console.ReadKey(true);
                                    }
                                }
                                else if (tree.Pages[page][index].type == Entry.Type.File)
                                {
                                    try
                                    {
                                        fi = (FileInfo)tree.Pages[page][index].GetInfoType();
                                        fi.Delete();
                                    }
                                    catch (Exception e)
                                    {
                                        warn.Show();
                                        warn.WriteText(e.Message);
                                        Console.ReadKey(true);
                                    }
                                }
                                tree.ReFresh();
                                page = 0;
                                index = 0;
                                frame.Refresh();
                                foreach (var item in tree.Pages[page])
                                {
                                    frame.WriteText(item.Name, 0, liner++);
                                }
                                break;
                            case ConsoleKey.N:
                            default:
                                break;
                        }
                        break;
                    case ConsoleKey.Applications:
                        Console.SetCursorPosition(0, 43);
                        Console.WriteLine(tree.Pages[page][index].Path);
                        Console.WriteLine(tree.Pages[page][index].Parent);
                        break;

                    case ConsoleKey.F1:
                        info.Show();
                        info.Clear();
                        break;
                    case ConsoleKey.F2:
                        break;
                    case ConsoleKey.F3:
                        break;
                    case ConsoleKey.F4:
                        break;
                    case ConsoleKey.Tab:
                        bool reader = true;
                        int memIndex = memory.Count - 1;
                        string consoleLine = "";
                        int cursorStartCol = tree.Pages[page][index].Path.Length + 1;
                        Console.SetCursorPosition(0, 43);
                        Console.Write(tree.Pages[page][index].Path + ">".PadRight(50, ' '));
                        Console.SetCursorPosition(0, 44);
                        Console.Write("".PadRight(50, ' '));
                        Console.Write(consoleLine);
                        Console.CursorVisible = true;
                        Console.SetCursorPosition(cursorStartCol, 43);
                        consoleReader.Clear();
                        do
                        {
                            var sub = Console.ReadKey(true);
                            switch (sub.Key)
                            {
                                case ConsoleKey.Tab:
                                case ConsoleKey.Escape:
                                    reader = false;
                                    break;
                                case ConsoleKey.UpArrow:
                                    if (memory.Count > 0)
                                    {
                                        consoleReader.Clear();
                                        if (memIndex < memory.Count - 1)
                                        {
                                            memIndex++;
                                            consoleLine = memory[memIndex].ToString();
                                        }
                                        consoleReader.Append(consoleLine);
                                        Console.SetCursorPosition(cursorStartCol, 43);
                                        Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                                        Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                                    }
                                    break;
                                case ConsoleKey.DownArrow:
                                    if (memory.Count > 0)
                                    {
                                        consoleReader.Clear();
                                        if (memIndex > 0)
                                        {
                                            memIndex--;
                                            consoleLine = memory[memIndex].ToString();
                                        }
                                        consoleReader.Append(consoleLine);
                                        Console.SetCursorPosition(cursorStartCol, 43);
                                        Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                                        Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                                    }
                                    break;
                                case ConsoleKey.Enter:
                                    consoleLine = consoleReader.ToString();
                                    if (!string.IsNullOrEmpty(consoleLine))
                                    {
                                        memory.Add(consoleLine);
                                    }
                                    comand.Reader(consoleLine, ref tree, tree.Pages[page][index], out bool refresh);
                                    if (refresh)
                                    {
                                        tree.ReFresh();
                                        page = 0;
                                        index = 0;
                                        frame.SetName(tree.Roots[0] + $"Page {page + 1}/{tree.Pages.Count}");
                                        frame.Refresh();
                                        foreach (var item in tree.Pages)
                                        {
                                            for (int i = 0; i < item.Count; i++)
                                            {
                                                frame.WriteText(item[i].Name, 0, i);
                                            }
                                        }
                                    }
                                    consoleReader.Clear();
                                    consoleLine = consoleReader.ToString();
                                    Console.SetCursorPosition(cursorStartCol, 43);
                                    Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                                    Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                                    break;
                                case ConsoleKey.Backspace:
                                    if (!string.IsNullOrEmpty(consoleLine))
                                    {
                                        consoleLine = consoleReader.Remove(consoleLine.Length - 1, 1).ToString();
                                    }
                                    Console.SetCursorPosition(cursorStartCol, 43);
                                    Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                                    Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                                    break;
                                default:
                                    if ((sub.KeyChar >= '\u0020' & sub.KeyChar <= '\u007A') | (sub.KeyChar >= '\u0430' & sub.KeyChar <= '\u044F'))
                                    {
                                        consoleReader.Append(sub.KeyChar);
                                        consoleLine = consoleReader.ToString();
                                        Console.SetCursorPosition(cursorStartCol, 43);
                                        Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                                        Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                                    }
                                    break;
                            }
                        } while (reader);
                        break;
                    default:
                        break;
                }
            } while (Cycle);
            Console.ResetColor();
        }
    }
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
    class Frame
    {
        public enum ControlType
        {
            ConsoleReader,
            TabSelector,
            Input,
            OK
        }
        public enum Colorscheme
        {
            Default,
            BIOS,
            Warning
        }
        public enum ColorsPreset
        {
            Normal,
            Selected,
            ContextNormal,
            ContextSelected,
            Standart
        }

        ControlType CType;
        Colorscheme Scheme;
        ConsoleColor NormalBackGround;              // = Black;
        ConsoleColor SelectedBackGround;            // = White;
        ConsoleColor NormalText;                    //  = White;
        ConsoleColor SelectedText;                  //  = Black;
        ConsoleColor ContexMenuNormalBackGround;    //  = Gray;
        ConsoleColor ContexMenuSelectedBackGround;  //  = Yellow;
        ConsoleColor SelectedContext;               //  = Red;
        ConsoleColor NormalContext;                 //  = Black;

        public int StartCol;
        public int StartRow;
        public int rows;
        public int cols;
        public string FrameName = "No Name";
        char LeftUpCorner = '╔';
        char LeftDownCorner = '╚';
        char RightUpCorner = '╗';
        char RightDownCorner = '╝';
        char Liner = '═';
        char Border = '║';

        bool ReFresh;
        int page;
        int index;
        Tree tree;
        Comands comand;
        public Entry entry;
        public delegate void Controller();

        public Frame(int startCol, int startRow, int rws, int cls)
        {
            StartCol = startCol;
            StartRow = startRow;
            rows = rws;
            cols = cls;
            if (Console.WindowWidth < startCol + cls)
            {
                Console.WindowWidth = startCol + cls;
            }
            if (Console.WindowHeight < startRow + rws + 8)
            {
                Console.WindowHeight = startRow + rws + 8;
            }
        }
        public Frame(int startCol, int startRow, int rws, int cls, ControlType ctype, int frameIndex)
        {
            CType = ctype;
            StartCol = startCol;
            StartRow = startRow;
            rows = rws;
            cols = cls;
            if (Console.WindowWidth < startCol + cls)
            {
                Console.WindowWidth = startCol + cls;
            }
            if (Console.WindowHeight < startRow + rws + 8)
            {
                Console.WindowHeight = startRow + rws + 8;
            }
        }
        public void Show()
        {
            SetColor(ColorsPreset.Normal);
            this.WriteName();
            Console.SetCursorPosition(StartCol, StartRow);
            Console.Write($"{LeftUpCorner}".PadRight(cols - 1, Liner) + RightUpCorner);
            if (!string.IsNullOrEmpty(FrameName))
            {
                WriteName();
            }
            for (int i = 1; i < rows; i++)
            {
                Console.SetCursorPosition(StartCol, StartRow + i);
                Console.Write(Border);
                Console.SetCursorPosition(StartCol + cols - 1, StartRow + i);
                Console.Write($"{Border}");
            }
            Console.SetCursorPosition(StartCol, StartRow + rows);
            Console.Write($"{LeftDownCorner}".PadRight(cols - 1, Liner) + RightDownCorner);
            Clear();
        }
        public void Clear()
        {
            SetColor(ColorsPreset.Normal);
            for (int i = 0; i < rows; i++)
            {
                Console.SetCursorPosition(StartCol + 1, StartRow + i);
                Console.Write("".PadRight(cols - 2, ' '));
            }
        }
        public void Refresh()
        {
            Show();
            Clear();
            WriteName();
        }
        public void Coloring(Colorscheme scheme)
        {
            Scheme = scheme;
            switch (scheme)
            {
                case Colorscheme.Default:
                    NormalBackGround = ConsoleColor.Black;
                    SelectedBackGround = ConsoleColor.White;
                    NormalText = ConsoleColor.White;
                    SelectedText = ConsoleColor.Black;

                    ContexMenuNormalBackGround = ConsoleColor.Gray;
                    ContexMenuSelectedBackGround = ConsoleColor.Yellow;
                    SelectedContext = ConsoleColor.Red;
                    NormalContext = ConsoleColor.Black;
                    break;
                case Colorscheme.BIOS:
                    NormalBackGround = ConsoleColor.Blue;
                    SelectedBackGround = ConsoleColor.Red;
                    NormalText = ConsoleColor.Yellow;
                    SelectedText = ConsoleColor.White;
                    ContexMenuNormalBackGround = ConsoleColor.Yellow;
                    ContexMenuSelectedBackGround = ConsoleColor.Red;
                    SelectedContext = ConsoleColor.Yellow;
                    NormalContext = ConsoleColor.Black;
                    break;
                case Colorscheme.Warning:
                    NormalBackGround = ConsoleColor.Red;
                    SelectedBackGround = ConsoleColor.White;
                    NormalText = ConsoleColor.Yellow;
                    SelectedText = ConsoleColor.Red;
                    ContexMenuNormalBackGround = ConsoleColor.Yellow;
                    ContexMenuSelectedBackGround = ConsoleColor.DarkRed;
                    SelectedContext = ConsoleColor.Yellow;
                    NormalContext = ConsoleColor.Black;
                    break;
                default: break;
            }
        }
        public void SetColor(ColorsPreset preset)
        {
            switch (preset)
            {
                case ColorsPreset.Normal:
                    Console.BackgroundColor = NormalBackGround;
                    Console.ForegroundColor = NormalText;
                    break;
                case ColorsPreset.Selected:
                    Console.BackgroundColor = SelectedBackGround;
                    Console.ForegroundColor = SelectedText;
                    break;
                case ColorsPreset.ContextNormal:
                    Console.BackgroundColor = ContexMenuNormalBackGround;
                    Console.ForegroundColor = NormalContext;
                    break;
                case ColorsPreset.ContextSelected:
                    Console.BackgroundColor = ContexMenuSelectedBackGround;
                    Console.ForegroundColor = SelectedContext;
                    break;
                case ColorsPreset.Standart:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    break;
            }

        }
        public void SetCursorPosition(int col, int row)
        {
            Console.SetCursorPosition(col + StartCol + 1, row + StartRow + 1);
        }
        public void SetName(string str)
        {
            if (str.Length < cols - 4)
            {
                FrameName = str;
            }
            else
            {
                string ready = $"{str.Split('\\')[0]}...\\{str.Split('\\')[^1]}";
                FrameName = ready;
            }
        }
        public void Write(string str)
        {
            Console.Write(str);
        }
        public void WriteText(string str, int col = 0, int row = 0)
        {
            Console.SetCursorPosition(col + StartCol + 1, row + StartRow + 1);
            if (str.Length <= cols)
            {
                Console.Write(str.PadRight(cols - 2, ' '));
            }
            else
            {
                int counter = 0;
                int lines = 0;
                do
                {
                    Console.SetCursorPosition(col + StartCol + 1, row + StartRow + 1 + lines);
                    for (int i = 0; i < cols - 2 & counter < str.Length; i++, counter++)
                    {
                        Console.Write(str[counter]);
                    }
                    lines++;
                } while (counter < str.Length & lines < rows);
            }
            Console.SetCursorPosition(col + StartCol + 1, row + StartRow + 1);
        }
        public void WriteName()
        {
            Console.SetCursorPosition(StartCol + 2, StartRow);
            Console.Write(FrameName.PadRight(cols - 3, Liner));
        }

        void ConsoleReader()
        {
            StringBuilder consoleReader = new StringBuilder();
            List<string> memory = new List<string>();
            bool reader = true;
            int memIndex = memory.Count - 1;
            string consoleLine = "";
            int cursorStartCol = tree.Pages[page][index].Path.Length + 1;
            Console.SetCursorPosition(0, 43);
            Console.Write(tree.Pages[page][index].Path + ">".PadRight(50, ' '));
            Console.SetCursorPosition(0, 44);
            Console.Write("".PadRight(50, ' '));
            Console.Write(consoleLine);
            Console.CursorVisible = true;
            Console.SetCursorPosition(cursorStartCol, 43);
            consoleReader.Clear();
            do
            {
                var sub = Console.ReadKey(true);
                switch (sub.Key)
                {
                    case ConsoleKey.Tab:
                    case ConsoleKey.Escape:
                        reader = false;
                        break;
                    case ConsoleKey.UpArrow:
                        if (memory.Count > 0)
                        {
                            consoleReader.Clear();
                            if (memIndex < memory.Count - 1)
                            {
                                memIndex++;
                                consoleLine = memory[memIndex].ToString();
                            }
                            consoleReader.Append(consoleLine);
                            Console.SetCursorPosition(cursorStartCol, 43);
                            Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                            Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (memory.Count > 0)
                        {
                            consoleReader.Clear();
                            if (memIndex > 0)
                            {
                                memIndex--;
                                consoleLine = memory[memIndex].ToString();
                            }
                            consoleReader.Append(consoleLine);
                            Console.SetCursorPosition(cursorStartCol, 43);
                            Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                            Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                        }
                        break;
                    case ConsoleKey.Enter:
                        consoleLine = consoleReader.ToString();
                        if (!string.IsNullOrEmpty(consoleLine))
                        {
                            memory.Add(consoleLine);
                        }
                        comand.Reader(consoleLine, ref tree, tree.Pages[page][index], out ReFresh);
                        consoleReader.Clear();
                        consoleLine = consoleReader.ToString();
                        Console.SetCursorPosition(cursorStartCol, 43);
                        Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                        Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                        break;
                    case ConsoleKey.Backspace:
                        if (!string.IsNullOrEmpty(consoleLine))
                        {
                            consoleLine = consoleReader.Remove(consoleLine.Length - 1, 1).ToString();
                        }
                        Console.SetCursorPosition(cursorStartCol, 43);
                        Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                        Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                        break;
                    default:
                        if ((sub.KeyChar >= '\u0020' & sub.KeyChar <= '\u007A') | (sub.KeyChar >= '\u0430' & sub.KeyChar <= '\u044F'))
                        {
                            consoleReader.Append(sub.KeyChar);
                            consoleLine = consoleReader.ToString();
                            Console.SetCursorPosition(cursorStartCol, 43);
                            Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                            Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                        }
                        break;
                }
            } while (reader);
        }
        void TabSelector(ref Dictionary<string,Frame> frames,out int outIndex)
        {
            outIndex = 0;
            DirectoryInfo di;
            FileInfo fi;
            bool Cycle = true;
            int liner = 0;
            foreach (var item in tree.Pages[page])
            {
                WriteText(item.Name, 0, liner++);
            }
            do
            {
                liner = 0;
                SetName($"╣{tree.Pages[page][index].Parent} | Page {page + 1}/{tree.Pages.Count}╠");
                WriteName();
                Console.CursorVisible = false;
                SetColor(Frame.ColorsPreset.Selected);
                WriteText(tree.Pages[page][index].Name, 0, index);
                var key = Console.ReadKey();
                SetColor(Frame.ColorsPreset.Normal);
                switch (key.Key)
                {
                    case ConsoleKey.Backspace:
                        tree.ChangeDirectory(tree.Pages[0][0].Parent);
                        page = 0;
                        index = 0;
                        Clear();
                        foreach (var item in tree.Pages[page])
                        {
                            WriteText(item.Name, 0, liner++);
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (page == 0 & index == 0)
                        {
                            tree.ChangeDirectory(tree.Pages[page][index].Parent);
                        }
                        else
                        {
                            tree.ChangeDirectory(tree.Pages[page][index].Path + '\\' + tree.Pages[page][index].Name);
                        }
                        page = 0;
                        index = 0;
                        Clear();
                        foreach (var item in tree.Pages[page])
                        {
                            WriteText(item.Name, 0, liner++);
                        }
                        break;
                    case ConsoleKey.Escape:
                        outIndex = 1;
                        Cycle = false;
                        break;

                    case ConsoleKey.PageUp:
                        if (page > 0)
                        {
                            page--;
                            index = 0;
                        }
                        Clear();
                        foreach (var item in tree.Pages[page])
                        {
                            WriteText(item.Name, 0, liner++);
                        }
                        break;
                    case ConsoleKey.PageDown:
                        if (page < tree.Pages.Count - 1)
                        {
                            page++;
                            index = 0;
                        }
                        Clear();
                        foreach (var item in tree.Pages[page])
                        {
                            WriteText(item.Name, 0, liner++);
                        }
                        break;
                    case ConsoleKey.End:
                        page = tree.Pages.Count - 1;
                        index = tree.Pages[page].Count - 1;
                        Clear();
                        foreach (var item in tree.Pages[page])
                        {
                            WriteText(item.Name, 0, liner++);
                        }
                        break;
                    case ConsoleKey.Home:
                        page = 0;
                        index = 0;
                        Clear();
                        foreach (var item in tree.Pages[page])
                        {
                            WriteText(item.Name, 0, liner++);
                        }
                        break;

                    case ConsoleKey.UpArrow:
                        WriteText(tree.Pages[page][index].Name, 0, index);
                        if (index > 0)
                        {
                            index--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        WriteText(tree.Pages[page][index].Name, 0, index);
                        if (index < tree.Pages[page].Count - 1)
                        {
                            index++;
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        break;
                    case ConsoleKey.RightArrow:
                        break;

                    case ConsoleKey.Insert:
                        frames.GetValueOrDefault("question").SetName("Creating");
                        frames.GetValueOrDefault("question").Show();
                        frames.GetValueOrDefault("question").WriteText($"Create Directory or File? [D/F] ?");
                        var q = Console.ReadKey(true);
                        string name;
                        switch (q.Key)
                        {
                            case ConsoleKey.D:
                                frames.GetValueOrDefault("readConsole").SetName("Input Name");
                                frames.GetValueOrDefault("readConsole").Show();
                                frames.GetValueOrDefault("readConsole").SetColor(Frame.ColorsPreset.ContextNormal);
                                frames.GetValueOrDefault("readConsole").WriteText("".PadRight(frames.GetValueOrDefault("readConsole").cols - 2, ' '));
                                frames.GetValueOrDefault("readConsole").SetCursorPosition(0, 0);
                                name = Console.ReadLine();
                                if (!Directory.Exists(tree.Pages[page][index].Path + '\\' + name))
                                {
                                    try
                                    {
                                        di = new DirectoryInfo(tree.Pages[page][index].Path + '\\' + name);
                                        di.Create();
                                    }
                                    catch (Exception e)
                                    {
                                        frames.GetValueOrDefault("warn").Show();
                                        frames.GetValueOrDefault("warn").WriteText(e.Message);
                                        Console.ReadKey(true);
                                    }

                                }
                                else
                                {
                                    frames.GetValueOrDefault("warn").Show();
                                    frames.GetValueOrDefault("warn").WriteText("Directory already exist.");
                                    Console.ReadKey(true);
                                }
                                break;
                            case ConsoleKey.F:
                                frames.GetValueOrDefault("readConsole").SetName("Input Name");
                                frames.GetValueOrDefault("readConsole").Show();
                                frames.GetValueOrDefault("readConsole").SetColor(Frame.ColorsPreset.ContextNormal);
                                frames.GetValueOrDefault("readConsole").WriteText("".PadRight(frames.GetValueOrDefault("readConsole").cols - 2, ' '));
                                frames.GetValueOrDefault("readConsole").SetCursorPosition(0, 0);
                                name = Console.ReadLine();
                                if (!File.Exists(tree.Pages[page][index].Path + '\\' + name))
                                {
                                    try
                                    {
                                        FileStream fs = File.Create(tree.Pages[page][index].Path + '\\' + name);
                                        fs.Close();
                                    }
                                    catch (Exception e)
                                    {
                                        frames.GetValueOrDefault("warn").Show();
                                        frames.GetValueOrDefault("warn").WriteText(e.Message);
                                        Console.ReadKey(true);
                                    }
                                }
                                else
                                {
                                    frames.GetValueOrDefault("warn").Show();
                                    frames.GetValueOrDefault("warn").WriteText("File already exist.");
                                    Console.ReadKey(true);
                                }
                                break;
                            default: break;
                        }
                        tree.ReFresh();
                        page = 0;
                        index = 0;
                        Refresh();
                        foreach (var item in tree.Pages[page])
                        {
                            WriteText(item.Name, 0, liner++);
                        }
                        break;
                    case ConsoleKey.Delete:
                        frames.GetValueOrDefault("question").SetName("Deleting");
                        frames.GetValueOrDefault("question").Show();
                        frames.GetValueOrDefault("question").WriteText($"Delete {tree.Pages[page][index].Name} Y/N ?");
                        q = Console.ReadKey(true);
                        switch (q.Key)
                        {
                            case ConsoleKey.Y:
                                if (tree.Pages[page][index].type == Entry.Type.Directory)
                                {
                                    try
                                    {
                                        di = (DirectoryInfo)tree.Pages[page][index].GetInfoType();
                                        di.Delete(true);
                                    }
                                    catch (Exception e)
                                    {
                                        frames.GetValueOrDefault("warn").Show();
                                        frames.GetValueOrDefault("warn").WriteText(e.Message);
                                        Console.ReadKey(true);
                                    }
                                }
                                else if (tree.Pages[page][index].type == Entry.Type.File)
                                {
                                    try
                                    {
                                        fi = (FileInfo)tree.Pages[page][index].GetInfoType();
                                        fi.Delete();
                                    }
                                    catch (Exception e)
                                    {
                                        frames.GetValueOrDefault("warn").Show();
                                        frames.GetValueOrDefault("warn").WriteText(e.Message);
                                        Console.ReadKey(true);
                                    }
                                }
                                tree.ReFresh();
                                page = 0;
                                index = 0;
                                Refresh();
                                foreach (var item in tree.Pages[page])
                                {
                                    WriteText(item.Name, 0, liner++);
                                }
                                break;
                            case ConsoleKey.N:
                            default:
                                break;
                        }
                        break;
                    case ConsoleKey.Applications:
                        Console.SetCursorPosition(0, 43);
                        Console.WriteLine(tree.Pages[page][index].Path);
                        Console.WriteLine(tree.Pages[page][index].Parent);
                        break;

                    case ConsoleKey.F1:
                        frames.GetValueOrDefault("info").Show();
                        break;
                    case ConsoleKey.F2:
                        break;
                    case ConsoleKey.F3:
                        break;
                    case ConsoleKey.F4:
                        break;
                    case ConsoleKey.Tab:
                        outIndex = 2;
                        Cycle = false;
                        break;
                    default:
                        break;
                }
            } while (Cycle);
        }
        void Input()
        {

        }
        void OK()
        {

        }
    }
    class Comands
    {
        public Comands()
        {

        }
        public void Reader(string str, ref Tree tree, Entry entry, out bool reFreshFrame)
        {
            string[] inLine = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            reFreshFrame = false;
            string comand = null;
            string path = null;
            string attr = null;
            int charCount = 0;
            for (int i = 0; i < str.Length; i++, charCount++)
            {
                if (!char.IsWhiteSpace(str[i]))
                {
                    comand += str[i];
                }
                else
                {
                    break;
                }
            }
            for (int i = ++charCount; i < str.Length; i++, charCount++)
            {
                if (str[i] != '-')
                {
                    path += str[i];
                }
                else
                {
                    break;
                }
            }
            for (int i = ++charCount; i < str.Length; i++, charCount++)
            {
                if (str[i] == '-')
                {
                    continue;
                }
                else if (!char.IsWhiteSpace(str[i]))
                {
                    attr += str[i];
                }
                else
                {
                    break;
                }
            }
            path.Trim();
            switch (comand)
            {
                case "CD":
                case "cd":
                    if (path == "\\")
                    {
                        tree.ChangeDirectory(tree.Pages[0][0].Parent);
                    }
                    else if (Directory.Exists(path + '\\'))
                    {
                        tree.ChangeDirectory(path + '\\');
                        reFreshFrame = true;
                    }
                    else if (Directory.Exists(entry.Path + path + '\\'))
                    {
                        tree.ChangeDirectory(entry.Path + path + '\\');
                        reFreshFrame = true;
                    }
                    else
                    {
                        Console.Write("Bad Path!");
                    }
                    break;
                case "del":
                case "DEL":
                case "Del":
                    if (Directory.Exists(path))
                    {

                        try
                        {
                            DirectoryInfo dif = new DirectoryInfo(entry.Path + path);
                            dif.Delete(true);
                            reFreshFrame = true;
                        }
                        catch (Exception e)
                        {
                            Frame Error = new Frame(30, 30, 5, e.Message.Length + 2);
                            Error.SetName("Error");
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
                    else if (File.Exists(path))
                    {
                        try
                        {
                            FileInfo fi = new FileInfo(entry.Path + path);
                            fi.Delete();
                        }
                        catch (Exception e)
                        {
                            Frame Error = new Frame(30, 30, 5, e.Message.Length + 2);
                            Error.SetName("Error");
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
                    break;
                case "new":
                case "New":
                case "NEW":
                    if (attr == "d" | attr == "D")
                    {
                        try
                        {
                            DirectoryInfo dif = new DirectoryInfo(tree.CurrentPath + path + '\\');
                            dif.Create();
                            reFreshFrame = true;
                        }
                        catch (Exception e)
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
                    else if (attr == "f" | attr == "F")
                    {
                        try
                        {
                            FileInfo fi = new FileInfo(tree.CurrentPath + path);
                            fi.Create();
                            reFreshFrame = true;
                        }
                        catch (Exception e)
                        {
                            Frame Error = new Frame(30, 30, 5, e.Message.Length + 2);
                            Error.SetName("Error");
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
                    break;
                case "move":
                case "Move":
                case "MOVE":
                    if (Directory.Exists(attr) & (Directory.Exists(path) | File.Exists(path)))
                    {
                        try
                        {
                            Directory.Move(path, attr);
                        }
                        catch (Exception e)
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
                    break;
                case "copy":
                case "COPY":
                case "Copy":
                    if (File.Exists(entry.Path + path) & !File.Exists(attr))
                    {
                        try
                        {
                            File.Copy(entry.Path + path, attr);
                        }
                        catch (Exception e)
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
                    else if (File.Exists(attr))
                    {
                        string errorStr = $"File {attr} already exist";
                        Frame Error = new Frame(30, 30, 5, errorStr.Length + 2);
                        Error.SetName("File exist");
                        Error.Coloring(Frame.Colorscheme.Warning);
                        Error.SetColor(Frame.ColorsPreset.Normal);
                        Error.Show();
                        Error.Clear();
                        Error.WriteName();
                        Error.WriteText(errorStr);
                        Console.ResetColor();
                        Console.ReadKey(true);
                    }
                    else if (!File.Exists(entry.Path + path))
                    {
                        string errorStr = $"File {path} not exist";
                        Frame Error = new Frame(30, 30, 5, errorStr.Length + 2);
                        Error.SetName("File not exist");
                        Error.Coloring(Frame.Colorscheme.Warning);
                        Error.SetColor(Frame.ColorsPreset.Normal);
                        Error.Show();
                        Error.Clear();
                        Error.WriteName();
                        Error.WriteText(errorStr);
                        Console.ResetColor();
                        Console.ReadKey(true);
                    }
                    break;
                default:
                    Console.Write("Bad Comand!");
                    break;
            }

        }
        public void Delete(Entry entry)
        {
            Frame warn = new Frame(30, 30, 5, 60);
            warn.Coloring(Frame.Colorscheme.Warning);

            if (entry.type == Entry.Type.Directory)
            {
                try
                {
                    DirectoryInfo di = (DirectoryInfo)entry.GetInfoType();
                    di.Delete(true);
                }
                catch (Exception e)
                {
                    warn.Show();
                    warn.Clear();
                    warn.WriteText(e.Message);
                    Console.ReadKey(true);
                }
            }
            else if (entry.type == Entry.Type.File)
            {
                try
                {
                    FileInfo fi = (FileInfo)entry.GetInfoType();
                    fi.Delete();
                }
                catch (Exception e)
                {
                    warn.Show();
                    warn.Clear();
                    warn.WriteText(e.Message);
                    Console.ReadKey(true);
                }
            }
        }
        public void Create(Entry entry, char type)
        {
            Frame warn = new Frame(30, 30, 5, 60);
            Frame readConsole = new Frame(30, 30, 5, 60);
            warn.Coloring(Frame.Colorscheme.Warning);
            readConsole.Coloring(Frame.Colorscheme.BIOS);
            string name;
            switch (type)
            {
                case 'D':
                case 'd':
                    readConsole.Show();
                    readConsole.Clear();
                    readConsole.SetName("Input Name");
                    readConsole.SetColor(Frame.ColorsPreset.ContextNormal);
                    readConsole.WriteText("".PadRight(readConsole.cols - 2, ' '));
                    readConsole.SetCursorPosition(0, 0);
                    name = Console.ReadLine();
                    if (!Directory.Exists(entry.Path + '\\' + name))
                    {
                        try
                        {
                            DirectoryInfo di = new DirectoryInfo(entry.Path + '\\' + name);
                            di.Create();
                        }
                        catch (Exception e)
                        {
                            warn.Show();
                            warn.Clear();
                            warn.WriteText(e.Message);
                            Console.ReadKey(true);
                        }

                    }
                    else
                    {
                        warn.Show();
                        warn.Clear();
                        warn.WriteText("Directory already exist.");
                        Console.ReadKey(true);
                    }
                    break;
                case 'F':
                case 'f':
                    readConsole.Show();
                    readConsole.Clear();
                    readConsole.SetName("Input Name");
                    readConsole.SetColor(Frame.ColorsPreset.ContextNormal);
                    readConsole.WriteText("".PadRight(readConsole.cols - 2, ' '));
                    readConsole.SetCursorPosition(0, 0);
                    name = Console.ReadLine();
                    if (!File.Exists(entry.Path + '\\' + name))
                    {
                        try
                        {
                            FileStream fs = File.Create(entry.Path + '\\' + name);
                            fs.Close();
                        }
                        catch (Exception e)
                        {
                            warn.Show();
                            warn.Clear();
                            warn.WriteText(e.Message);
                            Console.ReadKey(true);
                        }
                    }
                    else
                    {
                        warn.Show();
                        warn.Clear();
                        warn.WriteText("File already exist.");
                        Console.ReadKey(true);
                    }
                    break;
                default: break;
            }
        }
        public void Move(Entry entry, string destinationPath)
        {
            Frame warn = new Frame(30, 30, 5, 60);
            warn.Coloring(Frame.Colorscheme.Warning);
            if (entry.type == Entry.Type.Directory)
            {
                try
                {
                    DirectoryInfo di = (DirectoryInfo)entry.GetInfoType();
                    di.MoveTo(destinationPath);
                }
                catch (Exception e)
                {
                    warn.Show();
                    warn.Clear();
                    warn.WriteText(e.Message);
                    Console.ReadKey(true);
                }
            }
            else if (entry.type == Entry.Type.File)
            {
                try
                {
                    File.Move(entry.Path, destinationPath);
                }
                catch (Exception e)
                {
                    warn.Show();
                    warn.Clear();
                    warn.WriteText(e.Message);
                    Console.ReadKey(true);
                }
            }
        }
        public void CopyDir(DirectoryInfo source, DirectoryInfo target)
        {
            Frame warn = new Frame(30, 30, 5, 60);
            warn.Coloring(Frame.Colorscheme.Warning);

            if (source.FullName == target.FullName)
            {
                warn.Show();
                warn.Clear();
                warn.WriteText("Пути совпадают");
                Console.ReadKey(true);
            }
            else
            {
                if (Directory.Exists(target.FullName) == false)
                {
                    Directory.CreateDirectory(target.FullName);
                }
                foreach (FileInfo fi in source.GetFiles())
                {
                    try
                    {
                        fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);

                    }
                    catch (Exception e)
                    {
                        warn.Show();
                        warn.Clear();
                        warn.WriteText(e.Message);
                        Console.ReadKey(true);
                    }
                }
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                    CopyDir(diSourceSubDir, nextTargetSubDir);
                }
            }
        }
    }
}
