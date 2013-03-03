using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace TarikhFA
{
    public class PersianDayCollection : ObservableCollection<int>
    {
        public PersianDayCollection(int daysNumber)
        {
            var pc = new PersianCalendar();

            for (int i = 1; i <= daysNumber; i++)
            {
                this.Add(i);
            }
        }
    }

}
