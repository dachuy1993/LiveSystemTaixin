using LiveSystem.DAO;
using LiveSystem.Model;
using Microsoft.Win32;
using Newtonsoft.Json;
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
        string MonthVN = "";
        string MonthEn = "";
        string depatment = "ALL";
        string room = "ALL";
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
            GetDataCmbYear();
            GetOverTimeRateDetail();
            GetDataCmbDept();


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
            try
            {
            // Thông tin truy vấn
            string today = DateTime.Now.ToString("yyyyMMdd");
            string thisMonth = cbbMonth.Text;
            string thisYear = cbbYear.Text;

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
            string query1 = "SPGetDataOverTimeDetail @date , @month , @year ";
            //string query2 = "select * from update_employee";
            var listEmpOTDetail = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query1, new object[] { today , thisMonth , thisYear });
            //var listEmpInfo = DataProvider.Instance.MySqlExecuteQuery(Page_Main.path_TaixinWeb, query2);

            // Tổng hợp và chuyển đổi dữ liệu
            List<EmpOTModel> listAll = new List<EmpOTModel>();
            foreach (DataRow rowA in listEmpOTDetail.Rows)
            {
                //foreach (DataRow rowB in listEmpInfo.Rows)
                //{
                //    if (rowA["EmpId"].ToString().Trim().ToUpper() == rowB["EmpId"].ToString().Trim().ToUpper())
                //    {
                //        rowA["DeptNm"] = rowB["Deptlv2"];
                //        rowA["MinorNm"] = rowB["Deptlv3"];
                //    }
                //}

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
                    case "V92"://add 2023-09-06
                        emp.Division = "SL TEAM";
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
                    listAll = listAll.Where(x => x.MOT > 40 && x.MOT <= 52).ToList();
                    break;
                case "52h":
                    listAll = listAll.Where(x => x.MOT > 52 && x.MOT <= 104).ToList();
                    break;
                case "300h":
                    listAll = listAll.Where(x => x.YOT > 300).ToList();
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
            listAll = listAll.OrderByDescending(x => x.YOT).ToList();
            int i = 1;
            listAll.ForEach(x =>
            {
                x.ID = i;
                i++;
            });

            lvOverTime.ItemsSource = listAll;
            //list_Excell = listAll;
            lbSoLuong.Content = listAll.Count().ToString() + " (người)";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
        }



        private void GetEmpOTDetailExcel()
        {
            // Thông tin truy vấn
            try
            {
                string today = DateTime.Now.ToString("yyyyMMdd");
                string thisMonth = cbbMonth.Text;
                string thisYear = cbbYear.Text;

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
                string query1 = "SPGetDataOverTimeDetailExcel @date , @month , @year ";
                var listEmpOTDetail = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query1, new object[] { today, thisMonth, thisYear });
                
                // Tổng hợp và chuyển đổi dữ liệu
                List<EmpOTModel> listAll = new List<EmpOTModel>();
                foreach (DataRow rowA in listEmpOTDetail.Rows)
                {
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
                    emp.M1 = double.Parse(rowA["M1"].ToString());
                    emp.M2 = double.Parse(rowA["M2"].ToString());
                    emp.M3 = double.Parse(rowA["M3"].ToString());
                    emp.M4 = double.Parse(rowA["M4"].ToString());
                    emp.M5 = double.Parse(rowA["M5"].ToString());
                    emp.M6 = double.Parse(rowA["M6"].ToString());
                    emp.M7 = double.Parse(rowA["M7"].ToString());
                    emp.M8 = double.Parse(rowA["M8"].ToString());
                    emp.M9 = double.Parse(rowA["M9"].ToString());
                    emp.M10 = double.Parse(rowA["M10"].ToString());
                    emp.M11 = double.Parse(rowA["M11"].ToString());
                    emp.M12 = double.Parse(rowA["M12"].ToString());
                    listAll.Add(emp);
                }

                // Lọc theo điều kiện giờ OT
                switch (status)
                {
                    case "40h":
                        listAll = listAll.Where(x => x.MOT > 40 && x.MOT <= 52).ToList();
                        break;
                    case "52h":
                        listAll = listAll.Where(x => x.MOT > 52 && x.MOT <= 104).ToList();
                        break;
                    case "300h":
                        listAll = listAll.Where(x => x.YOT >= 300).ToList();
                        break;
                }


                // Lọc dữ liệu theo bộ phận, phòng ban, nhóm, mã nhân viên
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
                list_Excell = listAll;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Excel");
            }
        }
        //======================================================================================================================//


        // Tỷ lệ tăng ca
        private async void GetOverTimeRateDetail()
        {
            try
            {

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
                DataTable listOTRate = new DataTable();
                await Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        string dateCheck = DateTime.Now.ToString("yyyyMMdd");
                    string thisMonth = cbbMonth.Text;
                    string thisYear = cbbYear.Text;
                    string query = "SPGetDataOverTimeMainNow @date , @month , @year ";
                    listOTRate = DataProvider.Instance.ExecuteSP(path_Ksystem20, query, new object[] { dateCheck, thisMonth, thisYear });




                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);
                });
                lvOverTimeDetail.ItemsSource = listOTRate.DefaultView;
                lvOverTime3Detail.ItemsSource = listOTRate.DefaultView;
                lvOverTime4Detail.ItemsSource = listOTRate.DefaultView;
                GetOverTimeRateDetailOld();
                foreach (DataRow rowM in listOTRate.Rows)
                {
                    MonthVN = rowM["MonVN"].ToString();
                    MonthEn = rowM["MonEN"].ToString();
                }
                if (MainWindow.language == "vi-VN")
                {
                    lb_Month.Content = MonthVN;
                }
                else
                {
                    lb_Month.Content = MonthEn;
                }



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
            //lb_Month.Content = DateTime.Now.ToString("MMMM").ToUpper();

        }

        private void GetOverTimeRateDetailOld()
        {
            try
            {
                string dateCheck = DateTime.Now.ToString("yyyyMMdd");
                string thisMonth = cbbMonth.Text;
                string thisYear = cbbYear.Text;
                string query = "SPGetDataOverTimeMainOld @date , @month , @year ";
                var listOTRate = DataProvider.Instance.ExecuteSP(path_Ksystem20, query, new object[] { dateCheck, thisMonth, thisYear });

                lvOverTimeDetailOld.ItemsSource = listOTRate.DefaultView;
                lvOverTime3DetailOld.ItemsSource = listOTRate.DefaultView;
                lvOverTime4DetailOld.ItemsSource = listOTRate.DefaultView;

                foreach (DataRow rowM in listOTRate.Rows)
                {
                    MonthVN = rowM["MonVN"].ToString();
                    MonthEn = rowM["MonEN"].ToString();
                }
                if (MainWindow.language == "vi-VN")
                {
                    lb_MonthOld.Content = MonthVN;
                }
                else
                {
                    lb_MonthOld.Content = MonthEn;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            //lb_MonthOld.Content = DateTime.Now.AddMonths(-1).ToString("MMMM").ToUpper();
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
                    GetOverTimeRateDetail();
                    GetOverTimeRateDetailOld();
                    stackLoading.Visibility = Visibility.Hidden;
                    checkWorking = false;
                }, System.Windows.Threading.DispatcherPriority.ContextIdle);
            });
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
                            GetEmpOTDetailExcel();
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
                    ws.Column(3).Width = 30;//Phòng ban
                    ws.Column(4).Width = 10;//Nhóm
                    ws.Column(5).Width = 20;//Mã NV
                    ws.Column(6).Width = 20;//Họ tên
                    ws.Column(7).Width = 20;//tháng này
                    ws.Column(8).Width = 20;//1 năm  
                    ws.Column(9).Width = 20;//tháng 1 
                    ws.Column(10).Width = 20;//M2  
                    ws.Column(11).Width = 20;//M3
                    ws.Column(12).Width = 20;//M4
                    ws.Column(13).Width = 20;//M5
                    ws.Column(14).Width = 20;//M6
                    ws.Column(15).Width = 20;//M7 
                    ws.Column(16).Width = 20;//M8  
                    ws.Column(17).Width = 20;//M9 
                    ws.Column(18).Width = 20;//M10 
                    ws.Column(19).Width = 20;//M11
                    ws.Column(20).Width = 20;//M12 

                    ws.Row(1).Height = 10;
                    ws.Row(2).Height = 40;
                    ws.Row(3).Height = 20;
                    ws.Row(4).Height = 25;

                    //căn hàng và cột cho tất cả các ô                 


                    for (int i = 1; i < numberRow; i++)
                    {
                        string strCell = "A" + i.ToString() + ":" + "T" + i.ToString();
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
                        string strCell = "A" + i.ToString() + ":" + "T" + i.ToString();
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
                        //--
                        string strCell10 = "J" + i.ToString() + ":" + "J" + i.ToString();
                        ws.Cells[strCell10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell11 = "K" + i.ToString() + ":" + "K" + i.ToString();
                        ws.Cells[strCell11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell12 = "L" + i.ToString() + ":" + "L" + i.ToString();
                        ws.Cells[strCell12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell13 = "M" + i.ToString() + ":" + "M" + i.ToString();
                        ws.Cells[strCell13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell14 = "N" + i.ToString() + ":" + "N" + i.ToString();
                        ws.Cells[strCell14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell15 = "O" + i.ToString() + ":" + "O" + i.ToString();
                        ws.Cells[strCell15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell16 = "P" + i.ToString() + ":" + "P" + i.ToString();
                        ws.Cells[strCell16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell17 = "Q" + i.ToString() + ":" + "Q" + i.ToString();
                        ws.Cells[strCell17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell18 = "R" + i.ToString() + ":" + "R" + i.ToString();
                        ws.Cells[strCell18].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell19 = "S" + i.ToString() + ":" + "S" + i.ToString();
                        ws.Cells[strCell19].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell20 = "T" + i.ToString() + ":" + "T" + i.ToString();
                        ws.Cells[strCell20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    }

                    for (int i = 5; i < numberRow; i++)
                    {
                        if (i % 2 == 0)
                        {
                            string strCell = "A" + i.ToString() + ":" + "T" + i.ToString();
                            var cell = ws.Cells[strCell];
                            var fill = cell.Style.Fill;
                            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                        }
                    }

                    //Bôi den backgroud
                    //

                    ws.Cells["A2:T2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A2:T2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Azure);

                    ws.Cells["A4:T4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A4:T4"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Ivory);


                    ws.Cells["A1:A1"].Value = "";
                    ws.Cells["A1:T1"].Merge = true;
                    ws.Cells["A1:A1"].Style.Font.Size = 25;
                    ws.Cells["A1:A1"].Style.Font.Bold = true;


                    ws.Cells["A2:A2"].Value = "DANH SÁCH TỶ LỆ TĂNG CA CỦA CÔNG NHÂN VIÊN";
                    ws.Cells["A2:T2"].Merge = true;
                    ws.Cells["A2:A2"].Style.Font.Size = 22;
                    ws.Cells["A2:A2"].Style.Font.Bold = true;


                    //Ngày SX
                    ws.Cells["A3:A3"].Value = "Ngày : " + DateTime.Now.ToString("dd/MM/yyyy") + "  Số lượng : " + (numberRow - 5);
                    ws.Cells["A3:T3"].Merge = true;
                    ws.Cells["A3:A3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["A3:A3"].Style.Font.Bold = true;


                    //Head                  
                    ws.Cells["A4:T4"].Style.Font.Size = 12;
                    ws.Cells["A4:T4"].Style.Font.Bold = true;
                    ws.Cells["A4:A4"].Value = "STT";
                    ws.Cells["B4:B4"].Value = "Bộ phận";
                    ws.Cells["C4:C4"].Value = "Phòng ban";
                    ws.Cells["D4:D4"].Value = "Nhóm";
                    ws.Cells["E4:E4"].Value = "Mã NV";
                    ws.Cells["F4:F4"].Value = "Họ và tên";
                    ws.Cells["G4:G4"].Value = "Tháng này";
                    ws.Cells["H4:H4"].Value = "1 năm";
                    ws.Cells["I4:I4"].Value = "Tháng 1";
                    ws.Cells["J4:J4"].Value = "Tháng 2";
                    ws.Cells["K4:K4"].Value = "Tháng 3";
                    ws.Cells["L4:L4"].Value = "Tháng 4";
                    ws.Cells["M4:M4"].Value = "Tháng 5";
                    ws.Cells["N4:N4"].Value = "Tháng 6";
                    ws.Cells["O4:O4"].Value = "Tháng 7";
                    ws.Cells["P4:P4"].Value = "Tháng 8";
                    ws.Cells["Q4:Q4"].Value = "Tháng 9";
                    ws.Cells["R4:R4"].Value = "Tháng 10";
                    ws.Cells["S4:S4"].Value = "Tháng 11";
                    ws.Cells["T4:T4"].Value = "Tháng 12";


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
                            //--
                            string strCell9 = "I" + index.ToString() + ":" + "I" + index.ToString();
                            ws.Cells[strCell9].Value = item.M1;
                            //--
                            string strCell10 = "J" + index.ToString() + ":" + "J" + index.ToString();
                            ws.Cells[strCell10].Value = item.M2;
                            //--
                            string strCell11 = "K" + index.ToString() + ":" + "K" + index.ToString();
                            ws.Cells[strCell11].Value = item.M3;
                            //--
                            string strCell12 = "L" + index.ToString() + ":" + "L" + index.ToString();
                            ws.Cells[strCell12].Value = item.M4;
                            //--
                            string strCell13 = "M" + index.ToString() + ":" + "M" + index.ToString();
                            ws.Cells[strCell13].Value = item.M5;
                            //--
                            string strCell14 = "N" + index.ToString() + ":" + "N" + index.ToString();
                            ws.Cells[strCell14].Value = item.M6;
                            //--
                            string strCell15 = "O" + index.ToString() + ":" + "O" + index.ToString();
                            ws.Cells[strCell15].Value = item.M7;
                            //--
                            string strCell16 = "P" + index.ToString() + ":" + "P" + index.ToString();
                            ws.Cells[strCell16].Value = item.M8;
                            //--
                            string strCell17 = "Q" + index.ToString() + ":" + "Q" + index.ToString();
                            ws.Cells[strCell17].Value = item.M9;
                            //--
                            string strCell18 = "R" + index.ToString() + ":" + "R" + index.ToString();
                            ws.Cells[strCell18].Value = item.M10;
                            //--
                            string strCell19 = "S" + index.ToString() + ":" + "S" + index.ToString();
                            ws.Cells[strCell19].Value = item.M11;
                            //--
                            string strCell20 = "T" + index.ToString() + ":" + "T" + index.ToString();
                            ws.Cells[strCell20].Value = item.M12;
                        }
                    }

                    ws.PrinterSettings.PaperSize = ePaperSize.A4;
                    ws.PrinterSettings.Orientation = eOrientation.Landscape;
                    ws.PrinterSettings.FitToPage = true;
                    ws.Cells["A4:T4"].AutoFilter = true;
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
        

        private async void GetDataCmbYear()
        {
            string Year = "";
            string query = "SPGetDataCmbYearOverTime @date";
            // Lấy dữ liệu và hiển thị
            DataTable listCmb = new DataTable();

            listCmb = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query, new object[] { Year });


            List<string> listResult = new List<string>();
            foreach (DataRow Row in listCmb.Rows)
            {
                listResult.Add(Row["CbbYear"].ToString());
            }
            cbbYear.ItemsSource = listResult;
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
                if (Row["CbbDept"].ToString() != "CUSHION")
                {
                    listResult.Add(Row["CbbDept"].ToString());
                }
            }
            cbbDepatment.ItemsSource = listResult;
        }

        private void dpk_Check_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //var setting1 = new JsonSerializerSettings { DateFormatString = "yyyy-MM" };
            //var dt1 = JsonConvert.SerializeObject(DateTime.Parse(dpk_Check.SelectedDate.ToString()).ToString("yyyy-MM-dd"), setting1);
            //dateCheck = dt1.Substring(1, dt1.Length - 2);
            //lb_Month.Content = DateTime.Parse(dateCheck).ToString("MMMM").ToUpper();
        }

        private void cbbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cbbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
