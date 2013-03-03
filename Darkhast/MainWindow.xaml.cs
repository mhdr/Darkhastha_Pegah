using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Darkhast.Lib;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using Telerik.Windows.Persistence.Storage;
using Telerik.Windows.PersistenceFramework;
using TarikhFA;
using System.Threading;

namespace Darkhast
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseEntities _entities = Lib.Global.Entities;
        private string _searchDarkhastName = "";
        private string _searchShomareFani = "";
        private string _searchTozihat = "";
        private bool _showTrash = false;
        private static IsolatedStorageProvider isolatedStorage = new IsolatedStorageProvider();
        private bool deleteIsolatedStorage = false;
        ParallelQuery<Darkhastha> darkhasthaQuery = null;
        IQueryable<Darkhastha> darkhasthaQuery2 = null;
        private DateTime _startDate;
        private DateTime _endDate;
        private Timer timerLoadGridViewDarkhastha;
        private Darkhastha2 selectedItem;

        public MainWindow()
        {
            LocalizationManager.Manager = new MyLocalization();
            InitializeComponent();
        }

        public DatabaseEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public string SearchDarkhastName
        {
            get { return _searchDarkhastName; }
            set { _searchDarkhastName = value; }
        }

        public string SearchShomareFani
        {
            get { return _searchShomareFani; }
            set { _searchShomareFani = value; }
        }

        public bool ShowTrash
        {
            get { return _showTrash; }
            set { _showTrash = value; }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public string SearchTozihat
        {
            get { return _searchTozihat; }
            set { _searchTozihat = value; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPersistence();

            if (Global.CurrentUserRole == (int)UserRole.Admin)
            {
                RibbonTabTrash.Visibility = Visibility.Visible;
                RibbonTabManagment.Visibility = Visibility.Visible;
            }
            else
            {
                RibbonTabTrash.Visibility = Visibility.Hidden;
                RibbonTabManagment.Visibility = Visibility.Hidden;
            }

            SetApplicationNameForCurrentUser();

            SetDateRangeToCurrentMonth();
            LoadGridViewDarkhastha();
            ShowNumberOfTrashesOnRibbonTabTrash();
            //OfferChangePassword();

            //StartTimerLoadGridViewDarkhastha();
        }

        public void StartTimerLoadGridViewDarkhastha()
        {
            if (Properties.Settings.Default.TimerLoadGridViewDarkhasthaIsActive == true)
            {
                timerLoadGridViewDarkhastha = new Timer(LoadGridViewDarkhasthaUsingTimer, "load gridview..."
                    , 0, Properties.Settings.Default.TimerLoadGridViewDarkhasthaPeriod / 1000);
            }
        }

        public void StopTimerLoadGridViewDarkhastha()
        {
            if (timerLoadGridViewDarkhastha == null)
            {
                return;
            }

            timerLoadGridViewDarkhastha.Dispose();
            timerLoadGridViewDarkhastha = null;
        }


        private void LoadGridViewDarkhasthaUsingTimer(object obj)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new NextLoadGridViewdarkhastha(LoadGridViewDarkhastha));
        }

        private void SetApplicationNameForCurrentUser()
        {
            var userQuery = from b in Entities.Barghkarhas
                            where b.BarghkarGUID == Global.CurrentUserGuid
                            select b;
            Barghkarha user = userQuery.FirstOrDefault();
            RibbonView1.ApplicationName = string.Format("درخواست ها - {0} {1}", user.FirstName, user.LastName);
        }

        private static void LoadPersistence()
        {
            isolatedStorage.LoadFromStorage();
        }

        /// <summary>
        /// barresi mikone ke aya password pishfarz ke 1234 boode avaz shode ya na
        /// </summary>
        private void OfferChangePassword()
        {
            var passwordQuery = from b in Entities.Barghkarhas
                                where b.BarghkarGUID == Global.CurrentUserGuid & b.Password == "1234"
                                select b;

            if (passwordQuery.Any())
            {
                DialogBoxOfferChangePassword dialogBoxOfferChangePassword = new DialogBoxOfferChangePassword();
                dialogBoxOfferChangePassword.Show();
            }
        }

        private void ShowNumberOfTrashesOnRibbonTabTrash()
        {
            int countTrash = CountInTrashes();
            if (countTrash > 0)
            {
                RibbonTabTrash.Header = string.Format("سطل زباله : {0}", countTrash);
                RibbonTabTrash.ToolTip = string.Format("تعداد درخواست های موجود در سطل زباله : {0}", countTrash);
            }
            else
            {
                RibbonTabTrash.Header = "سطل زباله";
                RibbonTabTrash.ToolTip = null;
            }
        }

        private int CountInTrashes()
        {

            try
            {
                IQueryable<Darkhastha> countQuery = null;
                countQuery = from d in Entities.Darkhasthas
                             where d.IsTrash == (int)Trash.InTrash
                             select d;
                return countQuery.Count();
            }
            catch (Exception)
            {
                return 0;
            }

            return 0;
        }

        public void LoadGridViewDarkhastha()
        {
            selectedItem= (Darkhastha2)GridViewDarkhastha.SelectedItem;

            if (ShowTrash == true)
            {
                darkhasthaQuery2 = from d in Entities.Darkhasthas
                                   where d.IsTrash == (int)Trash.InTrash
                                   orderby d.Tarikh descending
                                   select d;
            }
            else
            {
                darkhasthaQuery2 = from d in Entities.Darkhasthas
                                   where d.IsTrash != (int)Trash.OutOfTrash
                                   orderby d.Tarikh descending
                                   select d;
            }

            darkhasthaQuery2 = darkhasthaQuery2.Where(x => x.Tarikh >= StartDate && x.Tarikh <= EndDate);

            if (SearchDarkhastName.Length > 0)
            {
                if (Properties.Settings.Default.SearchDarkhastNameSearchType == (int)SearchType.Contains)
                {
                    darkhasthaQuery2 = darkhasthaQuery2.Where(x => x.IsTrash != (int)Trash.OutOfTrash
                                                                  && x.DarkhastName.ToLower().Contains(SearchDarkhastName.ToLower()));
                }
                else if (Properties.Settings.Default.SearchDarkhastNameSearchType == (int)SearchType.StarstWith)
                {
                    darkhasthaQuery2 = darkhasthaQuery2.Where(x => x.IsTrash != (int)Trash.OutOfTrash
                                && x.DarkhastName.ToLower().StartsWith(SearchDarkhastName.ToLower()));
                }
                else if (Properties.Settings.Default.SearchDarkhastNameSearchType == (int)SearchType.EndsWith)
                {
                    darkhasthaQuery2 = darkhasthaQuery2.Where(x => x.IsTrash != (int)Trash.OutOfTrash
                                && x.DarkhastName.ToLower().EndsWith(SearchDarkhastName.ToLower()));
                }
                else if (Properties.Settings.Default.SearchDarkhastNameSearchType == (int)SearchType.MatchCase)
                {
                    darkhasthaQuery2 = darkhasthaQuery2.Where(x => x.IsTrash != (int)Trash.OutOfTrash
                                && x.DarkhastName.ToLower().Equals(SearchDarkhastName.ToLower()));
                }
            }


            if (SearchShomareFani.Length > 0)
            {
                if (Properties.Settings.Default.SearchShomareFaniSearchType == (int)SearchType.Contains)
                {
                    darkhasthaQuery2 = darkhasthaQuery2.Where(x => x.IsTrash != (int)Trash.OutOfTrash
                                && x.ShomareFani.ToLower().Contains(SearchShomareFani.ToLower()));
                }
                else if (Properties.Settings.Default.SearchShomareFaniSearchType == (int)SearchType.StarstWith)
                {
                    darkhasthaQuery2 = darkhasthaQuery2.Where(x => x.IsTrash != (int)Trash.OutOfTrash
                && x.ShomareFani.ToLower().StartsWith(SearchShomareFani.ToLower()));
                }
                else if (Properties.Settings.Default.SearchShomareFaniSearchType == (int)SearchType.EndsWith)
                {
                    darkhasthaQuery2 = darkhasthaQuery2.Where(x => x.IsTrash != (int)Trash.OutOfTrash
                && x.ShomareFani.ToLower().EndsWith(SearchShomareFani.ToLower()));
                }
                else if (Properties.Settings.Default.SearchShomareFaniSearchType == (int)SearchType.MatchCase)
                {
                    darkhasthaQuery2 = darkhasthaQuery2.Where(x => x.IsTrash != (int)Trash.OutOfTrash
                                                                  &&
                                                                  x.ShomareFani.ToLower().Equals(
                                                                      SearchShomareFani.ToLower()));
                }
            }


            if (SearchTozihat.Length > 0)
            {
                if (Properties.Settings.Default.SearchTozihatSearchType == (int)SearchType.Contains)
                {
                    darkhasthaQuery2 = darkhasthaQuery2.Where(x => x.IsTrash != (int)Trash.OutOfTrash
                                && x.Tozihat.ToLower().Contains(SearchTozihat.ToLower()));
                }
                else if (Properties.Settings.Default.SearchTozihatSearchType == (int)SearchType.StarstWith)
                {
                    darkhasthaQuery2 = darkhasthaQuery2.Where(x => x.IsTrash != (int)Trash.OutOfTrash
                && x.Tozihat.ToLower().StartsWith(SearchTozihat.ToLower()));
                }
                else if (Properties.Settings.Default.SearchTozihatSearchType == (int)SearchType.EndsWith)
                {
                    darkhasthaQuery2 = darkhasthaQuery2.Where(x => x.IsTrash != (int)Trash.OutOfTrash
                && x.Tozihat.ToLower().EndsWith(SearchTozihat.ToLower()));
                }
                else if (Properties.Settings.Default.SearchTozihatSearchType == (int)SearchType.MatchCase)
                {
                    darkhasthaQuery2 = darkhasthaQuery2.Where(x => x.IsTrash != (int)Trash.OutOfTrash
                                                                  &&
                                                                  x.Tozihat.ToLower().Equals(
                                                                      SearchTozihat.ToLower()));
                }
            }

            var itemSource = new Darkhastha2Collecttion(darkhasthaQuery2);
            GridViewDarkhastha.ItemsSource = itemSource;
        }

        private void RibbonButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            WindowAdd windowAdd = new WindowAdd();
            windowAdd.DarkhastAdded += new EventHandler(windowAdd_DarkhastAdded);
            windowAdd.Show();
        }

        void windowAdd_DarkhastAdded(object sender, EventArgs e)
        {
            LoadGridViewDarkhastha();
        }

        private void RibbonButtonDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogBoxConfrimDelete dialogBoxConfrimDelete = new DialogBoxConfrimDelete();
            dialogBoxConfrimDelete.Message = "آیا از حذف این درخواست اطمینان دارید؟";
            dialogBoxConfrimDelete.ShowDialog();

            if (dialogBoxConfrimDelete.DialogResult == false)
            {
                return;
            }

            Darkhastha2 selectedItem = (Darkhastha2)GridViewDarkhastha.SelectedItem;

            if (selectedItem == null)
            {
                return;
            }

            if (Global.CurrentUserRole == (int)UserRole.Admin)
            {
                MoveToTrash(selectedItem.DarkhastGuid);
                LoadGridViewDarkhastha();
                ShowNumberOfTrashesOnRibbonTabTrash();
            }
            else if (Global.CurrentUserRole == (int)UserRole.Mamoli)
            {
                if (selectedItem.Vaziat == (int)VaziatDarkhast.DarHaleBarresi)
                {
                    MoveOutOfTrash(selectedItem.DarkhastGuid);
                    LoadGridViewDarkhastha();
                }
                else
                {
                    MoveToTrash(selectedItem.DarkhastGuid);
                    LoadGridViewDarkhastha();
                }
            }
        }

        private void MoveOutOfTrash(Guid selectedItemGuid)
        {
            var darkhastQuery = from darkhast in Entities.Darkhasthas
                                where darkhast.DarkhastGUID == selectedItemGuid
                                select darkhast;
            Darkhastha darkhastForEdit = darkhastQuery.FirstOrDefault();

            darkhastForEdit.IsTrash = (int)Trash.OutOfTrash;
            Entities.SaveChanges();

            Darkhastha_Log2.InsertLog(darkhastForEdit, RevisionOperation.Update);
        }

        private void MoveToTrash(Guid selectedItemGuid)
        {
            var darkhastQuery = from darkhast in Entities.Darkhasthas
                                where darkhast.DarkhastGUID == selectedItemGuid
                                select darkhast;
            Darkhastha darkhastForEdit = darkhastQuery.FirstOrDefault();

            darkhastForEdit.IsTrash = (int)Trash.InTrash;
            Entities.SaveChanges();
            Darkhastha_Log2.InsertLog(darkhastForEdit, RevisionOperation.Update);
        }

        private void MenuItemSearchDarkhastName_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            WindowSearchDarkhastName windowSearchDarkhastName = new WindowSearchDarkhastName();
            windowSearchDarkhastName.Show();
            windowSearchDarkhastName.Search += new SearchKeywordEventHandler(windowSearchDarkhastName_Search);
        }

        void windowSearchDarkhastName_Search(object sender, SearchKeywordEventArgs e)
        {
            SearchDarkhastName = e.Keyword;
            LoadGridViewDarkhastha();
        }

        private void MenuItemSearchShomareFani_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            WindowSearchShomareFani windowSearchShomareFani = new WindowSearchShomareFani();
            windowSearchShomareFani.Show();
            windowSearchShomareFani.Search += new SearchKeywordEventHandler(windowSearchShomareFani_Search);
        }

        void windowSearchShomareFani_Search(object sender, SearchKeywordEventArgs e)
        {
            SearchShomareFani = e.Keyword;
            LoadGridViewDarkhastha();
        }

        private void MenuItemClearSearch_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearSearches();
            MenuItemClearSearch.IsEnabled = false;
            LoadGridViewDarkhastha();
        }

        private void ClearSearches()
        {
            SearchDarkhastName = "";
            SearchShomareFani = "";
            SearchTozihat = "";
            SetDateRangeToCurrentMonth();
        }

        private void RibbonDropDownButtonSearch_DropDownOpening(object sender, RoutedEventArgs e)
        {
            if (SearchShomareFani.Length > 0 || SearchDarkhastName.Length > 0)
            {
                MenuItemClearSearch.IsEnabled = true;
            }
        }

        private void GridViewDarkhastha_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            Darkhastha2 selectedItem = (Darkhastha2)GridViewDarkhastha.SelectedItem;

            if (selectedItem == null)
            {
                return;
            }

            RibbonToggleButtonDaryaftShod.IsEnabled = false;

            if (Global.CurrentUserRole == (int)UserRole.Admin)
            {
                if (selectedItem.IsTrash == (int)Trash.InTrash)
                {
                    RibbonButtonEdit.IsEnabled = false;
                    RibbonButtonDelete.IsEnabled = false;
                }
                else if (selectedItem.IsTrash == (int)Trash.NotATrash)
                {
                    RibbonButtonEdit.IsEnabled = true;
                    RibbonButtonDelete.IsEnabled = true;
                }

                RibbonToggleButtonDaryaftShod.IsEnabled = true;

                if (selectedItem.TarikhDaryaftKala == null)
                {
                    RibbonToggleButtonDaryaftShod.IsChecked = false;
                }
                else
                {
                    RibbonToggleButtonDaryaftShod.IsChecked = true;
                }

            }
            else if (Global.CurrentUserRole == (int)UserRole.Mamoli)
            {
                if (selectedItem.Vaziat == (int)VaziatDarkhast.DarHaleBarresi &&
                    selectedItem.BarghkarGuid == Global.CurrentUserGuid && selectedItem.IsTrash == (int)Trash.NotATrash)
                {
                    RibbonButtonEdit.IsEnabled = true;
                    RibbonButtonDelete.IsEnabled = true;
                }
                else if (selectedItem.Vaziat != (int)VaziatDarkhast.DarHaleBarresi &&
                    (selectedItem.BarghkarGuid != Global.CurrentUserGuid || selectedItem.IsTrash == (int)Trash.InTrash))
                {
                    RibbonButtonEdit.IsEnabled = false;
                    RibbonButtonDelete.IsEnabled = false;
                }
                else if (selectedItem.Vaziat != (int)VaziatDarkhast.DarHaleBarresi && selectedItem.BarghkarGuid == Global.CurrentUserGuid
                    && selectedItem.IsTrash == (int)Trash.NotATrash)
                {
                    RibbonButtonEdit.IsEnabled = false;
                    RibbonButtonDelete.IsEnabled = true;
                }

                if (selectedItem.BarghkarGuid == Global.CurrentUserGuid)
                {
                    RibbonToggleButtonDaryaftShod.IsEnabled = true;

                    if (selectedItem.TarikhDaryaftKala == null)
                    {
                        RibbonToggleButtonDaryaftShod.IsChecked = false;
                    }
                    else
                    {
                        RibbonToggleButtonDaryaftShod.IsChecked = true;
                    }
                }
            }

            if (selectedItem.Vaziat == (int)VaziatDarkhast.DarHaleBarresi)
            {
                RibbonButtonTaeed.IsEnabled = true;
                RibbonButtonAdamTaeed.IsEnabled = true;
            }
            else if (selectedItem.Vaziat == (int)VaziatDarkhast.TaeedNashode)
            {
                RibbonButtonTaeed.IsEnabled = true;
                RibbonButtonAdamTaeed.IsEnabled = false;
            }
            else if (selectedItem.Vaziat == (int)VaziatDarkhast.TaeedShode)
            {
                RibbonButtonTaeed.IsEnabled = false;
                RibbonButtonAdamTaeed.IsEnabled = true;
            }

            if (selectedItem.DarkhastGuid.ToString().Length > 0)
            {
                RibbonButtonShomareDarkhastTadbir.IsEnabled = true;
            }
            else
            {
                RibbonButtonShomareDarkhastTadbir.IsEnabled = false;
            }
        }

        private void RadRibbonView_SelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            if (e.AddedItems.Contains(RibbonTabTrash))
            {
                ShowTrash = true;
                LoadGridViewDarkhastha();
            }
            else if (e.RemovedItems.Contains(RibbonTabTrash))
            {
                ShowTrash = false;
                LoadGridViewDarkhastha();
            }

            if (e.AddedItems.Contains(RibbonTabManagment))
            {
                GridViewDarkhastha.SelectionMode = SelectionMode.Extended;
            }
            else if (e.RemovedItems.Contains(RibbonTabManagment))
            {
                GridViewDarkhastha.SelectionMode = SelectionMode.Single;
            }
        }

        private void RibbonButtonMoveOutOfTrash_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Darkhastha2 selectedItem = (Darkhastha2)GridViewDarkhastha.SelectedItem;

                MoveOutOfTrash(selectedItem.DarkhastGuid);

                LoadGridViewDarkhastha();
                ShowNumberOfTrashesOnRibbonTabTrash();
            }
            catch (Exception)
            {

            }
        }

        private void RibbonButtonRestore_Click(object sender, RoutedEventArgs e)
        {
            Darkhastha2 selectedItem = (Darkhastha2)GridViewDarkhastha.SelectedItem;

            RestoreFromTrash(selectedItem.DarkhastGuid);

            LoadGridViewDarkhastha();
            ShowNumberOfTrashesOnRibbonTabTrash();
        }

        private void RibbonButtonMoveAllOutOfTrash_Click(object sender, RoutedEventArgs e)
        {
            var trashQuery = from d in Entities.Darkhasthas.AsParallel()
                             where d.IsTrash == (int)Trash.InTrash
                             select d;

            foreach (var darkhast in trashQuery)
            {
                MoveOutOfTrash(darkhast.DarkhastGUID);
            }

            LoadGridViewDarkhastha();
            ShowNumberOfTrashesOnRibbonTabTrash();
        }

        private void RibbonButtonRestoreAll_Click(object sender, RoutedEventArgs e)
        {
            var trashQuery = from d in Entities.Darkhasthas.AsParallel()
                             where d.IsTrash == (int)Trash.InTrash
                             select d;

            foreach (var darkhast in trashQuery)
            {
                RestoreFromTrash(darkhast.DarkhastGUID);
            }

            LoadGridViewDarkhastha();
            ShowNumberOfTrashesOnRibbonTabTrash();
        }

        private void RestoreFromTrash(Guid guid)
        {
            var darkhastQuery = from darkhast in Entities.Darkhasthas
                                where darkhast.DarkhastGUID == guid
                                select darkhast;
            Darkhastha darkhastForEdit = darkhastQuery.FirstOrDefault();
            darkhastForEdit.IsTrash = (int)Trash.NotATrash;
            Entities.SaveChanges();
            Darkhastha_Log2.InsertLog(darkhastForEdit, RevisionOperation.Update);
        }

        private void RibbonButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            Darkhastha2 selectedItem = (Darkhastha2)GridViewDarkhastha.SelectedItem;
            var editQuery = from d in Entities.Darkhasthas
                            where d.DarkhastGUID == selectedItem.DarkhastGuid
                            select d;
            Darkhastha currentItem = editQuery.FirstOrDefault();

            WindowEdit windowEdit = new WindowEdit();
            windowEdit.Darkhast = currentItem;
            windowEdit.DarkhastEdited += new EventHandler(windowEdit_DarkhastEdited);
            windowEdit.Show();
        }

        void windowEdit_DarkhastEdited(object sender, EventArgs e)
        {

            //Entities = new DatabaseEntities();
            LoadGridViewDarkhastha();
        }

        private void RibbonButtonTaeed_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = GridViewDarkhastha.SelectedItems;

            if (selectedItems.Count == 0)
            {
                return;
            }

            foreach (Darkhastha2 item in selectedItems)
            {
                var darkhastQuery = from d in Entities.Darkhasthas
                                    where d.DarkhastGUID.Equals(item.DarkhastGuid)
                                    select d;

                Darkhastha darkhast = darkhastQuery.FirstOrDefault();
                darkhast.Vaziat = (int)VaziatDarkhast.TaeedShode;
                Darkhastha_Log2.InsertLog(darkhast, RevisionOperation.Update);
            }

            Entities.SaveChanges();

            LoadGridViewDarkhastha();

            RibbonButtonTaeed.IsEnabled = true;
            RibbonButtonAdamTaeed.IsEnabled = false;
        }

        private void RibbonButtonAdamTaeed_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = GridViewDarkhastha.SelectedItems;

            if (selectedItems.Count == 0)
            {
                return;
            }

            foreach (Darkhastha2 item in selectedItems)
            {
                var darkhastQuery = from d in Entities.Darkhasthas
                                    where d.DarkhastGUID.Equals(item.DarkhastGuid)
                                    select d;

                Darkhastha darkhast = darkhastQuery.FirstOrDefault();
                darkhast.Vaziat = (int)VaziatDarkhast.TaeedNashode;
                Darkhastha_Log2.InsertLog(darkhast, RevisionOperation.Update);
            }

            Entities.SaveChanges();

            LoadGridViewDarkhastha();

            RibbonButtonTaeed.IsEnabled = true;
            RibbonButtonAdamTaeed.IsEnabled = false;
        }

        private void RibbonButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            WindowChangePassword windowChangePassword = new WindowChangePassword();
            windowChangePassword.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (deleteIsolatedStorage == false)
            {
                SavePersistence();
            }

            StopTimerLoadGridViewDarkhastha();

            //Telerik.OpenAccess.ServiceHost.ServiceHostManager.StopProfilerService();
        }

        private static void SavePersistence()
        {
            try
            {
                isolatedStorage.SaveToStorage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RibbonButtonClearIsolatedStorage_Click(object sender, RoutedEventArgs e)
        {
            DialogBoxIsolatedStorageDeleted dialogBoxIsolatedStorageDeleted = new DialogBoxIsolatedStorageDeleted();
            dialogBoxIsolatedStorageDeleted.ShowDialog();

            if (dialogBoxIsolatedStorageDeleted.DialogResult == true)
            {
                isolatedStorage.DeleteIsolatedStorageFiles();
                deleteIsolatedStorage = true;
                Application.Current.Shutdown();
            }
        }

        private void RibbonToggleButtonDaryaftShod_Click(object sender, RoutedEventArgs e)
        {
            Darkhastha2 selectedItem = (Darkhastha2)GridViewDarkhastha.SelectedItem;

            if (selectedItem == null)
            {
                DialogBoxOk dialogBoxOk = new DialogBoxOk();
                dialogBoxOk.Message = "ابتدا یک درخواست را انتخاب کنید";
                dialogBoxOk.ShowDialog();
                return;
            }

            var darkhastQuery = from d in Entities.Darkhasthas
                                where d.DarkhastGUID == selectedItem.DarkhastGuid
                                select d;
            Darkhastha darkhast = darkhastQuery.FirstOrDefault();

            if (RibbonToggleButtonDaryaftShod.IsChecked == true)
            {
                darkhast.TarikhDaryaftKala = DateTime.Now;
            }
            else
            {
                darkhast.TarikhDaryaftKala = null;
            }

            Darkhastha_Log2.InsertLog(darkhast, RevisionOperation.Update);
            Entities.SaveChanges();
            LoadGridViewDarkhastha();
        }

        private void RibbonButtonDastgaha_Click(object sender, RoutedEventArgs e)
        {
            WindowDastgaha windowDastgaha = new WindowDastgaha();
            windowDastgaha.Show();
        }

        private void RibbonButtonAbout_Click(object sender, RoutedEventArgs e)
        {
            WindowAbout windowAbout = new WindowAbout();
            windowAbout.Show();
        }

        private void MenuItemCopyKalaName_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Darkhastha2 currentRow = (Darkhastha2)GridViewDarkhastha.SelectedItem;
            if (currentRow != null)
            {
                Clipboard.SetText(currentRow.DarkhastName, TextDataFormat.UnicodeText);
            }
            else
            {
                DialogBoxOk dialogBoxOk = new DialogBoxOk();
                dialogBoxOk.Message = "ابتدا یک درخواست را انتخاب کنید";
                dialogBoxOk.ShowDialog();
            }
        }

        private void MenuItemCopyShomareFani_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Darkhastha2 currentRow = (Darkhastha2)GridViewDarkhastha.SelectedItem;
            if (currentRow != null)
            {
                Clipboard.SetText(currentRow.ShomareFani, TextDataFormat.UnicodeText);
            }
            else
            {
                DialogBoxOk dialogBoxOk = new DialogBoxOk();
                dialogBoxOk.Message = "ابتدا یک درخواست را انتخاب کنید";
                dialogBoxOk.ShowDialog();
            }
        }

        private void MenuItemCopyTozihat_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Darkhastha2 currentRow = (Darkhastha2)GridViewDarkhastha.SelectedItem;
            if (currentRow != null)
            {
                Clipboard.SetText(currentRow.Tozihat, TextDataFormat.UnicodeText);
            }
            else
            {
                DialogBoxOk dialogBoxOk = new DialogBoxOk();
                dialogBoxOk.Message = "ابتدا یک درخواست را انتخاب کنید";
                dialogBoxOk.ShowDialog();
            }
        }

        private void MenuItemCopyTedadDarkhast_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Darkhastha2 currentRow = (Darkhastha2)GridViewDarkhastha.SelectedItem;
            if (currentRow != null)
            {
                Clipboard.SetText(currentRow.TedadDarkhast.ToString(), TextDataFormat.UnicodeText);
            }
            else
            {
                DialogBoxOk dialogBoxOk = new DialogBoxOk();
                dialogBoxOk.Message = "ابتدا یک درخواست را انتخاب کنید";
                dialogBoxOk.ShowDialog();
            }
        }

        private void RibbonButtonExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            string extension = "xls";
            SaveFileDialog dialog = new SaveFileDialog()
                                        {
                                            DefaultExt = extension,
                                            Filter =
                                                String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension,
                                                              "Excel"),
                                            FilterIndex = 1
                                        };
            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    GridViewDarkhastha.Export(stream,
                                              new GridViewExportOptions()
                                                  {
                                                      Format = ExportFormat.ExcelML,
                                                      ShowColumnHeaders = false,
                                                      ShowColumnFooters = false,
                                                      ShowGroupFooters = false,
                                                      Encoding = Encoding.UTF8
                                                  });
                }

            }
        }

        private void RibbonButtonExportToText_Click(object sender, RoutedEventArgs e)
        {
            string extension = "txt";
            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = extension,
                Filter =
                    String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension,
                                  "Text"),
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    GridViewDarkhastha.Export(stream,
                                              new GridViewExportOptions()
                                              {
                                                  Format = ExportFormat.Text,
                                                  ShowColumnHeaders = false,
                                                  ShowColumnFooters = false,
                                                  ShowGroupFooters = false,
                                                  Encoding = Encoding.UTF8
                                              });
                }

            }
        }

        private void RibbonButtonExportToCsv_Click(object sender, RoutedEventArgs e)
        {
            string extension = "csv";
            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = extension,
                Filter =
                    String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension,
                                  "CSV"),
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    GridViewDarkhastha.Export(stream,
                                              new GridViewExportOptions()
                                              {
                                                  Format = ExportFormat.Csv,
                                                  ShowColumnHeaders = false,
                                                  ShowColumnFooters = false,
                                                  ShowGroupFooters = false,
                                                  Encoding = Encoding.UTF8
                                              });
                }

            }
        }

        private void RibbonButtonExportToHtml_Click(object sender, RoutedEventArgs e)
        {
            string extension = "html";
            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = extension,
                Filter =
                    String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension,
                                  "HTML"),
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    GridViewDarkhastha.Export(stream,
                                              new GridViewExportOptions()
                                              {
                                                  Format = ExportFormat.Html,
                                                  ShowColumnHeaders = false,
                                                  ShowColumnFooters = false,
                                                  ShowGroupFooters = false,
                                                  Encoding = Encoding.UTF8
                                              });
                }

            }
        }

        public void SetDateRangeToCurrentMonth()
        {
            DateTime currentDate = DateTime.Now;
            currentDate = currentDate.AddDays(1);
            EndDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);

            TarikhFA.Tarikh tarikh = new Tarikh();
            tarikh.SetDateG(currentDate.Year, currentDate.Month, currentDate.Day);


            int month = tarikh.GetDateS().Month;
            int year = tarikh.GetDateS().Year;

            tarikh.SetDateS(year, month, 1);

            StartDate = tarikh.GetDateG();
        }

        private void MenuItemSearchTarikhDarkhast_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            WindowSearchByDate windowSearchByDate = new WindowSearchByDate();
            windowSearchByDate.TarkhStartDate.SetDateG(StartDate.Year, StartDate.Month, StartDate.Day);
            windowSearchByDate.TarikhEndDate.SetDateG(EndDate.Year, EndDate.Month, EndDate.Day);
            windowSearchByDate.StartDateChanged += new DateChangedEventHandler(windowSearchByDate_StartDateChanged);
            windowSearchByDate.EndDateChanged += new DateChangedEventHandler(windowSearchByDate_EndDateChanged);
            windowSearchByDate.SearchByDate += new EventHandler(windowSearchByDate_SearchByDate);
            windowSearchByDate.Show();
        }

        void windowSearchByDate_SearchByDate(object sender, EventArgs e)
        {
            LoadGridViewDarkhastha();
        }

        void windowSearchByDate_EndDateChanged(object sender, DateChangedEventArgs e)
        {
            this.EndDate = e.Date;
            MenuItemClearSearch.IsEnabled = true;
        }

        void windowSearchByDate_StartDateChanged(object sender, DateChangedEventArgs e)
        {
            this.StartDate = e.Date;
            MenuItemClearSearch.IsEnabled = true;
        }


        void windowSearchTozihat_Search(object sender, SearchKeywordEventArgs e)
        {
            SearchTozihat = e.Keyword;
            LoadGridViewDarkhastha();
            MenuItemClearSearch.IsEnabled = true;

        }

        private void MenuItemSearchTozihat_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            WindowSearchTozihat windowSearchTozihat = new WindowSearchTozihat();
            windowSearchTozihat.Search += new SearchKeywordEventHandler(windowSearchTozihat_Search);
            windowSearchTozihat.Show();
        }

        private void RibbonButtonTimerSettings_Click(object sender, RoutedEventArgs e)
        {
            WindowTimerLoadGridViewDarkhastha windowTimerLoadGridViewDarkhastha = new WindowTimerLoadGridViewDarkhastha();
            windowTimerLoadGridViewDarkhastha.SettingsChenged += new EventHandler(windowTimerLoadGridViewDarkhastha_SettingsChenged);
            windowTimerLoadGridViewDarkhastha.Show();
        }

        void windowTimerLoadGridViewDarkhastha_SettingsChenged(object sender, EventArgs e)
        {
            StopTimerLoadGridViewDarkhastha();
            StartTimerLoadGridViewDarkhastha();
        }

        private void RibbonButtonReloadGridViewDarkhastha_Click(object sender, RoutedEventArgs e)
        {
            LoadGridViewDarkhastha();
        }

        private void MenuItemCopyKalaNameArabi_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Darkhastha2 currentRow = (Darkhastha2)GridViewDarkhastha.SelectedItem;
            if (currentRow != null)
            {
                Clipboard.SetText(currentRow.DarkhastName.Replace("ی", "ي").Replace("ک", "ك"), TextDataFormat.UnicodeText);
            }
            else
            {
                DialogBoxOk dialogBoxOk = new DialogBoxOk();
                dialogBoxOk.Message = "ابتدا یک درخواست را انتخاب کنید";
                dialogBoxOk.ShowDialog();
            }
        }

        private void RibbonButtonShomareDarkhastTadbir_Click(object sender, RoutedEventArgs e)
        {
            Darkhastha2 darkhast = (Darkhastha2)GridViewDarkhastha.SelectedItem;
            if (darkhast == null)
            {
                return;
            }

            WindowShomareDarkhastTadbir windowShomareDarkhastTadbir = new WindowShomareDarkhastTadbir();
            windowShomareDarkhastTadbir.DarkhastGuid = darkhast.DarkhastGuid;
            windowShomareDarkhastTadbir.ShomareDarkhastSaved += new EventHandler(windowShomareDarkhastTadbir_ShomareDarkhastSaved);
            windowShomareDarkhastTadbir.Show();
        }

        private void windowShomareDarkhastTadbir_ShomareDarkhastSaved(object sender, EventArgs e)
        {
            LoadGridViewDarkhastha();
        }

        private void GridViewDarkhastha_DataLoaded(object sender, EventArgs e)
        {
            var itemSource = (Darkhastha2Collecttion)GridViewDarkhastha.ItemsSource;
            var itemQuery = itemSource.Where(x => x.DarkhastGuid == selectedItem.DarkhastGuid);

            if (selectedItem != null)
            {
                GridViewDarkhastha.SelectedItem = itemQuery.FirstOrDefault();
            }

        }
    }
}
