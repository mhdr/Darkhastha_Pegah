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
    /// Interaction logic for WindowNewMessage.xaml
    /// </summary>
    public partial class WindowNewMessage : Window
    {
        private DatabaseEntities _entities = Lib.Global.Entities;

        public WindowNewMessage()
        {
            InitializeComponent();
        }

        public DatabaseEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        private void ButtonTo_Click(object sender, RoutedEventArgs e)
        {
            WindowMessageTo windowMessageTo=new WindowMessageTo();
            windowMessageTo.MessageToAdded += new Lib.MessageToEventHandler(windowMessageTo_MessageToAdded);
            windowMessageTo.ShowDialog();
        }

        void windowMessageTo_MessageToAdded(object sender, Lib.MessageToEventArgs e)
        {
            Barghkarha user = Entities.Barghkarhas.FirstOrDefault(x => x.BarghkarGUID == e.UserGuid);
            TextBoxTo.Text += string.Format("{0} {1} ( {2} );", user.FirstName, user.LastName, user.BarghkarGUID);
        }
    }
}
