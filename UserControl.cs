using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileManager
{
    class UserControl
    {
        List<string> Memory;
        Comands Com;
        Tree MyTree;
        FrontView FW;
        Frame fr;
        public UserControl(FrontView frontView, Tree tree, Frame frame)
        {
            FW = frontView;
            MyTree = tree;
            fr = frame;
            Com = new Comands();
        }
        public void TwoTabs(List<List<Entry>> Pages, List<Entry> Entryes, int CursorLeft, int CursorTop)
        {
            bool Cycle = true;
            int Page = 0;
            int LastPage = Pages.Count - 1;
            int index = 0;
            int PageLeft = 0;
            int PageRight = 0;
            MyTree.CurrentCatalog = null;

            do
            {
                FW.SetColor(FrontView.ColorsPreset.Normal);
                Console.SetCursorPosition(CursorLeft, CursorTop);
                FW.FillRightFrame(Pages[Page]);
                FW.FillLeftFrame(Pages[Page]);

                Console.SetCursorPosition(CursorLeft, CursorTop);
                FW.SetColor(FrontView.ColorsPreset.Selected);
                Console.Write(Entryes[index].Name);

                FW.SetColor(FrontView.ColorsPreset.ContextNormal);
                Console.SetCursorPosition(3, 0);
                Console.Write($"Page = {Page}/{Pages.Count}, PageLeft = {PageLeft}/{Pages.Count}, PageRight = {PageRight}/{Pages.Count}");

                FW.SetColor(FrontView.ColorsPreset.Normal);

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        Console.SetCursorPosition(CursorLeft, CursorTop);
                        Console.Write(Entryes[index].Name);
                        if (CursorTop > FW.FrameTop)
                        {
                            index--;
                            CursorTop--;
                        }
                        Console.SetCursorPosition(CursorLeft, CursorTop);
                        break;
                    case ConsoleKey.DownArrow:
                        Console.SetCursorPosition(CursorLeft, CursorTop);
                        Console.Write(Entryes[index].Name);
                        if (CursorTop - FW.FrameTop < Pages[Page].Count - 1 & index < Entryes.Count - 1)
                        {
                            index++;
                            CursorTop++;
                        }
                        Console.SetCursorPosition(CursorLeft, CursorTop);
                        break;
                    case ConsoleKey.LeftArrow:
                        Console.SetCursorPosition(CursorLeft, CursorTop);
                        Console.Write(Entryes[index].Name);
                        if (CursorLeft == FW.LeftFrameCursorLeft)
                        {
                            CursorLeft = FW.RightFrameCursorLeft;
                            PageLeft = Page;
                            Page = PageRight;
                        }
                        else
                        {
                            CursorLeft = FW.LeftFrameCursorLeft;
                            PageRight = Page;
                            Page = PageLeft;
                        }
                        Console.SetCursorPosition(CursorLeft, CursorTop);
                        break;
                    case ConsoleKey.RightArrow:
                        Console.SetCursorPosition(CursorLeft, CursorTop);
                        Console.Write(Entryes[index].Name);
                        if (CursorLeft == FW.RightFrameCursorLeft)
                        {
                            CursorLeft = FW.LeftFrameCursorLeft;
                            PageRight = Page;
                            Page = PageLeft;
                        }
                        else
                        {
                            CursorLeft = FW.RightFrameCursorLeft;
                            PageLeft = Page;
                            Page = PageRight;
                        }
                        Console.SetCursorPosition(CursorLeft, CursorTop);
                        break;
                    case ConsoleKey.Enter:
                        if (Directory.Exists(Entryes[index].Path))
                        {
                            MyTree.CurrentCatalog = Entryes[index].Path;
                            Entryes = MyTree.GetEntryList(MyTree.CurrentCatalog);
                            index = 0;
                            CursorTop = FW.FrameTop;
                            Page = 0;
                            PageLeft = 0;
                            PageRight = 0;
                            //NewTable = FW.EntryesToArr(Entryes);
                            Pages = FW.ToPages(Entryes);
                            LastPage = Pages.Count - 1;
                            FW.FillRightFrame(Pages[PageRight]);
                            FW.FillLeftFrame(Pages[PageLeft]);
                        }
                        break;
                    case ConsoleKey.Backspace:
                        string[] tempPath = MyTree.CurrentCatalog.Split('\\');
                        if (tempPath.Length > 1)
                        {
                            for (int i = 0; i < tempPath.Length - 1; i++)
                            {
                                if (i == 0)
                                {
                                    MyTree.CurrentCatalog = tempPath[i] + '\\';
                                }
                                else
                                {
                                    MyTree.CurrentCatalog += tempPath[i] + '\\';

                                }
                            }
                            Entryes = MyTree.GetEntryList(MyTree.CurrentCatalog);
                            index = 0;
                            CursorTop = FW.FrameTop;
                            Page = 0;
                            PageLeft = 0;
                            PageRight = 0;
                            //NewTable = FW.EntryesToArr(Entryes);
                            Pages = FW.ToPages(Entryes);
                            LastPage = Pages.Count - 1;
                            FW.FillRightFrame(Pages[PageRight]);
                            FW.FillLeftFrame(Pages[PageLeft]);
                        }
                        break;
                    case ConsoleKey.Applications:
                        SelectorContext(CursorTop, CursorLeft, MyTree.CurrentCatalog);
                        if (CursorLeft == FW.LeftFrameCursorLeft)
                        {
                            FW.FillLeftFrame(Pages[Page]);
                        }
                        else if (CursorLeft == FW.RightFrameCursorLeft)
                        {
                            FW.FillRightFrame(Pages[Page]);
                        }

                        //ShowContext(Com.AllComands, index, CursorLeft);
                        break;



                    case ConsoleKey.Escape:
                        Cycle = false;
                        break;
                    case ConsoleKey.Tab:
                        //Memory.Add(Console.ReadLine());
                        break;


                    case ConsoleKey.PageUp:
                        if (CursorLeft == FW.LeftFrameCursorLeft & Page > 0)
                        {
                            Page--;
                            PageLeft = Page;
                            index = Page * 40;
                            FW.FillLeftFrame(Pages[PageLeft]);
                        }
                        else if (CursorLeft == FW.RightFrameCursorLeft & Page > 0)
                        {
                            Page--;
                            PageRight = Page;
                            index = Page * 40;
                            FW.FillRightFrame(Pages[PageRight]);
                        }
                        CursorTop = FW.FrameTop;
                        break;
                    case ConsoleKey.PageDown:
                        if (CursorLeft == FW.LeftFrameCursorLeft & Page < LastPage)
                        {
                            Page++;
                            PageLeft = Page;
                            index = Page * 40;
                            FW.FillLeftFrame(Pages[PageLeft]);
                        }
                        else if (CursorLeft == FW.RightFrameCursorLeft & Page < LastPage)
                        {
                            Page++;
                            PageRight = Page;
                            index = Page * 40;
                            FW.FillRightFrame(Pages[PageRight]);
                        }
                        CursorTop = FW.FrameTop;
                        break;

                    case ConsoleKey.End:
                        if (CursorLeft == FW.LeftFrameCursorLeft & Page < LastPage)
                        {
                            Page = LastPage;
                            PageLeft = Page;
                            index = Page * 40;
                            FW.FillLeftFrame(Pages[PageLeft]);
                        }
                        else if (CursorLeft == FW.RightFrameCursorLeft & Page < LastPage)
                        {
                            Page = LastPage;
                            PageRight = Page;
                            index = Page * 40;
                            FW.FillRightFrame(Pages[PageRight]);
                        }
                        CursorTop = FW.FrameTop;
                        break;
                    case ConsoleKey.Home:
                        if (CursorLeft == FW.LeftFrameCursorLeft & Page != 0)
                        {
                            Page = 0;
                            PageLeft = Page;
                            index = 0;
                            FW.FillLeftFrame(Pages[PageLeft]);
                        }
                        else if (CursorLeft == FW.RightFrameCursorLeft & Page > 0)
                        {
                            Page = 0;
                            PageRight = Page;
                            index = 0;
                            FW.FillRightFrame(Pages[PageRight]);
                        }
                        CursorTop = FW.FrameTop;
                        break;

                    case ConsoleKey.F1:
                        fr.Message("Presed F1", 'W');
                        FW.SetColor(FrontView.ColorsPreset.Normal);
                        Console.ReadKey();
                        fr.ShowTwo(43, 74, true);
                        break;
                    case ConsoleKey.F2:
                        break;
                    case ConsoleKey.F3:
                        break;
                    case ConsoleKey.F4:
                        break;

                    default:
                        break;
                }
            } while (Cycle);
        }
        public void OneTab(List<List<Entry>> Pages, List<Entry> Entryes, int CursorLeft, int CursorTop)
        {
            bool Cycle = true;
            int Page = 0;
            int index = 0;
            int LastPage = Pages.Count - 1;
            string CurrentCatalog = null;
            do
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Backspace:
                        break;
                    case ConsoleKey.Tab:
                        break;

                    case ConsoleKey.Enter:
                        break;
                    case ConsoleKey.Escape:
                        break;

                    case ConsoleKey.PageUp:
                        if (CursorLeft == FW.LeftFrameCursorLeft & Page > 0)
                        {
                            Page--;
                            index = Page * 40;
                            FW.FillLeftFrame(Pages[Page]);
                        }
                        else if (CursorLeft == FW.RightFrameCursorLeft & Page > 0)
                        {
                            Page--;
                            index = Page * 40;
                            FW.FillRightFrame(Pages[Page]);
                        }
                        CursorTop = FW.FrameTop;
                        break;
                    case ConsoleKey.PageDown:
                        if (CursorLeft == FW.LeftFrameCursorLeft & Page < LastPage)
                        {
                            Page++;
                            index = Page * 40;
                            FW.FillLeftFrame(Pages[Page]);
                        }
                        else if (CursorLeft == FW.RightFrameCursorLeft & Page < LastPage)
                        {
                            Page++;
                            index = Page * 40;
                            FW.FillRightFrame(Pages[Page]);
                        }
                        CursorTop = FW.FrameTop;
                        break;

                    case ConsoleKey.End:
                        break;
                    case ConsoleKey.Home:
                        break;

                    case ConsoleKey.UpArrow:
                        Console.SetCursorPosition(CursorLeft, CursorTop);
                        Console.Write(Entryes[index].Name);
                        if (CursorTop > FW.FrameTop)
                        {
                            index--;
                            CursorTop--;
                        }
                        Console.SetCursorPosition(CursorLeft, CursorTop);
                        break;
                    case ConsoleKey.DownArrow:
                        Console.SetCursorPosition(CursorLeft, CursorTop);
                        Console.Write(Entryes[index].Name);
                        if (CursorTop - FW.FrameTop < Pages[Page].Count - 1 & index < Entryes.Count - 1)
                        {
                            index++;
                            CursorTop++;
                        }
                        Console.SetCursorPosition(CursorLeft, CursorTop);
                        break;

                    case ConsoleKey.LeftArrow:
                        break;
                    case ConsoleKey.RightArrow:
                        break;

                    case ConsoleKey.Applications:
                        SelectorContext(CursorTop, CursorLeft, CurrentCatalog);
                        if (CursorLeft == FW.LeftFrameCursorLeft)
                        {
                            FW.FillLeftFrame(Pages[Page]);
                        }
                        else if (CursorLeft == FW.RightFrameCursorLeft)
                        {
                            FW.FillRightFrame(Pages[Page]);
                        }
                        break;

                    case ConsoleKey.F1:
                        break;
                    case ConsoleKey.F2:
                        break;
                    case ConsoleKey.F3:
                        break;
                    case ConsoleKey.F4:
                        break;

                    default:
                        break;
                }

            } while (Cycle);
        }
        void SelectorContext(int top, int left, string path)
        {
            int index = 0;
            int currentTop;
            if (top > 40 - Com.AllComands.Count)
            {
                currentTop = top - Com.AllComands.Count;
            }
            else
            {
                currentTop = top + 1;
            }
            int currentLeft = left + 10;
            bool cycle = true;
            string[] keys = new string[Com.AllComands.Count];
            Com.AllComands.Keys.CopyTo(keys, 0);

            ShowContext(Com.AllComands, FW, currentTop, currentLeft);
            FW.SetColor(FrontView.ColorsPreset.ContextNormal);
            do
            {
                Console.SetCursorPosition(currentLeft, currentTop);
                FW.SetColor(FrontView.ColorsPreset.ContextSelected);
                Console.Write(keys[index].PadRight(17, ' '));
                Console.SetCursorPosition(currentLeft, currentTop);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Enter:
                        Com.AllComands.GetValueOrDefault(keys[index])(path);
                        cycle = false;
                        break;
                    case ConsoleKey.Escape:
                        cycle = false;
                        break;
                    case ConsoleKey.UpArrow:
                        FW.SetColor(FrontView.ColorsPreset.ContextNormal);
                        Console.SetCursorPosition(currentLeft, currentTop);
                        Console.Write(keys[index].PadRight(17, ' '));
                        if (index > 0)
                        {
                            index--;
                            currentTop--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        FW.SetColor(FrontView.ColorsPreset.ContextNormal);
                        Console.SetCursorPosition(currentLeft, currentTop);
                        Console.Write(keys[index].PadRight(17, ' '));
                        if (index < Com.AllComands.Count - 1)
                        {
                            index++;
                            currentTop++;
                        }
                        break;
                    default:
                        break;
                }
            } while (cycle);
            FW.SetColor(FrontView.ColorsPreset.Normal);
        }
        static public void ShowContext(Dictionary<string, Comands.Comand> Cmds, FrontView FW, int top, int left)
        {

            int currentLeft = left;
            int currentTop;// = top + 2;
            string[] keys = new string[Cmds.Count];
            Cmds.Keys.CopyTo(keys, 0);
            if (top > 40 - Cmds.Count)
            {
                currentTop = top - Cmds.Count;
            }
            else
            {
                currentTop = top;
            }
            for (int i = 0; i < Cmds.Count; i++)
            {
                FW.SetColor(FrontView.ColorsPreset.ContextNormal);
                Console.SetCursorPosition(currentLeft, currentTop + i);
                Console.Write(keys[i].PadRight(17, ' '));
            }
            FW.SetColor(FrontView.ColorsPreset.Normal);
            Console.SetCursorPosition(currentLeft, currentTop);
        }

        void ReadCommand(string path)
        {
            string[] tempLine = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string comand;      // = tempLine[0];
            string target;    // = tempLine[1];
            string attr = null;        // = tempLine[2];
            if (tempLine.Length < 3)
            {
                comand = tempLine[0];
                target = tempLine[1];
            }
            else
            {
                comand = tempLine[0];
                target = tempLine[1];
                attr = tempLine[2];
            }
            switch (comand)
            {
                case "cd":
                    try
                    {
                        string temp = Com.ChangeDirectory(target, path);
                        if (Directory.Exists(temp))
                        {
                            MyTree.CurrentCatalog = temp;
                        }
                    }
                    catch (Exception e)
                    {

                    }
                    break;
                case "del":
                    if (Directory.Exists(path + '\\' + target))
                    {
                        if (Directory.GetDirectories(path + '\\' + target) == Array.Empty<string>())
                        {
                            Com.DeleteDir(path + '\\' + target);
                        }
                        if (!string.IsNullOrEmpty(attr) & attr == "-f")
                        {
                            Com.DeleteCatalog(path + '\\' + target);
                        }
                    }
                    else if (File.Exists(path + '\\' + target))
                    {
                        Com.DeleteFile(path + '\\' + target);
                    }
                    else
                    {
                        Console.Write("Bad path");
                    }
                    break;
                case "rename":
                    if (Directory.Exists(path + '\\' + target) & !string.IsNullOrEmpty(attr))
                    {
                        Com.RenameDir(path + '\\' + target, attr);
                    }
                    else if (File.Exists(path + '\\' + target) & !string.IsNullOrEmpty(attr))
                    {
                        Com.RenameFile(path + '\\' + target, attr);
                    }
                    else if (string.IsNullOrEmpty(attr))
                    {
                        Console.Write("Bad name");
                    }
                    else
                    {
                        Console.Write("Bad path");
                    }
                    break;
                case "copy":
                    if (Directory.Exists(path + '\\' + target) & !string.IsNullOrEmpty(attr))
                    {
                        if (Directory.Exists(attr))
                        {
                            Console.Write("Directory already exist");
                        }
                        else
                        {
                            Com.tempDirPath = path + '\\' + target;
                            Com.CopyDir(path + '\\' + target, attr);
                        }
                    }
                    else if (File.Exists(path + '\\' + target) & !string.IsNullOrEmpty(attr))
                    {
                        if (File.Exists(attr))
                        {
                            Console.Write("File already exist");
                        }
                        else
                        {
                            Com.tempFilePath = path + '\\' + target;
                            Com.CopyFile(path + '\\' + target, attr);
                        }
                    }
                    break;
                default:
                    Console.Write($"Command \"{comand}\" is not supported");
                    break;
            }
        }


        void ConsoleReader(int top, int left, string path)
        {
            StringBuilder sb = new StringBuilder();
            bool cycle = true;
            int index = 0;
            string temp;
            int x;
            char c;
            int cursorTop = top;
            int cursorLeft = left;
            Console.SetCursorPosition(0, cursorTop);
            Console.Write(path + ">");
            do
            {
                Console.SetCursorPosition(cursorLeft, cursorTop);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (Memory.Count > 0 & index > 0)
                        {
                            index--;
                            sb.Clear();
                            sb.Append(Memory[index]);
                            cursorLeft = 0;
                            Console.Write(sb.ToString());
                            cursorLeft = sb.ToString().Length;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (Memory.Count > 0 & index < Memory.Count)
                        {
                            index++;
                            sb.Clear();
                            sb.Append(Memory[index]);
                            cursorLeft = 0;
                            Console.Write(sb.ToString());
                            cursorLeft = sb.ToString().Length;
                        }
                        break;
                    case ConsoleKey.Backspace:
                        sb.Remove(sb.Length - 1, 1);
                        cursorLeft = 0;
                        Console.Write(sb.ToString().PadRight(sb.Length + 1, ' '));
                        cursorLeft = sb.Length;
                        break;
                    case ConsoleKey.Enter:
                        ReadCommand(sb.ToString());
                        break;
                    case ConsoleKey.Escape:
                    case ConsoleKey.Tab:
                        cycle = false;
                        break;
                    default:
                        x = Console.Read();
                        c = Convert.ToChar(x);
                        sb.Append(c);
                        cursorLeft = 0;
                        Console.Write(sb.ToString());
                        cursorLeft = sb.Length;
                        break;
                }
            } while (cycle);
        }

    }
}
