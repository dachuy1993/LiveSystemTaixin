using LiveSystem.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
    /// Interaction logic for Window_EnvironmentWeekData.xaml
    /// </summary>
    public partial class Window_EnvironmentWeekData : Window
    {
        public static string path_Ksystem20 = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";
        public Window_EnvironmentWeekData()
        {
            InitializeComponent();
            GetDataCmbYear();
        }

        private void cbbYearChange(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void GetDataCmbYear()
        {
            //lây dữ liệu lên cbb Year
            string queryYear = "SPGetDataCmbYearSafeDataTab3 ";

            // Lấy dữ liệu và hiển thị
            DataTable listCmbYear = new DataTable();

            listCmbYear = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, queryYear);


            List<string> listResultYear = new List<string>();


            foreach (DataRow Row in listCmbYear.Rows)
            {
                listResultYear.Add(Row["Name"].ToString());
            }
            cbbYearTab3.ItemsSource = listResultYear;

        }

        //private async void GetDataCmbWeek()
        //{
        //    string cbYear = cbbYearTab3.Text;
        //    //lây dữ liệu lên cbb Year
        //    string queryYear = "SPGetDataCmbWeekSafeTab3 @cbYear ";

        //    // Lấy dữ liệu và hiển thị
        //    DataTable listCmbWeek = new DataTable();

        //    listCmbWeek = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, queryYear, new object[] { cbYear });


        //    List<string> listResultWeek = new List<string>();


        //    foreach (DataRow Row in listCmbWeek.Rows)
        //    {
        //        listResultWeek.Add(Row["Name"].ToString());
        //    }
        //    cbbWeekTab3.ItemsSource = listResultWeek;


        //}

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            string query = "SPGetDataForSafeWeekDataQuery @Year ";
            string Year = cbbYearTab3.Text;
            DataTable ListDataForYear = new DataTable();
            ListDataForYear = DataProvider.Instance.ExecuteSP(path_Ksystem20, query, new object[] { Year });



            lvErrManager.ItemsSource = ListDataForYear.DefaultView;
        }
    }
}
