using LiveSystem.DAO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Window_EnvironNumDaySave.xaml
    /// </summary>
    public partial class Window_EnvironNumDaySave : Window
    {
        #region Khai báo 
        public static string path_Ksystem20 = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";
        public static string dateCheckNumday = DateTime.Now.ToString("yyyyMMdd");
        #endregion
        public Window_EnvironNumDaySave()
        {
            InitializeComponent();
        }


        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            string query = "SPGetDataNumDayTab1 ";
            var result = DataProvider.Instance.ExecuteSP(path_Ksystem20, query);
            List<ListEmpNum> listEmpNums = new List<ListEmpNum>();
            int id = 1;
            foreach (DataRow row in result.Rows)
            {
                listEmpNums.Add(new ListEmpNum
                {
                    AreaNm = row[0].ToString(),
                    NumDay = row[1].ToString(),
                    EmpNo = row[3].ToString(),
                    EmpNm = row[4].ToString(),
                    LocationAcc = row[5].ToString(),
                    TypeAcc = row[6].ToString(),
                    DateAcc = row[2].ToString(),
                    DateN = row[7].ToString(),
                    ID = id,

                });
                id++;
            }

            lvDayEccManager.ItemsSource = listEmpNums;
        }



        private void dpk_Check_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var setting1 = new JsonSerializerSettings { DateFormatString = "yyyyMMdd" };
            var dt1 = JsonConvert.SerializeObject(DateTime.Parse(dpk_Check.SelectedDate.ToString()).ToString("yyyyMMdd"), setting1);
            dateCheckNumday = dt1.Substring(1, dt1.Length - 2);
        }

        private void lvDayEccManager_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var click = sender as ListView;
            var clickItem = click.SelectedItem as ListEmpNum;

            txt_Area.Text = ((ListEmpNum)lvDayEccManager.SelectedItem).AreaNm;
            
            txt_EmpNo.Text = ((ListEmpNum)lvDayEccManager.SelectedItem).EmpNo;
            txt_LocationAcc.Text = ((ListEmpNum)lvDayEccManager.SelectedItem).LocationAcc;
            txt_TypeAcc.Text = ((ListEmpNum)lvDayEccManager.SelectedItem).TypeAcc;
            if(clickItem.DateN.ToString() != "")
            {
                dpk_Check.SelectedDate = DateTime.Parse(clickItem.DateN.ToString());
            }

            
            //dpkFinishApprove.SelectedDate = DateTime.Parse(clickItem.DATEFINISHAPPROVE.ToString());


        }
        public class ListEmpNum
        {
            public int ID { get; set; }
            public string AreaNm { get; set; }
            public string NumDay { get;set; }
            public string DateAcc { get; set; }
            public string EmpNo { get; set; }
            public string EmpNm { get; set; }
            public string LocationAcc { get; set; }
            public string TypeAcc { get; set; }
            public string _dateN { get; set; }

            public string DateN { get { return _dateN; } set { if (_dateN != value) { _dateN = value; NotifyPropertyChanged("DateN"); } } }

            private void NotifyPropertyChanged(string v)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txt_EmpNo.Text.Length > 6)
            {
                MessageBox.Show("Kiểm tra lại mã nhân viên", "Thông báo", MessageBoxButton.OK);
                return;
            }
            string querySave = "SPGetDataNumDaySaveTab1 @AreaNm , @DateAcc , @EmpNo , @LocationAcc , @TypeAcc ";
            var result = DataProvider.Instance.ExecuteSP(path_Ksystem20, querySave, new object[]
            {
                txt_Area.Text,
                dateCheckNumday,
                txt_EmpNo.Text,
                txt_LocationAcc.Text,
                txt_TypeAcc.Text,
            });
            string resultMess = "";
            foreach(DataRow row in result.Rows)
            {
                resultMess = row[0].ToString();
            }
            MessageBox.Show(resultMess, "Thông báo", MessageBoxButton.OK);

            Search();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (txt_Area.Text == "")
            {
                MessageBox.Show("Kiểm tra lại khu vực", "Thông báo", MessageBoxButton.OK);
                return;
            }
            string queryDel = "SPGetDataNumDayDelTab1 @AreaNm ";
            var result = DataProvider.Instance.ExecuteSP(path_Ksystem20, queryDel, new object[]
            {
                txt_Area.Text,
            });
            string resultMess = "";
            foreach (DataRow row in result.Rows)
            {
                resultMess = row[0].ToString();
            }
            MessageBox.Show(resultMess, "Thông báo", MessageBoxButton.OK);

            Search();
        }
    }
}
