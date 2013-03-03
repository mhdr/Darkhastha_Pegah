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
    /// Interaction logic for WindowAddBakhshName.xaml
    /// </summary>
    public partial class WindowAddBakhshName : Window
    {

        private DatabaseEntities _entities = Lib.Global.Entities;
        public event EventHandler BakhshNameAdded;

        public void OnBakhshNameAdded(EventArgs e)
        {
            EventHandler handler = BakhshNameAdded;
            if (handler != null) handler(this, e);
        }

        public WindowAddBakhshName()
        {
            InitializeComponent();
        }

        public DatabaseEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        private void ButtonAddBakhshName_Click(object sender, RoutedEventArgs e)
        {
            StatusBar1.Items.Clear();

            if (TextBoxBakhshName.Text.Equals(""))
            {
                StatusBar1.Items.Add("نام بخش نمی تواند خالی باشد");
                return;
            }

            Bakhshha bakhsh = new Bakhshha();
            bakhsh.BakhshGUID = Guid.NewGuid();
            bakhsh.BakhshName = TextBoxBakhshName.Text;
            Entities.Bakhshhas.AddObject(bakhsh);

            Entities.SaveChanges();

            StatusBar1.Items.Add("بخش جدید با موفقیت ثبت شد");
            TextBoxBakhshName.Text = "";
            OnBakhshNameAdded(new EventArgs());
        }
    }
}
