using System;
using System.Collections.Generic;
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
    /// Interaction logic for WindowAdd.xaml
    /// </summary>
    public partial class WindowAdd : Window
    {
        private DatabaseEntities _entities = Lib.Global.Entities;
        private DispatcherTimer TimerClearStatusBar = new DispatcherTimer();
        public event EventHandler DarkhastAdded;

        public void OnDarkhastAdded(EventArgs e)
        {
            EventHandler handler = DarkhastAdded;
            if (handler != null) handler(this, e);
        }


        public WindowAdd()
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
                Darkhastha darkhast = new Darkhastha();
                darkhast.DarkhastGUID = Guid.NewGuid();
                darkhast.DarkhastName = TextBoxDarkhastName.Text;
                darkhast.ShomareFani = TextBoxShomareFani.Text;


                if (ComboBoxDastgahName.SelectedIndex != -1)
                {
                    darkhast.DastgahGUID = ((Dastgahha)ComboBoxDastgahName.SelectedItem).DastgahGUID;
                }

                darkhast.VahedShomaresh = TextBoxVahedShomaresh.Text;
                darkhast.TedadDarkhast = Convert.ToInt32(NumericUpDownTedadDarkhast.Value);
                darkhast.Tarikh = DateTime.Now;
                darkhast.Tozihat = TextBoxTozihat.Text;
                darkhast.BarghkarGUID = Lib.Global.CurrentUserGuid;

                if (Lib.Global.CurrentUserRole == (int)UserRole.Admin)
                {
                    darkhast.Vaziat = (int)VaziatDarkhast.TaeedShode;
                }

                Entities.Darkhasthas.AddObject(darkhast);

                if (Entities.SaveChanges()>0)
                {
                    Darkhastha_Log2.InsertLog(darkhast, RevisionOperation.Insert);
                    StatusBar1.Items.Clear();
                    StatusBar1.Items.Add("درخواست جديد با موفقيت ثبت شد");
                    ClearAllInputs();
                    OnDarkhastAdded(new EventArgs());                    
                }
                else
                {
                    StatusBar1.Items.Clear();
                    StatusBar1.Items.Add("خطا در ثبت");
                }

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
