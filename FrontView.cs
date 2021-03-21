using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace FileManager
{
    class FrontView
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

        public int FrameHeight;                 //42
        public int FrameWidth;                  //74
        public int FrameTop;                    // 2
        public int LeftFrameCursorLeft;         // 1;
        public int RightFrameCursorLeft;        // FrameWidth + 2;
        public int WindowHeight;                // строки 50
        public int WindowWidth;                 // столбцы 150
        string CurrentPathLeft;
        string CurrentPathRight;
        Colorscheme Scheme;
        ConsoleColor NormalBackGround;              // = Black;
        ConsoleColor SelectedBackGround;            // = White;
        ConsoleColor NormalText;                    //  = White;
        ConsoleColor SelectedText;                  //  = Black;
        ConsoleColor ContexMenuNormalBackGround;    //  = Gray;
        ConsoleColor ContexMenuSelectedBackGround;  //  = Yellow;
        ConsoleColor SelectedContext;               //  = Red;
        ConsoleColor NormalContext;                 //  = Black;
        List<Entry> Entryes;
        Comands Com;
        List<string> Memory;
        void Coloring(Colorscheme scheme)
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
        
        public FrontView(int frameHeight, int frameWidth, List<Entry> entryes, Colorscheme scheme = Colorscheme.Default)
        {
            Entryes = entryes;
            FrameHeight = frameHeight;
            FrameWidth = frameWidth;
            FrameTop = 2;
            LeftFrameCursorLeft = 1;
            RightFrameCursorLeft = frameWidth + 2;
            Console.WindowHeight = frameHeight + 8;
            Console.WindowWidth = frameWidth*2 + 2;
            Scheme = scheme;
            Coloring(Scheme);
            SetColor(ColorsPreset.Normal);
            Com = new Comands();
        }
        public void ShowFrame()
        {
            // ╔0═74╦75═149╗150
            Console.Clear();
            //SetColor(ColorsPreset.Standart);
            string lineUp = "╔".PadRight(FrameWidth, '═') + "╦".PadRight(FrameWidth, '═') + "╗";
            string border = "║".PadRight(FrameWidth, ' ') + "║".PadRight(FrameWidth, ' ') + "║";
            string lineDown = "╚".PadRight(FrameWidth, '═') + "╩".PadRight(FrameWidth, '═') + "╝";
            Console.WriteLine(lineUp);
            for (int i = 0; i < FrameHeight; i++)
            {
                Console.WriteLine(border);
            }
            Console.WriteLine(lineDown);
        }
        public void ShowContext(Dictionary<string, Comands.Comand> Cmds,int top,int left)
        {

            int currentLeft = left;
            int currentTop;// = top + 2;
            string[] keys = new string[Cmds.Count];
            Cmds.Keys.CopyTo(keys,0);
            if (top>40-Cmds.Count)
            {
                currentTop = top - Cmds.Count;
            }
            else
            {
                currentTop = top + 2;
            }
            for (int i = 0; i < Cmds.Count; i++)
            {
                SetColor(ColorsPreset.ContextNormal);
                Console.SetCursorPosition(currentLeft + 10, currentTop + i);
                Console.Write(keys[i]);
            }
            SetColor(ColorsPreset.Normal);
            Console.SetCursorPosition(currentLeft, currentTop);
        }
        public void SelectedString(int left,int top,int index)
        {
            SetColor(ColorsPreset.Selected);
            Console.SetCursorPosition(left, top);
            Console.Write(Entryes[index].Name);
            SetColor(ColorsPreset.Normal);
        }
        public void NormalString(int left, int top, int index)
        {
            SetColor(ColorsPreset.Normal);
            Console.SetCursorPosition(left, top);
            Console.Write(Entryes[index].Name);
            SetColor(ColorsPreset.Normal);
        }
        void WriteCurrentPath(string path)
        {
            Console.Write(path.PadRight(FrameWidth,' ').Remove(FrameWidth-1));
        }
        public void FillLeftFrame(string[] filler)
        {
            
            for (int i = 0; i < filler.Length; i++)
            {
                Console.SetCursorPosition(LeftFrameCursorLeft, FrameTop + i);
                Console.Write(filler[i]);
            }
        }
        public void FillRightFrame(string[] filler)
        {
            for (int i = 0; i < filler.Length; i++)
            {
                Console.SetCursorPosition(RightFrameCursorLeft, FrameTop + i);
                Console.Write(filler[i]);
            }
        }
        public string[] EntryesToArr(List<Entry> lst, int StartIndex=0)
        {
            if (lst.Count == 40)
            {
                string[] result = new string[40];
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].type == Entry.Type.Directory)
                    {
                        result[i] = lst[i].Name.PadRight(FrameWidth, ' ').Remove(FrameWidth / 2) + lst[i].Extension;
                    }
                    else if (lst[i].type == Entry.Type.File)
                    {
                        result[i] = lst[i].Name.PadRight(FrameWidth / 2 + 1, ' ').Remove(FrameWidth / 2) + lst[i].Extension.PadRight(11, ' ').Remove(10) + lst[i].Size;
                    }
                }
                return result;
            }
            else if (lst.Count<40)
            {
                string[] result = new string[lst.Count];
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].type == Entry.Type.Directory)
                    {
                        result[i] = lst[i].Name.PadRight(FrameWidth, ' ').Remove(FrameWidth / 2) + lst[i].Extension;
                    }
                    else if (lst[i].type == Entry.Type.File)
                    {
                        result[i] = lst[i].Name.PadRight(FrameWidth / 2 + 1, ' ').Remove(FrameWidth / 2) + lst[i].Extension.PadRight(11, ' ').Remove(10) + lst[i].Size;
                    }
                }
                return result;
            }
            else if (lst.Count > 40)
            {
                string[] result = new string[40];
                for (int i = StartIndex; i < result.Length&i<lst.Count; i++)
                {
                    if (lst[i].type == Entry.Type.Directory)
                    {
                        result[i] = lst[i].Name.PadRight(FrameWidth, ' ').Remove(FrameWidth / 2) + lst[i].Extension;
                    }
                    else if (lst[i].type == Entry.Type.File)
                    {
                        result[i] = lst[i].Name.PadRight(FrameWidth / 2 + 1, ' ').Remove(FrameWidth / 2) + lst[i].Extension.PadRight(11, ' ').Remove(10) + lst[i].Size;
                    }
                }
                return result;
            }
            else
            {
                return null;
            }
        }
        public List<string[]> ToPages(string[] arr)
        {
            int counter = 0;
            List<string[]> result = new List<string[]>();
            if (arr.Length == 40)
            {
                result.Add(new string[40]);
                for (int i = 0; i < arr.Length; i++)
                {
                    result[^1] = arr;
                }
            }
            else if (arr.Length > 40)
            {
                for (int i = 0; i <= (arr.Length / 40) + 1; i++)
                {
                    if (i == (arr.Length / 40) + 1)
                    {
                        result.Add(new string[arr.Length % 40]);
                    }
                    else
                    {
                        result.Add(new string[40]);
                    }
                    for (int j = 0; j < result[^1].Length & counter < arr.Length; j++)
                    {
                        result[^1][j] = arr[counter];
                        counter++;
                    }
                }
            }
            else if (arr.Length < 40)
            {
                result.Add(new string[arr.Length % 40]);
                for (int j = 0; j < result[^1].Length & counter < arr.Length; j++)
                {
                    result[^1][j] = arr[counter];
                    counter++;
                }
            }
            return result;
        }
        public string[] TableForScreen(List<Entry> lst, int StartIndex)
        {
            string[] result = new string[40];
            for (int i = StartIndex; i < result.Length & i < lst.Count; i++)
            {
                if (i == lst.Count-1 & i == result.Length)
                {
                    for (; i < result.Length; i++)
                    {
                        result[i] = "".PadRight(FrameWidth - 1, ' ');
                    }
                }
                if (lst[i].type == Entry.Type.Directory)
                {
                    result[i] = lst[i].Name.PadRight(FrameWidth,' ').Remove(FrameWidth/2) + lst[i].Extension;
                }
                else if (lst[i].type == Entry.Type.File)
                {
                    result[i] = lst[i].Name.PadRight(FrameWidth/2+1, ' ').Remove(FrameWidth / 2) + lst[i].Extension.PadRight(11, ' ').Remove(10)+ lst[i].Size;
                }
            }
            return result;
        }
        public string[] TreeForScreen(List<Entry> lst,int StartIndex)
        {
            string[] result = new string[40];
            for (int i = StartIndex; i < result.Length & i < lst.Count; i++)
            {
                if (lst[i].type == Entry.Type.Directory & i < lst.Count-1)
                {
                    result[i] = $"╠═{lst[i].Name}".PadRight(FrameWidth, ' ').Remove(FrameWidth - 2);
                }
                else if (lst[i].type == Entry.Type.File & i < lst.Count - 1)
                {
                    result[i] = $"╟─{lst[i].Name}".PadRight(FrameWidth, ' ').Remove(FrameWidth - 2);
                }
                else if (i == lst.Count - 1)
                {
                    if (lst[i].type == Entry.Type.Directory)
                    {
                        result[i] = $"╚═{lst[i].Name}".PadRight(FrameWidth, ' ').Remove(FrameWidth - 2);
                    }
                    else if (lst[i].type == Entry.Type.File)
                    {
                        result[i] = $"╚─{lst[i].Name}".PadRight(FrameWidth, ' ').Remove(FrameWidth - 2);
                    }
                }
            }            
            return result;
        }
        List<List<Entry>> FrameList(List<Entry> lst)
        {
            List<List<Entry>> Page = new List<List<Entry>>();
            for (int i = 0; i < lst.Count;)
            {
                Page.Add(new List<Entry>());
                for (int j = 0; j < 40 & i < lst.Count; j++, i++)
                {
                    Page[^1].Add(lst[i]);
                }
            }
            return Page;
        }

        public void SelectorMain()
        {
            int CursorLeft = LeftFrameCursorLeft;
            bool Cycle = true;
            int indexMin = FrameTop;
            int indexMax = FrameHeight - 2;
            int index = indexMin;
            do
            {
                CursorControl(ref index, ref CursorLeft, indexMax, indexMin, ref Cycle);

            } while (Cycle);
        }
        void SelectorContext(int top, int left, string path)
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
            SetColor(FrontView.ColorsPreset.Normal);
            Console.SetCursorPosition(CursorLeft, index);
            SelectedString(CursorLeft, index, index - indexMin);
            ConsoleKey key = Console.ReadKey().Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    NormalString(CursorLeft, index, index - indexMin);
                    index--;
                    if (index < indexMin)
                    {
                        if (indexMax >= Entryes.Count+ indexMin)
                        {
                            index = Entryes.Count - 1;
                        }
                        else
                        {
                            index = indexMax;
                        }
                    }
                    break;
                case ConsoleKey.DownArrow:
                    NormalString(CursorLeft, index, index - indexMin);
                    index++;
                    if (index > indexMax|index >= Entryes.Count+ indexMin)
                    {
                        index = indexMin;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    NormalString(CursorLeft, index, index - indexMin);
                    if (CursorLeft == LeftFrameCursorLeft)
                    {
                        CursorLeft = RightFrameCursorLeft;
                    }
                    else
                    {
                        CursorLeft = LeftFrameCursorLeft;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    NormalString(CursorLeft, index, index - indexMin);
                    if (CursorLeft == RightFrameCursorLeft)
                    {
                        CursorLeft = LeftFrameCursorLeft;
                    }
                    else
                    {
                        CursorLeft = RightFrameCursorLeft;
                    }
                    break;

                case ConsoleKey.Enter:
                    break;
                case ConsoleKey.Escape:
                    cycle = false;
                    break;
                case ConsoleKey.Applications:
                    ShowContext(Com.AllComands, index, CursorLeft);
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
