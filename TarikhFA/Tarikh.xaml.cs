using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace TarikhFA
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Tarikh : UserControl
    {
        public Tarikh()
        {
            InitializeComponent();
        }

        public event EventHandler DateChanged;

        public void OnDateChanged(EventArgs e)
        {
            EventHandler handler = DateChanged;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// tarikh ra be soorat miladi migirad va be soorat shamsi namayash midahad
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        public void SetDateG(int year, int month, int day)
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = new DateTime(year, month, day, 0, 0, 0, 0);
            SetDateS(pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));
        }

        /// <summary>
        /// tarikh ra be soorat shamsi migirad va be soorat shamsi namayesh midahad
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        public void SetDateS(int year, int month, int day)
        {
            ComboBoxYear.SelectedValue = year;
            ComboBoxMonth.SelectedValue = month;
            ComboBoxDay.SelectedValue = day;

            FillComboBoxDay(year, month);
        }

        /// <summary>
        /// tarikh entekhab shode ra be soorat shamsi bar migardand
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateS()
        {
            int year = 0;
            int month = 0;
            int day = 0;
            DateTime result = new DateTime();

            try
            {
                year = Convert.ToInt32(ComboBoxYear.SelectedValue);
                month = Convert.ToInt32(ComboBoxMonth.SelectedValue);
                day = Convert.ToInt32(ComboBoxDay.SelectedValue);
            }
            catch (Exception)
            {

            }

            if (year > 0 && month > 0 && day > 0)
            {
                result = new DateTime(year, month, day, 0, 0, 0, 0);
            }

            return result;
        }

        /// <summary>
        /// tarikh entekhab shode ra be soorat miladi bar migardand
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateG()
        {
            int year = 0;
            int month = 0;
            int day = 0;
            DateTime result = new DateTime();

            try
            {
                year = Convert.ToInt32(ComboBoxYear.SelectedValue);
                month = Convert.ToInt32(ComboBoxMonth.SelectedValue);
                day = Convert.ToInt32(ComboBoxDay.SelectedValue);
            }
            catch (Exception)
            {

            }

            if (year > 0 && month > 0 && day > 0)
            {
                PersianCalendar pc = new PersianCalendar();
                result = pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            }

            return result;
        }

        private void ComboBoxYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxMonth.SelectedIndex != -1)
            {
                FillComboBoxDay(Convert.ToInt32(ComboBoxYear.SelectedValue), Convert.ToInt32(ComboBoxMonth.SelectedValue));
            }

            if (IsDate())
            {
                OnDateChanged(new EventArgs());
            }
        }

        private void ComboBoxMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxYear.SelectedIndex != -1)
            {
                FillComboBoxDay(Convert.ToInt32(ComboBoxYear.SelectedValue), Convert.ToInt32(ComboBoxMonth.SelectedValue));
            }

            if (IsDate())
            {
                OnDateChanged(new EventArgs());
            }
        }

        /// <summary>
        /// ba tavaje be inke mahe dade shode dar sale dade shode chand rooze ast ComboBoxDay ra por mikond
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        private void FillComboBoxDay(int year, int month)
        {
            PersianCalendar pc = new PersianCalendar();

            if (month == 12)
            {
                bool isLeap = pc.IsLeapYear(year);
                PersianDayCollection pdc = new PersianDayCollection(29);
                ComboBoxDay.ItemsSource = pdc;
            }
            else if (month < 7)
            {
                PersianDayCollection pdc = new PersianDayCollection(31);
                ComboBoxDay.ItemsSource = pdc;
            }
            else if (month >= 7)
            {
                PersianDayCollection pdc = new PersianDayCollection(30);
                ComboBoxDay.ItemsSource = pdc;
            }
        }

        /// <summary>
        /// baraye shekl gerftan tarikh har 3 combobox bayad entekhab beshan
        /// in tabe check mikone ke har 3 entekhab shodan ya na
        /// </summary>
        /// <returns></returns>
        private bool IsDate()
        {
            if (ComboBoxYear.SelectedIndex != -1 && ComboBoxMonth.SelectedIndex != -1 && ComboBoxDay.SelectedIndex != -1)
            {
                return true;
            }

            return false;
        }

        private void ComboBoxDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsDate())
            {
                OnDateChanged(new EventArgs());
            }
        }

        private void ComboBoxDay_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsDate())
            {
                ComboBoxDay.ToolTip = CalculateDayOfWeek();
            }
        }

        private string CalculateDayOfWeek()
        {
            DateTime dt = GetDateG();
            PersianCalendar pc = new PersianCalendar();

            System.DayOfWeek day = pc.GetDayOfWeek(dt);

            return DayOfWeek.GetDayofWeekName((int)day);
        }
    }
}
