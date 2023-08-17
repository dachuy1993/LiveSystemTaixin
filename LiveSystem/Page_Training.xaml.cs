using LiveSystem.DAO;
using LiveSystem.Model;
using Microsoft.Win32;
using OfficeOpenXml.Style;
using OfficeOpenXml;
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
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for Page_Training.xaml
    /// </summary>
    public partial class Page_Training : Page
    {
        bool checkWorking = false;
        string pathFileExcel = @"TempFile//ExcelFile.xlsx";
        List<EmpEduModel> list_Excell = new List<EmpEduModel>();
        

        public Page_Training()
        {
            InitializeComponent();
            String DateYM = DateTime.Now.ToString("yyyy");
            cbbYear.Text = DateYM;
            Loaded += Page_Training_Loaded;
            GetDataCmb();
            GetDataCmbYear();
        }

        
        
        private async void GetDataCmb()
        {
            try
            {
                string Year = cbbYear.Text;
                string query = "SPGetDataCmbTypeTraining @date";
                // Lấy dữ liệu và hiển thị
                DataTable listCmb = new DataTable();

                listCmb = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query, new object[] { Year });


                List<string> listResult = new List<string>();
                foreach (DataRow Row in listCmb.Rows)
                {
                    listResult.Add(Row["Name"].ToString());
                }
                cbbType.ItemsSource = listResult;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            
        }


        private async void GetPlanEduNow()
        {
            try
            {
                string TypeNm = cbbType.SelectedValue.ToString();
                string DateYM = DateTime.Now.ToString("yyyyMM");
                string query = "SPGetDataPlanEduNow @dateYM , @TypeNm";
                // Lấy dữ liệu và hiển thị
                DataTable listCmb = new DataTable();

                listCmb = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query, new object[] { DateYM, TypeNm });

                lvPlan.ItemsSource = listCmb.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            
        }

        private async void GetPlanEduAfter()
        {
            try
            {
                string TypeNm = cbbType.SelectedValue.ToString();
                string DateYM = DateTime.Now.ToString("yyyyMM");
                string query = "SPGetDataPlanEduAfter @dateYM , @TypeNm";
                // Lấy dữ liệu và hiển thị
                DataTable listCmb = new DataTable();

                listCmb = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query, new object[] { DateYM, TypeNm });

                lvPlanAfter.ItemsSource = listCmb.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            
        }

        private void Page_Training_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MainWindow.language);
            ApplyLanguage(MainWindow.language);
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

        private async void GetListEmpTraining()
        {
            try
            {
                string TypeNm = cbbType.SelectedValue.ToString();
                string Year = cbbYear.Text;
                string query = "SPGetDataTrainingDetail @year , @TypeNm";

                //hiển thị Page_loading
                await Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Page_LoadingData page_loading = new Page_LoadingData();
                        stackLoading.Visibility = Visibility.Visible;
                        frameLoading.Navigate(page_loading);
                        checkWorking = true;
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);
                });

                //lấy dữ liệu và hiển thị

                DataTable listTraining = new DataTable();
                await Task.Run(() =>
                {
                    listTraining = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query, new object[] { Year, TypeNm });

                });
                var listAll = new List<EmpEduModel>();
                foreach (DataRow rowA in listTraining.Rows)
                {
                    EmpEduModel emp = new EmpEduModel();
                    emp.PL1 = rowA["PL1"].ToString();
                    emp.PL2 = rowA["PL2"].ToString();
                    emp.TrainName = rowA["TrainName"].ToString();
                    emp.QDLP = rowA["QDLP"].ToString();
                    emp.TrainPos = rowA["TrainPos"].ToString();
                    emp.PTDT = rowA["PTDT"].ToString();
                    emp.TrainLocation = rowA["TrainLocation"].ToString();
                    emp.ChargeTrain = rowA["ChargeTrain"].ToString();
                    emp.TrainPrice = rowA["TrainPrice"].ToString();
                    emp.Cycle = rowA["Cycle"].ToString();
                    emp.Times = rowA["Times"].ToString();
                    emp.Duration1time = rowA["Duration1time"].ToString();
                    emp.Numper = rowA["Numper"].ToString();
                    emp.PlanTraning = rowA["PlanTraning"].ToString();
                    emp.Month1 = rowA["Month1"].ToString();
                    emp.Month2 = rowA["Month2"].ToString();
                    emp.Month3 = rowA["Month3"].ToString();
                    emp.Month4 = rowA["Month4"].ToString();
                    emp.Month5 = rowA["Month5"].ToString();
                    emp.Month6 = rowA["Month6"].ToString();
                    emp.Month7 = rowA["Month7"].ToString();
                    emp.Month8 = rowA["Month8"].ToString();
                    emp.Month9 = rowA["Month9"].ToString();
                    emp.Month10 = rowA["Month10"].ToString();
                    emp.Month11 = rowA["Month11"].ToString();
                    emp.Month12 = rowA["Month12"].ToString();
                    emp.Remark = rowA["Remark"].ToString();
                    emp.Year = rowA["Year"].ToString();
                    listAll.Add(emp);
                }

                //Thêm STT
                int i = 1;
                listAll.ForEach(x =>
                {
                    x.ID = i;
                    i++;
                });

                lvTraining.ItemsSource = listTraining.DefaultView;
                list_Excell = listAll;

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
            catch (Exception)
            {
                MessageBox.Show("Error when processing training data", "Error", MessageBoxButton.OK);
            }
            
        }


        // Hiển thị bảng quản lý đào tạo
        private async void GetEduInfo()
        {
            try
            {
                string TypeNm = cbbType.SelectedValue.ToString();
                string Year = cbbYear.Text;
                //string query = "SELECT * from tmmwrate where Shift = @shift and Insdt = @date";
                string query = "SPGetDateTrainingMainDetail @date , @TypeNm";


                // Lấy dữ liệu và hiển thị
                DataTable listEduInfo = new DataTable();
                await Task.Run(() =>
                {


                    listEduInfo = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query, new object[] { Year, TypeNm });
                    //foreach (DataRow row in listEduInfo.Rows)
                    //{
                    //    row["Rate"] = row["Rate"] + "%";
                    //}
                });
                lvEdu.ItemsSource = listEduInfo.DefaultView;
            }
            catch (Exception)
            {
                MessageBox.Show("Error when processing training data", "Error", MessageBoxButton.OK);
            }
            


        }

        private async void GetDataCmbYear()
        {
            string Year = "";
            string query = "SPGetDataCmbYearTrainning @date";
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
                    string totalPrice = "";
                    string TotalPlanEdu = "";
                    string Year = "";
                    foreach (var item in list_Excell)
                    {
                        if (item.PL1 != "")
                        {
                            numberRow++;
                        }
                    }
                    numberRow = numberRow + 5;
                    foreach (var item in list_Excell)
                    {
                        if (item.PL1 == "TOTAL ALL")
                        {
                            totalPrice = item.TrainPrice;
                            TotalPlanEdu = item.PlanTraning;
                            Year = item.Year;
                        }
                    }

                        
                    p.Workbook.Properties.Author = DateTime.Now.ToShortDateString();
                    p.Workbook.Properties.Title = "KẾ HOẠCH ĐÀO TẠO NĂM "+ Year;
                    p.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet ws = p.Workbook.Worksheets[1];
                    ws.Name = "Sheet1";

                    //Cột 1 
                    ws.Column(1).Width = 5;//stt
                    ws.Column(2).Width = 30;//PL1
                    ws.Column(3).Width = 30;//PL2
                    ws.Column(4).Width = 40;//Tên khoa dao tao
                    ws.Column(5).Width = 30;//QDLP
                    ws.Column(6).Width = 30;//Doi tuong dao tao
                    ws.Column(7).Width = 20;//PTDT
                    ws.Column(8).Width = 20;//Dia diem dao tao
                    ws.Column(9).Width = 20;//Phu trach dao tao
                    ws.Column(10).Width = 20;//Du toan chi phi

                    ws.Column(11).Width = 10;//Chu ky
                    ws.Column(12).Width = 10;//So lan
                    ws.Column(13).Width = 20;//Thoi luong 1 lan
                    ws.Column(14).Width = 30;//SO nguoi dc dao tao
                    ws.Column(15).Width = 30;//Ke hoach thoi luong dao tao
                    ws.Column(16).Width = 10;//T1
                    ws.Column(17).Width = 10;//T2
                    ws.Column(18).Width = 10;//T3
                    ws.Column(19).Width = 10;//T4
                    ws.Column(20).Width = 10;//T5
                    ws.Column(21).Width = 10;//T6
                    ws.Column(22).Width = 10;//T7
                    ws.Column(23).Width = 10;//T8
                    ws.Column(24).Width = 10;//T9
                    ws.Column(25).Width = 10;//T10
                    ws.Column(26).Width = 10;//T11
                    ws.Column(27).Width = 10;//T12
                    ws.Column(28).Width = 10;//Remark                        

                    ws.Row(1).Height = 10;
                    ws.Row(2).Height = 40;
                    ws.Row(3).Height = 20;
                    ws.Row(4).Height = 40;
                    ws.Row(5).Height = 40;

                    //căn hàng và cột cho tất cả các ô                 


                    for (int i = 1; i <= numberRow; i++)
                    {
                        string strCell = "A" + i.ToString() + ":" + "AB" + i.ToString();
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

                    for (int i = 6; i <= numberRow; i++)
                    {
                        string strCell = "A" + i.ToString() + ":" + "AB" + i.ToString();
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
                        //--
                        string strCell21 = "U" + i.ToString() + ":" + "U" + i.ToString();
                        ws.Cells[strCell21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell22 = "V" + i.ToString() + ":" + "V" + i.ToString();
                        ws.Cells[strCell22].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell23 = "W" + i.ToString() + ":" + "W" + i.ToString();
                        ws.Cells[strCell23].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell24 = "X" + i.ToString() + ":" + "X" + i.ToString();
                        ws.Cells[strCell24].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        string strCell25 = "Y" + i.ToString() + ":" + "Y" + i.ToString();
                        ws.Cells[strCell25].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell26 = "Z" + i.ToString() + ":" + "Z" + i.ToString();
                        ws.Cells[strCell26].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell27 = "AA" + i.ToString() + ":" + "AA" + i.ToString();
                        ws.Cells[strCell27].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //--
                        string strCell28 = "AB" + i.ToString() + ":" + "AB" + i.ToString();
                        ws.Cells[strCell28].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    for (int i = 5; i <= numberRow; i++)
                    {
                        if (i % 2 == 0)
                        {
                            string strCell = "A" + i.ToString() + ":" + "AB" + i.ToString();
                            var cell = ws.Cells[strCell];
                            var fill = cell.Style.Fill;
                            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                        }
                    }

                    //Bôi den backgroud
                    //

                    ws.Cells["A2:AB2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A2:AB2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Azure);

                    ws.Cells["A4:AB4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A4:AB4"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Ivory);


                    ws.Cells["A1:A1"].Value = "";
                    ws.Cells["A1:H1"].Merge = true;
                    ws.Cells["A1:A1"].Style.Font.Size = 25;
                    ws.Cells["A1:A1"].Style.Font.Bold = true;


                    ws.Cells["A2:A2"].Value = "KẾ HOẠCH ĐÀO TẠO NĂM " + (Year);
                    ws.Cells["A2:AB2"].Merge = true;
                    ws.Cells["A2:A2"].Style.Font.Size = 22;
                    ws.Cells["A2:A2"].Style.Font.Bold = true;


                    //Ngày SX
                    ws.Cells["A3:A3"].Value = "Ngày : " + DateTime.Now.ToString("dd/MM/yyyy") + "      교육예산비 Tổng dự toán chi phí : " + (totalPrice) + "         교육 총시간 Tổng kế hoạch thời lượng đào tạo : " + (TotalPlanEdu);
                    ws.Cells["A3:AB3"].Merge = true;
                    ws.Cells["A3:A3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["A3:A3"].Style.Font.Bold = true;


                    //Head                  
                    ws.Cells["A4:AB5"].Style.Font.Size = 12;
                    ws.Cells["A4:AB5"].Style.Font.Bold = true;
                    ws.Cells["A4:A5"].Value = "순서\r\nThứ tự";
                    ws.Cells["A4:A5"].Merge = true;
                    ws.Cells["B4:B5"].Value = "구분\r\nPhân loại 1";
                    ws.Cells["B4:B5"].Merge = true;
                    ws.Cells["C4:C5"].Value = "교육종류\r\nPhân loại 2";
                    ws.Cells["C4:C5"].Merge = true;
                    ws.Cells["D4:D5"].Value = "교육 과정명\r\nTên khóa đào tạo";
                    ws.Cells["D4:D5"].Merge = true;
                    ws.Cells["E4:E5"].Value = "교육관련 법규, 시행령\r\nQuy định luật pháp";
                    ws.Cells["E4:E5"].Merge = true;
                    ws.Cells["F4:F5"].Value = "교육 대상\r\nĐối tượng đào tạo";
                    ws.Cells["F4:F5"].Merge = true;
                    ws.Cells["G4:G5"].Value = "교육 방식\r\nPhương thức đào tạo";
                    ws.Cells["G4:G5"].Merge = true;
                    ws.Cells["H4:H5"].Value = "교육 장소\r\nĐịa điểm đào tạo";
                    ws.Cells["H4:H5"].Merge = true;
                    ws.Cells["I4:I5"].Value = "교육 담당자\r\nPhụ trách đào tạo";
                    ws.Cells["I4:I5"].Merge = true;
                    ws.Cells["J4:J5"].Value = "교육예산\r\nDự toán chi phí (VNĐ)";
                    ws.Cells["J4:J5"].Merge = true;

                    ws.Cells["K4:O4"].Value = "연간 교육 시간\r\nKế hoạch thời gian đào tạo 1 năm";
                    ws.Cells["K4:O4"].Merge = true;
                    ws.Cells["K5:K5"].Value = "주기\r\nChu kỳ";
                    ws.Cells["L5:L5"].Value = "횟수\r\nSố lần";
                    ws.Cells["M5:M5"].Value = "횟수당 시간\r\nThời lượng 1 lần";
                    ws.Cells["N5:N5"].Value = "교육 인원\r\nSố người được đào tạo";
                    ws.Cells["O5:O5"].Value = "총 교육 시간\r\nKế hoạch thời lượng đào tạo";

                    ws.Cells["P4:AA4"].Value = "년 월별 교육훈련 계획\r\nKế hoạch trong năm";
                    ws.Cells["P4:AA4"].Merge = true;
                    ws.Cells["P5:P5"].Value = "1월";
                    ws.Cells["Q5:Q5"].Value = "2월";
                    ws.Cells["R5:R5"].Value = "3월";
                    ws.Cells["S5:S5"].Value = "4월";
                    ws.Cells["T5:T5"].Value = "5월";
                    ws.Cells["U5:U5"].Value = "6월";
                    ws.Cells["V5:V5"].Value = "7월";
                    ws.Cells["W5:W5"].Value = "8월";
                    ws.Cells["X5:X5"].Value = "9월";
                    ws.Cells["Y5:Y5"].Value = "10월";
                    ws.Cells["Z5:Z5"].Value = "11월";
                    ws.Cells["AA5:AA5"].Value = "12월";
                    ws.Cells["AB5:AB5"].Value = "Ghi chú";


                    int index = 5;
                    int stt = 0;

                    foreach (var item in list_Excell)
                    {
                        if (item.PL1 != "")
                        {
                            index++;
                            stt++;
                            //--
                            string strCell1 = "A" + index.ToString() + ":" + "A" + index.ToString();
                            ws.Cells[strCell1].Value = stt;
                            //--
                            string strCell2 = "B" + index.ToString() + ":" + "B" + index.ToString();
                            ws.Cells[strCell2].Value = item.PL1;
                            //--
                            string strCell3 = "C" + index.ToString() + ":" + "C" + index.ToString();
                            ws.Cells[strCell3].Value = item.PL2;
                            //--
                            string strCell4 = "D" + index.ToString() + ":" + "D" + index.ToString();
                            ws.Cells[strCell4].Value = item.TrainName;
                            //--
                            string strCell5 = "E" + index.ToString() + ":" + "E" + index.ToString();
                            ws.Cells[strCell5].Value = item.QDLP;
                            //--
                            string strCell6 = "F" + index.ToString() + ":" + "F" + index.ToString();
                            ws.Cells[strCell6].Value = item.TrainPos;
                            //--
                            string strCell7 = "G" + index.ToString() + ":" + "G" + index.ToString();
                            ws.Cells[strCell7].Value = item.PTDT;
                            //--
                            string strCell8 = "H" + index.ToString() + ":" + "H" + index.ToString();
                            ws.Cells[strCell8].Value = item.TrainLocation;

                            string strCell9 = "I" + index.ToString() + ":" + "I" + index.ToString();
                            ws.Cells[strCell9].Value = item.ChargeTrain;
                            //--
                            string strCell10 = "J" + index.ToString() + ":" + "J" + index.ToString();
                            ws.Cells[strCell10].Value = item.TrainPrice;
                            //--
                            string strCell11 = "K" + index.ToString() + ":" + "K" + index.ToString();
                            ws.Cells[strCell11].Value = item.Cycle;
                            //--
                            string strCell12 = "L" + index.ToString() + ":" + "L" + index.ToString();
                            ws.Cells[strCell12].Value = item.Times;
                            //--
                            string strCell13 = "M" + index.ToString() + ":" + "M" + index.ToString();
                            ws.Cells[strCell13].Value = item.Duration1time;
                            //--
                            string strCell14 = "N" + index.ToString() + ":" + "N" + index.ToString();
                            ws.Cells[strCell14].Value = item.Numper;
                            //--
                            string strCell15 = "O" + index.ToString() + ":" + "O" + index.ToString();
                            ws.Cells[strCell15].Value = item.PlanTraning;
                            //--
                            string strCell16 = "P" + index.ToString() + ":" + "P" + index.ToString();
                            ws.Cells[strCell16].Value = item.Month1;
                            string strCell17 = "Q" + index.ToString() + ":" + "Q" + index.ToString();
                            ws.Cells[strCell17].Value = item.Month2;
                            //--
                            string strCell18 = "R" + index.ToString() + ":" + "R" + index.ToString();
                            ws.Cells[strCell18].Value = item.Month3;
                            //--
                            string strCell19 = "S" + index.ToString() + ":" + "S" + index.ToString();
                            ws.Cells[strCell19].Value = item.Month4;
                            //--
                            string strCell20 = "T" + index.ToString() + ":" + "T" + index.ToString();
                            ws.Cells[strCell20].Value = item.Month5;
                            //--
                            string strCell21 = "U" + index.ToString() + ":" + "U" + index.ToString();
                            ws.Cells[strCell21].Value = item.Month6;
                            //--
                            string strCell22 = "V" + index.ToString() + ":" + "V" + index.ToString();
                            ws.Cells[strCell22].Value = item.Month7;
                            //--
                            string strCell23 = "W" + index.ToString() + ":" + "W" + index.ToString();
                            ws.Cells[strCell23].Value = item.Month8;
                            //--
                            string strCell24 = "X" + index.ToString() + ":" + "X" + index.ToString();
                            ws.Cells[strCell24].Value = item.Month9;
                            //--
                            string strCell25 = "Y" + index.ToString() + ":" + "Y" + index.ToString();
                            ws.Cells[strCell25].Value = item.Month10;
                            //--
                            string strCell26 = "Z" + index.ToString() + ":" + "Z" + index.ToString();
                            ws.Cells[strCell26].Value = item.Month11;
                            //--
                            string strCell27 = "AA" + index.ToString() + ":" + "AA" + index.ToString();
                            ws.Cells[strCell27].Value = item.Month12;
                            //--
                            string strCell28 = "AB" + index.ToString() + ":" + "AB" + index.ToString();
                            ws.Cells[strCell28].Value = item.Remark;
                        }
                    }

                    ws.PrinterSettings.PaperSize = ePaperSize.A4;
                    ws.PrinterSettings.Orientation = eOrientation.Landscape;
                    ws.PrinterSettings.FitToPage = true;
                    ws.Cells["A4:AB5"].AutoFilter = true;
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

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            GetListEmpTraining();
            GetEduInfo();
            GetPlanEduNow();
            GetPlanEduAfter();
        }

        private void cbbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
