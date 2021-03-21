using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    class Selector
    {
        int FrameHeight;                 //42
        int FrameWidth;                  //74
        int FrameTop;                    // 2
        int LeftFrameCursorLeft;         // 1;
        int RightFrameCursorLeft;        // FrameWidth + 2;
        List<string> Memory;
        FrontView Front;
        Comands Com;
        public Selector(FrontView FW)
        {
            FrameHeight = FW.FrameHeight;
            FrameWidth = FW.FrameWidth;
            FrameTop = FW.FrameTop;
            LeftFrameCursorLeft = FW.LeftFrameCursorLeft;
            RightFrameCursorLeft = FW.RightFrameCursorLeft;
            Front = FW;
            Com = new Comands();
        }
        public void SelectorMain()
        {
            int CursorLeft = Front.LeftFrameCursorLeft;
            bool Cycle = true;
            int indexMin = Front.FrameTop + 1;
            int indexMax = Front.FrameHeight - 2;
            int index = indexMin;
            do
            {
                CursorControl(ref index,ref CursorLeft, indexMax, indexMin, ref Cycle);

            } while (Cycle);
        }
        void SelectorContext(int top, int left,string path)
        {
            int index = 0;
            int currentTop;
            if (top > 40 - Com.AllComands.Count)
            {
                currentTop = top - Com.AllComands.Count;
            }
            else
            {
                currentTop = top + 2;
            }
            int currentLeft = left;
            bool cycle = true;
            string[] keys = new string[Com.AllComands.Count];
            Com.AllComands.Keys.CopyTo(keys, 0);
            do
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Enter:
                        Com.AllComands.GetValueOrDefault(keys[index])(path);
                        cycle = false;
                        break;
                    case ConsoleKey.Escape:
                        cycle = false;
                        break;
                    case ConsoleKey.UpArrow:
                        if (index > 0)
                        {
                            index--;
                            currentTop--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (index < Com.AllComands.Count)
                        {
                            index++;
                            currentTop++;
                        }
                        break;
                    default:
                        break;
                }
            } while (cycle);
        }
        void ConsoleReader(int top, int left, string path)
        {
            StringBuilder sb = new StringBuilder();
            Comands com = new Comands();
            bool cycle = true;
            int index = 0;
            string temp;
            int x;
            char c;
            int cursorTop = top;
            int cursorLeft = left;
            Console.SetCursorPosition(0, cursorTop);
            Console.Write(path + ">");
            do
            {
                Console.SetCursorPosition(cursorLeft, cursorTop);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (Memory.Count > 0 & index > 0)
                        {
                            index--;
                            sb.Clear();
                            sb.Append(Memory[index]);
                            cursorLeft = 0;
                            Console.Write(sb.ToString());
                            cursorLeft = sb.ToString().Length;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (Memory.Count > 0 & index < Memory.Count)
                        {
                            index++;
                            sb.Clear();
                            sb.Append(Memory[index]);
                            cursorLeft = 0;
                            Console.Write(sb.ToString());
                            cursorLeft = sb.ToString().Length;
                        }
                        break;
                    case ConsoleKey.Backspace:
                        sb.Remove(sb.Length - 1, 1);
                        cursorLeft = 0;
                        Console.Write(sb.ToString().PadRight(sb.Length + 1, ' '));
                        cursorLeft = sb.Length;
                        break;
                    case ConsoleKey.Enter:
                        com.ReadCommand(sb.ToString());
                        break;
                    case ConsoleKey.Escape:
                    case ConsoleKey.Tab:
                        cycle = false;
                        break;
                    default:
                        x = Console.Read();
                        c = Convert.ToChar(x);
                        sb.Append(c);
                        cursorLeft = 0;
                        Console.Write(sb.ToString());
                        cursorLeft = sb.Length;
                        break;
                }
            } while (cycle);
        }
        void CursorControl(ref int index, ref int CursorLeft, int indexMax, int indexMin, ref bool cycle)
        {
            Front.SetColor(FrontView.ColorsPreset.Normal);
            Front.SelectedString(CursorLeft, index, index - indexMin);
            ConsoleKey key = Console.ReadKey().Key;
            Console.SetCursorPosition(CursorLeft, index);
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    Front.NormalString(CursorLeft, index, index - indexMin);
                    index--;
                    if (index < indexMin)
                    {                        
                        index = indexMax;
                    }                    
                    break;
                case ConsoleKey.DownArrow:
                    Front.NormalString(CursorLeft, index, index - indexMin);
                    index++;
                    if (index > indexMax)
                    {
                        index = indexMin;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    Front.NormalString(CursorLeft, index, index - indexMin);
                    if (CursorLeft == Front.LeftFrameCursorLeft)
                    {
                        CursorLeft = Front.RightFrameCursorLeft;
                    }
                    else
                    {
                        CursorLeft = Front.LeftFrameCursorLeft;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    Front.NormalString(CursorLeft, index, index - indexMin);
                    if (CursorLeft == Front.RightFrameCursorLeft)
                    {
                        CursorLeft = Front.LeftFrameCursorLeft;
                    }
                    else
                    {
                        CursorLeft = Front.RightFrameCursorLeft;
                    }
                    break;

                case ConsoleKey.Enter:
                    break;
                case ConsoleKey.Escape:
                    cycle = false;
                    break;
                case ConsoleKey.Applications:
                    Front.ShowContext(Com.AllComands, index, CursorLeft);
                    //SelectorContext(index, CursorLeft,);
                    break;



                case ConsoleKey.Backspace:
                    break;
                case ConsoleKey.Tab:
                    Memory.Add(Console.ReadLine());
                    break;


                case ConsoleKey.PageUp:
                    break;
                case ConsoleKey.PageDown:
                    break;


                case ConsoleKey.F1:
                    break;
                case ConsoleKey.F2:
                    break;
                case ConsoleKey.F3:
                    break;
                case ConsoleKey.F4:
                    break;

                default:
                    break;
            }
        }
        void Context()
        {

        }
    }
}
