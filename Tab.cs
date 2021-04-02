using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    class Tab
    {
        public string Name;
        public int Page = 0;
        public int index = 0;
        public Tree tree;
        public Frame work;
        Frame fr;

        public Tab()
        {

        }
        public Tab(bool newTab = true)
        {
            if (newTab)
            {
                tree = new Tree();
                fr = new Frame(30, 10, tree.Roots.Count, 10);
                for (int i = 0; i < tree.Roots.Count; i++)
                {
                    fr.SetContent(i, tree.Roots[i]);
                }
                tree.ChangeDirectory(RootSelect(tree.Roots, fr));
                work = new Frame(0, 0, 41, 150);
            }
        }
        public Tab(string path, bool newTab = true)
        {
            if (newTab)
            {
                tree = new Tree();
                fr = new Frame(30, 10, tree.Roots.Count, 10);
                for (int i = 0; i < tree.Roots.Count; i++)
                {
                    fr.SetContent(i, tree.Roots[i]);
                }
                tree.ChangeDirectory(RootSelect(tree.Roots, fr));
                work = new Frame(0, 0, 41, 150);
            }
            else
            {
                tree.ChangeDirectory(path);
                for (int i = 0; i < tree.Pages[0].Count; i++)
                {
                    fr.SetContent(i, tree.Pages[0][i].Name);
                }
            }
        }
        string RootSelect(List<string> str, Frame fr)
        {
            var K = Console.ReadKey(true);
            bool rootSelectorCycle = true;
            int index = 0;
            fr.Show(true, true, Frame.ColorsPreset.Normal);
            int liner = 0;
            do
            {
                fr.SetColor(Frame.ColorsPreset.Selected);
                fr.WriteText(str[index], 0, index);
                fr.SetColor(Frame.ColorsPreset.Normal);
                switch (K.Key)
                {
                    case ConsoleKey.UpArrow:
                        fr.WriteText(str[index], 0, index);
                        if (index > 0)
                        {
                            index--;
                        }
                        else
                        {
                            index = str.Count - 1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        fr.WriteText(str[index], 0, index);
                        if (index < str.Count - 1)
                        {
                            index++;
                        }
                        else
                        {
                            index = 0;
                        }
                        break;
                    case ConsoleKey.Escape:
                        rootSelectorCycle = false;
                        break;
                    case ConsoleKey.Enter:
                        return str[index];
                    default: break;
                }
            } while (rootSelectorCycle);
            return null;
        }

    }
}
