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

namespace Darkhast
{
    /// <summary>
    /// Interaction logic for WindowTimerLoadGridViewDarkhastha.xaml
    /// </summary>
    public partial class WindowTimerLoadGridViewDarkhastha : Window
    {
        public event EventHandler SettingsChenged;

        private void OnSettingsChenged(EventArgs e)
        {
            EventHandler handler = SettingsChenged;
            if (handler != null) handler(this, e);
        }

        public WindowTimerLoadGridViewDarkhastha()
        {
            InitializeComponent();
        }

        private void CheckBoxTimerActive_Checked(object sender, RoutedEventArgs e)
        {
            NumericUpDownTimerPeriod.IsEnabled = true;
        }

        private void CheckBoxTimerActive_Unchecked(object sender, RoutedEventArgs e)
        {
            NumericUpDownTimerPeriod.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();

            //if (Properties.Settings.Default.TimerLoadGridViewDarkhasthaIsActive == false)
            //{
            //    NumericUpDownTimerPeriod.IsEnabled = false;
            //}
        }

        private void LoadSettings()
        {
            CheckBoxTimerActive.IsChecked = Properties.Settings.Default.TimerLoadGridViewDarkhasthaIsActive;
            NumericUpDownTimerPeriod.Value = Properties.Settings.Default.TimerLoadGridViewDarkhasthaPeriod / 1000;
        }

        private void SaveSettings()
        {
            if (CheckBoxTimerActive.IsChecked == true)
            {
                Properties.Settings.Default.TimerLoadGridViewDarkhasthaIsActive = true;
            }
            else
            {
                Properties.Settings.Default.TimerLoadGridViewDarkhasthaIsActive = false;
            }

            Properties.Settings.Default.TimerLoadGridViewDarkhasthaPeriod = Convert.ToInt32(NumericUpDownTimerPeriod.Value * 1000);
            Properties.Settings.Default.Save();
            OnSettingsChenged(new EventArgs());
        }

        private void NumericUpDownTimerPeriod_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            if (NumericUpDownTimerPeriod.Value == 0)
            {
                NumericUpDownTimerPeriod.Value = 1;
            }

            ButtonSave.IsEnabled = true;
        }

        private void CheckBoxTimerActive_Click(object sender, RoutedEventArgs e)
        {
            ButtonSave.IsEnabled = true;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            ButtonSave.IsEnabled = false;
        }

        private void NumericUpDownTimerPeriod_KeyUp(object sender, KeyEventArgs e)
        {
            if (NumericUpDownTimerPeriod.Value == 0)
            {
                NumericUpDownTimerPeriod.Value = 1;
            }

            ButtonSave.IsEnabled = true;
        }
    }
}
