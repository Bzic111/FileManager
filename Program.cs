global using System;
global using System.IO;
global using System.Collections.Generic;
global using FileManager;
global using FileManager.Interfaces;
global using FileManager.Internal;
global using FileManager.Base;
global using FileManager.Enums;
global using FileManager.Extensions;

using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Text.Json;
using System.Windows;

Meth.WriteLog("\tStart Program.");
List<Tab> Tabs = new List<Tab>();
List<string> ComMemory = new List<string>();
Frame info = new Frame((30, 10, 20, 40), "Info");
Frame context = new Frame((0, 0, 25, 40), "Context");

bool clear = false;
bool Cycle = true;

int index = 0;
int page = 0;
int tabIndexer = 0;

Console.Clear();

if (File.Exists("test.xml"))
{
    XmlSerializer xmls = new XmlSerializer(typeof(List<Tab>));
    try
    {
        Meth.WriteLog("Try load test.xml");
        using (FileStream fs = new FileStream("test.xml", FileMode.Open, FileAccess.Read))
        {
            Tabs = (List<Tab>)xmls.Deserialize(fs)!;
        }
        Meth.WriteLog("Last state readed from test.xml");
        //FileStream fs = new FileStream("test.xml", FileMode.Open,FileAccess.Read);
        //Tabs = (List<Tab>)xmls.Deserialize(fs)!;
        //fs.Close();
    }
    catch (Exception e)
    {
        Frame warn = new Frame(30, 30, 5, 60, "Error", ColorScheme.Warning);
        warn.Show(true);
        warn.WriteText(e.Message);
        Console.ReadKey(true);
        Meth.WriteLog("Loading Last state error" + "".PadRight(DateTime.Now.ToString().Length, ' ') + e.Message);
    }
}
else
{
    Meth.WriteLog("Initialize new session.");
    index = 0;
    page = 0;
    tabIndexer = 0;
    Tabs.Add(new Tab(true));
}

info.Coloring(ColorScheme.BIOS);
info.SetName("Information");
Console.ResetColor();
Console.Clear();

Tabs[tabIndexer].WorkFrame.Coloring(ColorScheme.Default);
Tabs[tabIndexer].WorkFrame.SetName(Tabs[tabIndexer].WorkFrame.tree.Roots[0] + $"Page {page + 1}/{Tabs[tabIndexer].WorkFrame.tree.Pages.Count}");
Tabs[tabIndexer].WorkFrame.GetContentFromTree(Tabs[tabIndexer].WorkFrame.tree);
Tabs[tabIndexer].WorkFrame.Show(content: true);

do
{
    bool refresher = false;
    Console.CursorVisible = false;

    Tabs[tabIndexer].WorkFrame.SetName($"╣{Tabs[tabIndexer].WorkFrame.tree.Pages[page][index].Parent} | Page {page + 1}/{Tabs[tabIndexer].WorkFrame.tree.Pages.Count}╠");
    Tabs[tabIndexer].WorkFrame.WriteName();
    Tabs[tabIndexer].WorkFrame.SetColor(ColorsPreset.Selected);
    Tabs[tabIndexer].WorkFrame.WriteText(Tabs[tabIndexer].WorkFrame.tree.Pages[page][index].Name, 0, index);
    Tabs[tabIndexer].WorkFrame.SetColor(ColorsPreset.Normal);

    var key = Console.ReadKey();
    switch (key.Key)
    {
        case ConsoleKey.Escape: Cycle = false; clear = false; break;
        case ConsoleKey.Backspace: Tabs[tabIndexer].WorkFrame.Go(To.StepBack, ref page, ref index); break;
        case ConsoleKey.Enter: Tabs[tabIndexer].WorkFrame.Go(To.StepForward, ref page, ref index); break;
        case ConsoleKey.PageUp: Tabs[tabIndexer].WorkFrame.Go(To.NextPage, ref page, ref index); break;
        case ConsoleKey.PageDown: Tabs[tabIndexer].WorkFrame.Go(To.PreviousPage, ref page, ref index); break;
        case ConsoleKey.End: Tabs[tabIndexer].WorkFrame.Go(To.LastPage, ref page, ref index); break;
        case ConsoleKey.Home: Tabs[tabIndexer].WorkFrame.Go(To.FirstPage, ref page, ref index); break;
        case ConsoleKey.UpArrow: Tabs[tabIndexer].WorkFrame.Go(To.StepUp, ref page, ref index); break;
        case ConsoleKey.DownArrow: Tabs[tabIndexer].WorkFrame.Go(To.StepDown, ref page, ref index); break;

        case ConsoleKey.Insert: Tabs[tabIndexer].WorkFrame.Create(ref page, ref index); break;
        case ConsoleKey.Delete: Tabs[tabIndexer].WorkFrame.Delete(ref page, ref index); break;
        case ConsoleKey.Tab: Tabs[tabIndexer].WorkFrame.ConsoleReader(ComMemory, out refresher); break;

        case ConsoleKey.F1: info.Show(true); break;
        case ConsoleKey.F2: Meth.TabSelector(Tabs, ref tabIndexer, ref page, ref index); break;
        case ConsoleKey.F3: Meth.AddTabToList(Tabs, ref tabIndexer, ref page, ref index); break;
        case ConsoleKey.F4: Meth.DeleteTabFromList(Tabs, ref tabIndexer, ref page, ref index, ref Cycle, ref clear); break;

        case ConsoleKey.LeftArrow:
            context.tree.ChangeDirectory(Tabs[tabIndexer].WorkFrame.tree.Pages[page][index].Path);
            context.GetContentFromTree(context.tree);
            context.Show(true, true, ColorsPreset.ContextNormal);
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
    Tabs[tabIndexer].WorkFrame.SetColor(ColorsPreset.Normal);
} while (Cycle);

Meth.Exit(Tabs, ref index, ref page, ref tabIndexer, clear);
