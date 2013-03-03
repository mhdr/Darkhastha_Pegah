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
    /// Interaction logic for WindowDastgaha.xaml
    /// </summary>
    public partial class WindowDastgaha : Window
    {
        private DatabaseEntities _entities = Lib.Global.Entities;

        public WindowDastgaha()
        {
            InitializeComponent();
        }

        public DatabaseEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadComboBoxBakhshName();
        }

        private void LoadComboBoxBakhshName()
        {
            var bakhshQuery = from b in Entities.Bakhshhas orderby b.BakhshName select b;
            ComboBoxBakhshName.ItemsSource = bakhshQuery.ToList();
            ComboBoxBakhshName2.ItemsSource = bakhshQuery.ToList();

            if (ComboBoxBakhshName.Items.Count > 0)
            {
                ComboBoxBakhshName.SelectedIndex = 0;
            }

            if (ComboBoxBakhshName2.Items.Count > 0)
            {
                ComboBoxBakhshName2.SelectedIndex = 0;
            }
        }

        private void LoadComboBoxDastgahName()
        {
            if (ComboBoxBakhshName2.SelectedIndex == -1)
            {
                return;
            }

            Guid bakhshGUID = ((Bakhshha)ComboBoxBakhshName2.SelectedItem).BakhshGUID;

            var dastgahQuery = from d in Entities.Dastgahhas orderby d.DastgahName where d.BakhshGUID == bakhshGUID select d;
            ComboBoxDastgahName.ItemsSource = dastgahQuery.ToList();

            if (ComboBoxDastgahName.Items.Count > 0)
            {
                ComboBoxDastgahName.SelectedIndex = 0;
            }
        }

        private void ComboBoxBakhshName2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboBoxDastgahName();
        }

        private void ButtonAddBakhshName_Click(object sender, RoutedEventArgs e)
        {
            WindowAddBakhshName windowAddBakhshName = new WindowAddBakhshName();
            windowAddBakhshName.BakhshNameAdded += WindowAddBakhshNameOnBakhshNameAdded;
            windowAddBakhshName.Show();
        }

        private void WindowAddBakhshNameOnBakhshNameAdded(object sender, EventArgs eventArgs)
        {
            LoadComboBoxBakhshName();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(WindowAddDastgahName))
                {
                    WindowAddDastgahName windowAddDastgahName = (WindowAddDastgahName)window;
                    windowAddDastgahName.LoadComboBoxBakhshName();
                }

                if (window.GetType() == typeof(WindowAdd))
                {
                    WindowAdd windowAdd = (WindowAdd)window;
                    windowAdd.LoadComboBoxBakhshName();
                }

                if (window.GetType() == typeof(WindowEdit))
                {
                    WindowEdit windowEdit = (WindowEdit)window;
                    windowEdit.LoadComboBoxBakhshName();
                }
            }
        }

        private void ButtonEditBakhshName_Click(object sender, RoutedEventArgs e)
        {
            Bakhshha bakhsh = (Bakhshha)ComboBoxBakhshName.SelectedItem;

            WindowEditBakhshName windowEditBakhshName = new WindowEditBakhshName();
            windowEditBakhshName.BakhshGuid = bakhsh.BakhshGUID;
            windowEditBakhshName.BakhshNameEdited += new EventHandler(windowEditBakhshName_BakhshNameEdited);
            windowEditBakhshName.Show();

        }

        void windowEditBakhshName_BakhshNameEdited(object sender, EventArgs e)
        {
            LoadComboBoxBakhshName();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(WindowAddDastgahName))
                {
                    WindowAddDastgahName windowAddDastgahName = (WindowAddDastgahName)window;
                    windowAddDastgahName.LoadComboBoxBakhshName();
                }

                if (window.GetType() == typeof(WindowAdd))
                {
                    WindowAdd windowAdd = (WindowAdd)window;
                    windowAdd.LoadComboBoxBakhshName();
                }

                if (window.GetType() == typeof(WindowEdit))
                {
                    WindowEdit windowEdit = (WindowEdit)window;
                    windowEdit.LoadComboBoxBakhshName();
                }
            }
        }

        private void ButtonDeleteBakhshName_Click(object sender, RoutedEventArgs e)
        {
            Bakhshha selectedBakhsh = (Bakhshha)ComboBoxBakhshName.SelectedItem;

            if (selectedBakhsh == null)
            {
                return;
            }

            DialogBoxConfrimDelete dialogBoxConfrimDelete = new DialogBoxConfrimDelete();
            dialogBoxConfrimDelete.Message =
                "آیا از حذف این بخش اطمینان دارید؟در صورت حذف بخش دستگاه های آن بخش نیز حذف خواهند شد";
            dialogBoxConfrimDelete.ShowDialog();

            if (dialogBoxConfrimDelete.DialogResult == true)
            {
                var dastgahtQuery = from dastgah in Entities.Dastgahhas
                                    where dastgah.BakhshGUID == selectedBakhsh.BakhshGUID
                                    select dastgah;

                if (dastgahtQuery.Any())
                {
                    DialogBoxOk dialogBoxOk = new DialogBoxOk();
                    dialogBoxOk.Message = "این بخش شامل دستگاه هایی می باشد و قابل حذف نیست";
                    dialogBoxOk.ShowDialog();
                    return;
                }


                var dastgahQuery = from d in Entities.Dastgahhas
                                   where d.BakhshGUID == selectedBakhsh.BakhshGUID
                                   select d;

                foreach (var dd in dastgahQuery)
                {
                    Entities.Dastgahhas.DeleteObject(dd);
                }

                var bakhshQuery = from b in Entities.Bakhshhas
                                  where b.BakhshGUID == selectedBakhsh.BakhshGUID
                                  select b;
                Entities.Bakhshhas.DeleteObject(bakhshQuery.FirstOrDefault());

                Entities.SaveChanges();

                LoadComboBoxBakhshName();

                Darkhast.DialogBoxOk dialogBoxOk2 = new DialogBoxOk();
                dialogBoxOk2.Message = "بخش انتخاب شده حذف شد";
                dialogBoxOk2.ShowDialog();

            }
        }

        private void ButtonAddDastgahName_Click(object sender, RoutedEventArgs e)
        {
            WindowAddDastgahName windowAddDastgahName = new WindowAddDastgahName();
            windowAddDastgahName.BakhshGuid = ((Bakhshha)ComboBoxBakhshName2.SelectedItem).BakhshGUID;
            windowAddDastgahName.DastgahNameAdded += new EventHandler(windowAddDastgahName_DastgahNameAdded);
            windowAddDastgahName.Show();
        }

        void windowAddDastgahName_DastgahNameAdded(object sender, EventArgs e)
        {
            LoadComboBoxDastgahName();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(WindowAdd))
                {
                    WindowAdd windowAdd = (WindowAdd)window;
                    windowAdd.LoadComboBoxBakhshName();
                    windowAdd.LoadComboBoxdastgahName();
                }

                if (window.GetType() == typeof(WindowEdit))
                {
                    WindowEdit windowEdit = (WindowEdit)window;
                    windowEdit.LoadComboBoxBakhshName();
                    windowEdit.LoadComboBoxdastgahName();
                }
            }
        }

        private void ButtonDeleteDastgahName_Click(object sender, RoutedEventArgs e)
        {
            DialogBoxConfrimDelete dialogBoxConfrimDelete = new DialogBoxConfrimDelete();
            dialogBoxConfrimDelete.Message = "آیا از حذف این دستگاه اطمینان دارید؟";
            dialogBoxConfrimDelete.ShowDialog();

            if (dialogBoxConfrimDelete.DialogResult == true)
            {
                Bakhshha bakhsh = (Bakhshha)ComboBoxBakhshName2.SelectedItem;
                Dastgahha dastgah = (Dastgahha)ComboBoxDastgahName.SelectedItem;

                var darkhastQuery = from darkhast in Entities.Darkhasthas
                                    where darkhast.DastgahGUID == dastgah.DastgahGUID
                                    select darkhast;

                if (darkhastQuery.Any())
                {
                    DialogBoxOk dialogBoxOk2 = new DialogBoxOk();
                    dialogBoxOk2.Message = "این دستگاه قبلا در درخواست ها استفاده شده است و نمی توانید آن را حذف کنید.فقط می توانید آن را ویرایش کنید";
                    dialogBoxOk2.ShowDialog();
                    return;
                }

                var deleteQuery = from d in Entities.Dastgahhas
                                  where d.BakhshGUID == bakhsh.BakhshGUID && d.DastgahGUID == dastgah.DastgahGUID
                                  select d;

                Entities.Dastgahhas.DeleteObject(((Dastgahha)deleteQuery.FirstOrDefault()));
                Entities.SaveChanges();

                LoadComboBoxDastgahName();
                DialogBoxOk dialogBoxOk = new DialogBoxOk();
                dialogBoxOk.Message = "دستگاه حذف شد";
                dialogBoxOk.ShowDialog();

            }
        }

        private void ButtonEditDastgahName_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxDastgahName.SelectedIndex == -1)
            {
                return;
            }

            WindowEditDastgahName windowEditDastgahName = new WindowEditDastgahName();
            windowEditDastgahName.BakhshGuid = ((Bakhshha)ComboBoxBakhshName2.SelectedItem).BakhshGUID;
            windowEditDastgahName.DastgahGuid = ((Dastgahha)ComboBoxDastgahName.SelectedItem).DastgahGUID;
            windowEditDastgahName.DastgahNameEdited += new EventHandler(windowEditDastgahName_DastgahNameEdited);
            windowEditDastgahName.Show();
        }

        void windowEditDastgahName_DastgahNameEdited(object sender, EventArgs e)
        {
            LoadComboBoxDastgahName();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(WindowAdd))
                {
                    WindowAdd windowAdd = (WindowAdd)window;
                    windowAdd.LoadComboBoxBakhshName();
                    windowAdd.LoadComboBoxdastgahName();
                }

                if (window.GetType() == typeof(WindowEdit))
                {
                    WindowEdit windowEdit = (WindowEdit)window;
                    windowEdit.LoadComboBoxBakhshName();
                    windowEdit.LoadComboBoxdastgahName();
                }

                if (window.GetType() == typeof(MainWindow))
                {
                    MainWindow mainWindow = (MainWindow)window;
                    mainWindow.LoadGridViewDarkhastha();
                }
            }
        }
    }
}
