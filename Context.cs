using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    class Context
    {
        Comands cmd;
        public Context()
        {

        }

        public void Create(Tab tab, ref int page, ref int index)
        {
            Frame warn = new Frame(30, 30, 5, 60);
            Frame question = new Frame(30, 30, 5, 60);

            question.Show();
            question.SetName("Creating");
            question.WriteName();
            question.WriteText($"Create Directory or File? [D/F] ?");
            var q = Console.ReadKey(true);
            cmd.Create(tab.WorkFrame.tree.Pages[page][index], q.KeyChar);

            tab.WorkFrame.tree.ReFresh();
            page = 0;
            index = 0;
            tab.WorkFrame.Refresh();
            tab.WorkFrame.GetContentFromTree(tab.WorkFrame.tree);
            tab.WorkFrame.ShowContentFromTree(page);
        }
    }
}
