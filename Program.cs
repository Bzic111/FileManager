﻿using System;
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
            Frame fr = new Frame(0, 0, 42, 74);

            fr.Coloring(Frame.Colorscheme.Default);


            List<Entry> entr = new List<Entry>();
            MyTree.SetRoots();
            int rootIndex = 0;
            Console.ResetColor();
            fr.Show();
            foreach (var item in MyTree.Roots)
            {
                fr.SetColor(Frame.ColorsPreset.Normal);
                fr.WriteText(item, 0, rootIndex);
                rootIndex++;
            }
            rootIndex = 0;
            do
            {
                fr.SetColor(Frame.ColorsPreset.Selected);
                fr.WriteText(MyTree.Roots[rootIndex], 0, rootIndex);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Enter:
                        entr = MyTree.GetEntryList(MyTree.Roots[rootIndex]);
                        Cycle = false;
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
                        if (rootIndex < MyTree.Roots.Count-1)
                        {
                            rootIndex++;
                        }
                        break;
                    default:
                        break;
                }
            } while (Cycle);


        }

    }
}
