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
    public partial class WindowEditBakhshName : Window
    {

        private DatabaseEntities _entities = Lib.Global.Entities;
        private Guid _bakhshGuid;
        public event EventHandler BakhshNameEdited;

        public void OnBakhshNameEdited(EventArgs e)
        {
            EventHandler handler = BakhshNameEdited;
            if (handler != null) handler(this, e);
        }

        public WindowEditBakhshName()
        {
            InitializeComponent();
        }

        public DatabaseEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public Guid BakhshGuid
        {
            get { return _bakhshGuid; }
            set { _bakhshGuid = value; }
        }

        private void ButtonEditBakhshName_Click(object sender, RoutedEventArgs e)
        {
            StatusBar1.Items.Clear();

            if (TextBoxBakhshName.Text.Equals(""))
            {
                StatusBar1.Items.Add("نام بخش نمی تواند خالی باشد");
                return;
            }

            var bakhshQuery = from b in Entities.Bakhshhas
                              where b.BakhshGUID == this.BakhshGuid
                              select b;

            Bakhshha bakhsh = bakhshQuery.FirstOrDefault();

            bakhsh.BakhshName = TextBoxBakhshName.Text;

            Entities.SaveChanges();

            StatusBar1.Items.Add("نام بخش با موفقیت ویرایش شد");
            TextBoxBakhshName.Text = "";
            OnBakhshNameEdited(new EventArgs());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var bakhshQuery = from b in Entities.Bakhshhas
                              where b.BakhshGUID == this.BakhshGuid
                              select b;

            Bakhshha bakhsh = bakhshQuery.FirstOrDefault();

            TextBoxBakhshName.Text = bakhsh.BakhshName;
        }

    }
}
