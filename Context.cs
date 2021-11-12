using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    class Context
    {
        Frame frame;
        public Context(Entry entr, int index)
        {
            if (entr.type == Entry.Type.Directory)
            {
                frame = new Frame(0, 0, 25, 80);
                if (index < 15)
                {
                    frame.StartCol = 8;
                    frame.StartRow = index + 2;
                }
                else
                {
                    frame.StartCol = 8;
                    frame.StartRow = index - 14;
                }
                frame.FrameName = entr.Name;
                frame.tree = new Tree();
                frame.tree.ChangeDirectory(entr.Path + '\\' + entr.Name);
                frame.SetPages(frame.tree.Entryes);
            }
        }

        void Show()
        {
            frame.Show(true, true, Frame.ColorsPreset.ContextNormal);
        }
        public void Control()
        {
            bool Cycle = true;
            Show();
            int page = 0;
            int index = 0;

            do
            {
                frame.SetColor(Frame.ColorsPreset.ContextSelected);
                frame.WriteText(frame.Pages[page][index], 0, index);
                frame.SetColor(Frame.ColorsPreset.ContextNormal);

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.PageDown: frame.Go(Frame.To.NextPage, ref page, ref index); break;
                    case ConsoleKey.PageUp: frame.Go(Frame.To.PreviousPage, ref page, ref index); break;
                    case ConsoleKey.UpArrow: frame.Go(Frame.To.StepUp, ref page, ref index); break;
                    case ConsoleKey.DownArrow: frame.Go(Frame.To.StepDown, ref page, ref index); break;
                    case ConsoleKey.Applications: break;

                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.Escape:
                    default: Cycle = false; break;
                }
            } while (Cycle);
        }

    }
}
