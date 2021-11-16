using FileManager.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager.Old
{
    class Context
    {
        Frame frame;
        public Context(Entry entr, int index)
        {
            if (entr.type == Entry.Type.Directory)
            {
                frame = index < 15 ? new Frame(8, index + 2, 25, 80) : new Frame(8, index - 14, 25, 80);
                frame.FrameName = entr.Name;
                frame.tree = new Tree();
                frame.tree.ChangeDirectory(entr.Path + '\\' + entr.Name);
                frame.SetPages(frame.tree.Entryes);
            }
        }
        void Show() => frame.Show(true, true, ColorsPreset.ContextNormal);
        public void Control()
        {
            bool Cycle = true;
            Show();
            int page = 0;
            int index = 0;

            do
            {
                frame.SetColor(ColorsPreset.ContextSelected);
                frame.WriteText(frame.Pages[page][index], 0, index);
                frame.SetColor(ColorsPreset.ContextNormal);

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.PageDown: frame.Go(To.NextPage, ref page, ref index); break;
                    case ConsoleKey.PageUp: frame.Go(To.PreviousPage, ref page, ref index); break;
                    case ConsoleKey.UpArrow: frame.Go(To.StepUp, ref page, ref index); break;
                    case ConsoleKey.DownArrow: frame.Go(To.StepDown, ref page, ref index); break;
                    case ConsoleKey.Applications: break;

                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.Escape:
                    default: Cycle = false; break;
                }
            } while (Cycle);
        }

    }
}
