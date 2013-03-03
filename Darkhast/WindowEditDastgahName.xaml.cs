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
    /// Interaction logic for WindowEditDastgahName.xaml
    /// </summary>
    public partial class WindowEditDastgahName : Window
    {

        private DatabaseEntities _entities = Lib.Global.Entities;
        public event EventHandler DastgahNameEdited;
        private Guid _bakhshGuid;
        private Guid _dastgahGuid;

        public void OnDastgahNameEdited(EventArgs e)
        {
            EventHandler handler = DastgahNameEdited;
            if (handler != null) handler(this, e);
        }

        public WindowEditDastgahName()
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

        public Guid DastgahGuid
        {
            get { return _dastgahGuid; }
            set { _dastgahGuid = value; }
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

            var dastgahQuery = from d in Entities.Dastgahhas
                               where d.DastgahGUID == DastgahGuid
                               select d;
            Dastgahha dastgah = dastgahQuery.FirstOrDefault();

            dastgah.DastgahName = TextBoxDastgahName.Text;
            dastgah.BakhshGUID = ((Bakhshha)ComboBoxBakhshName.SelectedItem).BakhshGUID;

            Entities.SaveChanges();

            StatusBar1.Items.Add("دستگاه با موفقیت ویرایش شد");
            TextBoxDastgahName.Text = "";
            OnDastgahNameEdited(new EventArgs());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadComboBoxBakhshName();

            var bakhshQuery = from b in Entities.Bakhshhas
                              where b.BakhshGUID == BakhshGuid
                              select b;

            ComboBoxBakhshName.SelectedItem = (Bakhshha)bakhshQuery.FirstOrDefault();

            var dastgahQuery = from d in Entities.Dastgahhas
                               where d.DastgahGUID == DastgahGuid
                               select d;

            TextBoxDastgahName.Text = ((Dastgahha)dastgahQuery.FirstOrDefault()).DastgahName;
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
