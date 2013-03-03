// -----------------------------------------------------------------------
// <copyright file="ConvertDate.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;

namespace TarikhFA
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ConvertDate
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public void SetDateS(int year, int month, int day)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
        }

        public void SetDateG(int year, int month, int day)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;

            PersianCalendar pc = new PersianCalendar();
            DateTime dt = new DateTime(year, month, day, 0, 0, 0, 0);
            SetDateS(pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));
        }

        public DateTime GetDateS()
        {
            return new DateTime(this.Year, this.Month, this.Day);
        }

        public DateTime GetDateG()
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.ToDateTime(this.Year, this.Month, this.Day, 0, 0, 0, 0);
        }
    }
}
