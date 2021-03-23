using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    class Frame
    {
        int FrameRows; //42
        int FrameCols; //74
        int PageCount;
        int FrameLeftRow;
        int FrameLeftCol;
        int FrameRightRow;
        int FrameRightCol;
        int FrameSubRow;
        int FrameSubCol;
        public Frame()
        {
                
        }
        public Frame(int frameRows, int frameCols)
        {
            FrameRows = frameRows;
            FrameCols = frameCols;
            Console.WindowHeight = frameRows + 8;
            Console.WindowWidth = frameCols * 2 + 2;
        }
        public void ShowOne(int rows, int cols, bool sub = false)
        {
            Console.SetCursorPosition(0,0);
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
        void SubMessage(string str)
        {

        }
    }
}
