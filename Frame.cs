using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
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
        void TabSelector(ref Dictionary<string, Frame> frames, out int outIndex)
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
}
