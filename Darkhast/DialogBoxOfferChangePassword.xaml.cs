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
    /// Interaction logic for DialogBoxOfferChangePassword.xaml
    /// </summary>
    public partial class DialogBoxOfferChangePassword : Window
    {
        public DialogBoxOfferChangePassword()
        {
            InitializeComponent();
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            WindowChangePassword windowChangePassword=new WindowChangePassword();
            windowChangePassword.Show();
            this.Close();
        }

        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
