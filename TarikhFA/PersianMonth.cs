using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TarikhFA
{
    public class PersianMonth
    {
        private readonly int _monthNumber;
        private readonly string _monthName;

        public PersianMonth(int number, string name)
        {
            this._monthNumber = number;
            this._monthName = name;
        }

        public int MonthNumber
        {
            get { return _monthNumber; }
        }

        public string MonthName
        {
            get { return _monthName; }
        }

        public static string GetMonthName(int monthNumber)
        {
            if (monthNumber > 12 || monthNumber < 1)
            {
                return string.Empty;
            }

            PersianMonthCollection persianMonthCollection = new PersianMonthCollection();
            var persianMonth = persianMonthCollection.Where(x => x.MonthNumber == monthNumber);

            return persianMonth.FirstOrDefault().MonthName;
        }
    }
}
