using LiveSystem.DAO;
using LiveSystem.Model;
using Microsoft.Win32;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.IO;
using LiveCharts;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for Page_WorkingRate.xaml
    /// </summary>
    /// 

    public partial class Page_WorkingRate : Page
    {
        #region Khai báo

        public static string path_Ksystem20 = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";
        public static string shiftCheck = "Tất cả";
        List<EmpWorkingModel> list_Excell = new List<EmpWorkingModel>();
        
        string dateCheck = "";
        string depatment = "ALL";
        string room = "ALL";
        bool checkWorking = false;

        string pathFileExcel = @"TempFile//ExcelFile.xlsx";
        #endregion
        public Page_WorkingRate()
        {
            InitializeComponent();
            dpk_Check.SelectedDate = DateTime.Now;
            Loaded += Page_WorkingRate_Loaded;
        }

        private void Page_WorkingRate_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MainWindow.language);
            ApplyLanguage(MainWindow.language);
            GetWorkingRate();
            GetDataCmbDept();
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


        


        //===================================================================================================//
        private void GetListEmpWorking()
        {
            try
            {
                // Lọc dữ liệu theo điều kiện đi làm, nghỉ làm, đi muộn, ca làm việc
                string status = "";
                string shift = "";
                if (rb_On.IsChecked == true)
                    status = "On";
                if (rb_Off.IsChecked == true)
                    status = "Off";
                if (rb_Delay.IsChecked == true)
                    status = "Late";

                if (rb_ShiftA.IsChecked == true)
                    shift = "Ca ngày";
                if (rb_ShiftB.IsChecked == true)
                    shift = "Ca đêm";
                if (rb_ShiftAll.IsChecked == true)
                    shift = "Tất cả";

                // Lấy dữ liệu nhân viên
                string query1 = "exec SPGetDataWorkingRate @date , @shift , @status";
                string query2 = "select * from update_employee";
                DataTable listEmpWorking = new DataTable();
                DataTable listEmpInfo = new DataTable();
                listEmpWorking = DataProvider.Instance.executeQuery(Page_Main.path_Ksystem20, query1, new object[] { dateCheck, shift, status });
                listEmpInfo = DataProvider.Instance.MySqlExecuteQuery(Page_Main.path_TaixinWeb, query2);
                //var listAll = listEmpInfo.AsEnumerable().Join(listEmpWorking.AsEnumerable(), x => x["EmpId"].ToString().Trim().ToUpper(), y => y["EmpId"].ToString().Trim().ToUpper(), (x, y) => new {x, y})
                //    .Select(s => new EmpWorkingModel
                //    {
                //        Division = s.y["Division"].ToString(),
                //        DeptNm = s.x["Deptlv2"].ToString(),
                //        GroupNm = s.x["Deptlv3"].ToString(),
                //        EmpId = s.x["EmpId"].ToString(),
                //        EmpNm = s.x["EmpNm"].ToString(),
                //        Remark = s.y["Remark"].ToString()
                //    }).ToList();
                var listAll = new List<EmpWorkingModel>();
                foreach (DataRow rowA in listEmpWorking.Rows)
                {
                    //foreach(DataRow rowB in listEmpInfo.Rows)
                    //{
                    //    if (rowA["EmpId"].ToString().Trim().ToUpper() == rowB["EmpId"].ToString().Trim().ToUpper())
                    //    {
                    //        rowA["DeptNm"] = rowB["Deptlv2"];
                    //        rowA["GroupNm"] = rowB["Deptlv3"];
                    //    }
                    //}

                    switch (rowA["Division"].ToString().Trim().Substring(0, 3))
                    {
                        case "V93":
                            rowA["Division"] = "MANAGE";
                            break;
                        case "V94":
                            rowA["Division"] = "IT";
                            break;
                        case "V95":
                            rowA["Division"] = "MAR";
                            break;
                        case "V96":
                            rowA["Division"] = "PRO";
                            break;
                        case "V97":
                            rowA["Division"] = "QC";
                            break;
                        case "V98":
                            rowA["Division"] = "HICUP";
                            break;
                    }

                    EmpWorkingModel emp = new EmpWorkingModel();
                    emp.Division = rowA["Division"].ToString();
                    emp.DeptNm = rowA["DeptNm"].ToString();
                    emp.GroupNm = rowA["GroupNm"].ToString();
                    emp.EmpId = rowA["EmpId"].ToString();
                    emp.EmpNm = rowA["EmpNm"].ToString();
                    emp.Remark = rowA["Remark"].ToString();
                    listAll.Add(emp);
                }

                // Lọc dữ liệu theo điều kiện bộ phận, phòng ban, nhóm, EmpId
                if (txtName.Text == "")
                {
                    // Bộ phận khác ALL
                    if (cbbDepatment.Text != "ALL")
                    {
                        // Phòng ban = ALL
                        if (cbbRoom.Text == "ALL")
                        {
                            listAll = listAll.Where(x => x.Division == cbbDepatment.Text).ToList();
                        }
                        else
                        {
                            // Nhóm = ALL
                            if (cbbTeam.Text == "ALL")
                            {
                                listAll = listAll.Where(x => x.Division == cbbDepatment.Text && x.DeptNm == cbbRoom.Text).ToList();
                            }
                            else
                            {
                                listAll = listAll.Where(x => x.Division == cbbDepatment.Text && x.DeptNm == cbbRoom.Text && x.GroupNm == cbbTeam.Text).ToList();
                            }
                        }
                    }
                }
                // Lọc dữ liệu theo mã nhân viên
                else
                {
                    listAll = listAll.Where(x => x.EmpId.Trim().ToUpper() == txtName.Text.Trim().ToUpper()).ToList();
                }

                // Thêm STT
                int i = 1;
                listAll.ForEach(x =>
                {
                    x.ID = i;
                    i++;
                });

                // Hiển thị dữ liệu lên view
                lvThongTin.ItemsSource = listAll;
                list_Excell = listAll;
                lbSoLuong.Content = listAll.Count().ToString() + " (người)";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            
        }
        //===================================================================================================//

        // Hiển thị bảng tỷ lệ đi làm
        private async void GetWorkingRate()
        {
            try
            {
                //string query = "SELECT * from tmmwrate where Shift = @shift and Insdt = @date";
                string query = "SPGetDataRateWorkMain @shift , @date";
                // Hiển thị Page_LoadingData
                await Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Page_LoadingData page_Loading = new Page_LoadingData();
                        stackLoading.Visibility = Visibility.Visible;
                        frameLoading.Navigate(page_Loading);
                        checkWorking = true;
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);

                });
                string shift = "";
                if (rb_ShiftA.IsChecked == true)
                    shift = "Ca ngày";
                if (rb_ShiftB.IsChecked == true)
                    shift = "Ca đêm";
                if (rb_ShiftAll.IsChecked == true)
                    shift = "Tất cả";
                
                // Lấy dữ liệu và hiển thị
                DataTable listWorkingRate = new DataTable();
                await Task.Run(() =>
                {
                    if (dateCheck.Count() != 8)
                        dateCheck = DateTime.Parse(dateCheck).ToString("yyyyMMdd");

                    //listWorkingRate = DataProvider.Instance.executeQuery(path_Ksystem25, query, new object[] { shiftCheck, dateCheck });
                    listWorkingRate = DataProvider.Instance.executeQuery(path_Ksystem20, query, new object[] { shift, dateCheck });
                    foreach (DataRow row in listWorkingRate.Rows)
                    {
                        row["Rate"] = row["Rate"] + "%";
                    }
                });
                lvWorkingRateDetail.ItemsSource = listWorkingRate.DefaultView;

                // Chart
                var _qtyEmp = new ChartValues<double>();
                var _nameDept = new ChartValues<string>();
                DataChart.Values3 = _qtyEmp;
                List<string> listCity = new List<string>();
                foreach (DataRow row in listWorkingRate.Rows)
                {
                    if (row["DeptNm"].ToString() != "TOTAL")
                    {
                        _qtyEmp.Add(int.Parse(row["Rate"].ToString().Substring(0, row["Rate"].ToString().Length - 1)));
                        _nameDept.Add(row["DeptNm"].ToString());
                    }
                }
                DataChart.Labels = _nameDept;
                DataChart.YFormatter = _qtyEmp;
                DataChart.Step = 10;
                if (MainWindow.language == "vi-VN")
                {
                    DataChart.Title = "Tỷ lệ";
                }
                else
                {
                    DataChart.Title = "율(%)";
                }
                DataContext = this;
                Column column = new Column();
                frameChart_Tinh.Navigate(column);

                // Đóng Page_LoadingData
                await Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        stackLoading.Visibility = Visibility.Hidden;
                        checkWorking = false;
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            
        }

        private void dpk_Check_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dateCheck = DateTime.Parse(dpk_Check.SelectedDate.ToString()).ToString("yyyy-MM-dd");
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
                        GetListEmpWorking();
                        GetWorkingRate();
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
                            if (MainWindow.language == "vi-VN")
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
            catch
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
                    foreach (var item in list_Excell)
                    {
                        if (item.EmpId != "")
                        {
                            numberRow++;
                        }
                    }

                    numberRow = numberRow + 5;
                    p.Workbook.Properties.Author = DateTime.Now.ToShortDateString();
                    p.Workbook.Properties.Title = "Danh sách tỷ lệ tăng ca của công nhân viên";
                    p.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet ws = p.Workbook.Worksheets[1];
                    ws.Name = "Sheet1";

                    //Cột 1 
                    ws.Column(1).Width = 5;//stt
                    ws.Column(2).Width = 10;//Bộ phận
                    ws.Column(3).Width = 30;//ho ten
                    ws.Column(4).Width = 10;//gioi tinh
                    ws.Column(5).Width = 20;//ngay sinh
                    ws.Column(6).Width = 20;//sdt
                    ws.Column(7).Width = 20;//cmt 

                    ws.Row(1).Height = 10;
                    ws.Row(2).Height = 40;
                    ws.Row(3).Height = 20;
                    ws.Row(4).Height = 25;

                    //căn hàng và cột cho tất cả các ô 

                    for (int i = 1; i < numberRow; i++)
                    {
                        string strCell = "A" + i.ToString() + ":" + "G" + i.ToString();
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
                        string strCell = "A" + i.ToString() + ":" + "G" + i.ToString();
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

                    }

                    for (int i = 5; i < numberRow; i++)
                    {
                        if (i % 2 == 0)
                        {
                            string strCell = "A" + i.ToString() + ":" + "G" + i.ToString();
                            var cell = ws.Cells[strCell];
                            var fill = cell.Style.Fill;
                            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                        }
                    }

                    //Bôi den backgroud
                    //

                    ws.Cells["A2:G2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A2:G2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Azure);

                    ws.Cells["A4:G4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A4:G4"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Ivory);


                    ws.Cells["A1:A1"].Value = "";
                    ws.Cells["A1:G1"].Merge = true;
                    ws.Cells["A1:A1"].Style.Font.Size = 25;
                    ws.Cells["A1:A1"].Style.Font.Bold = true;


                    ws.Cells["A2:A2"].Value = "DANH SÁCH TỶ LỆ ĐI LÀM CỦA CÔNG NHÂN VIÊN";
                    ws.Cells["A2:G2"].Merge = true;
                    ws.Cells["A2:A2"].Style.Font.Size = 22;
                    ws.Cells["A2:A2"].Style.Font.Bold = true;


                    //Ngày SX
                    ws.Cells["A3:A3"].Value = "Ngày : " + DateTime.Now.ToString("dd/MM/yyyy") + "  Số lượng : " + (numberRow - 5);
                    ws.Cells["A3:G3"].Merge = true;
                    ws.Cells["A3:A3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["A3:A3"].Style.Font.Bold = true;


                    //Head                  
                    ws.Cells["A4:G4"].Style.Font.Size = 12;
                    ws.Cells["A4:G4"].Style.Font.Bold = true;
                    ws.Cells["A4:A4"].Value = "STT";
                    ws.Cells["B4:B4"].Value = "Bộ phận";
                    ws.Cells["C4:C4"].Value = "Phòng ban";
                    ws.Cells["D4:D4"].Value = "Nhóm";
                    ws.Cells["E4:E4"].Value = "Mã NV";
                    ws.Cells["F4:F4"].Value = "Họ và tên";
                    ws.Cells["G4:G4"].Value = "Ca";


                    int index = 4;
                    int stt = 0;

                    foreach (var item in list_Excell)
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
                            ws.Cells[strCell7].Value = item.Remark;

                        }
                    }
                    ws.PrinterSettings.PaperSize = ePaperSize.A4;
                    ws.PrinterSettings.Orientation = eOrientation.Landscape;
                    ws.PrinterSettings.FitToPage = true;
                    ws.Cells["A4:H4"].AutoFilter = true;
                    ws.PrinterSettings.TopMargin = Decimal.Parse("0");
                    ws.PrinterSettings.LeftMargin = Decimal.Parse("0.25");
                    ws.PrinterSettings.BottomMargin = Decimal.Parse("0.25");
                    ws.PrinterSettings.RightMargin = Decimal.Parse("0.25");
                    File.Delete(pathFileExcel);
                    Byte[] bin = p.GetAsByteArray();
                    File.WriteAllBytes(pathFileExcel, bin);


                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "CreatListExcel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        List<Helper_Employee> _tempRoom = new List<Helper_Employee>();
        List<Helper_Employee> _tempTeam = new List<Helper_Employee>();
        string _room = "";
        string _team = "";

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
                        cbbRoom.ItemsSource = listResultRoom.Select(x =>x.Deptlv2).ToList();
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

     
    }
}
