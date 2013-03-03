using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace TarikhFA
{
    public class DayOfWeekCollection : Collection<DayOfWeek>
    {
        public DayOfWeekCollection()
        {
            this.Add(new DayOfWeek(6, "شنبه"));
            this.Add(new DayOfWeek(0, "يکشنبه"));
            this.Add(new DayOfWeek(1, "دوشنبه"));
            this.Add(new DayOfWeek(2, "سه شنبه"));
            this.Add(new DayOfWeek(3, "چهارشنبه"));
            this.Add(new DayOfWeek(4, "پنج شنبه"));
            this.Add(new DayOfWeek(5, "جمعه"));
        }

        //public DayOfWeek GetDayName(int dayNumber)
        //{
        //    var dayQuery = from d in this where d.DayNumber == dayNumber select d;
        //    DayOfWeek day = dayQuery.FirstOrDefault();

        //    return day;
        //}
    }
}
