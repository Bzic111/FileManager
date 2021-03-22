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
            //Console.WindowHeight = frameHeight + 8;
            //Console.WindowWidth = frameWidth*2 + 2;
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
        public void ShowContext(Dictionary<string, Comands.Comand> Cmds, int top, int left)
        {

            int currentLeft = left;
            int currentTop;// = top + 2;
            string[] keys = new string[Cmds.Count];
            Cmds.Keys.CopyTo(keys, 0);
            if (top > 40 - Cmds.Count)
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
        public void SelectedString(int left, int top, int index)
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
            Console.Write(path.PadRight(FrameWidth, ' ').Remove(FrameWidth - 1));
        }
        public void FillLeftFrame(List<Entry> filler)
        {

            for (int i = 0; i < filler.Count; i++)
            {
                Console.SetCursorPosition(LeftFrameCursorLeft, FrameTop + i);
                Console.Write(filler[i].ShortInfo);
            }
            if (filler.Count < 40)
            {
                for (int i = filler.Count; i < 41; i++)
                {
                    Console.SetCursorPosition(LeftFrameCursorLeft, FrameTop + i);
                    Console.Write("".PadRight(72, ' '));
                }
            }
        }
        public void FillRightFrame(List<Entry> filler)
        {
            for (int i = 0; i < filler.Count; i++)
            {
                Console.SetCursorPosition(RightFrameCursorLeft, FrameTop + i);
                Console.Write(filler[i].ShortInfo);
            }
            if (filler.Count < 40)
            {
                for (int i = filler.Count; i < 40; i++)
                {
                    Console.SetCursorPosition(RightFrameCursorLeft, FrameTop + i);
                    Console.Write("".PadRight(72, ' '));
                }
            }
        }
        public string[] EntryesToArr(List<Entry> lst, int StartIndex = 0)
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
            else if (lst.Count < 40)
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
                for (int i = StartIndex; i < result.Length & i < lst.Count; i++)
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
        public List<List<Entry>> ToPages(List<Entry> Entryes)
        {
            List<List<Entry>> Pages = new List<List<Entry>>();
            for (int i = 0, counter = 0; counter < Entryes.Count; i++)
            {
                Pages.Add(new List<Entry>());
                for (int j = 0; j < 40 & counter < Entryes.Count; j++, counter++)
                {
                    Pages[i].Add(Entryes[counter]);
                }
            }
            return Pages;
        }
        public string[] TableForScreen(List<Entry> lst, int StartIndex)
        {
            string[] result = new string[40];
            for (int i = StartIndex; i < result.Length & i < lst.Count; i++)
            {
                if (i == lst.Count - 1 & i == result.Length)
                {
                    for (; i < result.Length; i++)
                    {
                        result[i] = "".PadRight(FrameWidth - 1, ' ');
                    }
                }
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
        public string[] TreeForScreen(List<Entry> lst, int StartIndex)
        {
            string[] result = new string[40];
            for (int i = StartIndex; i < result.Length & i < lst.Count; i++)
            {
                if (lst[i].type == Entry.Type.Directory & i < lst.Count - 1)
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




    }

}
