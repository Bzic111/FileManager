﻿using FileManager.Enums;

namespace FileManager.Old;
/// <summary>Класс для формирования вкладки</summary>
[Serializable]
public class Tab
{
    public string Name;
    public int Page = 0;
    public int index = 0;
    public Frame WorkFrame;

    public Tab()
    {

    }
    public Tab(bool newTab = true)
    {
        if (newTab)
        {
            WorkFrame = new Frame(0, 0, 41, 150, "Tab", ColorScheme.Default);
            WorkFrame.tree = new Tree();

            Frame fr = new Frame(30, 10, WorkFrame.tree.Roots.Count + 1, 10, "Drive", ColorScheme.BIOS);
            for (int i = 0; i < WorkFrame.tree.Roots.Count; i++) fr.SetContent(i, WorkFrame.tree.Roots[i]);
            WorkFrame.tree.ChangeDirectory(RootSelect(WorkFrame.tree.Roots, fr));
            Name = WorkFrame.tree.Pages[0][0].Name;
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

    /// <summary>Выбор диска.</summary>
    /// <param name="str">Список дисков</param>
    /// <param name="fr">Фрейм для заполнения</param>
    /// <returns></returns>
    string RootSelect(List<string> str, Frame fr)
    {
        bool rootSelectorCycle = true;
        int index = 0;
        fr.Show(true, false, ColorsPreset.Normal);
        fr.ShowContent();
        do
        {
            fr.SetColor(ColorsPreset.Selected);
            fr.WriteText(str[index], 0, index);
            fr.SetColor(ColorsPreset.Normal);
            var K = Console.ReadKey(true);

            switch (K.Key)
            {
                case ConsoleKey.UpArrow:
                    fr.WriteText(str[index], 0, index);
                    index = index > 0 ? index - 1 : str.Count - 1;
                    break;
                case ConsoleKey.DownArrow:
                    fr.WriteText(str[index], 0, index);
                    index = index < str.Count - 1 ? index + 1 : 0;
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