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
            int tabIndexer = 0;
            List<string> memory = new List<string>();
            StringBuilder consoleReader = new StringBuilder();
            Frame info = new Frame(30, 10, 20, 40);
            info.Coloring(Frame.ColorScheme.BIOS);
            info.SetName("Information");

            List<Tab> tabs = new List<Tab>();
            tabs.Add(new Tab(true));

            tabs[tabIndexer].WorkFrame.Coloring(Frame.ColorScheme.Default);
            tabs[tabIndexer].WorkFrame.SetName(tabs[tabIndexer].WorkFrame.tree.Roots[0] + $"Page {page + 1}/{tabs[tabIndexer].WorkFrame.tree.Pages.Count}");
            tabs[tabIndexer].WorkFrame.Show();
            tabs[tabIndexer].WorkFrame.GetContentFromTree(tabs[tabIndexer].WorkFrame.tree);
            tabs[tabIndexer].WorkFrame.ShowContentFromTree(page);

            do
            {
                tabs[tabIndexer].WorkFrame.SetName($"╣{tabs[tabIndexer].WorkFrame.tree.Pages[page][index].Parent} | Page {page + 1}/{tabs[tabIndexer].WorkFrame.tree.Pages.Count}╠");
                tabs[tabIndexer].WorkFrame.WriteName();
                Console.CursorVisible = false;
                tabs[tabIndexer].WorkFrame.SetColor(Frame.ColorsPreset.Selected);
                tabs[tabIndexer].WorkFrame.WriteText(tabs[tabIndexer].WorkFrame.tree.Pages[page][index].Name, 0, index);
                var key = Console.ReadKey();
                tabs[tabIndexer].WorkFrame.SetColor(Frame.ColorsPreset.Normal);
                switch (key.Key)
                {
                    case ConsoleKey.Escape:     Cycle = false;                                                              break;
                    case ConsoleKey.Backspace:  tabs[tabIndexer].WorkFrame.Go(Frame.To.StepBack, ref page, ref index);      break;
                    case ConsoleKey.Enter:      tabs[tabIndexer].WorkFrame.Go(Frame.To.StepForward, ref page, ref index);   break;
                    case ConsoleKey.PageUp:     tabs[tabIndexer].WorkFrame.Go(Frame.To.NextPage, ref page, ref index);      break;
                    case ConsoleKey.PageDown:   tabs[tabIndexer].WorkFrame.Go(Frame.To.PreviousPage, ref page, ref index);  break;
                    case ConsoleKey.End:        tabs[tabIndexer].WorkFrame.Go(Frame.To.LastPage, ref page, ref index);      break;
                    case ConsoleKey.Home:       tabs[tabIndexer].WorkFrame.Go(Frame.To.FirstPage, ref page, ref index);     break;
                    case ConsoleKey.UpArrow:    tabs[tabIndexer].WorkFrame.Go(Frame.To.StepUp, ref page, ref index);        break;
                    case ConsoleKey.DownArrow:  tabs[tabIndexer].WorkFrame.Go(Frame.To.StepDown, ref page, ref index);      break;

                    case ConsoleKey.Insert:     tabs[tabIndexer].WorkFrame.Create(ref page, ref index);                     break;
                    case ConsoleKey.Delete:     tabs[tabIndexer].WorkFrame.Delete(ref page, ref index);                     break;
                    case ConsoleKey.Tab:        tabs[tabIndexer].WorkFrame.ConsoleReader(memory, out bool refresher);       break;

                    case ConsoleKey.F1:         info.Show(); info.Clear(); break;
                    case ConsoleKey.F2:         TabSelector(tabs, ref tabIndexer, ref page, ref index);                     break;
                    case ConsoleKey.F3:         AddTabToList(tabs, ref tabIndexer, ref page, ref index);                    break;
                    case ConsoleKey.F4:         DeleteTabFromList(tabs, ref tabIndexer, ref page, ref index);               break;
                    



                    case ConsoleKey.LeftArrow: break;
                    case ConsoleKey.RightArrow: break;
                    case ConsoleKey.Applications: break;

                    default:
                        break;
                }
                if (refresher)
                {
                    tabs[tabIndexer].WorkFrame.tree.ReFresh();
                    tabs[tabIndexer].WorkFrame.Refresh();
                    tabs[tabIndexer].WorkFrame.GetContentFromTree(tabs[tabIndexer].WorkFrame.tree);
                    tabs[tabIndexer].WorkFrame.ShowContentFromTree(page);
                }
                tabs[tabIndexer].WorkFrame.SetColor(Frame.ColorsPreset.Normal);
            } while (Cycle);
            Exit();
        }
        static void TabSelector(List<Tab> Pager, ref int counter, ref int page, ref int index)
        {
            int count = counter;
            bool cycle = true;
            Frame frame = new Frame(0, 0, 2, 150);
            frame.Show(true);
            frame.Content = new string[Pager.Count];
            int liner = 0;
            foreach (var item in Pager)
            {
                frame.SetContentHorizontal(liner, Pager[liner++].Name.PadRight(11, ' ').Remove(10));
            }
            frame.SetColor(Frame.ColorsPreset.Normal);
            foreach (var item in frame.Content)
            {
                frame.WriteTextHorizontal(item, count * 10, 0);
                count++;
            }
            count = 0;
            do
            {
                frame.SetColor(Frame.ColorsPreset.Selected);
                frame.WriteTextHorizontal(frame.Content[count], count * 10, 0);
                frame.SetColor(Frame.ColorsPreset.Normal);
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        frame.WriteTextHorizontal(frame.Content[count], count * 10, 0);
                        if (count > 0)
                        {
                            count--;
                        }
                        else
                        {
                            count = frame.Content.Length - 1;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        frame.WriteTextHorizontal(frame.Content[count], count * 10, 0);
                        if (count < frame.Content.Length - 1)
                        {
                            count++;
                        }
                        else
                        {
                            count = 0;
                        }
                        break;
                    case ConsoleKey.Enter:
                        counter = count;
                        cycle = false;
                        break;
                    case ConsoleKey.Delete:
                        Pager.RemoveAt(counter);
                        cycle = false;
                        break;
                    case ConsoleKey.Insert:
                        AddTabToList(Pager, ref counter, ref page, ref index);
                        cycle = false;
                        break;
                    case ConsoleKey.Tab:
                    case ConsoleKey.Escape:
                        cycle = false;
                        break;
                    default:
                        break;
                }
            } while (cycle);
            page = 0;
            index = 0;
            Pager[counter].WorkFrame.Refresh();
            Pager[counter].WorkFrame.ShowContentFromTree(page);
        }
        static void AddTabToList(List<Tab> tabs, ref int counter, ref int page, ref int index)
        {
            page = 0;
            index = 0;
            tabs.Add(new Tab(true));
            counter = tabs.Count - 1;
            tabs[counter].WorkFrame.Refresh();
            tabs[counter].WorkFrame.ShowContentFromTree(page);
        }
        static void DeleteTabFromList(List<Tab> tabs, ref int counter, ref int page, ref int index)
        {
            if (tabs.Count > 1)
            {
                tabs.RemoveAt(counter);
                page = 0; index = 0;
                counter = tabs.Count - 1;
                tabs[counter].WorkFrame.Refresh();
                tabs[counter].WorkFrame.ShowContentFromTree(page);
            }
            else
            {
                Exit();
            }
        }

        static void Exit()
        {
            Console.ResetColor();
        }
    }
}
