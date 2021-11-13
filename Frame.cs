using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Collections;
using System.Collections.Generic;

namespace FileManager;
/// <summary>Фрейм.</summary>
[Serializable]
public partial class Frame
{
    private Colors Colors { get; set; }
    public Coordinates Geometry { get; private set; }
    private SymbolBorder Symbols { get; set; }
    public ColorScheme Scheme { get; private set; }

    public int Page;
    public string FrameName;
    public string[] Content;
    public List<string[]> Pages;
    public Tree tree;
    public Entry entry;

    int page = 0;
    int index = 0;
    Comands comand;

    private Frame() => comand = new Comands();
    private Frame(string frameName, ColorScheme scheme) : this()
    {
        FrameName = frameName;
        Scheme = scheme;
        Coloring(Scheme);
    }
    public Frame(int startCol, int startRow, int rws, int cls) : this("No Name", ColorScheme.Default)
    {
        Geometry = (StartCol: startCol, StartRow: startRow, rows: rws, cols: cls);
        Content = new string[Geometry.Rows - 1];
    }
    public Frame(int startCol, int startRow, int rws, int cls, string frameName, ColorScheme scheme) : this(frameName, scheme)
    {
        Geometry = (StartCol: startCol, StartRow: startRow, rows: rws, cols: cls);
        Content = new string[Geometry.Rows - 1];
    }
    /// <summary>Создание фрейма</summary>
    /// <param name="geometry">Координаты фрейма</param><param name="scheme">Цветовая схема</param>
    public Frame((int startCol, int startRow, int rws, int cls) geometry, string frameName, ColorScheme scheme) : this(frameName, scheme)
    {
        Geometry = Geometry;
        Content = new string[Geometry.Rows - 1];
    }

    public void Show()
    {
        if (Console.WindowWidth < Geometry.StartCol && Console.WindowHeight < Geometry.StartRow + Geometry.Rows + 8)
        {
            Console.SetWindowSize(Geometry.StartCol + Geometry.Cols + 1, Geometry.StartRow + Geometry.Rows + 8);
        }
        Console.SetCursorPosition(Geometry.StartCol, Geometry.StartRow);
        Console.Write($"{Symbols.LeftUpCorner}".PadRight(Geometry.Cols - 1, Symbols.Liner) + Symbols.RightUpCorner);
        WriteName();
        for (int i = 1; i < Geometry.Rows; i++)
        {
            Console.SetCursorPosition(Geometry.StartCol, Geometry.StartRow + i);
            Console.Write(Symbols.Border);
            Console.SetCursorPosition(Geometry.StartCol + Geometry.Cols - 1, Geometry.StartRow + i);
            Console.Write($"{Symbols.Border}");
        }
        Console.SetCursorPosition(Geometry.StartCol, Geometry.StartRow + Geometry.Rows);
        Console.Write($"{Symbols.LeftDownCorner}".PadRight(Geometry.Cols - 1, Symbols.Liner) + Symbols.RightDownCorner);
    }    
    /// <summary>Отображение фрейма в консоль</summary>
    /// <param name="clear">Очистить область</param><param name="content">Вывести контент</param><param name="preset">Сочетания цветов</param>
    public void Show(bool clear = false, bool content = false, ColorsPreset preset = ColorsPreset.Normal)
    {
        SetColor(preset);
        Show();
        if (clear) Clear();
        if (content) ShowContentFromTree(0);
        Console.ResetColor();
    }

    /// <summary>Очистка области фрейма</summary>
    public void Clear()
    {
        SetColor(ColorsPreset.Normal);
        for (int i = 0; i < Geometry.Rows - 1; i++)
        {
            Console.SetCursorPosition(Geometry.StartCol + 1, Geometry.StartRow + 1 + i);
            Console.Write("".PadRight(Geometry.Cols - 2, ' '));
        }
    }

    /// <summary>Обновить фрейм</summary>
    public void Refresh(bool content = false, int pge = 0)
    {
        Show(true);
        if (content)
        {
            tree.ReFresh();
            GetContentFromTree(tree);
            ShowContentFromTree(pge);
        }
    }

