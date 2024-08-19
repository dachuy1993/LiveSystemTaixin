using LiveSystem.DAO;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
//using Microsoft.Office.Interop.Excel;





namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for Window_RegWorkShiftExcel.xaml
    /// </summary>
    public partial class Window_RegWorkShiftExcel : System.Windows.Window
    {
        public Window_RegWorkShiftExcel()
        {
            InitializeComponent();
        }
        List<ListWorkShift> listDataExcel = new List<ListWorkShift>();
        List<ListWorkShift> listDataErr = new List<ListWorkShift>();
        private void btnUploadExcel_Click(object sender, RoutedEventArgs e)
        {
            listDataExcel.Clear();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".xlsx";
            dialog.Filter = "Excel Documents (*.xlsx)|*.xlsx";
            dialog.ShowDialog();

            if (string.IsNullOrEmpty(dialog.FileName))
                return;

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb = excel.Workbooks.Open(dialog.FileName);
            Worksheet ws = wb.Sheets["Sheet1"];

            Range range = ws.UsedRange;

            System.Data.DataTable dt = new System.Data.DataTable();

            dt.Columns.Add("STT");
            dt.Columns.Add("EmpId");
            dt.Columns.Add("EmpNm");
            dt.Columns.Add("DeptNm");
            dt.Columns.Add("ShiftNm");

            dt.Columns.Add("FrDate");
            dt.Columns.Add("ToDate");
            dt.Columns.Add("Remark");


            try
            {
                for (var row = 4; row <= range.Rows.Count; row++)
                {
                    var dr = dt.NewRow();
                    for (var column = 1; column <= range.Columns.Count; column++)
                    {
                        Microsoft.Office.Interop.Excel.Range xlCell = (Microsoft.Office.Interop.Excel.Range)range.Cells[row, column];
                        if (xlCell != null && xlCell.Value2 != null)
                        {
                            dr[column - 1] = xlCell.Value2.ToString();
                        }
                        else
                        {
                            dr[column - 1] = "";

                        }

                    }
                    dt.Rows.Add(dr);
                }
                int ii = 1;
                int check = 0;
                int totalQty = 0;

                foreach (DataRow dr in dt.Rows)
                {

                    ListWorkShift data = new ListWorkShift
                    {
                        ID = ii,
                        EmpId = dr[1].ToString(),
                        EmpNm = dr[2].ToString(),
                        DeptNm = dr[3].ToString(),
                        ShiftNm = dr[4].ToString(),
                        FrDate = dr[5].ToString(),
                        ToDate = dr[6].ToString(),
                        Remark = dr[7].ToString()
                    };
                    //UploadExcelData.Add(data);

                    check = 1;
                    ii = ii + 1;

                    listDataExcel.Add(data);
                    //_barcodeService.UploadMaster(barcode);
                }



                //SaveUploadExcel();



                wb.Close(true, Missing.Value, Missing.Value);
                excel.Quit();
                //GetAllMaster();

                //DialogHost.Close("MainDialog");
            }
            catch
            {
                wb.Close(true, Missing.Value, Missing.Value);
                excel.Quit();
                //DialogHost.Close("MainDialog");
                //MessageBox.Show("Upload thất bại, kiểm tra lại file upload và thực hiện lại.");
            }


            ListDataUpload.ItemsSource = listDataExcel;

        }
        

        private void BtnSaveUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(MainWindow.path_Ksystem20))
                {
                    conn.Open();

                    var command = "";
                    command = "Delete TYWEmpGroupRegVendorErr_VN";

                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        cmd.CommandTimeout = 100;
                        cmd.ExecuteNonQuery();
                    }

                    foreach (var data in listDataExcel)
                    {
                        

                        var str_applydt = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                        var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss" };
                        var jsonDateInput = JsonConvert.SerializeObject(DateTime.Now, settings);
                        var jsonDateStart = JsonConvert.SerializeObject(DateTime.Now, settings);
                        var jsonDateFinish = JsonConvert.SerializeObject(DateTime.Now.AddDays(1), settings);
                        string dateInput = jsonDateInput.Substring(1, jsonDateInput.Length - 2);
                        string dateStart = jsonDateStart.Substring(1, jsonDateStart.Length - 2);
                        string dateFinish = jsonDateFinish.Substring(1, jsonDateFinish.Length - 2);
                        
                        if (data.EmpId != "")
                        {
                            string query = "SPGetDataCreateWorkShiftSaveExcel @pEmpId , @pEmpNm , @pDeptNm , @pShift , @pDateFr , @pDateTo , @pRemark ";

                            System.Data.DataTable listCmb = new System.Data.DataTable();

                            listCmb = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query, new object[] { 
                                data.EmpId, 
                                data.EmpNm, 
                                data.DeptNm,
                                data.ShiftNm,
                                data.FrDate,
                                data.ToDate,
                                data.Remark
                            });

                           
                        }

                    }

                    

                    string queryerr = "select count(*) from TYWEmpGroupRegVendorErr_VN";
                    string NumErr = "";
                    using (SqlCommand cmd = new SqlCommand(queryerr, conn))
                    {
                         NumErr = cmd.ExecuteScalar().ToString();
                    }
                    
                    if(NumErr == "0")
                    {
                        MessageBox.Show("Bạn đã lưu dữ liệu thành công", "Thông báo", MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show("Có " + NumErr + " nhân viên bị lỗi upload, mời bạn kiểm tra lại", "Thông báo", MessageBoxButton.OK);
                    }

                    
                    conn.Close();

                }

            }
            catch
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu");
            }
        }

        public class ListWorkShift
        {
            public int ID { get; set; }
            public string EmpId { get; set; }
            public string EmpNm { get; set; }
            public string DeptNm { get; set; }
            public string ShiftNm { get; set; }
            public string Remark { get; set; }
            public string FrDate { get; set; }
            public string ToDate { get; set; }

            public string _frdatetxt { get; set; }
            public string _todatetxt { get; set; }

            public string FrDatetxt { get { return _frdatetxt; } set { if (_frdatetxt != value) { _frdatetxt = value; NotifyPropertyChanged("FrDatetxt"); } } }
            public string ToDatetxt { get { return _todatetxt; } set { if (_todatetxt != value) { _todatetxt = value; NotifyPropertyChanged("ToDatetxt"); } } }

            private void NotifyPropertyChanged(string v)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }

        private void BtnQryErr_Click(object sender, RoutedEventArgs e)
        {
            string query = "SPGetDataErrUploadExcelWorkShift  ";
            var result = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query, new object[] {});
            List<ListWorkShift> listEmpNums = new List<ListWorkShift>();
            int id = 1;
            foreach (DataRow row in result.Rows)
            {
                listEmpNums.Add(new ListWorkShift
                {
                    EmpId = row[0].ToString(),
                    EmpNm = row[1].ToString(),
                    DeptNm = row[2].ToString(),
                    ShiftNm = row[3].ToString(),
                    FrDate = row[4].ToString(),
                    ToDate = row[5].ToString(),
                    Remark = row[6].ToString(),
                    ID = id,

                });
                id++;
            }

            ListDataUpload.ItemsSource = listEmpNums;
        }
    }
}
