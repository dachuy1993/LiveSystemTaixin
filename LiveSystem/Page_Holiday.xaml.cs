using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Threading;
using LiveSystem.DAO;
using LiveSystem.Model;
using Microsoft.Win32;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.IO;
using LiveCharts;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for Page_Holiday.xaml
    /// </summary>
    public partial class Page_Holiday : Page
    {
        public static string path_Ksystem20 = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";
        string pathFileExcel = @"TempFile//ExcelFile.xlsx";
        List<EmpVacationLeave> list_Excel = new List<EmpVacationLeave>();
        string dateCheck = "";
        string depatment = "ALL";
        string room = "ALL";
        public Page_Holiday()
        {
            InitializeComponent();
            Loaded += Page_Holiday_Loaded;
        }

        private void Page_Holiday_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MainWindow.language);
            ApplyLanguage(MainWindow.language);
            GetVacationLeaveRateDetail();
            GetDataCmbDept();

        }

        //===========================================================================================================//
        private void GetEmpVacationLeave()
        {
            try
            {
                // Lấy dữ liệu phếp năm và thông tin nhân viên
                string query1 = "SPGetDataVacationLeave @date";
                string query2 = "select * from update_employee";
                var listEmp = DataProvider.Instance.executeQuery(Page_Main.path_Ksystem20, query1, new object[] { DateTime.Now.ToString("yyyy-MM-dd") });
                var listEmpInfo = DataProvider.Instance.MySqlExecuteQuery(Page_Main.path_TaixinWeb, query2);

                // Thêm đầy đủ thông tin nhân viên vào listAll
                var listAll = new List<EmpVacationLeave>();
                foreach (DataRow rowA in listEmp.Rows)
                {
                    //foreach(DataRow rowB in listEmpInfo.Rows)
                    //{
                    //    if (rowA["EmpId"].ToString().Trim().ToUpper() == rowB["EmpId"].ToString().Trim().ToUpper())
                    //    {
                    //        rowA["DeptNm"] = rowB["Deptlv2"];
                    //        rowA["MinorNm"] = rowB["Deptlv3"];
                    //    }
                    //}

                    switch (rowA["MinorCd"].ToString().Trim().Substring(0, 3))
                    {
                        case "V93":
                            rowA["MinorCd"] = "MANAGE";
                            break;
                        case "V94":
                            rowA["MinorCd"] = "IT";
                            break;
                        case "V95":
                            rowA["MinorCd"] = "MAR";
                            break;
                        case "V96":
                            rowA["MinorCd"] = "PRO";
                            break;
                        case "V97":
                            rowA["MinorCd"] = "QC";
                            break;
                        case "V98":
                            rowA["MinorCd"] = "HICUP";
                            break;
                    }

                    EmpVacationLeave emp = new EmpVacationLeave();
                    emp.Division = rowA["MinorCd"].ToString();
                    emp.DeptNm = rowA["DeptNm"].ToString();
                    emp.GroupNm = rowA["MinorNm"].ToString();
                    emp.EmpId = rowA["EmpId"].ToString();
                    emp.EmpNm = rowA["EmpNm"].ToString();
                    emp.Old = double.Parse(rowA["Oldtmmhy"].ToString());
                    emp.Total = double.Parse(rowA["Totaltmmhy"].ToString());
                    emp.Used = double.Parse(rowA["Ustmmhy"].ToString());
                    emp.Remain = double.Parse(rowA["Paytmmhy"].ToString());
                    listAll.Add(emp);
                }

                // Lọc dữ liệu theo điều kiện bộ phận, phòng ban, nhóm, mã nhân viên
                if (txtName.Text == "")
                {
                    // Bộ phận != ALL
                    if (cbbDepatment.Text != "ALL")
                    {
                        // Phòng ban == ALL
                        if (cbbRoom.Text == "ALL")
                        {
                            listAll = listAll.Where(x => x.Division == cbbDepatment.Text).ToList();
                        }
                        // Phòng ban != ALL
                        else
                        {
                            // Nhóm == ALL
                            if (cbbTeam.Text == "ALL")
                            {
                                listAll = listAll.Where(x => x.Division == cbbDepatment.Text && x.DeptNm == cbbRoom.Text).ToList();
                            }
                            // Nhóm != ALL
                            else
                            {
                                listAll = listAll.Where(x => x.Division == cbbDepatment.Text && x.DeptNm == cbbRoom.Text && x.GroupNm == cbbTeam.Text).ToList();
                            }
                        }
                    }
                }
                // TextBox mã nhân viên != ""
                else
                {
                    listAll = listAll.Where(x => x.EmpId.Trim().ToUpper() == txtName.Text.Trim().ToUpper()).ToList();
                }

                // Sắp xếp dữ liệu và thêm STT
                listAll = listAll.OrderBy(x => x.EmpId).ToList();
                int i = 1;
                listAll.ForEach(x =>
                {
                    x.ID = i;
                    i++;
                });

                // Hiển thị dữ liệu lên view
                lvThongTin.ItemsSource = listAll;
                list_Excel = listAll;
                lbSoLuong.Content = listAll.Count().ToString() + " (người)";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            
        }
        //===========================================================================================================//

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

        bool checkWorking = false;

        List<Helper_Employee> _tempRoom = new List<Helper_Employee>();
        List<Helper_Employee> _tempTeam = new List<Helper_Employee>();
        string _room = "";
        string _team = "";
        

        // Hiển thị bảng tỷ lệ phép năm
        private void GetVacationLeaveRateDetail()
        {
            try
            {
                string dateCheck = DateTime.Now.ToString("yyyy-MM-dd");
                string query1 = "SPGetDataHolidayMain @date";

                var listEmp = DataProvider.Instance.ExecuteSP(path_Ksystem20, query1, new object[] { dateCheck });


                // Hiển thị danh sách lên view
                lvPhepNam.ClearValue(ListView.ItemsSourceProperty);
                lvPhepNam.ItemsSource = listEmp.DefaultView;

                // Chart
                var _qtyOT = new ChartValues<double>();
                var _nameDept = new ChartValues<string>();
                DataChart.Values3 = _qtyOT;
                List<string> listCity = new List<string>();
                foreach (DataRow Row in listEmp.Rows)
                {
                    _nameDept.Add(Row["Division"].ToString());
                    if (Row["Rate"].ToString().IndexOf(",") > 0)
                    {
                        _qtyOT.Add(double.Parse(Row["Rate"].ToString().Substring(0, Row["Rate"].ToString().IndexOf(",") + 2)));
                    }
                    else
                    {
                        _qtyOT.Add(double.Parse(Row["Rate"].ToString().Substring(0, 2).ToString()));
                    }
                }
                if (MainWindow.language == "vi-VN")
                {
                    DataChart.Title = "Tỷ lệ";
                }
                else
                {
                    DataChart.Title = "율(%)";
                }
                DataChart.Labels = _nameDept;
                DataChart.YFormatter = _qtyOT;
                DataChart.Step = 5;
                DataContext = this;
                Column column = new Column();
                frameChart_Holiday.Navigate(column);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            
        }

        private void cbbRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var click = sender as ComboBox;
            var clickItem = click.SelectedItem;
            cbbTeam.ClearValue(ComboBox.ItemsSourceProperty);
            _room = "ALL";
            _team = "ALL";
            if (clickItem != null)
            {
                room = clickItem.ToString();

                if (room != "ALL")
                {
                    string query = "SPGetDataCbbDeptGroupList @Dept";
                    DataTable listCmbTeam = new DataTable();
                    //var listVSIPMeal = DataProvider.Instance.executeQuery(path_Ksystem25, query, new object[] { dateCheck });
                    listCmbTeam = DataProvider.Instance.executeQuery(path_Ksystem20, query, new object[] { room });
                    //public static List<Helper_Employee> listResultRoom = new List<Helper_Employee>();
                    List<Helper_Employee> listResultTeam = new List<Helper_Employee>();
                    //listResultRoom.Clear();
                    //listResultTeam.Add(new Helper_Employee { Deptlv3 = "ALL" });
                    foreach (DataRow row in listCmbTeam.Rows)
                    {
                        Helper_Employee item = new Helper_Employee();
                        item.Deptlv3 = row[0].ToString();
                        listResultTeam.Add(item);
                    }
                    cbbTeam.ItemsSource = listResultTeam.Select(x => x.Deptlv3).ToList();
                    cbbTeam.SelectedIndex = 0;
                }
            }
        }

        private void cbbTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var click = sender as ComboBox;
            var clickItem = click.SelectedItem as Helper_Employee;
            _team = "ALL";
            if (clickItem != null)
            {
                if (clickItem.EmpNm != "ALL")
                {
                    _team = clickItem.EmpNm;
                }
            }
        }

        private void cbbDepatment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //_tempRoom.Clear();
                //_tempTeam.Clear();
                var click = sender as ComboBox;
                var clickItem = click.SelectedItem;
                cbbRoom.ClearValue(ComboBox.ItemsSourceProperty);
                if (clickItem != null)
                {
                    depatment = clickItem.ToString();

                    if (depatment != "ALL")
                    {
                        string query = "SPGetDataCbbDeptList @Dept";
                        DataTable listCmbRoom = new DataTable();
                        //var listVSIPMeal = DataProvider.Instance.executeQuery(path_Ksystem25, query, new object[] { dateCheck });
                        listCmbRoom = DataProvider.Instance.executeQuery(path_Ksystem20, query, new object[] { depatment });
                        //public static List<Helper_Employee> listResultRoom = new List<Helper_Employee>();
                        List<Helper_Employee> listResultRoom = new List<Helper_Employee>();
                        //listResultRoom.Clear();
                        //listResultRoom.Add(new Helper_Employee { Deptlv2 = "ALL" });
                        foreach (DataRow row in listCmbRoom.Rows)
                        {
                            Helper_Employee item = new Helper_Employee();
                            item.Deptlv2 = row[0].ToString();
                            listResultRoom.Add(item);
                        }
                        cbbRoom.ItemsSource = listResultRoom.Select(x => x.Deptlv2).ToList();
                        cbbRoom.SelectedIndex = 0;
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        private async void GetDataCmbDept()
        {
            string query = "SPGetDataCmbDept ";
            // Lấy dữ liệu và hiển thị
            DataTable listCmb = new DataTable();

            listCmb = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query);


            List<string> listResult = new List<string>();
            foreach (DataRow Row in listCmb.Rows)
            {
                listResult.Add(Row["CbbDept"].ToString());
            }
            cbbDepatment.ItemsSource = listResult;
        }

        private async void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {            
            if (checkWorking == false && MainWindow._checkInternet == "Success")
            {
                await Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Page_LoadingData page_Loading = new Page_LoadingData();
                        stackLoading.Visibility = Visibility.Visible;
                        frameLoading.Navigate(page_Loading);
                        lvThongTin.ClearValue(ListView.ItemsSourceProperty);
                        checkWorking = true;
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);
                });

                await Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        GetEmpVacationLeave();
                        stackLoading.Visibility = Visibility.Hidden;
                        checkWorking = false;
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);
                });
            }
            else
            {
                if (MainWindow.language == "vi-VN")
                {
                    MessageBox.Show("Kiểm tra kết nối mạng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("네트워크 연결 확인", "정보", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            GetVacationLeaveRateDetail();
        }
        private void btnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            Process_ExportExcel();
        }

        public async void Process_ExportExcel()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                await Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        sfd.ShowDialog();
                        if (sfd.FileName == "")
                        {
                            if (MainWindow.language == "vi_VN")
                            {
                                MessageBox.Show("Vui lòng chọn vị trí lưu tập tin", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                MessageBox.Show("파일을 저장할 위치를 선택하십시오", "정보", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);
                });
                await Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        if (sfd.FileName != "")
                        {
                            Page_LoadingData page_Loading = new Page_LoadingData();
                            stackLoading.Visibility = Visibility.Visible;
                            frameLoading.Navigate(page_Loading);
                        }
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);
                });
                await Task.Run(() =>
                {
                    Thread.Sleep(500);
                    this.Dispatcher.Invoke(() =>
                    {
                        if (sfd.FileName != "")
                        {
                            CreatListExcel();
                            File.Copy(pathFileExcel, sfd.FileName + ".xlsx");
                        }
                        stackLoading.Visibility = Visibility.Hidden;
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);
                });
            }
            catch (Exception)
            {
                if (MainWindow.language == "vi-VN")
                {
                    MessageBox.Show("Tên file trùng với một file có sẵn.\nVui lòng nhập một tên mới", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("파일 이름이 중복되었습니다", "정보", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void CreatListExcel()
        {
            try
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    int numberRow = 0;
                    foreach (var item in list_Excel)
                    {
                        if (item.EmpId != "")
                        {
                            numberRow++;
                        }
                    }

                    numberRow = numberRow + 5;
                    p.Workbook.Properties.Author = DateTime.Now.ToShortDateString();
                    p.Workbook.Properties.Title = "Danh sách tỷ lệ phép năm của công nhân viên";
                    p.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet ws = p.Workbook.Worksheets[1];
                    ws.Name = "Sheet1";

                    //Cột 1 
                    ws.Column(1).Width = 5;//stt
                    ws.Column(2).Width = 15;//Bộ phận
                    ws.Column(3).Width = 30;//Phòng ban
                    ws.Column(4).Width = 10;//Nhóm
                    ws.Column(5).Width = 20;//Mã NV
                    ws.Column(6).Width = 30;//Họ tên
                    ws.Column(7).Width = 15;//Phép tồn
                    ws.Column(8).Width = 15;//Phép năm                
                    ws.Column(9).Width = 15;//Sử dụng
                    ws.Column(10).Width = 15;//Còn lại

                    ws.Row(1).Height = 10;
                    ws.Row(2).Height = 40;
                    ws.Row(3).Height = 20;
                    ws.Row(4).Height = 25;


                    //căn hàng và cột cho tất cả các ô                 


                    for (int i = 1; i < numberRow; i++)
                    {
                        string strCell = "A" + i.ToString() + ":" + "J" + i.ToString();
                        var cell = ws.Cells[strCell];
                        var border = cell.Style.Border;
                        border.Bottom.Style =
                        border.Top.Style =
                        border.Left.Style =
                        border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.WrapText = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    for (int i = 5; i < numberRow; i++)
                    {
                        string strCell = "A" + i.ToString() + ":" + "J" + i.ToString();
                        var cell = ws.Cells[strCell];
                        ws.Row(i).Height = 25;
                        cell.Style.Font.Size = 11;
                        cell.Style.Font.Bold = false;

                        string strCell1 = "A" + i.ToString() + ":" + "A" + i.ToString();
                        ws.Cells[strCell1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell2 = "B" + i.ToString() + ":" + "B" + i.ToString();
                        ws.Cells[strCell2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell3 = "C" + i.ToString() + ":" + "C" + i.ToString();
                        ws.Cells[strCell3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell4 = "D" + i.ToString() + ":" + "D" + i.ToString();
                        ws.Cells[strCell4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell5 = "E" + i.ToString() + ":" + "E" + i.ToString();
                        ws.Cells[strCell5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell6 = "F" + i.ToString() + ":" + "F" + i.ToString();
                        ws.Cells[strCell6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell7 = "G" + i.ToString() + ":" + "G" + i.ToString();
                        ws.Cells[strCell7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell8 = "H" + i.ToString() + ":" + "H" + i.ToString();
                        ws.Cells[strCell8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell9 = "I" + i.ToString() + ":" + "I" + i.ToString();
                        ws.Cells[strCell9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        string strCell10 = "J" + i.ToString() + ":" + "J" + i.ToString();
                        ws.Cells[strCell10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    for (int i = 5; i < numberRow; i++)
                    {
                        if (i % 2 == 0)
                        {
                            string strCell = "A" + i.ToString() + ":" + "J" + i.ToString();
                            var cell = ws.Cells[strCell];
                            var fill = cell.Style.Fill;
                            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                        }
                    }

                    //Bôi den backgroud
                    //

                    ws.Cells["A2:J2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A2:J2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Azure);

                    ws.Cells["A4:J4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A4:J4"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Ivory);


                    ws.Cells["A1:A1"].Value = "";
                    ws.Cells["A1:J1"].Merge = true;
                    ws.Cells["A1:A1"].Style.Font.Size = 25;
                    ws.Cells["A1:A1"].Style.Font.Bold = true;


                    ws.Cells["A2:A2"].Value = "DANH SÁCH TỶ LỆ PHÉP NĂM CỦA CÔNG NHÂN VIÊN";
                    ws.Cells["A2:J2"].Merge = true;
                    ws.Cells["A2:A2"].Style.Font.Size = 22;
                    ws.Cells["A2:A2"].Style.Font.Bold = true;

                    //Ngày SX
                    ws.Cells["A3:A3"].Value = "Ngày : " + DateTime.Now.ToString("dd/MM/yyyy") + "  Số lượng : " + (numberRow - 5);
                    ws.Cells["A3:T3"].Merge = true;
                    ws.Cells["A3:A3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["A3:A3"].Style.Font.Bold = true;

                    //Head                  
                    ws.Cells["A4:J4"].Style.Font.Size = 12;
                    ws.Cells["A4:J4"].Style.Font.Bold = true;
                    ws.Cells["A4:A4"].Value = "STT";
                    ws.Cells["B4:B4"].Value = "Bộ phận";
                    ws.Cells["C4:C4"].Value = "Phòng ban";
                    ws.Cells["D4:D4"].Value = "Nhóm";
                    ws.Cells["E4:E4"].Value = "Mã NV";
                    ws.Cells["F4:F4"].Value = "Họ và tên";
                    ws.Cells["G4:G4"].Value = "Phép tồn";
                    ws.Cells["H4:H4"].Value = "Phép năm";
                    ws.Cells["I4:I4"].Value = "Sử dụng";
                    ws.Cells["J4:J4"].Value = "Còn lại";

                    int index = 4;
                    int stt = 0;

                    foreach (var item in list_Excel)
                    {
                        if (item.EmpId != "")
                        {
                            index++;
                            stt++;
                            //--
                            string strCell1 = "A" + index.ToString() + ":" + "A" + index.ToString();
                            ws.Cells[strCell1].Value = stt;
                            //--
                            string strCell2 = "B" + index.ToString() + ":" + "B" + index.ToString();
                            ws.Cells[strCell2].Value = item.Division;
                            //--
                            string strCell3 = "C" + index.ToString() + ":" + "C" + index.ToString();
                            ws.Cells[strCell3].Value = item.DeptNm;
                            //--
                            string strCell4 = "D" + index.ToString() + ":" + "D" + index.ToString();
                            ws.Cells[strCell4].Value = item.GroupNm;
                            //--
                            string strCell5 = "E" + index.ToString() + ":" + "E" + index.ToString();
                            ws.Cells[strCell5].Value = item.EmpId;
                            //--
                            string strCell6 = "F" + index.ToString() + ":" + "F" + index.ToString();
                            ws.Cells[strCell6].Value = item.EmpNm;
                            //--
                            string strCell7 = "G" + index.ToString() + ":" + "G" + index.ToString();
                            ws.Cells[strCell7].Value = item.Old;
                            //--
                            string strCell8 = "H" + index.ToString() + ":" + "H" + index.ToString();
                            ws.Cells[strCell8].Value = item.Total;
                            //--
                            string strCell9 = "I" + index.ToString() + ":" + "I" + index.ToString();
                            ws.Cells[strCell9].Value = item.Used;
                            string strCell10 = "J" + index.ToString() + ":" + "J" + index.ToString();
                            ws.Cells[strCell10].Value = item.Remain;
                        }
                    }
                    ws.PrinterSettings.PaperSize = ePaperSize.A4;
                    ws.PrinterSettings.Orientation = eOrientation.Landscape;
                    ws.PrinterSettings.FitToPage = true;
                    ws.Cells["A4:J4"].AutoFilter = true;
                    ws.PrinterSettings.TopMargin = Decimal.Parse("0");
                    ws.PrinterSettings.LeftMargin = Decimal.Parse("0.25");
                    ws.PrinterSettings.BottomMargin = Decimal.Parse("0.25");
                    ws.PrinterSettings.RightMargin = Decimal.Parse("0.25");
                    File.Delete(pathFileExcel);
                    Byte[] bin = p.GetAsByteArray();
                    File.WriteAllBytes(pathFileExcel, bin);
                    //exportFileExcel = false;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CreatListExcel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