    /// <summary>Установка сочетаний цветов для фрейма по цветовой схеме</summary><param name="scheme">Цветовая схема</param>
    public void Coloring(ColorScheme scheme)
    {
        Scheme = scheme;
        switch (scheme)
        {
            case ColorScheme.BIOS:
                Colors = (
                NormalBackGround: ConsoleColor.Blue,
                SelectedBackGround: ConsoleColor.Red,
                NormalText: ConsoleColor.Yellow,
                SelectedText: ConsoleColor.White,
                ContexMenuNormalBackGround: ConsoleColor.Yellow,
                ContexMenuSelectedBackGround: ConsoleColor.Red,
                SelectedContext: ConsoleColor.Yellow,
                NormalContext: ConsoleColor.Black);
                break;
            case ColorScheme.Warning:
                Colors = (
                NormalBackGround: ConsoleColor.Red,
                SelectedBackGround: ConsoleColor.White,
                NormalText: ConsoleColor.Yellow,
                SelectedText: ConsoleColor.Red,
                ContexMenuNormalBackGround: ConsoleColor.Yellow,
                ContexMenuSelectedBackGround: ConsoleColor.DarkRed,
                SelectedContext: ConsoleColor.Yellow,
                NormalContext: ConsoleColor.Black);
                break;
            case ColorScheme.Default:
            default:
                Colors = (
                NormalBackGround: ConsoleColor.Black,
                SelectedBackGround: ConsoleColor.White,
                NormalText: ConsoleColor.White,
                SelectedText: ConsoleColor.Black,
                ContexMenuNormalBackGround: ConsoleColor.Gray,
                ContexMenuSelectedBackGround: ConsoleColor.Yellow,
                SelectedContext: ConsoleColor.Red,
                NormalContext: ConsoleColor.Black);
                break;
        }
    }

    /// <summary>Установка цветов консоли в сочетание цветов <paramref name="preset"/> согласно предустановленной схеме.</summary>
    /// <param name="preset">Сочетание цветов</param>
    public void SetColor(ColorsPreset preset)
    {
        ConsoleColor back;
        ConsoleColor fore;
        switch (preset)
        {
            case ColorsPreset.Selected:
                back = Colors.SelectedContext;
                fore = Colors.SelectedText;
                break;
            case ColorsPreset.ContextNormal:
                back = Colors.ContexMenuNormalBackGround;
                fore = Colors.NormalContext;
                break;
            case ColorsPreset.ContextSelected:
                back = Colors.ContexMenuSelectedBackGround;
                fore = Colors.SelectedContext;
                break;
            case ColorsPreset.Standart:
                back = ConsoleColor.Black;
                fore = ConsoleColor.White;
                break;
            case ColorsPreset.Normal:
            default:
                back = Colors.NormalBackGround;
                fore = Colors.NormalText;
                break;
        }
        Console.BackgroundColor = back;
        Console.ForegroundColor = fore;
    }

    /// <summary>установка курсора консоли внутри фрейма</summary><param name="col">столбец</param><param name="row">строка</param>
    public void SetCursorPosition(int col, int row) => Console.SetCursorPosition(col + Geometry.StartCol + 1, row + Geometry.StartRow + 1);

    /// <summary>Установка имени <paramref name="str"/> фрейма</summary><param name="str"></param>
    public void SetName(string str) => FrameName = str.Length < Geometry.Cols - 4 ? str : $"{str.Split('\\')[0]}...\\{str.Split('\\')[^1]}";

    /// <summary>Вывод текста <paramref name="str"/> внутри фрейма в столбец начиная с определённой позиции или первого столбца первой строки.</summary>
    /// <param name="str">Текст</param><param name="col">столбец</param><param name="row">строка</param>
    public void WriteText(string str, int col = 0, int row = 0)
    {
        Console.SetCursorPosition(col + Geometry.StartCol + 1, row + Geometry.StartRow + 1);
        if (str.Length <= Geometry.Cols)
        {
            Console.Write(str.PadRight(Geometry.Cols - 2, ' '));
        }
        else
        {
            int counter = 0;
            int lines = 0;
            do
            {
                Console.SetCursorPosition(col + Geometry.StartCol + 1, row + Geometry.StartRow + 1 + lines);
                for (int i = 0; i < Geometry.Cols - 2 & counter < str.Length; i++, counter++)
                {
                    Console.Write(str[counter]);
                }
                lines++;
            } while (counter < str.Length & lines < Geometry.Rows);
        }
        Console.SetCursorPosition(col + Geometry.StartCol + 1, row + Geometry.StartRow + 1);
    }

    /// <summary>Вывод текста <paramref name="str"/> внутри фрейма в строку начиная с определённой позиции или первого столбца первой строки.</summary>
    /// <param name="str">Текст</param><param name="col">столбец</param><param name="row">строка</param>
    public void WriteTextHorizontal(string str, int col = 0, int row = 0)
    {
        Console.SetCursorPosition(col + Geometry.StartCol + 1, row + Geometry.StartRow + 1);
        Console.Write(str);
        Console.SetCursorPosition(col + Geometry.StartCol + 1, row + Geometry.StartRow + 1);
    }

