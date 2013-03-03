using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Darkhast.Lib
{
    class PersianDate
    {
        private DateTime _date;

        public PersianDate(DateTime date)
        {
            this.Date = date;
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public DateTime ConvertMiladiToShamsi()
        {
            PersianCalendar pc = new PersianCalendar();

            return new DateTime(pc.GetYear(Date), pc.GetMonth(Date), pc.GetDayOfMonth(Date));
        }

        public DateTime ConvertShamsiToMiladi()
        {
            PersianCalendar pc=new PersianCalendar();

            return pc.ToDateTime(Date.Year, Date.Month, Date.Day, 0, 0, 0, 0);
        }
    }
}
