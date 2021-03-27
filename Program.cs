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
            bool Cycle = true;
            bool DirCycle = true;
            int Page = 0;
            int LastPage = 0;
            int PageLeft = 0;
            int PageRight = 0;
            string StepInCatalog = null;

            List<Entry> entr = new List<Entry>();
            List<Entry> entryes = new List<Entry>();
            List<List<Entry>> Pages = new List<List<Entry>>();

            Comands Com = new Comands();

            bool mainCycle = true;
            bool rootCycle = true;
            bool consoleReader = false;

            int index = 0;

            List<string> Memory = new List<string>();
            Tree MyTree = new Tree();

            Frame fr = new Frame(0, 0, 42, 74);
            Frame rootFrame = new Frame(21, 21, 10, 15);
            Frame cr = new Frame(0, 43, 5, 150);

            UserControl control = new UserControl(MyTree, fr);

            MyTree.SetRoots();
            int rootIndex = 0;
            Console.ResetColor();

            do
            {

                rootFrame.Coloring(Frame.Colorscheme.BIOS);
                rootFrame.Show();
                Console.ResetColor();
                foreach (var item in MyTree.Roots)
                {
                    rootFrame.SetColor(Frame.ColorsPreset.Normal);
                    rootFrame.WriteText(item, 0, rootIndex);
                    rootIndex++;
                }
                rootIndex = 0;
                do
                {
                    Console.CursorVisible = false;
                    rootFrame.SetColor(Frame.ColorsPreset.Selected);
                    rootFrame.WriteText(MyTree.Roots[rootIndex], 0, rootIndex);
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.Enter:
                            entryes = MyTree.GetEntryList(MyTree.Roots[rootIndex]);
                            Pages = MyTree.ToPages(entryes);
                            fr.Coloring(Frame.Colorscheme.Default);
                            fr.Show();
                            fr.Clear();
                            control.OneTab(Pages, out _);
                            rootCycle = false;
                            mainCycle = false;
                            consoleReader = false;
                            break;
                        case ConsoleKey.UpArrow:
                            rootFrame.SetColor(Frame.ColorsPreset.Normal);
                            rootFrame.WriteText(MyTree.Roots[rootIndex], 0, rootIndex);
                            if (rootIndex > 0)
                            {
                                rootIndex--;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            rootFrame.SetColor(Frame.ColorsPreset.Normal);
                            rootFrame.WriteText(MyTree.Roots[rootIndex], 0, rootIndex);
                            if (rootIndex < MyTree.Roots.Count - 1)
                            {
                                rootIndex++;
                            }
                            break;
                        case ConsoleKey.Escape:
                            Console.ResetColor();
                            rootCycle = false;
                            mainCycle = false;
                            consoleReader = false;
                            break;
                        default:
                            break;
                    }
                } while (rootCycle);

                if (consoleReader)
                {
                    bool readerCycle = true;
                    cr.Coloring(Frame.Colorscheme.Default);
                    cr.Show();

                    do
                    {
                        cr.WriteText(MyTree.Roots[rootIndex] + ">", 0, 0);
                        StringBuilder sb = new StringBuilder();
                        ConsoleKeyInfo key = Console.ReadKey();
                        switch (key.Key)
                        {
                            case ConsoleKey.UpArrow:
                                if (Memory.Count > 0 & index > 0)
                                {
                                    index--;
                                    sb.Clear();
                                    sb.Append(Memory[index]);
                                    cr.SetCursorPosition(0, 0);
                                    cr.Write(sb.ToString());
                                    cr.SetCursorPosition(sb.Length, 0);
                                }
                                break;
                            case ConsoleKey.DownArrow:
                                if (Memory.Count > 0 & index < Memory.Count)
                                {
                                    index++;
                                    sb.Clear();
                                    sb.Append(Memory[index]);
                                    cr.SetCursorPosition(0, 0);
                                    cr.Write(sb.ToString());
                                    cr.SetCursorPosition(sb.Length, 0);
                                }
                                break;
                            case ConsoleKey.Backspace:
                                sb.Remove(sb.Length - 1, 1);
                                cr.SetCursorPosition(0, 0);
                                cr.Write(sb.ToString().PadRight(sb.Length + 1, ' '));
                                cr.SetCursorPosition(sb.Length, 0);
                                break;
                            case ConsoleKey.Enter:
                                //ReadCommand(sb.ToString());
                                break;
                            case ConsoleKey.Escape:
                            case ConsoleKey.Tab:
                                readerCycle = false;
                                break;
                            default:
                                sb.Append(key);
                                cr.WriteText(sb.ToString(), false);
                                break;
                        }
                    } while (readerCycle);
                }

            } while (mainCycle);
        }
    }
}