    /// <summary>Отобразить имя фрейма</summary>
    public void WriteName()
    {
        Console.SetCursorPosition(Geometry.StartCol + 2, Geometry.StartRow);
        Console.Write(FrameName.PadRight(Geometry.Cols - 3, Symbols.Liner));
    }

    public void SetPages(List<Entry> entr)
    {
        Pages = new List<string[]>();
        for (int i = 0, counter = 0; counter < entr.Count; i++)
        {
            Pages.Add(new string[Geometry.Rows]);
            for (int j = 0; j < Geometry.Rows - 2 & counter < entr.Count; j++, counter++)
            {
                Pages[i][j] = entr[counter].Name;
            }
        }
    }

    /// <summary>Установка контента фрейма.</summary><param name="liner">строка</param><param name="str">Текст</param>
    public void SetContent(int liner, string str)
    {
        if (liner >= 0 & liner < Content.Length) Content[liner] = str.PadRight(Geometry.Cols - 2, ' ').Remove(Geometry.Cols - 3);
    }

    /// <summary>Установка контента фрейма.</summary><param name="liner">строка</param><param name="str">текст</param>
    public void SetContentHorizontal(int liner, string str)
    {
        if (liner >= 0 & liner < Content.Length) Content[liner] = str;
    }

    /// <summary>Вывод текста страницы <paramref name="page"/> контента фрейма полученного из древа.</summary>
    /// <param name="page">номер страницы</param>
    public void ShowContentFromTree(int page)
    {
        int line = 0;
        SetColor(ColorsPreset.Normal);
        foreach (var item in Pages[page]) WriteText(item, 0, line++);
    }
    public void ShowContent()
    {
        int line = 0;
        SetColor(ColorsPreset.Normal);
        foreach (var item in Content) WriteText(item, 0, line++);
    }

    /// <summary>Получение страниц элементов древа <paramref name="tree"/></summary>
    /// <param name="tree">Древо элементов</param>
    public void GetContentFromTree(Tree tree)
    {
        Pages = new List<string[]>();
        for (int i = 0; i < tree.Pages.Count; i++)
        {
            string[] cont = new string[tree.Pages[i].Count];
            Pages.Add(cont);
            for (int j = 0; j < tree.Pages[i].Count; j++)
            {
                Pages[i][j] = tree.Pages[i][j].Name;
            }
        }
    }

    /// <summary>Метод управления курсором внутри фрейма</summary><param name="where">переход</param>
    /// <param name="page">Страница</param><param name="index">индекс</param>
    public void Go(To where, ref int page, ref int index)
    {
        if (where == To.StepUp | where == To.StepDown)
        {
            SetColor(ColorsPreset.Normal);
            WriteText(tree.Pages[page][index].Name, 0, index);
        }
        switch (where)
        {
            case To.StepBack:
                tree.ChangeDirectory(tree.Pages[0][0].Parent);
                GetContentFromTree(tree);
                page = 0;
                index = 0;
                break;
            case To.StepForward:
                if (page == 0 & index == 0)
                {
                    tree.ChangeDirectory(tree.Pages[page][index].Parent);
                }
                else
                {
                    tree.ChangeDirectory(tree.Pages[page][index].Path + '\\' + tree.Pages[page][index].Name);
                }
                GetContentFromTree(tree);
                page = 0;
                index = 0;
                break;
            case To.NextPage:
                index = page > 0 ? 0 : index;
                page = page > 0 ? --page : page;
                break;
            case To.PreviousPage:
                index = page < tree.Pages.Count - 1 ? 0 : index;
                page = page < tree.Pages.Count - 1 ? ++page : page;
                break;
            case To.FirstPage:
                page = 0;
                index = 0;
                break;
            case To.LastPage:
                page = tree.Pages.Count - 1;
                index = 0;
                break;
            case To.StepUp:
                index = index > 0 ? --index : index;
                break;
            case To.StepDown:
                index = index < tree.Pages[page].Count - 1 ? ++index : index;
                break;
            default:
                break;
        }
        if (where == To.StepUp | where == To.StepDown)
        {
            SetColor(ColorsPreset.Selected);
            WriteText(tree.Pages[page][index].Name, 0, index);
        }
        else
        {
            Refresh();
            ShowContentFromTree(page);
            SetColor(ColorsPreset.Selected);
            WriteText(tree.Pages[page][index].Name, 0, index);
        }
    }

