using FileManager.Enums;
using FileManager.Old;
using System.Xml.Serialization;

public static class Meth
{
    public static string LogFile = $@"Log_{DateTime.Now:d}.txt";
    public static void TabSelector(List<Tab> Pager, ref int counter, ref int page, ref int index)
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
        frame.SetColor(ColorsPreset.Normal);
        foreach (var item in frame.Content)
        {
            frame.WriteTextHorizontal(item, count * 10, 0);
            count++;
        }
        count = 0;
        do
        {
            frame.SetColor(ColorsPreset.Selected);
            frame.WriteTextHorizontal(frame.Content[count], count * 10, 0);
            frame.SetColor(ColorsPreset.Normal);
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
    public static void AddTabToList(List<Tab> tabs, ref int counter, ref int page, ref int index)
    {
        page = 0;
        index = 0;
        tabs.Add(new Tab(true));
        counter = tabs.Count - 1;
        tabs[counter].WorkFrame.Refresh(true);
        tabs[counter].WorkFrame.ShowContentFromTree(page);
    }
    public static void DeleteTabFromList(List<Tab> tabs, ref int counter, ref int page, ref int index, ref bool Cycle, ref bool clear)
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
    public static void WriteLog(string str)
    {
        using (StreamWriter sw = new StreamWriter(LogFile))
        {
            sw.WriteLine(DateTime.Now.ToString("d") + str + "\n");
        }
    }
    public static void Exit(List<Tab> tabs, ref int index, ref int page, ref int tabIndexer, bool clear = false)
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
                Frame warn = new Frame(30, 30, 5, 60, "Error", ColorScheme.Warning);
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
                Frame warn = new Frame(30, 30, 5, 60, "Error", ColorScheme.Warning);
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
                Frame warn = new Frame(30, 30, 5, 60, "Error", ColorScheme.Warning);
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
    public static void ContextMenu(int col, int row, Entry entry, ColorScheme scheme = ColorScheme.Default)
    {
        Comands cmd = new Comands();
        Frame mfr = new Frame(col + 2, row + 1, 5, 12, "Action", scheme);
        int index = 0;

        mfr.SetContent(0, "Create");
        mfr.SetContent(1, "Delete");
        mfr.SetContent(2, "Copy to");
        mfr.SetContent(3, "Move to");

        mfr.Show(true, false, ColorsPreset.ContextNormal);
        bool cmc = true;
        do
        {
            mfr.SetColor(ColorsPreset.ContextSelected);
            mfr.WriteText(mfr.Content[index], 0, index);
            mfr.SetColor(ColorsPreset.ContextNormal);
            var c = Console.ReadKey(true);
            switch (c.Key)
            {
                case ConsoleKey.Enter:
                    switch (index)
                    {
                        case 0:
                            Frame creator = new Frame(30, 30, 5, 60, "Creating", ColorScheme.BIOS);
                            creator.Show(true);
                            creator.WriteText($"Create Directory or File? [D/F] ?");
                            var q = Console.ReadKey(true);
                            cmd.Create(entry, q.KeyChar);
                            break;
                        case 1:
                            Frame deletor = new Frame(30, 30, 5, 60, "Deleting", ColorScheme.BIOS);
                            deletor.Show(true);
                            deletor.WriteText($"Delete {entry.Name} Y/N ?");
                            if (Console.ReadKey(true).Key == ConsoleKey.Y) cmd.Delete(entry);
                            break;
                        case 2:
                            Frame copyer = new Frame(30, 30, 5, 70, "Copy to", ColorScheme.BIOS);
                            copyer.Show(true);
                            copyer.WriteText($"Input destination path");
                            copyer.SetCursorPosition(0, 1);
                            copyer.WriteText("".PadRight(65, ' '));
                            copyer.SetCursorPosition(0, 1);
                            string destination = Console.ReadLine();
                            cmd.Copy(entry, destination);
                            break;
                        default:
                            break;
                    }
                    break;

                case ConsoleKey.UpArrow: if (index > 0) index--; else index = 3; break;
                case ConsoleKey.DownArrow: if (index < 3) index++; else index = 0; break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.Escape: cmc = false; break;
                default: break;
            }
        } while (cmc);
    }
}