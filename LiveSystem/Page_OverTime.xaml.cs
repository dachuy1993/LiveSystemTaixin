using LiveSystem.DAO;
using LiveSystem.Model;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.Style;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for Page_OverTime.xaml
    /// </summary>
    public partial class Page_OverTime : Page
    {
        public static string path_Ksystem20 = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";
        string pathFileExcel = @"TempFile//ExcelFile.xlsx";
        List<EmpOTModel> list_Excell = new List<EmpOTModel>();
        public Page_OverTime()
        {
            InitializeComponent();    
            Loaded += Page_OverTime_Loaded;
        }
        bool checkWorking = false;
        private void Page_OverTime_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MainWindow.language);
            ApplyLanguage(MainWindow.language);
            GetOverTimeRateDetail();
            GetOverTimeRateDetailOld();
        }

        private void ApplyLanguage(string cultureName = null)
        {
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

        //======================================================================================================================//
        private void GetEmpOTDetail()
        {
            // Thông tin truy vấn
            string today = DateTime.Now.ToString("yyyyMMdd");
            string thisMonth = DateTime.Now.ToString("yyyyMM");
            string thisYear = DateTime.Now.ToString("yyyy");

            string status = "";
            if (rb_All.IsChecked == true)
                status = "ALL";
            if (rb_40.IsChecked == true)
                status = "40h";
            if (rb_104.IsChecked == true)
                status = "52h";
            if (rb_300.IsChecked == true)
                status = "300h";

            // Lấy dữ liệu OT và thông tin nhân viên
            string query1 = "SPGetDataOverTimeDetail @date ";
            string query2 = "select * from update_employee";
            var listEmpOTDetail = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query1, new object[] { today });
            var listEmpInfo = DataProvider.Instance.MySqlExecuteQuery(Page_Main.path_TaixinWeb, query2);

            // Tổng hợp và chuyển đổi dữ liệu
            List<EmpOTModel> listAll = new List<EmpOTModel>();
            foreach (DataRow rowA in listEmpOTDetail.Rows)
            {
                foreach (DataRow rowB in listEmpInfo.Rows)
                {
                    if (rowA["EmpId"].ToString().Trim().ToUpper() == rowB["EmpId"].ToString().Trim().ToUpper())
                    {
                        rowA["DeptNm"] = rowB["Deptlv2"];
                        rowA["MinorNm"] = rowB["Deptlv3"];
                    }
                }

                EmpOTModel emp = new EmpOTModel();
                switch (rowA["Division"].ToString().Trim().Substring(0, 3))
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
                }
                emp.DeptNm = rowA["DeptNm"].ToString();
                emp.GroupNm = rowA["MinorNm"].ToString();
                emp.EmpId = rowA["EmpId"].ToString();
                emp.EmpNm = rowA["EmpNm"].ToString();
                emp.MOT = double.Parse(rowA["MOT"].ToString());
                emp.YOT = double.Parse(rowA["YOT"].ToString());
                listAll.Add(emp);
            }

            // Lọc theo điều kiện giờ OT
            switch (status)
            {
                case "40h":
                    listAll = listAll.Where(x => x.MOT >= 40 && x.MOT < 52).ToList();
                    break;
                case "52h":
                    listAll = listAll.Where(x => x.MOT >= 52 && x.MOT < 104).ToList();
                    break;
                case "300h":
                    listAll = listAll.Where(x => x.YOT >= 300).ToList();
                    break;
            }
            

            // Lọc dữ liệu theo bộ phận, phòng ban, nhóm, mã nhân viên
            if (txtName.Text == "")
            {
                // Bộ phận != ALL
                if(cbbDepatment.Text != "ALL")
                {
                    // Phòng ban == ALL
                    if(cbbRoom.Text == "ALL")
                    {
                        listAll = listAll.Where(x => x.Division == cbbDepatment.Text).ToList();
                    }
                    // Phòng ban != ALL
                    else
                    {
                        // Nhóm == ALL
                        if(cbbTeam.Text == "ALL")
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
            // Textbox mã nhân viên != ""
            else
            {
                listAll = listAll.Where(x => x.EmpId.Trim().ToUpper() == txtName.Text.Trim().ToUpper()).ToList();
            }

            // Sắp xếp và thêm STT
            listAll = listAll.OrderBy(x => x.EmpId).ToList();
            int i = 1;
            listAll.ForEach(x =>
            {
                x.ID = i;
                i++;
            });

            lvOverTime.ItemsSource = listAll;
            list_Excell = listAll;
            lbSoLuong.Content = listAll.Count().ToString() + " (người)";
        }
        //======================================================================================================================//


        // Tỷ lệ tăng ca
        private void GetOverTimeRateDetail()
        {
            string dateCheck = DateTime.Now.ToString("yyyyMMdd");
            string query = "SPGetDataOverTimeMain @date";
            var listOTRate = DataProvider.Instance.ExecuteSP(path_Ksystem20, query, new object[] { dateCheck });

            lvOverTimeDetail.ItemsSource = listOTRate.DefaultView;
            lvOverTime3Detail.ItemsSource = listOTRate.DefaultView;
            lvOverTime4Detail.ItemsSource = listOTRate.DefaultView;
            lb_Month.Content = DateTime.Now.ToString("MMMM").ToUpper();
        }

        private void GetOverTimeRateDetailOld()
        {
            string dateCheck = DateTime.Now.ToString("yyyyMMdd");
            string query = "SPGetDataOverTimeMainOld @date";
            var listOTRate = DataProvider.Instance.ExecuteSP(path_Ksystem20, query, new object[] { dateCheck });

            lvOverTimeDetailOld.ItemsSource = listOTRate.DefaultView;
            lvOverTime3DetailOld.ItemsSource = listOTRate.DefaultView;
            lvOverTime4DetailOld.ItemsSource = listOTRate.DefaultView;
            lb_MonthOld.Content = DateTime.Now.AddMonths(-1).ToString("MMMM").ToUpper();
        }
        private async void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
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
            await Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    GetEmpOTDetail();
                    stackLoading.Visibility = Visibility.Hidden;
                    checkWorking = false;
                }, System.Windows.Threading.DispatcherPriority.ContextIdle);
            });
            GetOverTimeRateDetail();
            GetOverTimeRateDetailOld();
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
                    ws.Column(2).Width = 10;//mã nv
                    ws.Column(3).Width = 30;//ho ten
                    ws.Column(4).Width = 10;//gioi tinh
                    ws.Column(5).Width = 20;//ngay sinh
                    ws.Column(6).Width = 20;//sdt
                    ws.Column(7).Width = 20;//cmt
                    ws.Column(8).Width = 20;//noi cap  

                    ws.Row(1).Height = 10;
                    ws.Row(2).Height = 40;
                    ws.Row(3).Height = 20;
                    ws.Row(4).Height = 25;

                    //căn hàng và cột cho tất cả các ô                 


                    for (int i = 1; i < numberRow; i++)
                    {
                        string strCell = "A" + i.ToString() + ":" + "H" + i.ToString();
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
                        string strCell = "A" + i.ToString() + ":" + "H" + i.ToString();
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
                    }

                    for (int i = 5; i < numberRow; i++)
                    {
                        if (i % 2 == 0)
                        {
                            string strCell = "A" + i.ToString() + ":" + "H" + i.ToString();
                            var cell = ws.Cells[strCell];
                            var fill = cell.Style.Fill;
                            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                        }
                    }

                    //Bôi den backgroud
                    //

                    ws.Cells["A2:H2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A2:H2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Azure);

                    ws.Cells["A4:H4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A4:H4"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Ivory);


                    ws.Cells["A1:A1"].Value = "";
                    ws.Cells["A1:H1"].Merge = true;
                    ws.Cells["A1:A1"].Style.Font.Size = 25;
                    ws.Cells["A1:A1"].Style.Font.Bold = true;


                    ws.Cells["A2:A2"].Value = "DANH SÁCH TỶ LỆ TĂNG CA CỦA CÔNG NHÂN VIÊN";
                    ws.Cells["A2:H2"].Merge = true;
                    ws.Cells["A2:A2"].Style.Font.Size = 22;
                    ws.Cells["A2:A2"].Style.Font.Bold = true;


                    //Ngày SX
                    ws.Cells["A3:A3"].Value = "Ngày : " + DateTime.Now.ToString("dd/MM/yyyy") + "  Số lượng : " + (numberRow - 5);
                    ws.Cells["A3:H3"].Merge = true;
                    ws.Cells["A3:A3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["A3:A3"].Style.Font.Bold = true;


                    //Head                  
                    ws.Cells["A4:H4"].Style.Font.Size = 12;
                    ws.Cells["A4:H4"].Style.Font.Bold = true;
                    ws.Cells["A4:A4"].Value = "STT";
                    ws.Cells["B4:B4"].Value = "Bộ phận";
                    ws.Cells["C4:C4"].Value = "Phòng ban";
                    ws.Cells["D4:D4"].Value = "Nhóm";
                    ws.Cells["E4:E4"].Value = "Mã NV";
                    ws.Cells["F4:F4"].Value = "Họ và tên";
                    ws.Cells["G4:G4"].Value = "Tháng này";
                    ws.Cells["H4:H4"].Value = "1 năm";


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
                            ws.Cells[strCell7].Value = item.MOT;
                            //--
                            string strCell8 = "H" + index.ToString() + ":" + "H" + index.ToString();
                            ws.Cells[strCell8].Value = item.YOT;
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
        string depatment = "";
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
                switch (depatment)
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

                }
                _room = "ALL";
                _team = "ALL";

                if (depatment != "ALL")
                {
                    cbbRoom.ClearValue(ComboBox.ItemsSourceProperty);
                    cbbTeam.ClearValue(ComboBox.ItemsSourceProperty);
                    _tempRoom = Page_Main.listRoom.Where(X => X.cmpcode == code).ToList();
                    _tempRoom.Add(new Helper_Employee { EmpId = "0", EmpNm = "ALL", cmpcode = "0" });
                    cbbRoom.ItemsSource = _tempRoom.OrderBy(x => x.cmpcode).ToList();
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
    }
}
