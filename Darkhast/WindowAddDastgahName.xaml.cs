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
    /// Interaction logic for WindowAddDastgahName.xaml
    /// </summary>
    public partial class WindowAddDastgahName : Window
    {

        private DatabaseEntities _entities = Lib.Global.Entities;
        private Guid _bakhshGUID;
        public event EventHandler DastgahNameAdded;

        public void OnDastgahNameAdded(EventArgs e)
        {
            EventHandler handler = DastgahNameAdded;
            if (handler != null) handler(this, e);
        }

        public WindowAddDastgahName()
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
            get { return _bakhshGUID; }
            set { _bakhshGUID = value; }
        }

        private void ButtonAddDastgahName_Click(object sender, RoutedEventArgs e)
        {
            StatusBar1.Items.Clear();

            if (ComboBoxBakhshName.SelectedIndex == -1)
            {
                StatusBar1.Items.Add("ابتدا یک بخش را انتخاب کنید");
                return;
            }

            if (TextBoxDastgahName.Text.Equals(""))
            {
                StatusBar1.Items.Add("نام دستگاه نمی تواند خالی باشد");
                return;
            }

            Dastgahha dastgah = new Dastgahha();
            dastgah.DastgahGUID = Guid.NewGuid();
            dastgah.DastgahName = TextBoxDastgahName.Text;
            dastgah.BakhshGUID = ((Bakhshha)ComboBoxBakhshName.SelectedItem).BakhshGUID;
            Entities.Dastgahhas.AddObject(dastgah);

            Entities.SaveChanges();

            StatusBar1.Items.Add("دستگاه جدید با موفقیت ثبت شد");
            TextBoxDastgahName.Text = "";
            OnDastgahNameAdded(new EventArgs());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadComboBoxBakhshName();

            var bakhshQuery = from b in Entities.Bakhshhas
                              where b.BakhshGUID == BakhshGuid
                              select b;

            ComboBoxBakhshName.SelectedItem = (Bakhshha)bakhshQuery.FirstOrDefault();
        }

        public void LoadComboBoxBakhshName()
        {
            var bakhshQuery = from b in Entities.Bakhshhas orderby b.BakhshName select b;
            ComboBoxBakhshName.ItemsSource = bakhshQuery.ToList();

            if (ComboBoxBakhshName.Items.Count > 0)
            {
                ComboBoxBakhshName.SelectedIndex = 0;
            }
        }
    }
}
