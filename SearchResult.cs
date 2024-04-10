using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableExploration
{
    public class SearchResult
    {
        private int index;
        private string input;

        public SearchResult(int index, string input)
        {
            this.index = index;
            this.input = input;
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public string Input
        {
            get { return input; }
            set { input = value; }
        }
    }
}
