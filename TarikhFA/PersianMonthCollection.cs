using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace TarikhFA
{
    public class PersianMonthCollection : ObservableCollection<PersianMonth>
    {

        public PersianMonthCollection()
        {
            this.Add(new PersianMonth(1, "فروردین"));
            this.Add(new PersianMonth(2, "اردیبهشت"));
            this.Add(new PersianMonth(3, "خرداد"));
            this.Add(new PersianMonth(4, "تیر"));
            this.Add(new PersianMonth(5, "مرداد"));
            this.Add(new PersianMonth(6, "شهریور"));
            this.Add(new PersianMonth(7, "مهر"));
            this.Add(new PersianMonth(8, "آبان"));
            this.Add(new PersianMonth(9, "آذر"));
            this.Add(new PersianMonth(10, "دی"));
            this.Add(new PersianMonth(11, "بهمن"));
            this.Add(new PersianMonth(12, "اسفند"));
        }

    }
}
