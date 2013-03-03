using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Data.SqlServerCe;


namespace Darkhast
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(currentDomain_UnhandledException);
        }

        void currentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject.GetType()==typeof(SqlCeException))
            {
                DialogBoxOk dialogBoxOk=new DialogBoxOk();
                dialogBoxOk.Message = "دیتابیس مشغول است.لطفا بعد از چند لحظه دوباره سعی کنید.";
                dialogBoxOk.ShowDialog();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            DatabaseEntities entities = Lib.Global.Entities;
            var loginQuery = from log in entities.LoginsLogs
                             where log.LoginGUID == Lib.Global.LoginGuid
                             select log;

            if (loginQuery.Any())
            {
                LoginsLog loginsLog = loginQuery.FirstOrDefault();
                loginsLog.LogoutDate = DateTime.Now;
                entities.SaveChanges();
            }
            

        }
    }
}
