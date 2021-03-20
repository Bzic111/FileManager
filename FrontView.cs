using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace FileManager
{
    class FrontView
    {
        //delegate void Filler(List<Entry> lst, int StartIndex);

        public int FrameHeight;                 //42
        public int FrameWidth;                  //74
        public int LeftFrameCursorLeft;         // 1;
        public int RightFrameCursorLeft;        // FrameWidth + 2;
        public int WindowHeight;                // строки 50
        public int WindowWidth;                 // столбцы 150

        public FrontView(int frameHeight, int frameWidth)
        {
            FrameHeight = frameHeight;
            FrameWidth = frameWidth;
            LeftFrameCursorLeft = 1;
            RightFrameCursorLeft = frameWidth + 2;
            Console.WindowHeight = frameHeight + 8;
            Console.WindowWidth = frameWidth + 2;
        }
        void ShowFrame()
        {
            // ╔0═74╦75═149╗150
            Console.Clear();
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
        void FillLeftFrame(string[] filler)
        {
            Console.SetCursorPosition(LeftFrameCursorLeft, 1);
        }
        void FillRightFrame(string[] filler)
        {
            for (int i = 0; i < filler.Length; i++)
            {
            Console.SetCursorPosition(RightFrameCursorLeft, 1);
                Console.Write(filler[i]);
            }

        }

        string[] TableForScreen(List<Entry> lst, int StartIndex)
        {
            string[] result = new string[40];
            for (int i = 0; i < result.Length & i < lst.Count; i++)
            {
                if (lst[StartIndex].type == Entry.Type.Directory)
                {
                    result[i] = $"{lst[i].Name}".PadRight(FrameWidth,' ').Remove(FrameWidth/2) + lst[i].Extension;
                }
                else if (lst[StartIndex].type == Entry.Type.File)
                {
                    result[i] = $"{lst[i].Name}".PadRight(FrameWidth, ' ').Remove(FrameWidth / 2) + $"{lst[i].Extension}".PadRight(10, ' ').Remove(9)+ lst[i].Size;
                }
                if (i == lst.Count-1)
                {
                    for (; i < result.Length; i++)
                    {
                        result[i] = "".PadRight(FrameWidth - 1, ' ');
                    }
                }
            }
            return result;
        }
        string[] TreeForScreen(List<Entry> lst,int StartIndex)
        {
            string[] result = new string[40];
            for (int i = 0; i < result.Length & i < lst.Count; i++)
            {
                if (lst[StartIndex].type == Entry.Type.Directory & i < lst.Count-1)
                {
                    result[i] = $"╠═{lst[i].Name}".PadRight(FrameWidth, ' ').Remove(FrameWidth - 2);
                }
                else if (lst[StartIndex].type == Entry.Type.File & i < lst.Count - 1)
                {
                    result[i] = $"╟─{lst[i].Name}".PadRight(FrameWidth, ' ').Remove(FrameWidth - 2);
                }
                else if (i == lst.Count - 1)
                {
                    if (lst[StartIndex].type == Entry.Type.Directory)
                    {
                        result[i] = $"╚═{lst[i].Name}".PadRight(FrameWidth, ' ').Remove(FrameWidth - 2);
                    }
                    else if (lst[StartIndex].type == Entry.Type.File)
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




    }

}
