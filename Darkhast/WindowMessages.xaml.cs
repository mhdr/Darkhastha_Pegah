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
using Telerik.Windows.Controls;

namespace Darkhast
{
    /// <summary>
    /// Interaction logic for WindowMessages.xaml
    /// </summary>
    public partial class WindowMessages : Window
    {
        private DatabaseEntities _entities = Lib.Global.Entities;

        public WindowMessages()
        {
            LocalizationManager.Manager = new MyLocalization();
            InitializeComponent();
        }

        public DatabaseEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void LoadGridViewMessages()
        {
            
        }

        private void RibbonButtonNewMessage_Click(object sender, RoutedEventArgs e)
        {
            WindowNewMessage windowNewMessage=new WindowNewMessage();
            windowNewMessage.Show();
        }
    }
}
