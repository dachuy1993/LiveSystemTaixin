using LiveSystem.DAO;
using LiveSystem.Model;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections;
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

    public partial class Page_Food : Page
    {
        #region Khai báo
        public static string path_TaixinAccessManager = "Data Source=192.168.2.5\\SQLEXPRESS;Initial Catalog=NitgenAccessManager;Persist Security Info=True;User ID=sa;Password=123456a@";
        string path_Ksystem = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";
        //public static string path_TaixinYP = "Data Source=113.160.208.231,1433;Initial Catalog=ChamCom;Persist Security Info=True;User ID=WiseEyeOn39;Password= cNca@123#!";

        List<Helper_Employee> List_Person_Total = new List<Helper_Employee>();
        List<Helper_Employee> List_Depatment_Total = new List<Helper_Employee>();
        List<Helper_Employee> List_Depatment_Per = new List<Helper_Employee>();
        List<Helper_Employee> list_PerReal = new List<Helper_Employee>();
        List<Helper_Employee> List_Depatment_Food = new List<Helper_Employee>();

        List<EmpVSIPMealModel> list_Excel = new List<EmpVSIPMealModel>();
        string dateCheck = "";
        string shiftCheck = "ca ngày";
        string depatment = "ALL";
        string work = "ON";
        bool checkWorking = false;
        string pathFileExcel = @"TempFile//ExcelFile.xlsx";
        #endregion
        public Page_Food()
        {
            InitializeComponent();
            dpk_Check.SelectedDate = DateTime.Now;
            Loaded += Page_WorkingRate_Loaded;
        }

        private void Page_WorkingRate_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MainWindow.language);
            ApplyLanguage(MainWindow.language);
            GetVSIPMealDetail();


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

        //================================================================================================================//
        private void GetListVSIPMeal()
        {
            try
            {
                string shift = "";
                if (rb_ShiftA.IsChecked == true)
                    shift = "Ca sáng";
                if (rb_ShiftB.IsChecked == true)
                    shift = "Ca trưa";
                if (rb_ShiftC.IsChecked == true)
                    shift = "Ca chiều";
                if (rb_ShiftD.IsChecked == true)
                    shift = "Ca đêm";

                // Lấy dữ liệu xuất ăn và thông tin nhân viên
                string query1 = "SPGetDateFoodMainDetail @date , @shift";
                //string query2 = "select * from update_employee";
                var listVSIPMeal = DataProvider.Instance.ExecuteSP(Page_Main.path_TaixinAccessManager, query1, new object[] { dateCheck, shift });
                //var listEmpInfo = DataProvider.Instance.MySqlExecuteQuery(Page_Main.path_TaixinWeb, query2);

                string query2 = "SPGetDateFoodMainDetailYenPhong @date , @shift";
                //string query2 = "select * from update_employee";
                //var listVSIPMealYP = DataProvider.Instance.ExecuteSP(Page_Main.path_TaixinYP, query2, new object[] { dateCheck, shift });
                // Kết hợp và bổ sung thông tin cho danh sách
                List<EmpVSIPMealModel> listAll = new List<EmpVSIPMealModel>();
                foreach (DataRow rowA in listVSIPMeal.Rows)
                {
                    EmpVSIPMealModel emp = new EmpVSIPMealModel();
                    //foreach (DataRow rowB in listEmpInfo.Rows)
                    //{
                    //    if (rowA["EmpID"].ToString().Trim().ToUpper() == rowB["EmpId"].ToString().Trim().ToUpper())
                    //    {
                    //        emp.DeptNm = rowB["Deptlv2"].ToString();
                    //        emp.GroupNm = rowB["Deptlv3"].ToString();
                    //    }
                    //}

                    emp.EmpId = rowA["EmpID"].ToString();
                    emp.EmpNm = rowA["EmpName"].ToString();
                    emp.Division = rowA["Division"].ToString();
                    emp.DeptNm = rowA["DeptNm"].ToString();
                    emp.GroupNm = rowA["GroupNm"].ToString();
                    emp.TimeScan = rowA["TimeScan"].ToString();
                    emp.Times = rowA["Times"].ToString();
                    switch (emp.Division)
                    {
                        case "V93":
                            emp.Division = "MANAGE";
                            break;
                        case "V94":
                            emp.Division = "IT";
                            break;
                        case "V95":
                            emp.Division = "MAR";
                            break;
                        case "V96":
                            emp.Division = "PRO";
                            break;
                        case "V97":
                            emp.Division = "QC";
                            break;
                        case "V98":
                            emp.Division = "HICUP";
                            break;
                        case "V92":
                            emp.Division = "AUTO TEAM";
                            break;
                    }
                    listAll.Add(emp);
                }

                //foreach (DataRow rowA in listVSIPMealYP.Rows)
                //{
                //    EmpVSIPMealModel emp = new EmpVSIPMealModel();

                //    emp.EmpId = rowA["EmpID"].ToString();
                //    emp.EmpNm = rowA["EmpName"].ToString();
                //    emp.Division = rowA["Division"].ToString();
                //    emp.DeptNm = rowA["DeptNm"].ToString();
                //    emp.GroupNm = rowA["GroupNm"].ToString();
                //    emp.TimeScan = rowA["TimeScan"].ToString();
                //    emp.Times = rowA["Times"].ToString();
                //    emp.Division = "CUSHION";
                //    listAll.Add(emp);
                //}

                // Lọc dữ liệu theo bộ phận, phòng ban, nhóm, mã nhân viên
                if (txtName.Text == "")
                {
                    // Bộ phận != ALL
                    if (cbbDepatment.Text != "ALL")
                    {
                        if (cbbDepatment.Text == "OTHER")
                        {
                            listAll = listAll.Where(x => x.Division == "ETC" || x.Division == "OTHER").ToList();
                        }
                        else if (cbbDepatment.Text == "CUSHION")
                        {
                            listAll = listAll.Where(x => x.Division == "CUSHION").ToList();
                        }    
                        else
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
                }
                // Textbox mã nhân viên != ""
                else
                {
                    listAll = listAll.Where(x => x.EmpId.Trim().ToUpper() == txtName.Text.Trim().ToUpper()).ToList();
                }

                listAll = listAll.OrderByDescending(x => x.Times).ToList();
                // Sắp xếp và thêm STT
                //listAll = listAll.OrderBy(x => x.EmpId).ToList();
                int i = 1;
                listAll.ForEach(x =>
                {
                    x.ID = i;
                    i++;
                });
                lvThongTin.ItemsSource = listAll;
                list_Excel = listAll;
                lbSoLuong.Content = listAll.Count().ToString() + " (người)";
            }
            catch (Exception)
            {
                MessageBox.Show("Error", "Error processing meal data Details", MessageBoxButton.OK);
            }
            
        }
        //================================================================================================================//


        // Thông tin suất ăn VSIP
        private void GetVSIPMealDetail()
        {
            try
            {
                //string datettt = "";
                //if (dateCheck.Count() != 8)
                //    dateCheck = DateTime.Parse(dateCheck).ToString("yyyyMMdd");
                //string query = "select * from tmmfod where Insdt = @date";
                string query = "SPGetDateFoodMain @date";
                //var listVSIPMeal = DataProvider.Instance.executeQuery(path_Ksystem25, query, new object[] { dateCheck });
                var listVSIPMeal = DataProvider.Instance.ExecuteSP(path_TaixinAccessManager, query, new object[] { dateCheck });

                string queryYP = "SPGetDateFoodMainYenPhong @date";
                //var listYPMealYenPhong = DataProvider.Instance.ExecuteSP(path_TaixinYP, queryYP, new object[] { dateCheck });
                string EmpId = "";
                string EmpNm = "";
                int Sang = 0;
                int Trua = 0;
                int Chieu = 0;
                int Dem = 0;
                string Insdt = "";

                //foreach (DataRow item in listYPMealYenPhong.Rows)
                //{
                //    EmpId = item["EmpId"].ToString();
                //    EmpNm = item["EmpNm"].ToString();
                //    Sang = int.Parse(item["Qty_Sang"].ToString());
                //    Trua = int.Parse(item["Qty_Trua"].ToString());
                //    Chieu = int.Parse(item["Qty_Chieu"].ToString());
                //    Dem = int.Parse(item["Qty_Dem"].ToString());
                //    Insdt = item["Insdt"].ToString();
                //}

                //listVSIPMeal.Rows.Add(EmpId, EmpNm, Sang, Trua, Chieu, Dem, Insdt);



                foreach (DataRow item in listVSIPMeal.Rows)
                {
                    if (item["EmpNm"].ToString() == "TOTAL")
                    {
                        item["Qty_Sang"] = int.Parse(item["Qty_Sang"].ToString()) + Sang;
                        item["Qty_Trua"] = int.Parse(item["Qty_Trua"].ToString()) + Trua;
                        item["Qty_Chieu"] = int.Parse(item["Qty_Chieu"].ToString()) + Chieu;
                        item["Qty_Dem"] = int.Parse(item["Qty_Dem"].ToString()) + Dem;
                    }
                }


                DataView dv = listVSIPMeal.DefaultView;
                dv.Sort = "EmpId ASC";


                lvVSIPMealDetail.ItemsSource = dv;



                // Chart
                var _qtyCity = new ChartValues<double>();
                var _nameCity = new ChartValues<string>();
                DataChart.Values3 = _qtyCity;
                if (MainWindow.language == "vi-VN")
                {
                    foreach (DataRow row in listVSIPMeal.Rows)
                    {
                        if (row["EmpNm"].ToString() == "TOTAL")
                        {
                            _nameCity.Add("Sáng");
                            _qtyCity.Add(int.Parse(row["Qty_Sang"].ToString()));
                            _nameCity.Add("Trưa");
                            _qtyCity.Add(int.Parse(row["Qty_Trua"].ToString()));
                            _nameCity.Add("Chiều");
                            _qtyCity.Add(int.Parse(row["Qty_Chieu"].ToString()));
                            _nameCity.Add("Đêm");
                            _qtyCity.Add(int.Parse(row["Qty_Dem"].ToString()));
                        }
                    }
                    DataChart.Title = "Số lượng";
                }
                else
                {
                    foreach (DataRow row in listVSIPMeal.Rows)
                    {
                        if (row["EmpNm"].ToString() == "TOTAL")
                        {
                            _nameCity.Add("아침");
                            _qtyCity.Add(int.Parse(row["Qty_Sang"].ToString()));
                            _nameCity.Add("정오");
                            _qtyCity.Add(int.Parse(row["Qty_Trua"].ToString()));
                            _nameCity.Add("오후");
                            _qtyCity.Add(int.Parse(row["Qty_Chieu"].ToString()));
                            _nameCity.Add("밤");
                            _qtyCity.Add(int.Parse(row["Qty_Dem"].ToString()));
                        }
                    }
                    DataChart.Title = "수량";
                }
                DataChart.Labels = _nameCity;
                DataChart.YFormatter = _qtyCity;
                DataChart.Step = 100;
                DataContext = this;
                Column column = new Column();
                frameChart_Food.Navigate(column);
            }
            catch (Exception)
            {
                MessageBox.Show("Error", "Error processing meal detail data Main", MessageBoxButton.OK);
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
                        GetListVSIPMeal();
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
            GetVSIPMealDetail();

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
                if (MainWindow.language == "vi_VN")
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
                        if (item.Division != "")
                        {
                            numberRow++;
                        }
                    }

                    numberRow = numberRow + 5;
                    p.Workbook.Properties.Author = DateTime.Now.ToShortDateString();
                    p.Workbook.Properties.Title = "Danh sách nhân viên ăn cơm VSIP";
                    p.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet ws = p.Workbook.Worksheets[1];
                    ws.Name = "Sheet1";

                    ws.Column(1).Width = 5;//Stt
                    ws.Column(2).Width = 15;//Bộ phận
                    ws.Column(3).Width = 30;//phong ban
                    ws.Column(4).Width = 30; //nhom
                    ws.Column(5).Width = 10;//Mã NV
                    ws.Column(6).Width = 30;
                    ws.Column(7).Width = 50;

                    ws.Row(1).Height = 10;
                    ws.Row(2).Height = 40;
                    ws.Row(3).Height = 20;
                    ws.Row(4).Height = 25;

                    //căn hàng và cột cho các ô

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
                    ws.Cells["A1:T1"].Merge = true;
                    ws.Cells["A1:A1"].Style.Font.Size = 25;
                    ws.Cells["A1:A1"].Style.Font.Bold = true;


                    ws.Cells["A2:A2"].Value = "DANH SÁCH NHÂN VIÊN ĂN CƠM VSIP";
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
                    ws.Cells["G4:G4"].Value = "Thời gian";

                    int index = 4;
                    int stt = 0;

                    foreach (var item in list_Excel)
                    {
                        if (item.Division != "")
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
                            ws.Cells[strCell7].Value = item.TimeScan;
                        }
                    }

                    ws.PrinterSettings.PaperSize = ePaperSize.A4;
                    ws.PrinterSettings.Orientation = eOrientation.Landscape;
                    ws.PrinterSettings.FitToPage = true;
                    ws.Cells["A4:G4"].AutoFilter = true;
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

        private void cbbDepatment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _tempRoom.Clear();
            _tempTeam.Clear();
            var click = sender as ComboBox;
            var clickItem = click.SelectedItem as ComboBoxItem;
            if (clickItem != null)
            {
                depatment = clickItem.Content.ToString();
                string code = "";
                switch(depatment)
                {
                    case "MANAGE":
                        {
                            code = "1";
                            break;
                        }
                    case "IT":
                        {
                            code = "2";
                            break;
                        }
                    case "HICUP":
                        {
                            code = "3";
                            break;
                        }
                    case "MAR":
                        {
                            code = "4";
                            break;
                        }
                    case "QC":
                        {
                            code = "5";
                            break;
                        }
                    case "PRO":
                        {
                            code = "6";
                            break;
                        }
                    case "KOREA":
                        {
                            code = "7";
                            break;
                        }
                    case "AUTO TEAM": //add 2023-09-06
                        {
                            code = "8";
                            break;
                        }

                }
                _room = "ALL";
                _team = "ALL";
               
                if (depatment!="ALL")
                {
                    cbbRoom.ClearValue(ComboBox.ItemsSourceProperty);
                    cbbTeam.ClearValue(ComboBox.ItemsSourceProperty);
                    _tempRoom = Page_Main.listRoom.Where(X => X.cmpcode == code).ToList();
                    _tempRoom.Add(new Helper_Employee { EmpId = "0", EmpNm = "ALL", cmpcode = "0" });
                    cbbRoom.ItemsSource = _tempRoom.OrderBy(x=>x.cmpcode).ToList();
                    cbbRoom.SelectedIndex = 0;                    
                }                   
            }
        }

        private void cbbRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var click = sender as ComboBox;
            var clickItem = click.SelectedItem as Helper_Employee;
            cbbTeam.ClearValue(ComboBox.ItemsSourceProperty);
            _room = "ALL";
            _team = "ALL";
            if (clickItem != null)
            {
                if (clickItem.EmpNm != "ALL")
                {
                    _tempTeam = Page_Main.listTeam.Where(X => X.cmpcode == clickItem.EmpId).ToList();
                    _tempTeam.Add(new Helper_Employee { EmpId = "0", EmpNm = "ALL", cmpcode = "0" });
                    cbbTeam.ItemsSource = _tempTeam.OrderBy(x => x.cmpcode).ToList();
                    _room = clickItem.EmpNm;
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

        private void rb_On_Checked(object sender, RoutedEventArgs e)
        {
            work = "ON";
        }

        private void rb_Off_Checked(object sender, RoutedEventArgs e)
        {
            work = "OFF";
        }

        private void rb_Delay_Checked(object sender, RoutedEventArgs e)
        {
            work = "DELAY";
        }

      
    }
}
