using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    class Tab
    {
        public string Name;
        public int Page = 0;
        public int index = 0;
        public Tree tree;

        public Tab()
        {

        }
        public Tab(Tree tr)
        {
            tree = tr;
        }
    }
}
