using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    class Frame
    {
        public enum Colorscheme
        {
            Default,
            BIOS
        }
        public enum ColorsPreset
        {
            Normal,
            Selected,
            ContextNormal,
            ContextSelected,
            Standart
        }

        Colorscheme Scheme;
        ConsoleColor NormalBackGround;              // = Black;
        ConsoleColor SelectedBackGround;            // = White;
        ConsoleColor NormalText;                    //  = White;
        ConsoleColor SelectedText;                  //  = Black;
        ConsoleColor ContexMenuNormalBackGround;    //  = Gray;
        ConsoleColor ContexMenuSelectedBackGround;  //  = Yellow;
        ConsoleColor SelectedContext;               //  = Red;
        ConsoleColor NormalContext;                 //  = Black;


        int FrameRows; //42
        int FrameCols; //74
        int PageCount;
        int FrameLeftRow;
        int FrameLeftCol;
        int FrameRightRow;
        int FrameRightCol;
        int FrameSubRow;
        int FrameSubCol;

        int StartCol;
        int StartRow;
        public int rows;
        public int cols;
        public List<Object> Lines;

        char LeftUpCorner = '╔';
        char LeftDownCorner = '╚';
        char RightUpCorner = '╗';
        char RightDownCorner = '╝';
        char Liner = '═';
        char Border = '║';

        public Frame()
        {

        }
        public Frame(int startCol, int startRow, int rws, int cls)
        {
            StartCol = startCol;
            StartRow = startRow;
            rows = rws;
            cols = cls;
            if (Console.WindowWidth < startCol + cls)
            {
                Console.WindowWidth = startCol + cls;
            }
            if (Console.WindowHeight < startRow + rws + 8)
            {
                Console.WindowHeight = startRow + rws + 8;
            }
            Lines = new List<object>();
        }
        public Frame(int frameRows, int frameCols)
        {
            FrameRows = frameRows;
            FrameCols = frameCols;
            Console.WindowHeight = frameRows + 8;
            Console.WindowWidth = frameCols * 2 + 2;
        }
        public void Show()
        {
            Console.Write($"{LeftUpCorner}".PadRight(cols - 1, Liner) + RightUpCorner);
            for (int i = 1; i < rows; i++)
            {
                Console.SetCursorPosition(StartCol, StartRow + i);
                Console.Write(Border);
                Console.SetCursorPosition(cols - 1, i);
                Console.Write($"{Border}");
            }
            Console.SetCursorPosition(StartCol, rows);
            Console.Write($"{LeftDownCorner}".PadRight(cols - 1, Liner) + RightDownCorner);
        }
        void ShowLines()
        {

        }
        public void ShowOne(int rows, int cols, bool sub = false)
        {
            Console.SetCursorPosition(0, 0);
            string lineUp = "╔".PadRight(FrameCols * 2, '═') + "╗"; //"╦".PadRight(FrameWidth, '═') +
            string border = "║";
            string lineDown = "╚".PadRight(FrameCols * 2, '═') + "╝"; //"╩".PadRight(FrameWidth, '═') +
            Console.Write(lineUp);
            for (int i = 1; i < rows; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(border);
                Console.SetCursorPosition(FrameCols * 2, i);
                Console.Write($"{border}");
            }
            Console.SetCursorPosition(0, rows);
            Console.Write(lineDown);
            if (sub)
            {
                int subPositionV = rows + 1;
                ShowSub(subPositionV, cols);
            }
            FrameLeftRow = 1;
            FrameLeftCol = 1;
        }
        public void ShowTwo(int rows, int cols, bool sub = false)
        {
            Console.SetCursorPosition(0, 0);
            string lineUp = "╔".PadRight(FrameCols, '═') + "╦".PadRight(FrameCols, '═') + "╗";
            string border = "║";
            string lineDown = "╚".PadRight(FrameCols, '═') + "╩".PadRight(FrameCols, '═') + "╝";
            Console.Write(lineUp);
            for (int i = 1; i < rows; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(border);
                Console.SetCursorPosition(FrameCols - 1, i);
                Console.Write($" {border} ");
                Console.SetCursorPosition(FrameCols * 2, i);
                Console.Write(border);
            }
            Console.SetCursorPosition(0, rows);
            Console.Write(lineDown);
            if (sub)
            {
                ShowSub(rows + 1, cols);
            }
            FrameLeftRow = 1;
            FrameLeftCol = 1;
            FrameRightRow = 1;
            FrameRightCol = FrameCols + 2;
        }
        void ShowSub(int rows, int cols)
        {
            string lineUp = "╔".PadRight(cols * 2, '═') + "╗";
            string border = "║";
            string lineDown = "╚".PadRight(cols * 2, '═') + "╝";

            Console.SetCursorPosition(0, rows);
            Console.Write(lineUp);
            for (int i = rows + 1; i < rows + 4; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(border);
                Console.SetCursorPosition(cols * 2, i);
                Console.Write(border);
            }
            Console.SetCursorPosition(0, rows + 4);
            Console.Write(lineDown);
            FrameSubRow = rows + 1;
            FrameSubCol = 1;
        }

        public void WriteText(string str, int col, int row)
        {
            Console.SetCursorPosition(col + StartCol + 1, row + StartRow + 1);
            Console.Write(str.PadRight(cols - 2, ' '));
            Console.SetCursorPosition(col + StartCol + 1, row + StartRow + 1);
        }

        public void Message(string str, Frame fr)
        {
            fr.WriteText(str, 0, 0);
        }

        public void Message(string str, char frame)
        {
            int row;// = Console.WindowHeight / 2 - 4;
            int col;// = FrameCols / 2;
            switch (frame)
            {
                case 'L':
                case 'l':
                    Console.SetCursorPosition(FrameLeftCol, FrameLeftRow);
                    Console.Write(str);
                    break;
                case 'R':
                case 'r':
                    Console.SetCursorPosition(FrameRightCol, FrameRightRow);
                    Console.Write(str);
                    break;
                case 'S':
                case 's':
                    Console.SetCursorPosition(FrameSubCol, FrameSubRow);
                    Console.Write(str);
                    break;
                case 'N':
                case 'n':
                    row = Console.WindowHeight / 2 - 4;
                    col = FrameCols / 2;
                    Console.SetCursorPosition(col, row);
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("╔".PadRight(FrameCols, '═') + "╗");
                    for (int i = row + 1; i < row + 7; i++)
                    {
                        Console.SetCursorPosition(col, i);
                        Console.Write("║".PadRight(FrameCols, ' ') + "║");
                    }
                    Console.SetCursorPosition(col, row + 7);
                    Console.Write("╚".PadRight(FrameCols, '═') + "╝");
                    Console.SetCursorPosition(col + 1, row + 1);
                    Console.Write(str);
                    break;
                case 'W':
                case 'w':
                    row = Console.WindowHeight / 2 - 4;
                    col = FrameCols / 2;
                    Console.SetCursorPosition(col, row);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("╔".PadRight(FrameCols, '═') + "╗");
                    for (int i = row + 1; i < row + 7; i++)
                    {
                        Console.SetCursorPosition(col, i);
                        Console.Write("║".PadRight(FrameCols, ' ') + "║");
                    }
                    Console.SetCursorPosition(col, row + 7);
                    Console.Write("╚".PadRight(FrameCols, '═') + "╝");
                    Console.SetCursorPosition(col + 1, row + 1);
                    Console.Write(str);
                    break;
                default:
                    break;
            }

        }
        public void Coloring(Colorscheme scheme)
        {
            switch (scheme)
            {
                case Colorscheme.Default:
                    NormalBackGround = ConsoleColor.Black;
                    SelectedBackGround = ConsoleColor.White;
                    NormalText = ConsoleColor.White;
                    SelectedText = ConsoleColor.Black;

                    ContexMenuNormalBackGround = ConsoleColor.Gray;
                    ContexMenuSelectedBackGround = ConsoleColor.Yellow;
                    SelectedContext = ConsoleColor.Red;
                    NormalContext = ConsoleColor.Black;
                    break;
                case Colorscheme.BIOS:
                    NormalBackGround = ConsoleColor.Blue;
                    SelectedBackGround = ConsoleColor.Red;
                    NormalText = ConsoleColor.Yellow;
                    SelectedText = ConsoleColor.White;
                    ContexMenuNormalBackGround = ConsoleColor.Yellow;
                    ContexMenuSelectedBackGround = ConsoleColor.Red;
                    SelectedContext = ConsoleColor.Yellow;
                    NormalContext = ConsoleColor.Black;
                    break;
                default: break;
            }
        }
        public void SetColor(ColorsPreset preset)
        {
            switch (preset)
            {
                case ColorsPreset.Normal:
                    Console.BackgroundColor = NormalBackGround;
                    Console.ForegroundColor = NormalText;
                    break;
                case ColorsPreset.Selected:
                    Console.BackgroundColor = SelectedBackGround;
                    Console.ForegroundColor = SelectedText;
                    break;
                case ColorsPreset.ContextNormal:
                    Console.BackgroundColor = ContexMenuNormalBackGround;
                    Console.ForegroundColor = NormalContext;
                    break;
                case ColorsPreset.ContextSelected:
                    Console.BackgroundColor = ContexMenuSelectedBackGround;
                    Console.ForegroundColor = SelectedContext;
                    break;
                case ColorsPreset.Standart:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    break;
            }

        }
    }
}
