using LiveSystem.DAO;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiveSystem
{

    /// <summary>
    /// Interaction logic for Page_EnvironmentTab.xaml
    /// </summary>
    public partial class Page_EnvironmentTab : Page
    {
        #region Khai báo 
        public static string path_Ksystem20 = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";
        #endregion 
        public Page_EnvironmentTab()
        {
            InitializeComponent();
            Loaded += Page_Environment_Loaded;
            GetdataTab1();
            
            GetDataCmbYearTab2();
            GetDataCmbYearTab3();
            GetDataCmbWeekTab3();
            SearchTab3();
        }
        public void Page_Environment_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MainWindow.language);
            ApplyLanguage(MainWindow.language);
        }
        private void ApplyLanguage(string cultureName = null)
        {

            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MainWindow.language);
            //ApplyLanguage(MainWindow.language);
            if (cultureName != null)
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);

            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "vi-VN":
                    dict.Source = new Uri("..\\Lang\\Vietnam.xaml", UriKind.Relative);
                    break;
                // ...
                default:
                    dict.Source = new Uri("..\\Lang\\Korea.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }

        private void btnCheckData_Click(object sender, RoutedEventArgs e)
        {
            Window_EnvironSave EnvironSave = new Window_EnvironSave();
            EnvironSave.Show();
        }


        private void btnDanhsach_Click(object sender, RoutedEventArgs e)
        {
            Window_EnvironmentData EnvironData = new Window_EnvironmentData();
            EnvironData.Show();
        }

        private void GetdataTab1()
        {
            try
            {
                var query = "SPGetDataSafeDetailTab1 ";
                var result = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query);
                result.Columns.Add("ID",typeof(string));
                int id = 1;
                foreach ( DataRow item in result.Rows )
                {
                    item["ID"] = id;
                    id++;
                }

                lvSafeTab1.ItemsSource = result.DefaultView;
            }
            catch (Exception)
            {
                MessageBox.Show("Error processing data 'Safe days'", "Error", MessageBoxButton.OK);
            }
        }
        private void GetdataTab2()
        {
            try
            {
                var query = "SPGetDataSafeDetailTab2 @year";
                string thisYear = cbbYearTab2.SelectedItem.ToString();
                var result = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query, new object[] { thisYear });
                result.Columns.Add("ID", typeof(string));
                int id = 1;
                foreach (DataRow item in result.Rows)
                {
                    item["ID"] = id;
                    id++;
                }

                lvSafeTab2.ItemsSource = result.DefaultView;
            }
            catch (Exception)
            {
                MessageBox.Show("Error processing data 'Safe days'", "Error", MessageBoxButton.OK);
            }
        }
       

        private async void GetDataCmbYearTab2()
        {
            string query = "SPGetDataCmbYearSafeTab2 ";
            // Lấy dữ liệu và hiển thị
            DataTable listCmb = new DataTable();

            listCmb = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query);


            List<string> listResult = new List<string>();
            foreach (DataRow Row in listCmb.Rows)
            {
                listResult.Add(Row["Name"].ToString());
            }
            cbbYearTab2.ItemsSource = listResult;
        }


        private async void GetDataCmbYearTab3()
        {
            string query = "SPGetDataCmbYearEnviroTab3 ";
            // Lấy dữ liệu và hiển thị
            DataTable listCmb = new DataTable();

            listCmb = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query);


            List<string> listResult = new List<string>();
            foreach (DataRow Row in listCmb.Rows)
            {
                listResult.Add(Row["CbbYear"].ToString());
            }
            cbbYearTab3.ItemsSource = listResult;
        }

        private async void GetDataCmbWeekTab3()
        {
            string year = cbbYearTab3.Text;
            string query = "SPGetDataCmbWeekEnviroTab3 @Year ";
            // Lấy dữ liệu và hiển thị
            DataTable listCmb = new DataTable();

            listCmb = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query, new object[] { year });


            List<string> listResult = new List<string>();
            foreach (DataRow Row in listCmb.Rows)
            {
                listResult.Add(Row["CbbYear"].ToString());
            }
            cbbWeekTab3.ItemsSource = listResult;
        }

        private void cbbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbbYearTab3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetDataCmbWeekTab3();
        }

        private void BtnCheckTab3_Click(object sender, RoutedEventArgs e)
        {
            SearchTab3();
        }

        private void SearchTab3()
        {
            string year = cbbYearTab3.Text;
            string week = cbbWeekTab3.SelectedItem.ToString();
            string query = "SPGetDataEnviroTab3 @Year , @Week , @Language ";
            // Lấy dữ liệu và hiển thị

            var listCmb = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query, new object[] { year, week, Thread.CurrentThread.CurrentCulture.ToString() });
            listCmb.Columns.Add("ID", typeof(string));
            int id = 1;

            foreach (DataRow Row in listCmb.Rows)
            {
                Row["Rate"] = Row["Rate"] + "%";
                Row["ID"] = id;
                id++;
            }
            lvSafeTab3.ItemsSource = listCmb.DefaultView;
        }

        private void cbbYearTab2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetdataTab2();
        }

        private void BtnCheckTab2_Click(object sender, RoutedEventArgs e)
        {
            GetdataTab2();
        }

        private void BtnCheckDataTab3_Click(object sender, RoutedEventArgs e)
        {
            Window_EnvironTabSave EnvironSave = new Window_EnvironTabSave();
            EnvironSave.Show();
        }

        private void btnCheckData2_Click(object sender, RoutedEventArgs e)
        {
            Window_EnvironSave EnvironSave = new Window_EnvironSave();
            EnvironSave.Show();
        }

        private void btnDanhsach2_Click(object sender, RoutedEventArgs e)
        {
            Window_EnvironmentData EnvironData = new Window_EnvironmentData();
            EnvironData.Show();
        }

        private void btnQueryDataTab3_Click(object sender, RoutedEventArgs e)
        {
            Window_EnvironmentWeekData EnvironDataTab3 = new Window_EnvironmentWeekData();
            EnvironDataTab3.Show();
        }

        private void BtnCheckData1_Click(object sender, RoutedEventArgs e)
        {
            Window_EnvironNumDaySave EnvironDataTab1 = new Window_EnvironNumDaySave();
            EnvironDataTab1.Show();
        }

        private void BtnSearchData1_Click(object sender, RoutedEventArgs e)
        {
            GetdataTab1();
        }

        private void cbbWeekTab3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTab3();
        }
    }
}
