using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace TarikhFA
{
    public class PersianYearCollection : ObservableCollection<int>
    {
        public PersianYearCollection()
        {
            int beginYear = 1390;
            int endYear;

            var pc = new PersianCalendar();

            endYear = pc.GetYear(DateTime.Now);

            for (int i = beginYear; i <= endYear; i++)
            {
                this.Add(i);
            }
        }
    }
}
