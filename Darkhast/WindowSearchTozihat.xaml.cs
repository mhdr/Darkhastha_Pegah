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
    /// Interaction logic for WindowSearchTozihat.xaml
    /// </summary>
    public partial class WindowSearchTozihat : Window
    {
        public event SearchKeywordEventHandler Search;

        public void OnSearch(SearchKeywordEventArgs e)
        {
            SearchKeywordEventHandler handler = Search;
            if (handler != null) handler(this, e);
        }


        public WindowSearchTozihat()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            ToggleButtonSearchRealTime.IsChecked = Properties.Settings.Default.SearchTozihatSearchRealTime;

            switch (Properties.Settings.Default.SearchTozihatSearchType)
            {
                case (int)SearchType.StarstWith:
                    RadioButtonStartsWith.IsChecked = true;
                    break;

                case (int)SearchType.EndsWith:
                    RadioButtonEndsWith.IsChecked = true;
                    break;


                case (int)SearchType.Contains:
                    RadioButtonContains.IsChecked = true;
                    break;

                case (int)SearchType.MatchCase:
                    RadioButtonMatchCase.IsChecked = true;
                    break;
            }

            if (ToggleButtonSearchRealTime.IsChecked == true)
            {
                ButtonSearch.IsEnabled = false;
            }
            else if (ToggleButtonSearchRealTime.IsChecked == false)
            {
                ButtonSearch.IsEnabled = true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            if (ToggleButtonSearchRealTime.IsChecked != null)
                Properties.Settings.Default.SearchTozihatSearchRealTime = (bool)ToggleButtonSearchRealTime.IsChecked;

            if (RadioButtonStartsWith.IsChecked == true)
            {
                Properties.Settings.Default.SearchTozihatSearchType = (int)SearchType.StarstWith;
            }
            else if (RadioButtonEndsWith.IsChecked == true)
            {
                Properties.Settings.Default.SearchTozihatSearchType = (int)SearchType.EndsWith;
            }
            else if (RadioButtonContains.IsChecked == true)
            {
                Properties.Settings.Default.SearchTozihatSearchType = (int)SearchType.Contains;
            }
            else if (RadioButtonMatchCase.IsChecked == true)
            {
                Properties.Settings.Default.SearchTozihatSearchType = (int)SearchType.MatchCase;
            }

            Properties.Settings.Default.Save();
        }

        private void ToggleButtonSearchRealTime_Checked(object sender, RoutedEventArgs e)
        {
            ButtonSearch.IsEnabled = false;
            OnSearch(new SearchKeywordEventArgs(TextBoxSearch.Text.Trim()));
        }

        private void ToggleButtonSearchRealTime_Unchecked(object sender, RoutedEventArgs e)
        {
            ButtonSearch.IsEnabled = true;
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            OnSearch(new SearchKeywordEventArgs(TextBoxSearch.Text.Trim()));
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ToggleButtonSearchRealTime.IsChecked == true)
            {
                OnSearch(new SearchKeywordEventArgs(TextBoxSearch.Text.Trim()));
            }
        }


        private void RadioButtonsChanged_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            OnSearch(new SearchKeywordEventArgs(TextBoxSearch.Text.Trim()));
        }

    }
}
