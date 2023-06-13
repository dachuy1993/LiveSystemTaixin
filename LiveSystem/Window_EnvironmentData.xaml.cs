using LiveSystem.DAO;
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
using System.Windows.Shapes;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for Window_EnvironmentData.xaml
    /// </summary>
    public partial class Window_EnvironmentData : Window
    {
        public static string path_Ksystem20 = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";
        public Window_EnvironmentData()
        {
            InitializeComponent();
            Loaded += Page_Environment_Loaded;
            GetDataCmb();
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

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            string query = "SPGetDataForSafeQuery @Year , @TimeRv , @AreaRv";
            string Year = cbbYear.Text;
            string TimeRv = cbbTimeReview.Text;
            string AreaRv = cbbAreaNm.Text;
            DataTable ListDataForYear = new DataTable();
            ListDataForYear = DataProvider.Instance.ExecuteSP(path_Ksystem20, query, new object[] { Year, TimeRv, AreaRv });

            

            lvErrManager.ItemsSource = ListDataForYear.DefaultView;
        }

        private async void GetDataCmb()
        {
            //lây dữ liệu lên cbb Year
            string cbYear = "";
            string queryYear = "SPGetDataCmbYearSafe @cbYear ";

            // Lấy dữ liệu và hiển thị
            DataTable listCmbYear = new DataTable();

            listCmbYear = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, queryYear, new object[] { cbYear });


            List<string> listResultYear = new List<string>();


            foreach (DataRow Row in listCmbYear.Rows)
            {
                listResultYear.Add(Row["Name"].ToString());
            }
            cbbYear.ItemsSource = listResultYear;

        }

        private void cbbYearChange(object sender, SelectionChangedEventArgs e)
        {
            var click = sender as ComboBox;
            var clickItem = click.SelectedItem as ComboBoxItem;
            string queryTimes = "SPGetDataCmbTimesSafe @cbYear";
            string queryArea = "SPGetDataCmbAreaSafe @cbYear";
            //lấy dữ liệu cbb times
            string Year = "ALL";
            //Year = clickItem.Content.ToString();

            DataTable listCmbTimes = new DataTable();
            DataTable listCmbArea = new DataTable();
            listCmbTimes = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, queryTimes, new object[] { Year });
            listCmbArea = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, queryArea, new object[] { Year });

            List<string> listResultTimes = new List<string>();
            List<string> listResultArea = new List<string>();
            foreach (DataRow Row in listCmbTimes.Rows)
            {
                listResultTimes.Add(Row["Name"].ToString());
            }
            cbbTimeReview.ItemsSource = listResultTimes;

            foreach (DataRow Row in listCmbArea.Rows)
            {
                listResultArea.Add(Row["Name"].ToString());
            }
            cbbAreaNm.ItemsSource = listResultArea;
        }
    }
}
