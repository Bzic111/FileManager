using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;

namespace FileManager
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
            Frame tabs = new Frame(0, 0, 3, 150);

            Dictionary<string, Frame> FrameCollection = new Dictionary<string, Frame>();
            FrameCollection.Add("warning", new Frame(30, 30, 5, 60));
            FrameCollection.Add("question", new Frame(30, 30, 5, 60));
            FrameCollection.Add("info", new Frame(30, 10, 20, 40));
            FrameCollection.Add("tabs", new Frame(0, 0, 3, 150));
            FrameCollection.Add("read", new Frame(30, 30, 5, 60));
            FrameCollection.Add("FrameA", new Frame(0, 0, 41, 74));



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
}
