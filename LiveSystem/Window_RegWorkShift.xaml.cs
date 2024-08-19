using LiveSystem.DAO;
using Newtonsoft.Json;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
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
using static LiveSystem.Window_EnvironNumDaySave;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for Window_RegWorkShift.xaml
    /// </summary>
    public partial class Window_RegWorkShift : Window
    {
        #region Khai báo 
        public static string path_Ksystem20 = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";
        public static string dateCheckFr = DateTime.Now.ToString("yyyyMMdd");
        public static string dateCheckTo = DateTime.Now.ToString("yyyyMMdd");
        List<ListWorkShift> listEmpNums = new List<ListWorkShift>();
        List<ListWorkShift> listEmpCheck = new List<ListWorkShift>();
        public string checkAUD = "A";
        #endregion
        public Window_RegWorkShift()
        {
            InitializeComponent();
        }

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (dpk_DateStr.Text == "" || dpk_DateEnd.Text == "")
            {
                MessageBox.Show("Ngày bắt đầu và ngày kết thúc không được trống", "Thông báo", MessageBoxButton.OK);
                return;
            }
            if (Cbb_DeptNm.Text == "" || Cbb_WorkShift.Text == "")
            {
                MessageBox.Show("Kiểm tra lại bộ phận hoặc ca làm việc không được trống", "Thông báo", MessageBoxButton.OK);
                return;
            }    
            if (txt_EmpNo.Text.Length > 6)
            {
                MessageBox.Show("Kiểm tra lại mã nhân viên", "Thông báo", MessageBoxButton.OK);
                return;
            }
            string querySave = "SPGetDataCreateWorkShiftSave @pEmpNo , @pEmpNm , @pDeptNm , @pShift , @pDateFr , @pDateTo , @pRemark , @pCheckAUD   ";
            var result = DataProvider.Instance.ExecuteSP(path_Ksystem20, querySave, new object[]
            {
                txt_EmpNo.Text,
                txt_EmpNm.Text,
                Cbb_DeptNm.Text,
                Cbb_WorkShift.Text,

                dateCheckFr,
                dateCheckTo,
                Txt_Remark.Text,
                checkAUD
            });
            string resultMess = "";
            foreach (DataRow row in result.Rows)
            {
                resultMess = row[0].ToString();
            }
            MessageBox.Show(resultMess, "Thông báo", MessageBoxButton.OK);

            Search();
        }

        private void Search()
        {
            lvWorkShiftVendor.ClearValue(ListView.ItemsSourceProperty);
            listEmpNums.Clear();
            listEmpCheck.Clear();
            string query = "SPGetDataCreateWorkShiftQry  @pEmpNo , @pEmpNm , @pDeptNm , @pShift , @pDateFr , @pDateTo , @pRemark  ";
            var result = DataProvider.Instance.ExecuteSP(path_Ksystem20, query, new object[]
            {
                txt_EmpNo.Text,
                txt_EmpNm.Text,
                Cbb_DeptNm.Text,
                Cbb_WorkShift.Text,
                dateCheckFr,
                dateCheckTo,
                Txt_Remark.Text,
            });
            
            int id = 1;
            foreach (DataRow row in result.Rows)
            {
                listEmpNums.Add(new ListWorkShift
                {
                    EmpId = row[0].ToString(),
                    EmpNm = row[1].ToString(),
                    DeptNm = row[2].ToString(),
                    Shift = row[3].ToString(),
                    FrDate = row[4].ToString(),
                    ToDate = row[5].ToString(),
                    Remark = row[6].ToString(),
                    FrDatetxt = row[7].ToString(),
                    ToDatetxt = row[8].ToString(),
                    DateFr = row[9].ToString(),
                    DateTo = row[10].ToString(),
                    ID = id,

                });


                listEmpCheck.Add(new ListWorkShift
                {
                    EmpId = row[0].ToString(),
                    EmpNm = row[1].ToString(),
                    DeptNm = row[2].ToString(),
                    Shift = row[3].ToString(),
                    FrDate = row[4].ToString(),
                    ToDate = row[5].ToString(),
                    Remark = row[6].ToString(),
                    FrDatetxt = row[7].ToString(),
                    ToDatetxt = row[8].ToString(),
                    DateFr = row[9].ToString(),
                    DateTo = row[10].ToString(),
                    ID = id,

                });
                id++;
            }

            lvWorkShiftVendor.ItemsSource = listEmpNums;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //string resultMess = "";
            checkAUD = "D";
            foreach(ListWorkShift item in lvWorkShiftVendor.Items)
            {
                if(item.checkUpload == "True")
                {
                    string querySave = "SPGetDataCreateWorkShiftSave @pEmpNo , @pEmpNm , @pDeptNm , @pShift , @pDateFr , @pDateTo , @pRemark , @pCheckAUD   ";
                    var result = DataProvider.Instance.ExecuteSP(path_Ksystem20, querySave, new object[]
                    {
                        item.EmpId,
                        item.EmpNm,
                        item.DeptNm,
                        item.Shift,
                        item.DateFr,
                        item.DateTo,
                        item.Remark,
                        checkAUD
                    });
                    
                    //foreach (DataRow row in result.Rows)
                    //{
                    //    resultMess = row[0].ToString();
                    //}
                }
            }
            MessageBox.Show("Đã xoá dữ liệu thành công", "Thông báo", MessageBoxButton.OK);

            //string resultMess = "";
            //foreach (DataRow row in result.Rows)
            //{
            //    resultMess = row[0].ToString();
            //}
            //MessageBox.Show(resultMess, "Thông báo", MessageBoxButton.OK);

            Search();
        }

        private void dpk_DateStr_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var setting1 = new JsonSerializerSettings { DateFormatString = "yyyyMMdd" };
            var dt1 = JsonConvert.SerializeObject(DateTime.Parse(dpk_DateStr.SelectedDate.ToString()).ToString("yyyyMMdd"), setting1);
            dateCheckFr = dt1.Substring(1, dt1.Length - 2);
        }

        private void dpk_DateEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var setting1 = new JsonSerializerSettings { DateFormatString = "yyyyMMdd" };
            var dt1 = JsonConvert.SerializeObject(DateTime.Parse(dpk_DateEnd.SelectedDate.ToString()).ToString("yyyyMMdd"), setting1);
            dateCheckTo = dt1.Substring(1, dt1.Length - 2);
        }

        

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            checkAUD = "A";
            txt_EmpNo.IsEnabled = true;
            Cbb_WorkShift.IsEnabled = true;
            dpk_DateStr.IsEnabled = true;
            txt_EmpNo.Text = "";
            txt_EmpNm.Text = "";
            Txt_Remark.Text = "";
            Cbb_DeptNm.Text = "";
            Cbb_WorkShift.Text = "";
            dateCheckFr = DateTime.Now.ToString("yyyyMMdd");
            dateCheckTo = DateTime.Now.ToString("yyyyMMdd");
        }

        public class ListWorkShift
        {
            public int ID { get; set; }
            public string EmpId { get; set; }
            public string EmpNm { get; set; }
            public string DeptNm { get; set; }
            public string Shift { get; set; }
            public string Remark { get; set; }
            public string FrDate { get; set; }
            public string ToDate { get; set; }

            public string _frdatetxt { get; set; }
            public string _todatetxt { get; set; }

            public string FrDatetxt { get { return _frdatetxt; } set { if (_frdatetxt != value) { _frdatetxt = value; NotifyPropertyChanged("FrDatetxt"); } } }
            public string ToDatetxt { get { return _todatetxt; } set { if (_todatetxt != value) { _todatetxt = value; NotifyPropertyChanged("ToDatetxt"); } } }
            private string _checkUpload;
            public string checkUpload { get { return _checkUpload; } set { if (_checkUpload != value) { _checkUpload = value; NotifyPropertyChanged("checkUpload"); } } }

            public string DateFr { get; set; }
            public string DateTo { get; set; }
            private void NotifyPropertyChanged(string v)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }

        private void lvWorkShiftVendor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            checkAUD = "U";
            txt_EmpNo.IsEnabled = false;
            Cbb_WorkShift.IsEnabled = false;
            dpk_DateStr.IsEnabled = false;
            var click = sender as ListView;
            var clickItem = click.SelectedItem as ListWorkShift;
            if (clickItem != null)
            {
                txt_EmpNo.Text = ((ListWorkShift)lvWorkShiftVendor.SelectedItem).EmpId;
                txt_EmpNm.Text = ((ListWorkShift)lvWorkShiftVendor.SelectedItem).EmpNm;
                Cbb_DeptNm.Text = ((ListWorkShift)lvWorkShiftVendor.SelectedItem).DeptNm;
                Cbb_WorkShift.Text = ((ListWorkShift)lvWorkShiftVendor.SelectedItem).Shift;
                if (clickItem.FrDatetxt.ToString() != "")
                {
                    dpk_DateStr.SelectedDate = DateTime.Parse(clickItem.FrDatetxt.ToString());
                }
                if (clickItem.ToDatetxt.ToString() != "")
                {
                    dpk_DateEnd.SelectedDate = DateTime.Parse(clickItem.ToDatetxt.ToString());
                }
                Txt_Remark.Text = ((ListWorkShift)lvWorkShiftVendor.SelectedItem).Remark;
            }
        }

        private void btnUploadExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                Window_RegWorkShiftExcel window_RegWorkShiftExcel = new Window_RegWorkShiftExcel();
                window_RegWorkShiftExcel.Show();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error Upload Excel to system", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void CheckUpload()
        {
            foreach (var item in listEmpCheck)
            {
                item.checkUpload = "True";
            }
            if (lvWorkShiftVendor != null)
            {
                lvWorkShiftVendor.ClearValue(ListView.ItemsSourceProperty);
            }


            lvWorkShiftVendor.ItemsSource = listEmpCheck;

        }

        public void UnCheckUpload()
        {
            foreach (var item in listEmpCheck)
            {
                item.checkUpload = "False";
            }
            if(lvWorkShiftVendor != null)
            {
                lvWorkShiftVendor.ClearValue(ListView.ItemsSourceProperty);
            }

            lvWorkShiftVendor.ItemsSource = listEmpCheck;

        }



        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }


        private void ckb_All_Checked(object sender, RoutedEventArgs e)
        {
            CheckUpload();
        }

        private void ckb_All_Unchecked(object sender, RoutedEventArgs e)
        {
            UnCheckUpload();
        }

        private void checkDetail_Checked(object sender, RoutedEventArgs e)
        {
            var click = sender as CheckBox;
            var clickItem = click.DataContext as ListWorkShift;
            if (clickItem != null)
            {
                clickItem.checkUpload = "True";
            }
        }

        private void checkDetail_UnChecked(object sender, RoutedEventArgs e)
        {
            var click = sender as CheckBox;
            var clickItem = click.DataContext as ListWorkShift;
            if (clickItem != null)
            {
                clickItem.checkUpload = "False";
            }
        }
    }
}
