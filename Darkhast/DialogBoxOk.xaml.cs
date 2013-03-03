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
    /// Interaction logic for DialogBoxOk.xaml
    /// </summary>
    public partial class DialogBoxOk : Window
    {
        private string _message;

        public DialogBoxOk()
        {
            InitializeComponent();
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.TextBlockMessage.Text = this.Message;
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
