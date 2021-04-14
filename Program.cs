using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;

namespace FileManager
{
    [Serializable]
    public class LastState
    {
        public List<Tab> Tabs;
        public int tabIndexer = 0;
        public LastState()
        {

        }
        public LastState(List<Tab> tabs)
        {
            Tabs = tabs;
        }
    }
    class Program
    {
        static string LogFile = $"Log_{DateTime.Now}.txt";
        static public List<Tab> Tabs;
        static List<string> ComMemory = new List<string>();

        static void Main(string[] args)
        {
            Frame info = new Frame(30, 10, 20, 40);
            Frame context = new Frame(0, 0, 25, 40, "Context", Frame.ColorScheme.Default);

            bool clear = false;
            bool Cycle = true;
            
            int index = 0;
            int page = 0;
            int tabIndexer = 0;

            Start(ref Tabs,ref index, ref page, ref tabIndexer);

            Tabs[tabIndexer].WorkFrame.Coloring(Frame.ColorScheme.Default);
            Tabs[tabIndexer].WorkFrame.SetName(Tabs[tabIndexer].WorkFrame.tree.Roots[0] + $"Page {page + 1}/{Tabs[tabIndexer].WorkFrame.tree.Pages.Count}");
            Tabs[tabIndexer].WorkFrame.Show();
            Tabs[tabIndexer].WorkFrame.GetContentFromTree(Tabs[tabIndexer].WorkFrame.tree);
            Tabs[tabIndexer].WorkFrame.ShowContentFromTree(page);

            info.Coloring(Frame.ColorScheme.BIOS);
            info.SetName("Information");

            do
            {
                bool refresher = false;
                Console.CursorVisible = false;

                Tabs[tabIndexer].WorkFrame.SetName($"╣{Tabs[tabIndexer].WorkFrame.tree.Pages[page][index].Parent} | Page {page + 1}/{Tabs[tabIndexer].WorkFrame.tree.Pages.Count}╠");
                Tabs[tabIndexer].WorkFrame.WriteName();
                Tabs[tabIndexer].WorkFrame.SetColor(Frame.ColorsPreset.Selected);
                Tabs[tabIndexer].WorkFrame.WriteText(Tabs[tabIndexer].WorkFrame.tree.Pages[page][index].Name, 0, index);
                Tabs[tabIndexer].WorkFrame.SetColor(Frame.ColorsPreset.Normal);
                
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Escape:     Cycle = false; clear = false;                                                       break;
                    case ConsoleKey.Backspace:  Tabs[tabIndexer].WorkFrame.Go(Frame.To.StepBack, ref page, ref index);              break;
                    case ConsoleKey.Enter:      Tabs[tabIndexer].WorkFrame.Go(Frame.To.StepForward, ref page, ref index);           break;
                    case ConsoleKey.PageUp:     Tabs[tabIndexer].WorkFrame.Go(Frame.To.NextPage, ref page, ref index);              break;
                    case ConsoleKey.PageDown:   Tabs[tabIndexer].WorkFrame.Go(Frame.To.PreviousPage, ref page, ref index);          break;
                    case ConsoleKey.End:        Tabs[tabIndexer].WorkFrame.Go(Frame.To.LastPage, ref page, ref index);              break;
                    case ConsoleKey.Home:       Tabs[tabIndexer].WorkFrame.Go(Frame.To.FirstPage, ref page, ref index);             break;
                    case ConsoleKey.UpArrow:    Tabs[tabIndexer].WorkFrame.Go(Frame.To.StepUp, ref page, ref index);                break;
                    case ConsoleKey.DownArrow:  Tabs[tabIndexer].WorkFrame.Go(Frame.To.StepDown, ref page, ref index);              break;

                    case ConsoleKey.Insert:     Tabs[tabIndexer].WorkFrame.Create(ref page, ref index);                             break;
                    case ConsoleKey.Delete:     Tabs[tabIndexer].WorkFrame.Delete(ref page, ref index);                             break;
                    case ConsoleKey.Tab:        Tabs[tabIndexer].WorkFrame.ConsoleReader(ComMemory, out refresher);                 break;

                    case ConsoleKey.F1:         info.Show(true); break;
                    case ConsoleKey.F2:         TabSelector(Tabs, ref tabIndexer, ref page, ref index);                             break;
                    case ConsoleKey.F3:         AddTabToList(Tabs, ref tabIndexer, ref page, ref index);                            break;
                    case ConsoleKey.F4:         DeleteTabFromList(Tabs, ref tabIndexer, ref page, ref index, ref Cycle, ref clear); break;
                                        
                    case ConsoleKey.LeftArrow:
                        context.tree.ChangeDirectory(Tabs[tabIndexer].WorkFrame.tree.Pages[page][index].Path);
                        context.GetContentFromTree(context.tree);
                        context.Show(true, true, Frame.ColorsPreset.ContextNormal);
                        bool contextCycle = true;
                        do
                        {

                        } while (contextCycle);
                        break;
                    case ConsoleKey.RightArrow: break;
                    case ConsoleKey.Applications: break;

                    default: break;
                }
                if (refresher)
                {
                    Tabs[tabIndexer].WorkFrame.tree.ReFresh();
                    Tabs[tabIndexer].WorkFrame.Refresh(true, page);
                }
                Tabs[tabIndexer].WorkFrame.SetColor(Frame.ColorsPreset.Normal);
            } while (Cycle);
            Exit(Tabs, ref index, ref page, ref tabIndexer,clear);
        }
        static void TabSelector(List<Tab> Pager, ref int counter, ref int page, ref int index)
        {
            int count = 0;
            bool cycle = true;
            Frame frame = new Frame(0, 0, 2, 150);
            frame.SetName("Tabs");
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
            tabs[counter].WorkFrame.Refresh(true);
            tabs[counter].WorkFrame.ShowContentFromTree(page);
        }
        static void DeleteTabFromList(List<Tab> tabs, ref int counter, ref int page, ref int index, ref bool Cycle,ref bool clear)
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
                Cycle = false;
                clear = true;
            }
        }

        static public void WriteLog(string str)
        {
            File.AppendAllText(LogFile, DateTime.Now.ToString() + str + "\n");
        }
        static void Start(ref List<Tab> tabs, ref int index, ref int page, ref int tabIndexer)
        {
            Console.ResetColor();
            Console.Clear();
            WriteLog("\tStart.");
            if (File.Exists("test.xml"))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(List<Tab>));
                try
                {
                    FileStream fs = new FileStream("test.xml", FileMode.Open);
                    tabs = (List<Tab>)xmls.Deserialize(fs);
                    fs.Close();
                    WriteLog("Last state readed from test.xml");
                }
                catch (Exception e)
                {
                    Frame warn = new Frame(30, 30, 5, 60,"Error",Frame.ColorScheme.Warning);
                    warn.Show(true);
                    warn.WriteText(e.Message);
                    Console.ReadKey(true);
                    WriteLog("Loading Last state error"+ "".PadRight(DateTime.Now.ToString().Length, ' ') + e.Message);
                }
            }
            else
            {
                index = 0;
                page = 0;
                tabIndexer = 0;
                tabs = new List<Tab>();
                tabs.Add(new Tab(true));
                WriteLog("Initialize new session.");
            }
            Console.Clear();
        }
        static void Exit(List<Tab> tabs, ref int index, ref int page, ref int tabIndexer, bool clear = false)
        {
            if (clear & File.Exists("test.xml"))
            {
                FileInfo fi = new FileInfo("test.xml");
                try
                {
                    fi.Delete();
                }
                catch (Exception e)
                {
                    Frame warn = new Frame(30, 30, 5, 60, "Error", Frame.ColorScheme.Warning);
                    warn.Show(true);
                    warn.WriteText(e.Message);
                    Console.ReadKey(true);
                }
            }
            else if (File.Exists("test.xml"))
            {
                try
                {
                    FileStream fs = new FileStream("test.xml", FileMode.OpenOrCreate);
                    XmlSerializer xmls = new XmlSerializer(typeof(List<Tab>));
                    xmls.Serialize(fs, tabs);
                    fs.Close();
                    WriteLog("Last state saved to test.xml");
                }
                catch (Exception e)
                {
                    Frame warn = new Frame(30, 30, 5, 60, "Error", Frame.ColorScheme.Warning);
                    warn.Show(true);
                    warn.WriteText(e.Message);
                    Console.ReadKey(true);
                    WriteLog("Last state saving fail \n" + "".PadRight(DateTime.Now.ToString().Length, ' ') + e.Message);
                }
            }
            else
            {
                try
                {
                    FileStream fs = new FileStream("test.xml", FileMode.OpenOrCreate);
                    XmlSerializer xmls = new XmlSerializer(typeof(List<Tab>));
                    xmls.Serialize(fs, tabs);
                    fs.Close();
                    WriteLog("Last state saved to test.xml");
                }
                catch (Exception e)
                {
                    Frame warn = new Frame(30, 30, 5, 60, "Error", Frame.ColorScheme.Warning);
                    warn.Show(true);
                    warn.WriteText(e.Message);
                    Console.ReadKey(true);
                    WriteLog("Last state saving fail \n" + "".PadRight(DateTime.Now.ToString().Length, ' ') + e.Message);
                }
            }
            WriteLog("End Programm.");
            Console.ResetColor();
            Console.CursorVisible = true;
        }

    }
}
