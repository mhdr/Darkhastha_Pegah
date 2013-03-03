using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TarikhFA
{
    public class DayOfWeek
    {
        private int _dayNumber;
        private string _dayName;

        public DayOfWeek()
        {

        }

        public DayOfWeek(int dayNumber, string dayName)
        {
            this.DayNumber = dayNumber;
            this.DayName = dayName;
        }

        public int DayNumber
        {
            get { return _dayNumber; }
            set { _dayNumber = value; }
        }

        public string DayName
        {
            get { return _dayName; }
            set { _dayName = value; }
        }

        public static string GetDayofWeekName(int dayNumber)
        {
            DayOfWeekCollection dayOfWeekCollection = new DayOfWeekCollection();
            var dayQuery = dayOfWeekCollection.Where(x => x.DayNumber == dayNumber);

            return dayQuery.FirstOrDefault().DayName;
        }
    }
}
