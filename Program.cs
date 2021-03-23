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

            //fr.ShowTwo(43, 74, true);
            fr.ShowOne(43, 74, true);
            FW.FillFrame(Pages[Page]);
            //FW.ShowFrame();
            //FW.FillRightFrame(Pages[PageRight]);
            //FW.FillLeftFrame(Pages[PageLeft]);

            int CursorLeft = FW.LeftFrameCursorLeft;
            int CursorTop = FW.FrameTop;
            Console.SetCursorPosition(CursorLeft, CursorTop);
            Console.CursorVisible = false;

            control.OneTab(Pages, Entryes, CursorLeft, CursorTop);
        }
        static void ChoseDrive(Frame fr)
        {
            
        }
    }
}
