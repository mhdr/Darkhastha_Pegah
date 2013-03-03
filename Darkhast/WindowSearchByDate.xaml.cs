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
using System.Windows.Shapes;
using Darkhast.Lib;

namespace Darkhast
{
    /// <summary>
    /// Interaction logic for WindowSearchByDate.xaml
    /// </summary>
    public partial class WindowSearchByDate : Window
    {
        public event DateChangedEventHandler StartDateChanged;
        public event EventHandler SearchByDate;

        public void OnSearchByDate(EventArgs e)
        {
            EventHandler handler = SearchByDate;
            if (handler != null) handler(this, e);
        }

        public void OnStartDateChanged(DateChangedEventArgs e)
        {
            DateChangedEventHandler handler = StartDateChanged;
            if (handler != null) handler(this, e);
        }

        public event DateChangedEventHandler EndDateChanged;

        public void OnEndDateChanged(DateChangedEventArgs e)
        {
            DateChangedEventHandler handler = EndDateChanged;
            if (handler != null) handler(this, e);
        }

        public WindowSearchByDate()
        {
            InitializeComponent();
        }

        private void TarkhStartDate_DateChanged(object sender, EventArgs e)
        {
            OnStartDateChanged(new DateChangedEventArgs(TarkhStartDate.GetDateG()));
        }

        private void TarikhEndDate_DateChanged(object sender, EventArgs e)
        {
            DateTime endDate = TarikhEndDate.GetDateG();
            //endDate = endDate.AddDays(1);
            OnEndDateChanged(new DateChangedEventArgs(endDate));
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            OnSearchByDate(new EventArgs());
        }
    }


}
