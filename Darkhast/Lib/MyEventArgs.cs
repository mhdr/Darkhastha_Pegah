using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Darkhast.Lib
{
    class MyEventArgs
    {
    }

    public class DateChangedEventArgs : EventArgs
    {
        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public DateChangedEventArgs(DateTime date)
        {
            Date = date;
        }
    }
}
