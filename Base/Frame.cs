using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FileManager.Base;
internal class Frame : IFrame
{
    public Coordinates Geometry { get; private set; }
    private SymbolBorder Symbols { get; set; }
    public string Name { get; private set; }
    public string[] Content { get; set; }

    private Frame()
    {
        Symbols = (LeftUpCorner: '╔', LeftDownCorner: '╚', RightUpCorner: '╗', RightDownCorner: '╝', Liner: '═', Border: '║');
    }
    private Frame(Coordinates geometry) : this()
    {
        Geometry = geometry;
        Content = new string[Geometry.Rows - 2];
    }
    public Frame(Coordinates geometry, string name = "Noname") : this(geometry) => Name = name;

    public void Move(int startRow, int startCol)
    {
        Geometry = (StartCol: startCol, StartRow: startRow, Rows: Geometry.Rows, Cols: Geometry.Cols);
    }
    public void SetContent(string content)
    {
        if (String.IsNullOrEmpty(content)) return;
        int freeSpace = Geometry.GetFreeSpace();
        char[] chrs = content.ToCharArray();
        if (content.Length <= freeSpace) writeContent(chrs);
        else if (content.Length > freeSpace) writeContent(chrs[..freeSpace]);

        void writeContent(char[] chrs)
        {
            StringBuilder sb = new StringBuilder(Geometry.Cols - 2);
            for (int i = 0, j = 0; i < Content.Length; i++, j += Geometry.Cols - 2)
            {
                if (j < chrs.Length && j + Geometry.Cols - 2 > chrs.Length)
                {
                    Content[i] = sb.Append(chrs[j..]).ToString();
                    sb.Clear();
                    return;
                }
                else
                {
                    Content[i] = sb.Append(chrs[j..(j + Geometry.Cols - 2)]).ToString()!;
                    sb.Clear();
                }
            }
        }
    }
    public void SetName(string name) => Name = name;
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
    private void WriteName()
    {
        Console.SetCursorPosition(Geometry.StartCol + 2, Geometry.StartRow);
        Console.Write(Name.PadRight(Geometry.Cols - 3, Symbols.Liner));
    }
    public void Clear()
    {
        for (int i = 0; i < Geometry.Rows - 1; i++)
        {
            Console.SetCursorPosition(Geometry.StartCol + 1, Geometry.StartRow + 1 + i);
            Console.Write("".PadRight(Geometry.Cols - 2, ' '));
        }
    }
    public void WriteContent()
    {
        for (int i = 0; i < Content.Length; i++)
        {
            Console.SetCursorPosition(Geometry.StartCol + 1, Geometry.StartRow + 1 + i);
            Console.Write(Content[i]);
        }
    }
}
