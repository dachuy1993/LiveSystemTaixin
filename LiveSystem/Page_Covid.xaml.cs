using LiveSystem.DAO;
using LiveSystem.ViewModel;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Page_Covid.xaml
    /// </summary>
    public partial class Page_Covid : Page
    {
        //string path_TaixinWeb = "server=192.168.2.40;Port=3307;user id=txadmin;database=LiveSystem;password=Taixinweb1!";
        List<Helper_Covid> listExportExcel = new List<Helper_Covid>();

        public Page_Covid()
        {
            InitializeComponent();
            GetListVTimes();
            Loaded += Page_Covid_Loaded;
            VaccineInfo();
        }

        private void Page_Covid_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MainWindow.language);
            ApplyLanguage(MainWindow.language);            
        }

        //======================================================================================================//
        private void GetAllVaccineInfo()
        {
            try
            {
                var listEmpVaccine = new List<Emp_Vaccine>();
                var listEmpVaccine0 = new List<Emp_Vaccine>();
                //if (cbb_NumberVaccine.Text != "0")
                //{
                    // Lấy dữ liệu nhân viên thực tế từ Ksystem
                    string query1 = "SELECT * FROM TDAEmpMaster where RetDate>= @date and len(EmpId) > 4 and len(EmpId) < 8";
                    var listEmp = DataProvider.Instance.executeQuery(Page_Main.path_Ksystem20, query1, new object[] { DateTime.Now.ToString("yyyy-MM-dd") });

                    // Lấy dữ liệu số mũi vaccine
                    string query2 = "SELECT B.EmpId, B.EmpNm, B.Deptlv1, B.Deptlv2,case when isnull(A.Vtimes) = 0 then A.Vtimes else 0 end as Vtimes FROM LiveSystem.update_employee B left join LiveSystem.vacxin A on A.EmpId = B.EmpId where  (case when isnull(A.Vtimes) = 0 then A.Vtimes else 0 end) = @vtimes";
                    var listAllEmpVaccine = DataProvider.Instance.MySqlExecuteQuery(Page_Main.path_TaixinWeb, query2, new object[] { cbb_NumberVaccine.Text });

                    // Join 2 table ở trên
                    listEmpVaccine = listAllEmpVaccine.AsEnumerable().Join(listEmp.AsEnumerable(), x => x["EmpId"].ToString().Trim().ToUpper(), y => y["EmpId"].ToString().Trim().ToUpper(), (x, y) => new { x, y })
                        .Select(s => new Emp_Vaccine
                        {
                            EmpId = s.x["EmpId"].ToString(),
                            EmpNm = s.x["EmpNm"].ToString(),
                            Deptlv1 = s.x["Deptlv1"].ToString(),
                            Deptlv2 = s.x["Deptlv2"].ToString(),
                            Vtimes = int.Parse(s.x["Vtimes"].ToString())
                        }).OrderBy(x => x.EmpId).ToList();
                //}
                //else
                //{
                //    // Lấy dữ liệu nhân viên thực tế từ Ksystem
                //    string query1 = "SELECT * FROM TDAEmpMaster where RetDate>= @date and len(EmpId) > 4 and len(EmpId) < 8";
                //    var listEmp = DataProvider.Instance.executeQuery(Page_Main.path_Ksystem20, query1, new object[] { DateTime.Now.ToString("yyyy-MM-dd") });

                //    // Lấy dữ liệu số mũi vaccine
                //    string query2 = "SELECT B.EmpId, B.EmpNm, B.Deptlv1, B.Deptlv2, A.Vtimes FROM LiveSystem.vacxin A join LiveSystem.update_employee B on A.EmpId = B.EmpId where Vtimes = 1";
                //    var listAllEmpVaccine = DataProvider.Instance.MySqlExecuteQuery(Page_Main.path_TaixinWeb, query2);

                //    // Join 2 table ở trên
                //    listEmpVaccine = listAllEmpVaccine.AsEnumerable().Join(listEmp.AsEnumerable(), x => x["EmpId"].ToString().Trim().ToUpper(), y => y["EmpId"].ToString().Trim().ToUpper(), (x, y) => new { x, y })
                //        .Select(s => new Emp_Vaccine
                //        {
                //            EmpId = s.x["EmpId"].ToString(),
                //            EmpNm = s.x["EmpNm"].ToString(),
                //            Deptlv1 = s.x["Deptlv1"].ToString(),
                //            Deptlv2 = s.x["Deptlv2"].ToString(),
                //            Vtimes = int.Parse(s.x["Vtimes"].ToString())
                //        }).OrderBy(x => x.EmpId).ToList();
                //    foreach (DataRow row in listEmp.Rows)
                //    {
                //        bool checkExist = false;
                //        listEmpVaccine.ForEach(x => {
                //            if (row["EmpId"].ToString().Trim().ToUpper() == x.EmpId.Trim().ToUpper())
                //            {
                //                checkExist = true;
                //            }
                //        });
                //        if (checkExist == false)
                //        {
                //            Emp_Vaccine emp = new Emp_Vaccine();
                //            emp.EmpId = row["EmpId"].ToString();
                //            emp.EmpNm = row["EmpNm"].ToString();
                //            listEmpVaccine0.Add(emp);
                //        }
                //    }
                    //listEmpVaccine = listEmpVaccine0.OrderBy(x => x.EmpId).ToList();
                //}


                int i = 1;
                listEmpVaccine.ForEach(x =>
                {
                    x.ID = i;
                    i++;
                });

                // Hiển thị danh sách lên view
                lvKhaibaoYte.ItemsSource = listEmpVaccine;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            
        }

        private void GetListVTimes()
        {
            string query = "select distinct Vtimes from vacxin";
            var list = new List<string>();
            list.Add("0");
            list.AddRange(DataProvider.Instance.MySqlGetList(Page_Main.path_TaixinWeb, query));
            cbb_NumberVaccine.ItemsSource = list;
            cbb_NumberVaccine.SelectedIndex = 1;
        }
        //======================================================================================================//

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

        bool checkWorking = false;
        private async void btnTimKiemYte_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    Page_LoadingData page_Loading = new Page_LoadingData();
                    stackLoading.Visibility = Visibility.Visible;
                    frameLoading.Navigate(page_Loading);
                    lvKhaibaoYte.ClearValue(ListView.ItemsSourceProperty);
                    checkWorking = true;
                }, System.Windows.Threading.DispatcherPriority.ContextIdle);
            });

            await Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    GetAllVaccineInfo();
                    stackLoading.Visibility = Visibility.Hidden;
                    checkWorking = false;
                }, System.Windows.Threading.DispatcherPriority.ContextIdle);
            });
        }

        public void CreatListExcel(List<Helper_Covid> listEmpVaccine)
        {
            try
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    int numberRow = 0;
                    foreach (var item in listEmpVaccine)
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
                    ws.Column(4).Width = 30;//bo phan
                    ws.Column(5).Width = 20;//suc khoe
                    ws.Column(6).Width = 20;//F0
                    ws.Column(7).Width = 40;//tiep xuc
                    ws.Column(8).Width = 40;//di chuyen               
                    //ws.Column(9).Width = 15;//ngay cap
                    //ws.Column(10).Width = 15;//dan toc
                    //ws.Column(11).Width = 20;//bo phan
                    //ws.Column(12).Width = 20;//phong ban
                    //ws.Column(13).Width = 20;//nhom
                    //ws.Column(14).Width = 20;//khu vuc
                    //ws.Column(15).Width = 15;//ca
                    //ws.Column(16).Width = 10;//cap bac
                    //ws.Column(17).Width = 20;//tinh
                    //ws.Column(18).Width = 20;//huyen
                    //ws.Column(19).Width = 20;//xa
                    //ws.Column(20).Width = 20;//thon

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
                        //string strCell8 = "H" + i.ToString() + ":" + "H" + i.ToString();
                        //ws.Cells[strCell8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ////--
                        //string strCell9 = "I" + i.ToString() + ":" + "I" + i.ToString();
                        //ws.Cells[strCell9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //string strCell10 = "J" + i.ToString() + ":" + "J" + i.ToString();
                        //ws.Cells[strCell10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //string strCell11 = "K" + i.ToString() + ":" + "K" + i.ToString();
                        //ws.Cells[strCell11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //string strCell12 = "L" + i.ToString() + ":" + "L" + i.ToString();
                        //ws.Cells[strCell12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //string strCell13 = "M" + i.ToString() + ":" + "M" + i.ToString();
                        //ws.Cells[strCell13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //string strCell14 = "N" + i.ToString() + ":" + "N" + i.ToString();
                        //ws.Cells[strCell4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //string strCell15 = "O" + i.ToString() + ":" + "O" + i.ToString();
                        //ws.Cells[strCell5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //string strCell16 = "P" + i.ToString() + ":" + "P" + i.ToString();
                        //ws.Cells[strCell6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //string strCell17 = "Q" + i.ToString() + ":" + "Q" + i.ToString();
                        //ws.Cells[strCell7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //string strCell18 = "R" + i.ToString() + ":" + "R" + i.ToString();
                        //ws.Cells[strCell8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //string strCell19 = "S" + i.ToString() + ":" + "S" + i.ToString();
                        //ws.Cells[strCell9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //string strCell20 = "T" + i.ToString() + ":" + "T" + i.ToString();
                        //ws.Cells[strCell20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

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


                    ws.Cells["A2:A2"].Value = "THÔNG TIN CÔNG NHÂN VIÊN";
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
                    ws.Cells["B4:B4"].Value = "Mã NV";
                    ws.Cells["C4:C4"].Value = "Họ và Tên";
                    ws.Cells["D4:D4"].Value = "Bộ phận";
                    ws.Cells["E4:E4"].Value = "Sức khỏe";
                    ws.Cells["F4:F4"].Value = "Tiếp xúc";
                    ws.Cells["G4:G4"].Value = "Nội dung tiếp xúc";
                    ws.Cells["H4:H4"].Value = "Di chuyển";
                    //ws.Cells["I4:I4"].Value = "Ngày cấp";
                    //ws.Cells["J4:J4"].Value = "Dân tộc";
                    //ws.Cells["K4:K4"].Value = "Bộ phận";
                    //ws.Cells["L4:L4"].Value = "Phòng ban";
                    //ws.Cells["M4:M4"].Value = "Nhóm";
                    //ws.Cells["N4:N4"].Value = "Khu vực";
                    //ws.Cells["O4:O4"].Value = "Ca";
                    //ws.Cells["P4:P4"].Value = "Cấp bậc";
                    //ws.Cells["Q4:Q4"].Value = "Tỉnh/Thành phố";
                    //ws.Cells["R4:R4"].Value = "Quận/Huyện";
                    //ws.Cells["S4:S4"].Value = "Phường/Xã";
                    //ws.Cells["T4:T4"].Value = "Thôn/xóm";


                    int index = 4;
                    int stt = 0;

                    foreach (var item in listEmpVaccine)
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
                            ws.Cells[strCell4].Value = item.Dept;
                            //--
                            string strCell5 = "E" + index.ToString() + ":" + "E" + index.ToString();
                            ws.Cells[strCell5].Value = item.StatusHealth;
                            //--
                            string strCell6 = "F" + index.ToString() + ":" + "F" + index.ToString();
                            ws.Cells[strCell6].Value = item.ContactF;
                            //--
                            string strCell7 = "G" + index.ToString() + ":" + "G" + index.ToString();
                            ws.Cells[strCell7].Value = item.StatusContact;
                            //--
                            string strCell8 = "H" + index.ToString() + ":" + "H" + index.ToString();
                            ws.Cells[strCell8].Value = item.HistoryTravel;
                            ////--
                            //string strCell9 = "I" + index.ToString() + ":" + "I" + index.ToString();
                            //ws.Cells[strCell9].Value = item.ResidDate;
                            //string strCell10 = "J" + index.ToString() + ":" + "J" + index.ToString();
                            //ws.Cells[strCell10].Value = item.Nation;
                            //string strCell11 = "K" + index.ToString() + ":" + "K" + index.ToString();
                            //ws.Cells[strCell11].Value = item.TenBoPhan;
                            //string strCell12 = "L" + index.ToString() + ":" + "L" + index.ToString();
                            //ws.Cells[strCell12].Value = item.TenPhongBan;
                            //string strCell13 = "M" + index.ToString() + ":" + "M" + index.ToString();
                            //ws.Cells[strCell13].Value = item.TenNhom;
                            //string strCell14 = "N" + index.ToString() + ":" + "N" + index.ToString();
                            //ws.Cells[strCell14].Value = item.Position;
                            //string strCell15 = "O" + index.ToString() + ":" + "O" + index.ToString();
                            //ws.Cells[strCell15].Value = item.Shift;
                            //string strCell16 = "P" + index.ToString() + ":" + "P" + index.ToString();
                            //ws.Cells[strCell16].Value = item.Level;
                            //string strCell17 = "Q" + index.ToString() + ":" + "Q" + index.ToString();
                            //ws.Cells[strCell17].Value = item.Temp_TenTinh;
                            //string strCell18 = "R" + index.ToString() + ":" + "R" + index.ToString();
                            //ws.Cells[strCell18].Value = item.Temp_TenHuyen;
                            //string strCell19 = "S" + index.ToString() + ":" + "S" + index.ToString();
                            //ws.Cells[strCell19].Value = item.Temp_TenXa;
                            //string strCell20 = "T" + index.ToString() + ":" + "T" + index.ToString();
                            //ws.Cells[strCell20].Value = item.TempVilla;

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
                    //exportFileExcel = false;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CreatListExcel", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        string pathFileExcel = @"TempFile//ExcelFile.xlsx";


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
                            CreatListExcel(listExportExcel);
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


        private void VaccineInfo()
        {
            try
            {
                // Lấy dữ liệu nhân viên thực tế từ Ksystem
                string query2 = "SELECT * FROM TDAEmpMaster where RetDate>= @date and len(EmpId) > 4 and len(EmpId) < 8";
                var listEmp = DataProvider.Instance.executeQuery(Page_Main.path_Ksystem20, query2, new object[] { Page_Main.dateCheck });

                // Lấy dữ liệu số mũi vaccine
                string query = "select * from vacxin";
                var listAllEmpVaccine = DataProvider.Instance.MySqlExecuteQuery(Page_Main.path_TaixinWeb, query);

                // Join 2 table ở trên
                var listEmpVaccine = listAllEmpVaccine.AsEnumerable().Join(listEmp.AsEnumerable(), x => x["EmpId"].ToString().Trim().ToUpper(), y => y["EmpId"].ToString().Trim().ToUpper(), (x, y) => new { x, y })
                    .Select(s => new Emp_Vaccine
                    {
                        EmpId = s.x["EmpId"].ToString(),
                        Vtimes = int.Parse(s.x["Vtimes"].ToString())
                    });

                // Hiển thị lên view
                double vaccine1 = listEmpVaccine.Where(x => x.Vtimes == 1).Count();
                double vaccine2 = listEmpVaccine.Where(x => x.Vtimes == 2).Count();
                double vaccine3 = listEmpVaccine.Where(x => x.Vtimes == 3).Count();
                double vaccine4 = listEmpVaccine.Where(x => x.Vtimes >= 4).Count();
                double total = listEmp.Rows.Count;

                lb_Vaccine1.Content = vaccine1;
                lb_Vaccine2.Content = vaccine2;
                lb_Vaccine3.Content = vaccine3;
                lb_Vaccine4.Content = vaccine4;
                lb_VaccineNo.Content = total - vaccine1;

                if (listEmpVaccine.Count() != 0)
                {
                    lb_Rate1.Content = Math.Round(100 / total * vaccine1, 1).ToString() + "%";
                    lb_Rate2.Content = Math.Round(100 / total * vaccine2, 1).ToString() + "%";
                    lb_Rate3.Content = Math.Round(100 / total * vaccine3, 1).ToString() + "%";
                    lb_Rate4.Content = Math.Round(100 / total * vaccine4, 1).ToString() + "%";
                    lb_RateNo.Content = Math.Round(100 / total * (total - vaccine1), 1).ToString() + "%";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in processing vaccination data", "Error", MessageBoxButton.OK);
            }

        }
        private void btnExcelYte_Click(object sender, RoutedEventArgs e)
        {
            Process_ExportExcel();
        }

    }
}
