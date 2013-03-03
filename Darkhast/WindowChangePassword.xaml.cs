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
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class WindowChangePassword : Window
    {
        private DatabaseEntities _entities = Lib.Global.Entities;

        public WindowChangePassword()
        {
            InitializeComponent();
        }

        public DatabaseEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StatusBar1.Items.Clear();

            var passwordQuery = from op in Entities.Barghkarhas
                                where
                                    op.BarghkarGUID == Global.CurrentUserGuid &
                                    op.Password == PasswordBoxOldPassword.Password
                                select op;

            if (passwordQuery.Any() == false)
            {
                StatusBar1.Items.Add("کلمه عبور قدیم نادرست است");
                return;
            }


            if (PasswordBoxNewPassword.Password != PasswordBoxNewPasswordConfirm.Password)
            {
                StatusBar1.Items.Add("کلمه عبور جدید و تکرار آن با هم مطابقت ندارند");
                return;
            }
            else
            {
                if (PasswordBoxOldPassword.Password == PasswordBoxNewPassword.Password)
                {
                    StatusBar1.Items.Add("کلمه عبور جدید و قدیم نمی توانند یکی باشند");
                    return;
                }
            }

            try
            {
                Barghkarha barghkar = passwordQuery.FirstOrDefault();

                barghkar.Password = PasswordBoxNewPassword.Password;

                Entities.SaveChanges();

                StatusBar1.Items.Add("کلمه عبور جدید با موفقیت ثبت شد");
                PasswordBoxOldPassword.Clear();
                PasswordBoxNewPassword.Clear();
                PasswordBoxNewPasswordConfirm.Clear();

            }
            catch (Exception)
            {
                StatusBar1.Items.Add("خطا در ثبت");
            }
        }
    }
}
