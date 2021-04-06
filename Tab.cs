using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    /// <summary>
    /// Класс для формирования вкладки
    /// </summary>
    [Serializable]
    public class Tab
    {
        public string Name { get => WorkFrame.tree.CurrentPath.Split('\\', StringSplitOptions.RemoveEmptyEntries)[^1]; set => name = value; }
        public int Page = 0;
        public int index = 0;
        public Frame WorkFrame;
        private string name;

        public Tab()
        {

        }
        public Tab(bool newTab = true)
        {
            if (newTab)
            {
                WorkFrame = new Frame(0, 0, 41, 150, "Tab", Frame.ColorScheme.Default);
                WorkFrame.tree = new Tree();

                Frame fr = new Frame(30, 10, WorkFrame.tree.Roots.Count + 1, 10, "Drive", Frame.ColorScheme.Default);
                for (int i = 0; i < WorkFrame.tree.Roots.Count; i++)
                {
                    fr.SetContent(i, WorkFrame.tree.Roots[i]);
                }
                WorkFrame.tree.ChangeDirectory(RootSelect(WorkFrame.tree.Roots, fr));
                Name = WorkFrame.tree.CurrentPath.Split('\\', StringSplitOptions.RemoveEmptyEntries)[^1];
            }
        }
        public Tab(string path, bool newTab = true)
        {
            if (newTab)
            {
                WorkFrame = new Frame(0, 0, 41, 150);
                WorkFrame.tree = new Tree();
                Frame fr = new Frame(30, 10, WorkFrame.tree.Roots.Count, 10);
                for (int i = 0; i < WorkFrame.tree.Roots.Count; i++)
                {
                    fr.SetContent(i, WorkFrame.tree.Roots[i]);
                }
                WorkFrame.tree.ChangeDirectory(RootSelect(WorkFrame.tree.Roots, fr));
            }
            else
            {
                WorkFrame.tree.ChangeDirectory(path);
                for (int i = 0; i < WorkFrame.tree.Pages[0].Count; i++)
                {
                    WorkFrame.SetContent(i, WorkFrame.tree.Pages[0][i].Name);
                }
            }
        }

        /// <summary>
        /// Выбор диска.
        /// </summary>
        /// <param name="str">Список дисков</param>
        /// <param name="fr">Фрейм для заполнения</param>
        /// <returns></returns>
        string RootSelect(List<string> str, Frame fr)
        {
            bool rootSelectorCycle = true;
            int index = 0;
            fr.Show(true, true, Frame.ColorsPreset.Normal);

            do
            {
                fr.SetColor(Frame.ColorsPreset.Selected);
                fr.WriteText(str[index], 0, index);
                fr.SetColor(Frame.ColorsPreset.Normal);
                var K = Console.ReadKey(true);

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
