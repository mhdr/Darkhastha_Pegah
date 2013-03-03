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
using Telerik.Windows.Controls;

namespace Darkhast
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private DatabaseEntities _entities = Lib.Global.Entities;

        public Login()
        {
            StyleManager.ApplicationTheme = new Windows7Theme();
            InitializeComponent();
        }

        public DatabaseEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Telerik.OpenAccess.ServiceHost.ServiceHostManager.StartProfilerService(15555);

            var barghkarhaQuery = from b in Entities.Barghkarhas orderby b.LastName select b;
            ComboBoxNames.ItemsSource = barghkarhaQuery.ToList();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            StatusBar1.Items.Clear();

            if (ComboBoxNames.SelectedIndex == -1)
            {
                StatusBar1.Items.Add("ابتدا يک شناسه انتخاب کنيد");
                return;
            }

            if (PasswordBox1.Password.Length == 0)
            {
                StatusBar1.Items.Add("کلمه عبور نمي تواند خالي باشد");
            }
            else
            {
                Barghkarha barghkar = (Barghkarha)ComboBoxNames.SelectedItem;
                var passwordQuery = from b in Entities.Barghkarhas where b.BarghkarGUID == barghkar.BarghkarGUID && b.Password == PasswordBox1.Password select b;
                int count = passwordQuery.Count();

                if (count == 1)
                {
                    Lib.Global.CurrentUserGuid = barghkar.BarghkarGUID;
                    Lib.Global.CurrentUserRole = (int)barghkar.Role;

                    LoginsLog loginsLog=new LoginsLog();
                    loginsLog.LoginGUID = Guid.NewGuid();
                    loginsLog.BarghkarGUID = Lib.Global.CurrentUserGuid;
                    loginsLog.LoginDate = DateTime.Now;
                    Entities.LoginsLogs.AddObject(loginsLog);
                    if (Entities.SaveChanges()>0)
                    {
                        Lib.Global.LoginGuid = loginsLog.LoginGUID;
                    }

                    //MainWindow mainWindow = new MainWindow();
                    //mainWindow.Show();

                    WindowMessages windowNewMessage=new WindowMessages();
                    windowNewMessage.Show();

                    this.Close();
                }
                else
                {
                    StatusBar1.Items.Add("کلمه عبور اشتباه است");
                }
            }
        }
    }
}
