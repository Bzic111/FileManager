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
        Frame fr;
        Frame cr;
        public UserControl(Tree tree, Frame frame)
        {
            MyTree = tree;
            fr = frame;
            Com = new Comands();
        }

        //public void TwoTabs(List<List<Entry>> Pages, List<Entry> Entryes, int CursorLeft, int CursorTop)
        //{
        //    bool Cycle = true;
        //    int Page = 0;
        //    int LastPage = Pages.Count - 1;
        //    int index = 0;
        //    int PageLeft = 0;
        //    int PageRight = 0;
        //    MyTree.CurrentCatalog = null;

        //    do
        //    {
        //        FW.SetColor(FrontView.ColorsPreset.Normal);
        //        Console.SetCursorPosition(CursorLeft, CursorTop);
        //        FW.FillRightFrame(Pages[Page]);
        //        FW.FillLeftFrame(Pages[Page]);

        //        Console.SetCursorPosition(CursorLeft, CursorTop);
        //        FW.SetColor(FrontView.ColorsPreset.Selected);
        //        Console.Write(Entryes[index].Name);

        //        FW.SetColor(FrontView.ColorsPreset.ContextNormal);
        //        Console.SetCursorPosition(3, 0);
        //        Console.Write($"Page = {Page}/{Pages.Count}, PageLeft = {PageLeft}/{Pages.Count}, PageRight = {PageRight}/{Pages.Count}");

        //        FW.SetColor(FrontView.ColorsPreset.Normal);

        //        switch (Console.ReadKey().Key)
        //        {
        //            case ConsoleKey.UpArrow:
        //                Console.SetCursorPosition(CursorLeft, CursorTop);
        //                Console.Write(Entryes[index].Name);
        //                if (CursorTop > FW.FrameTop)
        //                {
        //                    index--;
        //                    CursorTop--;
        //                }
        //                Console.SetCursorPosition(CursorLeft, CursorTop);
        //                break;
        //            case ConsoleKey.DownArrow:
        //                Console.SetCursorPosition(CursorLeft, CursorTop);
        //                Console.Write(Entryes[index].Name);
        //                if (CursorTop - FW.FrameTop < Pages[Page].Count - 1 & index < Entryes.Count - 1)
        //                {
        //                    index++;
        //                    CursorTop++;
        //                }
        //                Console.SetCursorPosition(CursorLeft, CursorTop);
        //                break;
        //            case ConsoleKey.LeftArrow:
        //                Console.SetCursorPosition(CursorLeft, CursorTop);
        //                Console.Write(Entryes[index].Name);
        //                if (CursorLeft == FW.LeftFrameCursorLeft)
        //                {
        //                    CursorLeft = FW.RightFrameCursorLeft;
        //                    PageLeft = Page;
        //                    Page = PageRight;
        //                }
        //                else
        //                {
        //                    CursorLeft = FW.LeftFrameCursorLeft;
        //                    PageRight = Page;
        //                    Page = PageLeft;
        //                }
        //                Console.SetCursorPosition(CursorLeft, CursorTop);
        //                break;
        //            case ConsoleKey.RightArrow:
        //                Console.SetCursorPosition(CursorLeft, CursorTop);
        //                Console.Write(Entryes[index].Name);
        //                if (CursorLeft == FW.RightFrameCursorLeft)
        //                {
        //                    CursorLeft = FW.LeftFrameCursorLeft;
        //                    PageRight = Page;
        //                    Page = PageLeft;
        //                }
        //                else
        //                {
        //                    CursorLeft = FW.RightFrameCursorLeft;
        //                    PageLeft = Page;
        //                    Page = PageRight;
        //                }
        //                Console.SetCursorPosition(CursorLeft, CursorTop);
        //                break;
        //            case ConsoleKey.Enter:
        //                if (Directory.Exists(Entryes[index].Path))
        //                {
        //                    MyTree.CurrentCatalog = Entryes[index].Path;
        //                    Entryes = MyTree.GetEntryList(MyTree.CurrentCatalog);
        //                    index = 0;
        //                    CursorTop = FW.FrameTop;
        //                    Page = 0;
        //                    PageLeft = 0;
        //                    PageRight = 0;
        //                    //NewTable = FW.EntryesToArr(Entryes);
        //                    Pages = FW.ToPages(Entryes);
        //                    LastPage = Pages.Count - 1;
        //                    FW.FillRightFrame(Pages[PageRight]);
        //                    FW.FillLeftFrame(Pages[PageLeft]);
        //                }
        //                break;
        //            case ConsoleKey.Backspace:
        //                string[] tempPath = MyTree.CurrentCatalog.Split('\\');
        //                if (tempPath.Length > 1)
        //                {
        //                    for (int i = 0; i < tempPath.Length - 1; i++)
        //                    {
        //                        if (i == 0)
        //                        {
        //                            MyTree.CurrentCatalog = tempPath[i] + '\\';
        //                        }
        //                        else
        //                        {
        //                            MyTree.CurrentCatalog += tempPath[i] + '\\';

        //                        }
        //                    }
        //                    Entryes = MyTree.GetEntryList(MyTree.CurrentCatalog);
        //                    index = 0;
        //                    CursorTop = FW.FrameTop;
        //                    Page = 0;
        //                    PageLeft = 0;
        //                    PageRight = 0;
        //                    //NewTable = FW.EntryesToArr(Entryes);
        //                    Pages = FW.ToPages(Entryes);
        //                    LastPage = Pages.Count - 1;
        //                    FW.FillRightFrame(Pages[PageRight]);
        //                    FW.FillLeftFrame(Pages[PageLeft]);
        //                }
        //                break;
        //            case ConsoleKey.Applications:
        //                SelectorContext(CursorTop, CursorLeft, MyTree.CurrentCatalog);
        //                if (CursorLeft == FW.LeftFrameCursorLeft)
        //                {
        //                    FW.FillLeftFrame(Pages[Page]);
        //                }
        //                else if (CursorLeft == FW.RightFrameCursorLeft)
        //                {
        //                    FW.FillRightFrame(Pages[Page]);
        //                }

        //                //ShowContext(Com.AllComands, index, CursorLeft);
        //                break;



        //            case ConsoleKey.Escape:
        //                Cycle = false;
        //                break;
        //            case ConsoleKey.Tab:
        //                //Memory.Add(Console.ReadLine());
        //                break;


        //            case ConsoleKey.PageUp:
        //                if (CursorLeft == FW.LeftFrameCursorLeft & Page > 0)
        //                {
        //                    Page--;
        //                    PageLeft = Page;
        //                    index = Page * 40;
        //                    FW.FillLeftFrame(Pages[PageLeft]);
        //                }
        //                else if (CursorLeft == FW.RightFrameCursorLeft & Page > 0)
        //                {
        //                    Page--;
        //                    PageRight = Page;
        //                    index = Page * 40;
        //                    FW.FillRightFrame(Pages[PageRight]);
        //                }
        //                CursorTop = FW.FrameTop;
        //                break;
        //            case ConsoleKey.PageDown:
        //                if (CursorLeft == FW.LeftFrameCursorLeft & Page < LastPage)
        //                {
        //                    Page++;
        //                    PageLeft = Page;
        //                    index = Page * 40;
        //                    FW.FillLeftFrame(Pages[PageLeft]);
        //                }
        //                else if (CursorLeft == FW.RightFrameCursorLeft & Page < LastPage)
        //                {
        //                    Page++;
        //                    PageRight = Page;
        //                    index = Page * 40;
        //                    FW.FillRightFrame(Pages[PageRight]);
        //                }
        //                CursorTop = FW.FrameTop;
        //                break;

        //            case ConsoleKey.End:
        //                if (CursorLeft == FW.LeftFrameCursorLeft & Page < LastPage)
        //                {
        //                    Page = LastPage;
        //                    PageLeft = Page;
        //                    index = Page * 40;
        //                    FW.FillLeftFrame(Pages[PageLeft]);
        //                }
        //                else if (CursorLeft == FW.RightFrameCursorLeft & Page < LastPage)
        //                {
        //                    Page = LastPage;
        //                    PageRight = Page;
        //                    index = Page * 40;
        //                    FW.FillRightFrame(Pages[PageRight]);
        //                }
        //                CursorTop = FW.FrameTop;
        //                break;
        //            case ConsoleKey.Home:
        //                if (CursorLeft == FW.LeftFrameCursorLeft & Page != 0)
        //                {
        //                    Page = 0;
        //                    PageLeft = Page;
        //                    index = 0;
        //                    FW.FillLeftFrame(Pages[PageLeft]);
        //                }
        //                else if (CursorLeft == FW.RightFrameCursorLeft & Page > 0)
        //                {
        //                    Page = 0;
        //                    PageRight = Page;
        //                    index = 0;
        //                    FW.FillRightFrame(Pages[PageRight]);
        //                }
        //                CursorTop = FW.FrameTop;
        //                break;

        //            case ConsoleKey.F1:
        //                fr.Message("Presed F1", 'W');
        //                FW.SetColor(FrontView.ColorsPreset.Normal);
        //                Console.ReadKey();
        //                fr.ShowTwo(43, 74, true);
        //                break;
        //            case ConsoleKey.F2:
        //                break;
        //            case ConsoleKey.F3:
        //                break;
        //            case ConsoleKey.F4:
        //                break;

        //            default:
        //                break;
        //        }
        //    } while (Cycle);
        //}
        public void OneTab(List<List<Entry>> Pages, out Entry entr)
        {
            entr = Pages[0][0];
            bool Cycle = true;
            int Page = 0;
            int index = 0;
            int LastPage = Pages.Count - 1;
            string CurrentCatalog = Pages[0][0].Parent;

            List<Entry> tempEntryes;
            fr.SetColor(Frame.ColorsPreset.Normal);
            int lineCount = 0;
            foreach (var item in Pages[Page])
            {
                fr.WriteText(item.Name, 0, lineCount++);
            }
            MyTree.CurrentCatalog = null;

            do
            {
                fr.SetColor(Frame.ColorsPreset.Normal);
                fr.WriteText(Pages[Page][index].Name, 0, index);

                foreach (var item in Pages[Page])
                {
                    fr.WriteText(item.Name, 0, index);
                }

                fr.SetColor(Frame.ColorsPreset.Selected);
                fr.WriteText(Pages[Page][index].Name, 0, index);

                fr.SetColor(Frame.ColorsPreset.ContextNormal);
                Console.SetCursorPosition(3, 0);
                Console.Write($"Page = {Page}/{Pages.Count}");

                fr.SetColor(Frame.ColorsPreset.Normal);

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Backspace:
                        break;
                    case ConsoleKey.Tab:
                        entr = Pages[Page][index];
                        
                        break;

                    case ConsoleKey.Enter:
                        if (Pages[Page][index].type == Entry.Type.Directory)
                        {
                            List<Entry> InEntry = new List<Entry>();
                            List<List<Entry>> InPages = new List<List<Entry>>();
                            InEntry = MyTree.GetEntryList(Pages[Page][index].Path + '\\' + Pages[Page][index].Name);
                            InPages = MyTree.ToPages(InEntry);
                            UserControl uc = new UserControl(MyTree, fr);
                            uc.OneTab(InPages,out entr);
                        }
                        fr.Clear();
                        lineCount = 0;
                        foreach (var item in Pages[Page])
                        {
                            fr.WriteText(item.Name, 0, lineCount++);
                        }
                        break;
                    case ConsoleKey.Escape:
                        fr.Clear();
                        Cycle = false;
                        break;

                    case ConsoleKey.PageUp:
                        if (Page > 0)
                        {
                            Page--;
                            index = 0;
                            fr.Clear();
                            lineCount = 0;
                            foreach (var item in Pages[Page])
                            {
                                fr.WriteText(item.Name, 0, lineCount++);
                            }
                        }
                        break;
                    case ConsoleKey.PageDown:
                        if (Page < LastPage)
                        {
                            Page++;
                            index = 0;
                            fr.Clear();
                            lineCount = 0;
                            foreach (var item in Pages[Page])
                            {
                                fr.WriteText(item.Name, 0, lineCount++);
                            }
                        }
                        break;
                    case ConsoleKey.Home:
                        if (Page > 0)
                        {
                            Page = 0;
                            index = 0;
                            lineCount = 0;
                            foreach (var item in Pages[Page])
                            {
                                fr.WriteText(item.Name, 0, lineCount++);
                            }
                        }
                        break;
                    case ConsoleKey.End:
                        if (Page < LastPage)
                        {
                            Page = LastPage;
                            index = 0;
                            foreach (var item in Pages[Page])
                            {
                                fr.WriteText(item.Name, 0, index);
                            }
                        }
                        break;


                    case ConsoleKey.UpArrow:
                        fr.WriteText(Pages[Page][index].Name, 0, index);
                        if (index > fr.StartRow)
                        {
                            index--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        fr.WriteText(Pages[Page][index].Name, 0, index);
                        if (index < Pages[Page].Count - 1)
                        {
                            index++;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        break;
                    case ConsoleKey.RightArrow:
                        break;

                    case ConsoleKey.Applications:
                        //SelectorContext(CursorTop, CursorLeft, CurrentCatalog);                        
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

        //void SelectorContext(int top, int left, string path)
        //{
        //    int index = 0;
        //    int currentTop;
        //    if (top > 40 - Com.AllComands.Count)
        //    {
        //        currentTop = top - Com.AllComands.Count;
        //    }
        //    else
        //    {
        //        currentTop = top + 1;
        //    }
        //    int currentLeft = left + 10;
        //    bool cycle = true;
        //    string[] keys = new string[Com.AllComands.Count];
        //    Com.AllComands.Keys.CopyTo(keys, 0);

        //    ShowContext(Com.AllComands, FW, currentTop, currentLeft);
        //    FW.SetColor(FrontView.ColorsPreset.ContextNormal);
        //    do
        //    {
        //        Console.SetCursorPosition(currentLeft, currentTop);
        //        FW.SetColor(FrontView.ColorsPreset.ContextSelected);
        //        Console.Write(keys[index].PadRight(17, ' '));
        //        Console.SetCursorPosition(currentLeft, currentTop);
        //        switch (Console.ReadKey().Key)
        //        {
        //            case ConsoleKey.Enter:
        //                Com.AllComands.GetValueOrDefault(keys[index])(path);
        //                cycle = false;
        //                break;
        //            case ConsoleKey.Escape:
        //                cycle = false;
        //                break;
        //            case ConsoleKey.UpArrow:
        //                FW.SetColor(FrontView.ColorsPreset.ContextNormal);
        //                Console.SetCursorPosition(currentLeft, currentTop);
        //                Console.Write(keys[index].PadRight(17, ' '));
        //                if (index > 0)
        //                {
        //                    index--;
        //                    currentTop--;
        //                }
        //                break;
        //            case ConsoleKey.DownArrow:
        //                FW.SetColor(FrontView.ColorsPreset.ContextNormal);
        //                Console.SetCursorPosition(currentLeft, currentTop);
        //                Console.Write(keys[index].PadRight(17, ' '));
        //                if (index < Com.AllComands.Count - 1)
        //                {
        //                    index++;
        //                    currentTop++;
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    } while (cycle);
        //    FW.SetColor(FrontView.ColorsPreset.Normal);
        //}
        public void ShowContext(Dictionary<string, Comands.Comand> Cmds, FrontView FW, int top, int left)
        {

            int currentLeft = left;
            int currentTop; // = top + 2;
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



        void RootSelector()
        {
            bool Cycle = true;
            int rootIndex = 0;
            do
            {
                fr.SetColor(Frame.ColorsPreset.Selected);
                fr.WriteText(MyTree.Roots[rootIndex], 0, rootIndex);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Enter:
                        List<Entry> InEntry = new List<Entry>();
                        List<List<Entry>> InPages = new List<List<Entry>>();
                        InEntry = MyTree.GetEntryList(MyTree.Roots[rootIndex]);
                        InPages = MyTree.ToPages(InEntry);
                        OneTab(InPages,out _);
                        int count = 0;
                        fr.Clear();
                        foreach (var item in MyTree.Roots)
                        {
                            fr.SetColor(Frame.ColorsPreset.Normal);
                            fr.WriteText(item, 0, count++);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        fr.SetColor(Frame.ColorsPreset.Normal);
                        fr.WriteText(MyTree.Roots[rootIndex], 0, rootIndex);
                        if (rootIndex > 0)
                        {
                            rootIndex--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        fr.SetColor(Frame.ColorsPreset.Normal);
                        fr.WriteText(MyTree.Roots[rootIndex], 0, rootIndex);
                        if (rootIndex < MyTree.Roots.Count - 1)
                        {
                            rootIndex++;
                        }
                        break;
                    case ConsoleKey.Escape:
                        Cycle = false;
                        break;
                    case ConsoleKey.Applications:
                        break;
                    default:
                        break;
                }
            } while (Cycle);
        }
        void ConsoleReader(int top, int left, string path)
        {
            StringBuilder sb = new StringBuilder();
            bool cycle = true;
            int index = 0;
            string temp;

            int cursorTop = top;
            int cursorLeft = left;
            Console.SetCursorPosition(0, cursorTop);
            Console.Write(path + ">");
            do
            {
                Console.SetCursorPosition(cursorLeft, cursorTop);
                var key = Console.ReadKey();
                switch (key.Key)
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
                        //ReadCommand(sb.ToString());
                        break;
                    case ConsoleKey.Escape:
                    case ConsoleKey.Tab:
                        cycle = false;
                        break;
                    default:
                        sb.Append(key);
                        cursorLeft = 0;
                        Console.Write(sb.ToString());
                        cursorLeft = sb.Length;
                        break;
                }
            } while (cycle);
        }

    }
}
