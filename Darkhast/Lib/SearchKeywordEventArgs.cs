using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Darkhast.Lib
{
    public class SearchKeywordEventArgs : EventArgs
    {
        private string _keyword;

        public SearchKeywordEventArgs(string keyword)
        {
            this._keyword = keyword;
        }

        public string Keyword
        {
            get { return _keyword; }
        }
    }
}