    /// <summary>Создать новый элемент.</summary><param name="page">Страница элементов</param><param name="index">Индекс элемента</param>
    public void Create(ref int page, ref int index)
    {
        Frame question = new Frame(30, 30, 5, 60, "Creating", ColorScheme.BIOS);
        question.Show(true);
        question.WriteText($"Create Directory or File? [D/F] ?");
        var q = Console.ReadKey(true);
        comand.Create(tree.Pages[page][index], q.KeyChar);
        Refresh(true);
        page = 0;
        index = 0;
    }

    /// <summary>Удалить текущий элемент.</summary><param name="page">Страница элементов</param><param name="index">Индекс элемента</param>
    public void Delete(ref int page, ref int index)
    {
        Frame question = new Frame(30, 30, 5, 60);
        question.Show();
        question.SetName("Deleting");
        question.WriteName();
        question.WriteText($"Delete {tree.Pages[page][index].Name} Y/N ?");
        var q = Console.ReadKey(true);
        if (q.Key == ConsoleKey.Q)
        {
            comand.Delete(tree.Pages[page][index]);
            Refresh(true);
            page = 0;
            index = 0;
        }
    }

    /// <summary>Консольный ввод</summary><param name="memory">Список ввода</param>
    public void ConsoleReader(List<string> memory, out bool refresh)
    {
        Meth.WriteLog("Reading command.");
        StringBuilder consoleReader = new StringBuilder();
        refresh = false;
        bool reader = true;
        int memIndex = memory.Count - 1;
        string consoleLine = "";
        int cursorStartCol = tree.Pages[page][index].Path.Length + 1;
        Console.SetCursorPosition(0, 43);
        Console.Write(tree.Pages[page][index].Path + ">".PadRight(50, ' '));
        Console.SetCursorPosition(0, 44);
        Console.Write("".PadRight(50, ' '));
        Console.Write(consoleLine);
        Console.CursorVisible = true;
        Console.SetCursorPosition(cursorStartCol, 43);
        consoleReader.Clear();
        do
        {
            var sub = Console.ReadKey(true);
            switch (sub.Key)
            {
                case ConsoleKey.Tab:
                case ConsoleKey.Escape:
                    reader = false;
                    break;
                case ConsoleKey.UpArrow:
                    if (memory.Count > 0)
                    {
                        consoleReader.Clear();
                        if (memIndex < memory.Count - 1)
                        {
                            memIndex++;
                            consoleLine = memory[memIndex].ToString();
                        }
                        consoleReader.Append(consoleLine);
                        Console.SetCursorPosition(cursorStartCol, 43);
                        Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                        Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (memory.Count > 0)
                    {
                        consoleReader.Clear();
                        if (memIndex > 0)
                        {
                            memIndex--;
                            consoleLine = memory[memIndex].ToString();
                        }
                        consoleReader.Append(consoleLine);
                        Console.SetCursorPosition(cursorStartCol, 43);
                        Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                        Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                    }
                    break;
                case ConsoleKey.Enter:
                    consoleLine = consoleReader.ToString();
                    if (!string.IsNullOrEmpty(consoleLine))
                    {
                        memory.Add(consoleLine);
                        Meth.WriteLog("Input command : " + consoleLine);
                    }
                    comand.Reader(consoleLine, ref tree, tree.Pages[page][index], out refresh);
                    consoleReader.Clear();
                    consoleLine = consoleReader.ToString();
                    Console.SetCursorPosition(cursorStartCol, 43);
                    Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                    Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                    break;
                case ConsoleKey.Backspace:
                    if (!string.IsNullOrEmpty(consoleLine))
                    {
                        consoleLine = consoleReader.Remove(consoleLine.Length - 1, 1).ToString();
                    }
                    Console.SetCursorPosition(cursorStartCol, 43);
                    Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                    Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                    break;
                default:
                    if ((sub.KeyChar >= '\u0020' & sub.KeyChar <= '\u007A') | (sub.KeyChar >= '\u0430' & sub.KeyChar <= '\u044F'))
                    {
                        consoleReader.Append(sub.KeyChar);
                        consoleLine = consoleReader.ToString();
                        Console.SetCursorPosition(cursorStartCol, 43);
                        Console.WriteLine(consoleLine.PadRight(Console.WindowWidth - Console.CursorLeft - 1, ' '));
                        Console.SetCursorPosition(consoleLine.Length + cursorStartCol, 43);
                    }
                    break;
            }
        } while (reader);
        Meth.WriteLog("Reader end.");
    }
}