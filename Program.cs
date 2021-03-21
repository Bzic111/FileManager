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
            string CurrentCatalog = MyTree.Drives[0];
            List<Entry> Entryes = MyTree.GetEntryList(CurrentCatalog);

            FrontView FW = new FrontView(42, 74, Entryes);

            string[] Table = FW.TableForScreen(Entryes, 0);
            //string[] Tree = FW.TreeForScreen(Entryes, 0);

            string[] NewTable = FW.EntryesToArr(Entryes);

            List<string[]> Pages = FW.ToPages(NewTable);
            LastPage = Pages.Count-1;

            FW.ShowFrame();
            FW.FillRightFrame(Pages[PageRight]);
            FW.FillLeftFrame(Pages[PageLeft]);

            int CursorLeft = FW.LeftFrameCursorLeft;
            int CursorTop = FW.FrameTop;
            Console.SetCursorPosition(CursorLeft, CursorTop);
            Console.CursorVisible = false;

            do
            {
                FW.SetColor(FrontView.ColorsPreset.Normal);
                FW.FillRightFrame(Pages[PageRight]);
                FW.FillLeftFrame(Pages[PageLeft]);

                Console.SetCursorPosition(CursorLeft, CursorTop);
                FW.SetColor(FrontView.ColorsPreset.Selected);
                Console.Write(Entryes[index].Name);

                FW.SetColor(FrontView.ColorsPreset.Normal);
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        Console.SetCursorPosition(CursorLeft, CursorTop);
                        Console.Write(Entryes[index].Name);
                        if (index > 0 & Page > 0)
                        {
                            index--;
                            CursorTop = FW.FrameHeight;
                            Page--;
                        }
                        else if (index > 0 & Page == 0)
                        {
                            CursorTop--;
                            index--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        Console.SetCursorPosition(CursorLeft, CursorTop);
                        Console.Write(Entryes[index].Name);

                        if (index < Pages[Page].Length - 1 & index < Entryes.Count)
                        {
                            index++;
                            CursorTop++;
                        }
                        else if (index == Pages[Page].Length - 1 & Page < Pages.Count - 1)
                        {
                            index++;
                            Page++;
                            CursorTop = FW.FrameTop;
                        }
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
                        break;

                    case ConsoleKey.Enter:
                        if (Directory.Exists(Entryes[index].Path))
                        {
                            CurrentCatalog = Entryes[index].Path;
                            Entryes = MyTree.GetEntryList(CurrentCatalog);
                            index = 0;
                            CursorTop = FW.FrameTop;
                            Page = 0;
                            PageLeft = 0;
                            PageRight = 0;
                            NewTable = FW.EntryesToArr(Entryes);
                            Pages = FW.ToPages(NewTable);
                            LastPage = Pages.Count-1;
                            FW.FillRightFrame(Pages[PageRight]);
                            FW.FillLeftFrame(Pages[PageLeft]);
                        }
                        break;
                    case ConsoleKey.Backspace:
                        string[] tempPath = CurrentCatalog.Split('\\');
                        
                        if (tempPath.Length>1)
                        {
                            for (int i = 0; i < tempPath.Length - 1; i++)
                            {
                                if (i==0)
                                {
                                    CurrentCatalog = tempPath[i];
                                }
                                else
                                {
                                CurrentCatalog += '\\'+tempPath[i];

                                }
                            }
                            Entryes = MyTree.GetEntryList(CurrentCatalog);
                            index = 0;
                            CursorTop = FW.FrameTop;
                            Page = 0;
                            PageLeft = 0;
                            PageRight = 0;
                            NewTable = FW.EntryesToArr(Entryes);
                            Pages = FW.ToPages(NewTable);
                            LastPage = Pages.Count-1;
                            FW.FillRightFrame(Pages[PageRight]);
                            FW.FillLeftFrame(Pages[PageLeft]);
                        }
                        break;
                    case ConsoleKey.Applications:
                        //ShowContext(Com.AllComands, index, CursorLeft);
                        //SelectorContext(index, CursorLeft,);
                        break;



                    case ConsoleKey.Escape:
                        Cycle = false;
                        break;
                    case ConsoleKey.Tab:
                        //Memory.Add(Console.ReadLine());
                        break;


                    case ConsoleKey.PageUp:
                        if (Page > 0)
                        {
                            Page--;
                            index = Page * 40;
                        }
                        break;
                    case ConsoleKey.PageDown:
                        if (Page < LastPage)
                        {
                            Page++;
                            index = Page*40;
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
    }
}
