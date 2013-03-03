using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Darkhast.Lib;

namespace Darkhast
{
    /// <summary>
    /// Interaction logic for WindowEdit.xaml
    /// </summary>
    public partial class WindowEdit : Window
    {
        private DatabaseEntities _entities = Lib.Global.Entities;
        private DispatcherTimer TimerClearStatusBar = new DispatcherTimer();
        public event EventHandler DarkhastEdited;
        private Darkhastha _darkhast;


        public void OnDarkhastEdited(EventArgs e)
        {
            EventHandler handler = DarkhastEdited;
            if (handler != null) handler(this, e);
        }


        public WindowEdit()
        {
            InitializeComponent();
        }

        public DatabaseEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public Darkhastha Darkhast
        {
            get { return _darkhast; }
            set { _darkhast = value; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadComboBoxBakhshName();
            ShowDarkhastForEditing();
        }

        private void ShowDarkhastForEditing()
        {
            object lockObj = new object();
            TextBoxDarkhastName.Text = Darkhast.DarkhastName;
            TextBoxShomareFani.Text = Darkhast.ShomareFani;

            if (Darkhast.DastgahGUID != null)
            {
                var dastgahQuery = from d in Entities.Dastgahhas
                                   where d.DastgahGUID == Darkhast.DastgahGUID
                                   select d;
                Dastgahha dastgah = dastgahQuery.FirstOrDefault();
                ComboBoxBakshName.SelectedItem = dastgah.Bakhshha;
                LoadComboBoxdastgahName();

                ComboBoxDastgahName.SelectedItem = dastgah;
            }

            TextBoxVahedShomaresh.Text = Darkhast.VahedShomaresh;
            NumericUpDownTedadDarkhast.Value = Darkhast.TedadDarkhast;
            TextBoxTozihat.Text = Darkhast.Tozihat;

        }

        public void LoadComboBoxBakhshName()
        {
            var bakhshQuery = from b in Entities.Bakhshhas orderby b.BakhshName select b;
            ComboBoxBakshName.ItemsSource = bakhshQuery.ToList();


            //if (ComboBoxBakshName.Items.Count > 0)
            //{
            //    ComboBoxBakshName.SelectedIndex = 0;
            //}
        }

        public void LoadComboBoxdastgahName()
        {
            if (ComboBoxBakshName.SelectedIndex == -1)
            {
                return;
            }

            Guid bakhshGUID = ((Bakhshha)ComboBoxBakshName.SelectedItem).BakhshGUID;

            var dastgahQuery = from d in Entities.Dastgahhas orderby d.DastgahName where d.BakhshGUID == bakhshGUID select d;
            ComboBoxDastgahName.ItemsSource = dastgahQuery.ToList();

            //if (ComboBoxDastgahName.Items.Count > 0)
            //{
            //    ComboBoxDastgahName.SelectedIndex = 0;
            //}
        }

        private void ComboBoxBakshName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboBoxdastgahName();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                var darkhastQuery = from d in Entities.Darkhasthas
                                    where d.DarkhastGUID == Darkhast.DarkhastGUID
                                    select d;
                Darkhastha darkhast = darkhastQuery.FirstOrDefault();
                darkhast.DarkhastName = TextBoxDarkhastName.Text;
                darkhast.ShomareFani = TextBoxShomareFani.Text;


                if (ComboBoxDastgahName.SelectedIndex != -1)
                {
                    darkhast.DastgahGUID = ((Dastgahha)ComboBoxDastgahName.SelectedItem).DastgahGUID;
                }

                darkhast.VahedShomaresh = TextBoxVahedShomaresh.Text;
                darkhast.TedadDarkhast = Convert.ToInt32(NumericUpDownTedadDarkhast.Value);
                //darkhast.Tarikh = DateTime.Now;
                darkhast.Tozihat = TextBoxTozihat.Text;
                darkhast.BarghkarGUID = Lib.Global.CurrentUserGuid;

                if (Lib.Global.CurrentUserRole == (int)UserRole.Admin)
                {
                    darkhast.Vaziat = (int)VaziatDarkhast.TaeedShode;
                }

                //Entities.Darkhasthas.AddObject(Darkhast);
                Darkhastha_Log2.InsertLog(darkhast, RevisionOperation.Update);
                Entities.SaveChanges();

                StatusBar1.Items.Clear();
                StatusBar1.Items.Add("درخواست با موفقيت ویرایش شد");
                //ClearAllInputs();
                OnDarkhastEdited(new EventArgs());

                //StatusBar1.Items.Clear();
                //StatusBar1.Items.Add("خطا در ثبت");


                TimerClearStatusBar.Interval = new TimeSpan(0, 0, 10);
                TimerClearStatusBar.Tick += new EventHandler(TimerClearStatusBar_Tick);
                TimerClearStatusBar.Start();
            }
        }

        void TimerClearStatusBar_Tick(object sender, EventArgs e)
        {
            ClearStatusBarAfterButtonAdd();
        }

        private bool Validation()
        {
            StatusBar1.Items.Clear();

            if (TextBoxDarkhastName.Text.Length > 0 && NumericUpDownTedadDarkhast.Value > 0)
            {
                return true;
            }

            StatusBar1.Items.Add("فيلد هاي ستاره دار نمي توانند خالي باشند");
            return false;
        }

        private void ClearAllInputs()
        {
            TextBoxDarkhastName.Text = "";
            TextBoxShomareFani.Text = "";
            ComboBoxBakshName.SelectedIndex = -1;
            ComboBoxDastgahName.SelectedIndex = -1;
            TextBoxVahedShomaresh.Text = "";
            NumericUpDownTedadDarkhast.Value = 0;
            TextBoxTozihat.Text = "";
        }

        private void ClearStatusBarAfterButtonAdd()
        {
            StatusBar1.Items.Clear();
            TimerClearStatusBar.Stop();
            TimerClearStatusBar.Tick -= new EventHandler(TimerClearStatusBar_Tick);
        }
    }
}
