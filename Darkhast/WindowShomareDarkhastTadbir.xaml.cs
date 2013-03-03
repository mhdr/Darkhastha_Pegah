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

namespace Darkhast
{
    /// <summary>
    /// Interaction logic for WindowShomareDarkhastTadbir.xaml
    /// </summary>
    public partial class WindowShomareDarkhastTadbir : Window
    {
        public WindowShomareDarkhastTadbir()
        {
            InitializeComponent();
        }

        private DatabaseEntities _entities = Lib.Global.Entities;
        private Guid _darkhastGuid;
        public event EventHandler ShomareDarkhastSaved;

        public void OnShomareDarkhastSaved(EventArgs e)
        {
            EventHandler handler = ShomareDarkhastSaved;
            if (handler != null) handler(this, e);
        }

        public DatabaseEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public Guid DarkhastGuid
        {
            get { return _darkhastGuid; }
            set { _darkhastGuid = value; }
        }

        private void ButtonSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StatusBar1.Items.Clear();
            string tempShomareDarkhast = TextBoxShomareDarkhast.Text;
            int shomareDarkhast = -1;

            try
            {
                shomareDarkhast = Convert.ToInt32(tempShomareDarkhast);
            }
            catch (Exception ex)
            {
                if (tempShomareDarkhast != "")
                {
                    StatusBar1.Items.Add("شماره درخواست باید عدد باشد");
                    return;
                }

            }

            var darkhastQuery = from d in Entities.Darkhasthas
                                where d.DarkhastGUID == this.DarkhastGuid
                                select d;

            Darkhastha darkhast = darkhastQuery.FirstOrDefault();
            darkhast.ShomareDarkhastTadbir = tempShomareDarkhast;
            Darkhastha_Log2.InsertLog(darkhast, RevisionOperation.Update);
            Entities.SaveChanges();

            StatusBar1.Items.Add("شماره درخواست ثبت شد");
            TextBoxShomareDarkhast.Text = "";

            OnShomareDarkhastSaved(new EventArgs());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var darkhastQuery = from d in Entities.Darkhasthas
                                where d.DarkhastGUID == DarkhastGuid
                                select d;
            Darkhastha darkhast = darkhastQuery.FirstOrDefault();
            TextBoxShomareDarkhast.Text = darkhast.ShomareDarkhastTadbir;
        }


    }
}
