using LiveCharts;
using LiveSystem.DAO;
using LiveSystem.Model;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for Page_Address.xaml
    /// </summary>
    public partial class Page_Address : Page
    {
        #region Khai báo biến
        string path_TaixinWeb = "server=192.168.2.40;Port=3307;user id=txadmin;database=LiveSystem;password=Taixinweb1!";
        public static string path_Ksystem20 = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";

        string pathFileExcel = @"TempFile//ExcelFile.xlsx";
        string updateThongTin = "";
        public static string dateCheck = "";
        List<Helper_Employee> list_Excell = new List<Helper_Employee>();

        //Temp
        public static List<Helper_Employee> listThongtinTemp = new List<Helper_Employee>();
        #endregion

        List<Emp> listAllEmp = new List<Emp>();
        List<Emp> listAllEmpOK = new List<Emp>();
        List<Emp> listAllEmpNG = new List<Emp>();

        public Page_Address()
        {
            InitializeComponent();
            if (!Directory.Exists(@"TempFile"))
                Directory.CreateDirectory(@"TempFile");
            Loaded += Page_Address_Loaded;
        }

        private void Page_Address_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MainWindow.language);
            ApplyLanguage(MainWindow.language);
            if (MainWindow.EmpId.ToUpper() == "K12022" || MainWindow.EmpId.ToUpper() == "L07099" || MainWindow.EmpId.ToUpper() == "L07139" || MainWindow.EmpId.ToUpper() == "A05001")
            {
                btnEditAdd.Visibility = Visibility.Visible;
                //btnEditData.Visibility = Visibility.Visible;
            }
            GetEmpInfo();
            EmpInfoUpdateStatusDetail();
        }

        // THANGDN
        private void GetAllEmp()
        {
            try
            {
                // Lấy dữ liệu thông tin nhân viên
                string query1 = "select * from update_employee";
                var listEmpInformation = DataProvider.Instance.MySqlExecuteQuery(path_TaixinWeb, query1);
                // Lấy dữ liệu nhân viên thực tế từ Ksystem
                string query2 = "SELECT * FROM TDAEmpMaster where RetDate >= @date and len(EmpId) > 4 and len(EmpId) < 8";
                var listEmp = DataProvider.Instance.executeQuery(Page_Main.path_Ksystem20, query2, new object[] { Page_Main.dateCheck });

                // Lọc dữ liệu theo điều kiện Update
                if (rbUpdate_OK.IsChecked == true)
                {
                    updateThongTin = "OK";
                }
                if (rbUpdate_NG.IsChecked == true)
                {
                    updateThongTin = "NG";
                }
                if (rbUpdate_ALL.IsChecked == true)
                {
                    updateThongTin = "ALL";
                }
                // Lấy dữ liệu nhân viên đã được update thông tin trên taixin web
                listAllEmpOK = listEmp.AsEnumerable().Join(listEmpInformation.AsEnumerable(), x => x["EmpId"].ToString().Trim().ToUpper(), y => y["EmpId"].ToString().Trim().ToUpper(), (x, y) => new { x, y })
                    .Select(s => new Emp
                    {
                        EmpId = s.y["EmpId"].ToString(),
                        EmpNm = s.y["EmpNm"].ToString(),
                        SexCd = s.y["SexCd"].ToString(),
                        BOD = DateTime.Parse(s.y["BOD"].ToString()).ToString("yyyy-MM-dd"),
                        HpTel = s.y["HpTel"].ToString(),
                        ResidId = s.y["ResidId"].ToString(),
                        ResidPlace = s.y["ResidPlace"].ToString(),
                        ResidDate = DateTime.Parse(s.y["ResidDate"].ToString()).ToString("yyyy-MM-dd"),
                        Nation = s.y["Nation"].ToString(),
                        Deptlv1 = s.y["Deptlv1"].ToString(),
                        Deptlv2 = s.y["Deptlv2"].ToString(),
                        Deptlv3 = s.y["Deptlv3"].ToString(),
                        Position = s.y["Position"].ToString(),
                        Shift = s.y["Shift"].ToString(),
                        Level = s.y["Level"].ToString(),

                        TempProv = s.y["TempProv"].ToString(),
                        TempDist = s.y["TempDist"].ToString(),
                        TempComm = s.y["TempComm"].ToString(),
                        TempVilla = s.y["TempVilla"].ToString(),

                        PermProv = s.y["PermProv"].ToString(),
                        PermDist = s.y["PermDist"].ToString(),
                        PermComm = s.y["PermComm"].ToString(),
                        PermVilla = s.y["PermVilla"].ToString(),
                        TaxCode = s.y["TaxCode"].ToString(),
                    }).ToList();

                // Thêm dữ liệu người chưa cập nhật thông tin trên taixin web vào danh sách tất cả nhân viên - danh sách người chưa cập nhật
                foreach (DataRow row in listEmp.Rows)
                {
                    bool isUpdated = false;
                    listAllEmpOK.ForEach(x =>
                    {
                        if (row["EmpId"].ToString().Trim().ToUpper() == x.EmpId.Trim().ToUpper())
                        {
                            isUpdated = true;
                        }
                    });
                    if (isUpdated == false)
                    {
                        Emp emp = new Emp();
                        emp.EmpId = row["EmpId"].ToString();
                        emp.EmpNm = row["EmpNm"].ToString();
                        emp.SexCd = row["SexCd"].ToString();
                        listAllEmpNG.Add(emp);
                    }
                }

                // Check điều kiện lọc
                switch (updateThongTin)
                {
                    case "OK":
                        listAllEmp = listAllEmpOK;
                        break;
                    case "NG":
                        listAllEmp = listAllEmpNG;
                        break;
                    case "ALL":
                        listAllEmp = listAllEmpOK.Union(listAllEmpNG).ToList();
                        break;
                }

                // Lọc dữ liệu theo Địa chỉ nếu radio box không phải là Chưa cập nhật
                if (updateThongTin != "NG")
                {
                    // Lọc theo điều kiện cư trú là tạm trú hoặc thường trú
                    bool thuongTru = true;
                    if (rbTamTru.IsChecked == true) { thuongTru = false; }

                    if (cbbTinh.Text != "ALL")
                    {
                        if (cbbHuyen.Text == "ALL")
                        {
                            // Lọc theo tỉnh thường trú
                            if (thuongTru == true)
                            {
                                listAllEmp = listAllEmp.Where(x => x.PermProv == cbbTinh.Text).ToList();
                            }
                            // Lọc theo tỉnh tạm trú
                            else
                            {
                                listAllEmp = listAllEmp.Where(x => x.TempProv == cbbTinh.Text).ToList();
                            }
                        }
                        else
                        {
                            if (cbbXa.Text == "ALL")
                            {
                                // Lọc theo tỉnh và huyện thường trú
                                if (thuongTru == true)
                                {
                                    listAllEmp = listAllEmp.Where(x => x.PermProv == cbbTinh.Text && x.PermDist == cbbHuyen.Text).ToList();
                                }
                                // Lọc theo tỉnh và huyện tạm trú
                                else
                                {
                                    listAllEmp = listAllEmp.Where(x => x.TempProv == cbbTinh.Text && x.TempDist == cbbHuyen.Text).ToList();
                                }
                            }
                            else
                            {
                                // Lọc theo tỉnh, huyện và xã thường trú
                                if (thuongTru == true)
                                {
                                    listAllEmp = listAllEmp.Where(x => x.PermProv == cbbTinh.Text && x.PermDist == cbbHuyen.Text && x.PermComm == cbbXa.Text).ToList();
                                }
                                // Lọc theo tỉnh, huyện và xã tạm trú
                                else
                                {
                                    listAllEmp = listAllEmp.Where(x => x.TempProv == cbbTinh.Text && x.TempDist == cbbHuyen.Text && x.TempComm == cbbXa.Text).ToList();
                                }
                            }
                        }
                    }
                }

                // Sắp sếp dữ liệu
                listAllEmp = listAllEmp.OrderBy(x => x.Deptlv1).ToList();

                // Thêm STT và sửa Giới tính
                int i = 1;
                listAllEmp.ForEach(x =>
                {
                    x.ID = i;
                    if (x.SexCd.Contains("001")) { x.SexCd = "Nam"; } else { x.SexCd = "Nữ"; }
                    i++;
                });

                // Hiển thị dữ liệu lên view
                lvThongTin.ClearValue(ListView.ItemsSourceProperty);
                lvThongTin.ItemsSource = listAllEmp;
                lb_Qty.Content = listAllEmp.Count() + " (người)";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            
        }
        // END THANGDN



        // Thông tin cư trú
        private void GetEmpInfo()
        {
            try
            {
                var cbbTinhDetail = "Tỉnh Bắc Ninh";
                if (cbbTinh.Text != "ALL")
                {
                    cbbTinhDetail = cbbTinh.Text;
                }
                if (dateCheck.Count() != 8)
                    dateCheck = DateTime.Now.ToString("yyyyMMdd");

                // Lấy dữ liệu thông tin nhân viên
                string query1 = "select * from update_employee where TempProv = @cbbTinh";
                var listEmpInformation = DataProvider.Instance.MySqlExecuteQuery(path_TaixinWeb, query1, new object[] { cbbTinhDetail });



                // Lấy dữ liệu nhân viên thực tế từ Ksystem
                string query2 = "SELECT * FROM TDAEmpMaster where RetDate >= @date";
                var listEmp = DataProvider.Instance.executeQuery(path_Ksystem20, query2, new object[] { dateCheck });

                // Group by dữ liệu
                var listAddressInfo = listEmpInformation.AsEnumerable().Join(listEmp.AsEnumerable(), x => x["EmpId"].ToString().Trim().ToUpper(), y => y["EmpId"].ToString().Trim().ToUpper(), (x, y) => new { x, y })
                    .GroupBy(g => g.x["TempDist"])
                    .Select(s => new
                    {
                        Dist = s.Key.ToString(),
                        Qty = s.Count()
                    }).ToList();
                listAddressInfo = listAddressInfo.OrderByDescending(x => x.Qty).ToList();

                // Chart
                var _qtyCity = new ChartValues<double>();
                var _nameCity = new ChartValues<string>();
                DataChart.Values3 = _qtyCity;

                listAddressInfo.ForEach(x =>
                {
                    int len = x.Dist.Length;
                    if (x.Dist.Contains("Thành phố"))
                    {
                        _nameCity.Add(x.Dist.Substring(10, len - 10));
                    }
                    if (x.Dist.Contains("Quận"))
                    {
                        _nameCity.Add(x.Dist.Substring(5, len - 5));
                    }
                    if (x.Dist.Contains("Thị xã"))
                    {
                        _nameCity.Add(x.Dist.Substring(7, len - 7));
                    }
                    if (x.Dist.Contains("Huyện"))
                    {
                        _nameCity.Add(x.Dist.Substring(6, len - 6));
                    }
                    _qtyCity.Add(x.Qty);
                });

                if (MainWindow.language == "vi-VN")
                {
                    DataChart.Title = "Số người";
                }
                else
                {
                    DataChart.Title = "수량";
                }

                DataChart.Labels = _nameCity;
                DataChart.YFormatter = _qtyCity;
                DataChart.Step = 200;
                if (cbbTinh.Text != "ALL" && cbbTinh.Text != "Tỉnh Bắc Ninh")
                {
                    DataChart.Step = 10;
                }

                DataContext = this;
                Column column = new Column();
                frameChart_HuyenDetail.Navigate(column);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            
        }

        private void EmpInfoUpdateStatusDetail()
        {
            try
            {
                // Lấy dữ liệu thông tin nhân viên
                string query1 = "select * from update_employee";
                var listEmpInformation = DataProvider.Instance.MySqlExecuteQuery(path_TaixinWeb, query1);

                // Lấy thông tin nhân viên đang làm việc
                string query2 = "select * from TDAEmpMaster where RetDate >= @date and len(EmpId) > 4 and len(EmpId) < 8";
                var listEmp = DataProvider.Instance.executeQuery(path_Ksystem20, query2, new object[] { dateCheck });

                var EmpInfoUpdateStatusOK = listEmpInformation.AsEnumerable().Join(listEmp.AsEnumerable(), x => x["EmpId"].ToString().Trim().ToUpper(), y => y["EmpId"].ToString().Trim().ToUpper(), (x, y) => new { x, y })
                    .Select(s => new { s }).ToList().Count();
                var EmpInfoUpdateStatusNG = listEmp.Rows.Count - EmpInfoUpdateStatusOK;
                lb_UpdateDiaChi_OKDetail.Content = EmpInfoUpdateStatusOK;
                lb_UpdateDiaChi_NGDetail.Content = EmpInfoUpdateStatusNG;
                lb_TotalDetail.Content = listEmp.Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            
        }

        //===========================================================================================================================================//
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

        private void cbbTinh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbbTinh.SelectedValue == null || cbbTinh.SelectedValue.ToString() == "ALL")
                {
                    List<string> list = new List<string>();
                    list.Add("ALL");
                    cbbHuyen.ClearValue(ListView.ItemsSourceProperty);
                    cbbXa.ClearValue(ListView.ItemsSourceProperty);
                    cbbHuyen.ItemsSource = list;
                    cbbXa.ItemsSource = list;
                    cbbHuyen.SelectedIndex = 0;
                    cbbXa.SelectedIndex = 0;
                    return;
                }

                // Do không so sánh với danh sách nhân viên thực tế nên vẫn có huyện và xã hiển thị nhưng không có danh sách
                string query = "";
                if (rbThuongTru.IsChecked == true)
                {
                    query = "select distinct PermDist from update_employee where PermProv = @permProv";
                }
                else
                {
                    query = "select distinct TempDist from update_employee where TempProv = @tempProv";
                }
                List<string> listDist = new List<string>();
                listDist.Add("ALL");
                listDist.AddRange(DataProvider.Instance.MySqlGetList(path_TaixinWeb, query, new object[] { cbbTinh.SelectedValue.ToString() }));
                if (cbbHuyen.ItemsSource != null)
                    cbbHuyen.ClearValue(ListView.ItemsSourceProperty);

                cbbHuyen.ItemsSource = listDist;
                cbbHuyen.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            
        }

        private void cbbHuyen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbbHuyen.SelectedValue == null || cbbHuyen.SelectedValue.ToString() == "ALL")
                {
                    List<string> list = new List<string>();
                    list.Add("ALL");
                    cbbXa.ClearValue(ListView.ItemsSourceProperty);
                    cbbXa.ItemsSource = list;
                    cbbXa.SelectedIndex = 0;
                    return;
                }

                string query = "";
                if (rbThuongTru.IsChecked == true)
                {
                    query = "select distinct PermComm from update_employee where PermProv = @permProv and PermDist = @permDist";
                }
                else
                {
                    query = "select distinct TempComm from update_employee where TempProv = @tempProv and TempDist = @tempDist";
                }
                List<string> listComm = new List<string>();
                listComm.Add("ALL");
                listComm.AddRange(DataProvider.Instance.MySqlGetList(path_TaixinWeb, query, new object[] { cbbTinh.SelectedValue.ToString(), cbbHuyen.SelectedValue.ToString() }));
                if (cbbXa.ItemsSource != null)
                    cbbXa.ClearValue(ComboBox.ItemsSourceProperty);

                cbbXa.ItemsSource = listComm;
                cbbXa.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            
        }

        bool checkWorking = false;
        private async void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            try
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
                        GetAllEmp();
                        stackLoading.Visibility = Visibility.Hidden;
                        checkWorking = false;
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);
                });
            }
            catch
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

            GetEmpInfo();
            EmpInfoUpdateStatusDetail();

        }

        private void GetDistinctProv()
        {
            try
            {
                // Lấy dữ liệu thông tin nhân viên
                string query1 = "select * from update_employee";
                var listEmpInformation = DataProvider.Instance.MySqlExecuteQuery(path_TaixinWeb, query1);
                // Lấy dữ liệu nhân viên thực tế từ Ksystem
                string query2 = "SELECT * FROM TDAEmpMaster where RetDate >= @date and len(EmpId) > 4 and len(EmpId) < 8";
                var listEmp = DataProvider.Instance.executeQuery(Page_Main.path_Ksystem20, query2, new object[] { Page_Main.dateCheck });
                // Lấy dữ liệu nhân viên đã được update thông tin trên taixin web
                var listAllEmp = listEmp.AsEnumerable().Join(listEmpInformation.AsEnumerable(), x => x["EmpId"].ToString().Trim().ToUpper(), y => y["EmpId"].ToString().Trim().ToUpper(), (x, y) => new { x, y })
                    .Select(s => new
                    {
                        TempProv = s.y["TempProv"].ToString(),
                        PermProv = s.y["PermProv"].ToString(),
                    }).ToList();

                // Lấy danh sách tỉnh từ bảng update_employee
                List<string> listProv = new List<string>();
                listProv.Add("ALL");
                if (rbThuongTru.IsChecked == true)
                {
                    var listPermProv = listAllEmp.GroupBy(x => x.PermProv).Select(g => g.First()).ToList();
                    listPermProv.ForEach(x =>
                    {
                        listProv.Add(x.PermProv);
                    });
                }
                else
                {
                    var listTempProv = listAllEmp.GroupBy(x => x.TempProv).Select(g => g.First()).ToList();
                    listTempProv.ForEach(x =>
                    {
                        listProv.Add(x.TempProv);
                    });
                }
                cbbTinh.ItemsSource = listProv;
                cbbTinh.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            
        }

        public void CreatListExcel()
        {
            try
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    int numberRow = 0;
                    foreach (var item in listAllEmp)
                    {
                        if (item.EmpId != "")
                        {
                            numberRow++;
                        }
                    }

                    numberRow = numberRow + 5;
                    p.Workbook.Properties.Author = DateTime.Now.ToShortDateString();
                    p.Workbook.Properties.Title = "Thông tin nhân viên";
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
                    ws.Column(9).Width = 15;//ngay cap
                    ws.Column(10).Width = 15;//dan toc
                    ws.Column(11).Width = 20;//bo phan
                    ws.Column(12).Width = 20;//phong ban
                    ws.Column(13).Width = 20;//nhom
                    ws.Column(14).Width = 20;//khu vuc
                    ws.Column(15).Width = 15;//ca
                    ws.Column(16).Width = 10;//cap bac
                    ws.Column(17).Width = 20;//tinh
                    ws.Column(18).Width = 20;//huyen
                    ws.Column(19).Width = 20;//xa
                    ws.Column(20).Width = 20;//thon
                    ws.Column(21).Width = 20;

                    ws.Row(1).Height = 10;
                    ws.Row(2).Height = 40;
                    ws.Row(3).Height = 20;
                    ws.Row(4).Height = 25;

                    //căn hàng và cột cho tất cả các ô                 


                    for (int i = 1; i < numberRow; i++)
                    {
                        string strCell = "A" + i.ToString() + ":" + "U" + i.ToString();
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
                        string strCell = "A" + i.ToString() + ":" + "U" + i.ToString();
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

                        string strCell11 = "K" + i.ToString() + ":" + "K" + i.ToString();
                        ws.Cells[strCell11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        string strCell12 = "L" + i.ToString() + ":" + "L" + i.ToString();
                        ws.Cells[strCell12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        string strCell13 = "M" + i.ToString() + ":" + "M" + i.ToString();
                        ws.Cells[strCell13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        string strCell14 = "N" + i.ToString() + ":" + "N" + i.ToString();
                        ws.Cells[strCell4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        string strCell15 = "O" + i.ToString() + ":" + "O" + i.ToString();
                        ws.Cells[strCell5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        string strCell16 = "P" + i.ToString() + ":" + "P" + i.ToString();
                        ws.Cells[strCell6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        string strCell17 = "Q" + i.ToString() + ":" + "Q" + i.ToString();
                        ws.Cells[strCell7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        string strCell18 = "R" + i.ToString() + ":" + "R" + i.ToString();
                        ws.Cells[strCell8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        string strCell19 = "S" + i.ToString() + ":" + "S" + i.ToString();
                        ws.Cells[strCell9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        string strCell20 = "T" + i.ToString() + ":" + "T" + i.ToString();
                        ws.Cells[strCell20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        string strCell21 = "U" + i.ToString() + ":" + "U" + i.ToString();
                        ws.Cells[strCell21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    }


                    for (int i = 5; i < numberRow; i++)
                    {
                        if (i % 2 == 0)
                        {
                            string strCell = "A" + i.ToString() + ":" + "U" + i.ToString();
                            var cell = ws.Cells[strCell];
                            var fill = cell.Style.Fill;
                            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                        }
                    }

                    //Bôi den backgroud
                    //

                    ws.Cells["A2:U2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A2:U2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Azure);

                    ws.Cells["A4:U4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A4:U4"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Ivory);


                    ws.Cells["A1:A1"].Value = "";
                    ws.Cells["A1:U1"].Merge = true;
                    ws.Cells["A1:A1"].Style.Font.Size = 25;
                    ws.Cells["A1:A1"].Style.Font.Bold = true;


                    ws.Cells["A2:A2"].Value = "THÔNG TIN CÔNG NHÂN VIÊN";
                    ws.Cells["A2:U2"].Merge = true;
                    ws.Cells["A2:A2"].Style.Font.Size = 22;
                    ws.Cells["A2:A2"].Style.Font.Bold = true;


                    //Ngày SX
                    ws.Cells["A3:A3"].Value = "Ngày : " + DateTime.Now.ToString("dd/MM/yyyy") + "  Số lượng : " + (numberRow-5);
                    ws.Cells["A3:U3"].Merge = true;
                    ws.Cells["A3:A3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["A3:A3"].Style.Font.Bold = true;


                    //Head                  
                    ws.Cells["A4:U4"].Style.Font.Size = 12;
                    ws.Cells["A4:U4"].Style.Font.Bold = true;
                    ws.Cells["A4:A4"].Value = "STT";
                    ws.Cells["B4:B4"].Value = "Mã NV";
                    ws.Cells["C4:C4"].Value = "Họ và Tên";
                    ws.Cells["D4:D4"].Value = "Giới tính";
                    ws.Cells["E4:E4"].Value = "Ngày sinh";
                    ws.Cells["F4:F4"].Value = "Số điện thoại";
                    ws.Cells["G4:G4"].Value = "Mã số thuế";
                    ws.Cells["H4:H4"].Value = "CMT/CCCD";
                    ws.Cells["I4:I4"].Value = "Nơi cấp";
                    ws.Cells["J4:J4"].Value = "Ngày cấp";
                    ws.Cells["K4:K4"].Value = "Dân tộc";
                    ws.Cells["L4:L4"].Value = "Bộ phận";
                    ws.Cells["M4:M4"].Value = "Phòng ban";
                    ws.Cells["N4:N4"].Value = "Nhóm";
                    ws.Cells["O4:O4"].Value = "Khu vực";
                    ws.Cells["P4:P4"].Value = "Ca";
                    ws.Cells["Q4:Q4"].Value = "Cấp bậc";
                    ws.Cells["R4:R4"].Value = "Tỉnh/Thành phố";
                    ws.Cells["S4:S4"].Value = "Quận/Huyện";
                    ws.Cells["T4:T4"].Value = "Phường/Xã";
                    ws.Cells["U4:U4"].Value = "Thôn/xóm";
                    


                    int index = 4;
                    int stt = 0;

                    foreach (var item in listAllEmp)
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
                            ws.Cells[strCell2].Value = item.EmpId;
                            //--
                            string strCell3 = "C" + index.ToString() + ":" + "C" + index.ToString();
                            ws.Cells[strCell3].Value = item.EmpNm;
                            //--
                            string strCell4 = "D" + index.ToString() + ":" + "D" + index.ToString();
                            ws.Cells[strCell4].Value = item.SexCd;
                            //--
                            string strCell5 = "E" + index.ToString() + ":" + "E" + index.ToString();
                            ws.Cells[strCell5].Value = item.BOD;
                            //--
                            string strCell6 = "F" + index.ToString() + ":" + "F" + index.ToString();
                            ws.Cells[strCell6].Value = item.HpTel;

                            string strCell7 = "G" + index.ToString() + ":" + "G" + index.ToString();
                            ws.Cells[strCell7].Value = item.TaxCode;
                            //--
                            string strCell8 = "H" + index.ToString() + ":" + "H" + index.ToString();
                            ws.Cells[strCell8].Value = item.ResidId;
                            //--
                            string strCell9 = "I" + index.ToString() + ":" + "I" + index.ToString();
                            ws.Cells[strCell9].Value = item.ResidPlace;
                            //--
                            string strCell10 = "J" + index.ToString() + ":" + "J" + index.ToString();
                            ws.Cells[strCell10].Value = item.ResidDate;
                            string strCell11 = "K" + index.ToString() + ":" + "K" + index.ToString();
                            ws.Cells[strCell11].Value = item.Nation;
                            string strCell12 = "L" + index.ToString() + ":" + "L" + index.ToString();
                            ws.Cells[strCell12].Value = item.Deptlv1;
                            string strCell13 = "M" + index.ToString() + ":" + "M" + index.ToString();
                            ws.Cells[strCell13].Value = item.Deptlv2;
                            string strCell14 = "N" + index.ToString() + ":" + "N" + index.ToString();
                            ws.Cells[strCell14].Value = item.Deptlv3;
                            string strCell15 = "O" + index.ToString() + ":" + "O" + index.ToString();
                            ws.Cells[strCell15].Value = item.Position;
                            string strCell16 = "P" + index.ToString() + ":" + "P" + index.ToString();
                            ws.Cells[strCell16].Value = item.Shift;
                            string strCell17 = "Q" + index.ToString() + ":" + "Q" + index.ToString();
                            ws.Cells[strCell17].Value = item.Level;
                            string strCell18 = "R" + index.ToString() + ":" + "R" + index.ToString();
                            ws.Cells[strCell18].Value = item.TempProv;
                            string strCell19 = "S" + index.ToString() + ":" + "S" + index.ToString();
                            ws.Cells[strCell19].Value = item.TempDist;
                            string strCell20 = "T" + index.ToString() + ":" + "T" + index.ToString();
                            ws.Cells[strCell20].Value = item.TempComm;
                            string strCell21 = "U" + index.ToString() + ":" + "U" + index.ToString();
                            ws.Cells[strCell21].Value = item.TempVilla;
                           

                        }

                    }


                    ws.PrinterSettings.PaperSize = ePaperSize.A4;
                    ws.PrinterSettings.Orientation = eOrientation.Landscape;
                    ws.PrinterSettings.FitToPage = true;
                    ws.Cells["A4:U4"].AutoFilter = true;
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

        private void btnEditAdd_Click(object sender, RoutedEventArgs e)
        {
            Window_CheckUser emp = new Window_CheckUser();
            emp.Show();
        }

        private void btnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            Process_ExportExcel();
        }

        private void rbThuongTru_Checked(object sender, RoutedEventArgs e)
        {
            GetDistinctProv();
        }

        private void rbTamTru_Checked(object sender, RoutedEventArgs e)
        {
            GetDistinctProv();
        }
    }
}
