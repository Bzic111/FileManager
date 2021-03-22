using System;
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
            bool Cycle = true;
            int index = 0;
            int Page = 0;
            int LastPage = 0;
            int PageLeft = 0;
            int PageRight = 0;

            string StepInCatalog = null;

            Tree MyTree = new Tree();
            Comands Com = new Comands();
            Frame fr = new Frame(42, 74);


            MyTree.CurrentCatalog = MyTree.Roots[0];

            List<Entry> Entryes = MyTree.GetEntryList(MyTree.CurrentCatalog);

            FrontView FW = new FrontView(42, 74, Entryes);
            UserControl control = new UserControl(FW, MyTree, fr);


            List<List<Entry>> Pages = FW.ToPages(Entryes);

            LastPage = Pages.Count - 1;

            fr.ShowTwo(43, 74, true);
            //FW.ShowFrame();
            //FW.FillRightFrame(Pages[PageRight]);
            //FW.FillLeftFrame(Pages[PageLeft]);

            int CursorLeft = FW.LeftFrameCursorLeft;
            int CursorTop = FW.FrameTop;
            Console.SetCursorPosition(CursorLeft, CursorTop);
            Console.CursorVisible = false;

            control.TwoTabs(Pages, Entryes, CursorLeft, CursorTop);
            //do
            //{
            //    FW.SetColor(FrontView.ColorsPreset.Normal);
            //    Console.SetCursorPosition(CursorLeft, CursorTop);
            //    FW.FillRightFrame(Pages[Page]);
            //    FW.FillLeftFrame(Pages[Page]);

            //    Console.SetCursorPosition(CursorLeft, CursorTop);
            //    FW.SetColor(FrontView.ColorsPreset.Selected);
            //    Console.Write(Entryes[index].Name);

            //    FW.SetColor(FrontView.ColorsPreset.ContextNormal);
            //    Console.SetCursorPosition(3, 0);
            //    Console.Write($"Page = {Page}/{Pages.Count}, PageLeft = {PageLeft}/{Pages.Count}, PageRight = {PageRight}/{Pages.Count}");

            //    FW.SetColor(FrontView.ColorsPreset.Normal);

            //    switch (Console.ReadKey().Key)
            //    {
            //        case ConsoleKey.UpArrow:
            //            Console.SetCursorPosition(CursorLeft, CursorTop);
            //            Console.Write(Entryes[index].Name);
            //            if (CursorTop > FW.FrameTop)
            //            {
            //                index--;
            //                CursorTop--;
            //            }
            //            Console.SetCursorPosition(CursorLeft, CursorTop);
            //            break;
            //        case ConsoleKey.DownArrow:
            //            Console.SetCursorPosition(CursorLeft, CursorTop);
            //            Console.Write(Entryes[index].Name);
            //            if (CursorTop - FW.FrameTop < Pages[Page].Count - 1 & index < Entryes.Count-1)
            //            {
            //                index++;
            //                CursorTop++;
            //            }
            //            Console.SetCursorPosition(CursorLeft, CursorTop);
            //            break;
            //        case ConsoleKey.LeftArrow:
            //            Console.SetCursorPosition(CursorLeft, CursorTop);
            //            Console.Write(Entryes[index].Name);
            //            if (CursorLeft == FW.LeftFrameCursorLeft)
            //            {
            //                CursorLeft = FW.RightFrameCursorLeft;
            //                PageLeft = Page;
            //                Page = PageRight;
            //            }
            //            else
            //            {
            //                CursorLeft = FW.LeftFrameCursorLeft;
            //                PageRight = Page;
            //                Page = PageLeft;
            //            }
            //            Console.SetCursorPosition(CursorLeft, CursorTop);
            //            break;
            //        case ConsoleKey.RightArrow:
            //            Console.SetCursorPosition(CursorLeft, CursorTop);
            //            Console.Write(Entryes[index].Name);
            //            if (CursorLeft == FW.RightFrameCursorLeft)
            //            {
            //                CursorLeft = FW.LeftFrameCursorLeft;
            //                PageRight = Page;
            //                Page = PageLeft;
            //            }
            //            else
            //            {
            //                CursorLeft = FW.RightFrameCursorLeft;
            //                PageLeft = Page;
            //                Page = PageRight;
            //            }
            //            Console.SetCursorPosition(CursorLeft, CursorTop);
            //            break;
            //        case ConsoleKey.Enter:
            //            if (Directory.Exists(Entryes[index].Path))
            //            {
            //                CurrentCatalog = Entryes[index].Path;
            //                Entryes = MyTree.GetEntryList(CurrentCatalog);
            //                index = 0;
            //                CursorTop = FW.FrameTop;
            //                Page = 0;
            //                PageLeft = 0;
            //                PageRight = 0;
            //                //NewTable = FW.EntryesToArr(Entryes);
            //                Pages = FW.ToPages(Entryes);
            //                LastPage = Pages.Count-1;
            //                FW.FillRightFrame(Pages[PageRight]);
            //                FW.FillLeftFrame(Pages[PageLeft]);
            //            }
            //            break;
            //        case ConsoleKey.Backspace:
            //            string[] tempPath = CurrentCatalog.Split('\\');                        
            //            if (tempPath.Length>1)
            //            {
            //                for (int i = 0; i < tempPath.Length - 1; i++)
            //                {
            //                    if (i==0)
            //                    {
            //                        CurrentCatalog = tempPath[i]+ '\\';
            //                    }
            //                    else
            //                    {
            //                    CurrentCatalog += tempPath[i]+ '\\';

            //                    }
            //                }
            //                Entryes = MyTree.GetEntryList(CurrentCatalog);
            //                index = 0;
            //                CursorTop = FW.FrameTop;
            //                Page = 0;
            //                PageLeft = 0;
            //                PageRight = 0;
            //                //NewTable = FW.EntryesToArr(Entryes);
            //                Pages = FW.ToPages(Entryes);
            //                LastPage = Pages.Count-1;
            //                FW.FillRightFrame(Pages[PageRight]);
            //                FW.FillLeftFrame(Pages[PageLeft]);
            //            }
            //            break;
            //        case ConsoleKey.Applications:
            //            SelectorContext(Com,FW, CursorTop, CursorLeft, CurrentCatalog);
            //            if (CursorLeft == FW.LeftFrameCursorLeft)
            //            {
            //                FW.FillLeftFrame(Pages[Page]);
            //            }
            //            else if (CursorLeft == FW.RightFrameCursorLeft)
            //            {
            //                FW.FillRightFrame(Pages[Page]);
            //            }

            //            //ShowContext(Com.AllComands, index, CursorLeft);
            //            break;



            //        case ConsoleKey.Escape:
            //            Cycle = false;
            //            break;
            //        case ConsoleKey.Tab:
            //            //Memory.Add(Console.ReadLine());
            //            break;


            //        case ConsoleKey.PageUp:
            //            if (CursorLeft == FW.LeftFrameCursorLeft & Page >0)
            //            {
            //                Page--;
            //                PageLeft = Page;
            //                index = Page * 40;
            //                FW.FillLeftFrame(Pages[PageLeft]);
            //            }
            //            else if (CursorLeft == FW.RightFrameCursorLeft & Page >0)
            //            {
            //                Page--;
            //                PageRight = Page;
            //                index = Page * 40;
            //                FW.FillRightFrame(Pages[PageRight]);
            //            }
            //            CursorTop = FW.FrameTop;
            //            break;
            //        case ConsoleKey.PageDown:
            //            if (CursorLeft == FW.LeftFrameCursorLeft & Page < LastPage)
            //            {
            //                Page++;
            //                PageLeft = Page;
            //                index = Page * 40;
            //                FW.FillLeftFrame(Pages[PageLeft]);
            //            }
            //            else if(CursorLeft == FW.RightFrameCursorLeft & Page < LastPage) 
            //            {
            //                Page++;
            //                PageRight = Page;
            //                index = Page * 40;
            //                FW.FillRightFrame(Pages[PageRight]);
            //            }
            //            CursorTop = FW.FrameTop;
            //            break;
            //        case ConsoleKey.F1:
            //            fr.Message("Presed F1", 'W');
            //            FW.SetColor(FrontView.ColorsPreset.Normal);
            //            Console.ReadKey();
            //            fr.ShowTwo(43, 74, true);
            //            break;
            //        case ConsoleKey.F2:
            //            break;
            //        case ConsoleKey.F3:
            //            break;
            //        case ConsoleKey.F4:
            //            break;

            //        default:
            //            break;
            //    }
            //} while (Cycle);
            //fr.Message("Exit Program",'W');
            //Console.SetCursorPosition(0, 0);
        }

        static public void SelectorContext(Comands Com, FrontView FW, int top, int left, string path)
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
    }
}
