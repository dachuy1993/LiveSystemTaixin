using LiveCharts;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveSystem.DAO;
using LiveSystem.ViewModel;
using System.Windows.Threading;

namespace LiveSystem
{
    public partial class Page_Main:Page
    {
        #region Khai báo biến
       
        public static string path_Ksystem20 = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";
        public static string path_TaixinWeb = "server=192.168.2.40;Port=3307;user id=txadmin;database=LiveSystem;password=Taixinweb1!;Convert Zero Datetime=true"; // Add Convert Zero Datetime - fix error "Unable to convert MySQL date/time value to System.DateTime"
        public static string path_TaixinAccessManager = "Data Source=192.168.2.5\\SQLEXPRESS;Initial Catalog=NitgenAccessManager;Persist Security Info=True;User ID=sa;Password=123456a@";
        public static string path_TaixinAccessManager_1 = "Data Source=192.168.2.20\\SQLEXPRESS;Initial Catalog=NitgenAccessManager;Persist Security Info=True;User ID=sa;Password=123456a@";
        public static string path_Ksystem25 = "Data Source=192.168.2.5;Initial Catalog=LiveSystem;Persist Security Info=True;User ID=sa;Password= oneuser1!";

        bool checkWorking = false;
        public static string dateCheck = DateTime.Now.ToString("yyyyMMdd");
        public static string shiftCheck = "Ca ngày";
        public int s = 0;


        //Temp
        public static string Depatmen_Code = "";
        #endregion

        DataTable listWorkingRate = new DataTable();
        DataTable listEduInfo = new DataTable();


        public Page_Main()
        {
            InitializeComponent();
            dpk_Check.SelectedDate = DateTime.Now;

            Loaded += Page_Main_Loaded;

            // Change Language
            
        }


        private async void Page_Main_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MainWindow.language);
            ApplyLanguage(MainWindow.language);

            
            GetWorkingRate();
            

            
            // Tỷ lệ phép năm

            timer1_Tick();

            //Thông tin đào tạo

        }

        /*New Code - THANGDN*/
        // Hiển thị bảng tỷ lệ đi làm
        private async void GetWorkingRate()
        {

            try
            {
                string query = "SPGetDataRateWorkMain @shift , @date";
                //string query = "SELECT * from tmmwrate where Shift = @shift and Insdt = @date";

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

                // Lấy dữ liệu và hiển thị
                await Task.Run(() =>
                {
                    
                        // Tỷ lệ đi làm
                        if (dateCheck.Count() != 8)
                            dateCheck = DateTime.Parse(dateCheck).ToString("yyyyMMdd");
                        listWorkingRate = DataProvider.Instance.ExecuteSP(path_Ksystem20, query, new object[] { shiftCheck, dateCheck });
                        foreach (DataRow row in listWorkingRate.Rows)
                        {
                            row["Rate"] = row["Rate"] + "%";
                        }
                       this.Dispatcher.Invoke(() =>
                    { 
                        GetVacationLeaveRate();
                        // Tỷ lệ tăng ca
                        GetOverTimeRate();
                        // Thông tin suất ăn
                        GetVSIPMeal();
                        VaccineInfo();
                        EmpInfoUpdateStatus();
                        GetCarInfo();
                        ScheduleInfo();
                        GetEduInfo();
                        Db_Read_Room();
                        Db_Read_Team();

                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);

                });
                lvWorkingRate.ItemsSource = listWorkingRate.DefaultView;

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
                MessageBox.Show("Error when processing data going to work", "Error", MessageBoxButton.OK);
            }
            
        }

        // Hiển thị bảng quản lý đào tạo
        private async void GetEduInfo()
        {
            try
            {
                //string query = "SELECT * from tmmwrate where Shift = @shift and Insdt = @date";
                


                // Lấy dữ liệu và hiển thị
                
                await Task.Run(() =>
                {
                    string query = "SPGetDateTrainingMain @date";
                    if (dateCheck.Count() != 8)
                        dateCheck = DateTime.Parse(dateCheck).ToString("yyyyMMdd");


                    listEduInfo = DataProvider.Instance.ExecuteSP(path_Ksystem20, query, new object[] { dateCheck });
                });
                lvEdu.ItemsSource = listEduInfo.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing training data", "Error", MessageBoxButton.OK);
            }
            

            
        }

        // Hiển thị bảng tỷ lệ phép năm
        private void GetVacationLeaveRate()
        {
            try
            {
                string query1 = "SPGetDataHolidayMain @date";

                var listEmp = DataProvider.Instance.ExecuteSP(path_Ksystem20, query1, new object[] { dateCheck });


                // Hiển thị danh sách lên view
                lvPhepNam.ClearValue(ListView.ItemsSourceProperty);
                lvPhepNam.ItemsSource = listEmp.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while processing annual leave data", "Error", MessageBoxButton.OK);
            }
            // Lấy dữ liệu phép của toàn bộ nhân viên LiveSystem
            //string query1 = "select * from tmmhyUpdate";
            //var listEmpVacationLeave = DataProvider.Instance.executeQuery(path_Ksystem25, query1);
            // Lấy dữ liệu nhân viên thực tế từ Ksystem theo phòng ban
            //string query2 = "SPGetDataHolidayMain";

            

            
        }

        // Tỷ lệ tăng ca
        private void GetOverTimeRate()
        {
            try
            {
                if (dateCheck.Count() != 8)
                    dateCheck = DateTime.Parse(dateCheck).ToString("yyyyMMdd");
                string query = "SPGetDataOverTimeMain @date";
                var listOTRate = DataProvider.Instance.executeQuery(path_Ksystem20, query, new object[] { dateCheck });

                lvOverTime.ItemsSource = listOTRate.DefaultView;
                lvOverTime3.ItemsSource = listOTRate.DefaultView;
                lvOverTime4.ItemsSource = listOTRate.DefaultView;
            }
            catch (Exception)
            {
                MessageBox.Show("Error when processing overtime data", "Error", MessageBoxButton.OK);
            }
            
        }

        // Thông tin suất ăn VSIP
        private void GetVSIPMeal()
        {
            try
            {
                string query = "SPGetDateFoodMain @date";
                var listVSIPMeal = DataProvider.Instance.ExecuteSP(path_TaixinAccessManager, query, new object[] { dateCheck });
                lvVSIPMeal.ItemsSource = listVSIPMeal.DefaultView;

            }
            catch (Exception)
            {
                MessageBox.Show("Error processing meal data", "Error", MessageBoxButton.OK);
            }
            
        }

        // Thông tin cư trú
        private void GetEmpInfo()
        {
            try
            {
                // Lấy dữ liệu thông tin nhân viên
                string query1 = "select * from update_employee where TempProv = N'Tỉnh Bắc Ninh'";
                var listEmpInformation = DataProvider.Instance.MySqlExecuteQuery(path_TaixinWeb, query1);
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
                //var _qtyCity = new ChartValues<double>();
                //var _nameCity = new ChartValues<string>();
                //DataChart.Values3 = _qtyCity;

                //listAddressInfo.ForEach(x =>
                //{
                //    int len = x.Dist.Length;
                //    if (x.Dist.Contains("Thành phố"))
                //    {
                //        _nameCity.Add(x.Dist.Substring(10, len - 10));
                //    }
                //    if (x.Dist.Contains("Quận"))
                //    {
                //        _nameCity.Add(x.Dist.Substring(5, len - 5));
                //    }
                //    if (x.Dist.Contains("Thị xã"))
                //    {
                //        _nameCity.Add(x.Dist.Substring(7, len - 7));
                //    }
                //    if (x.Dist.Contains("Huyện"))
                //    {
                //        _nameCity.Add(x.Dist.Substring(6, len - 6));
                //    }
                //    _qtyCity.Add(x.Qty);
                //});

                //if (MainWindow.language == "vi-VN")
                //{
                //    DataChart.Title = "Số người";
                //}
                //else
                //{
                //    DataChart.Title = "수량";
                //}

                //DataChart.Labels = _nameCity;
                //DataChart.YFormatter = _qtyCity;
                //DataChart.Step = 200;
                //DataContext = this;
                //Column column = new Column();
                //frameChart_Huyen.Navigate(column);
            }
            catch (Exception)
            {
                MessageBox.Show("Error processing residence information data", "Error", MessageBoxButton.OK);
            }
            
        }
        public void ScheduleInfo()
        {
            try
            {
                if (dateCheck.Count() != 8)
                    dateCheck = DateTime.Parse(dateCheck).ToString("yyyyMMdd");
                string query = "SPGetDateScheduleMain @date";
                //var listVSIPMeal = DataProvider.Instance.executeQuery(path_Ksystem25, query, new object[] { dateCheck });
                var listScheduleInfo = DataProvider.Instance.ExecuteScalar(path_Ksystem25, query, new object[] { dateCheck });
                //lvLichTrinh.ItemsSource = listScheduleInfo.DefaultView;
                lb_Note.Text = listScheduleInfo.ToString();
            }
            catch (Exception)
            {

                MessageBox.Show("Error processing schedule data", "Error", MessageBoxButton.OK);
            }
            

        }


        // Lấy dữ liệu tiêm Vaccine
        private void VaccineInfo()
        {
            try
            {
                // Lấy dữ liệu nhân viên thực tế từ Ksystem
                string query2 = "SELECT * FROM TDAEmpMaster where RetDate>= @date and len(EmpId) > 4 and len(EmpId) < 8";
                var listEmp = DataProvider.Instance.executeQuery(path_Ksystem20, query2, new object[] { dateCheck });

                // Lấy dữ liệu số mũi vaccine
                string query = "select * from vacxin";
                var listAllEmpVaccine = DataProvider.Instance.MySqlExecuteQuery(path_TaixinWeb, query);

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

        // Employee Information Update Status
        private void EmpInfoUpdateStatus()
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
                lb_UpdateDiaChi_OK.Content = EmpInfoUpdateStatusOK;
                lb_UpdateDiaChi_NG.Content = EmpInfoUpdateStatusNG;
                lb_Total.Content = listEmp.Rows.Count;
            }
            catch (Exception)
            {
                MessageBox.Show("Error processing address update data", "Error", MessageBoxButton.OK);
            }
            
        }

        // Lấy thông tin đặt xe
        private void GetCarInfo()
        {
            try
            {
                string date1 = DateTime.Now.ToString("yyyy-MM-dd");
                string dateKM1;
                string dateKM2;
                if (DateTime.Now.Month == 1)
                {
                    if (int.Parse(DateTime.Now.Day.ToString()) <= 25)
                    {
                        dateKM1 = DateTime.Now.AddYears(-1).ToString("yyyy") + "-12-26";
                        dateKM2 = DateTime.Now.ToString("yyyy-MM") + "-25";

                    }
                    else
                    {
                        dateKM1 = DateTime.Now.AddYears(-1).ToString("yyyy") + "-12-26";
                        dateKM2 = DateTime.Now.AddYears(-1).ToString("yyyy") + "-12-31";
                    }
                }
                else
                {
                    if (int.Parse(DateTime.Now.Day.ToString()) <= 25)
                    {
                        dateKM1 = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.AddMonths(-1).ToString("MM") + "-26";
                        dateKM2 = DateTime.Now.ToString("yyyy-MM") + "-25";
                    }
                    else
                    {
                        dateKM1 = DateTime.Now.ToString("yyyy-MM") + "-26";
                        dateKM2 = DateTime.Now.ToString("yyyy-MM") + "-31";
                    }
                }

                string query = "Call SPGetListCarForDate( @date1 , @dateKM1 , @dateKM2 )";
                var listCar = DataProvider.Instance.MySqlExecuteQuery(path_TaixinWeb, query, new object[] { date1, dateKM1, dateKM2 });
                listCar.Columns.Add("Color", typeof(string));
                foreach (DataRow row in listCar.Rows)
                {
                    if (row["Status"].ToString() == "Finish")
                    {
                        row["Color"] = "DodgerBlue";
                    }
                    else
                    {
                        row["Color"] = "Red";
                    }

                    if (row["Destination"].ToString() == "")
                    {
                        row["Destination"] = row["OtherDestination"];
                    }

                    if (row["User"].ToString() == "")
                    {
                        row["User"] = row["OtherOrderer"];
                    }
                }

                lvCar.ItemsSource = listCar.DefaultView;
            }
            catch (Exception)
            {
                MessageBox.Show("Error when processing car data", "Error", MessageBoxButton.OK);
            }
            
        }



        
        /*Old Code*/
        /*===============================================================================================================================================*/

        

        private void timer1_Tick()
        {
            DispatcherTimer DispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            DispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            DispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            DispatcherTimer.Start();
            //++s;
            //if(s == 60)
            //{
            //    Loaded += Page_Main_Loaded;
            //    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MainWindow.language);
            //    ApplyLanguage(MainWindow.language);
            //}
            //string kq = "";
            //kq = s.ToString();
            //lb.Content = kq;



        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            lb.Content = DateTime.Now.Second;
            if(DateTime.Now.Second == 59)
            {
                GetCarInfo();
                //GetWorkingRate();
            }    


            CommandManager.InvalidateRequerySuggested();
        }

        // Thay đổi ngôn ngữ
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

        // Lấy danh sách phòng ban và nhóm - sử dụng tại các page Tỷ lệ đi làm, Tỷ lệ ăn cơm, Tỷ lệ phép năm, Tỷ lệ tăng ca
        public static List<Helper_Employee> listRoom = new List<Helper_Employee>();
        public static List<Helper_Employee> listTeam = new List<Helper_Employee>();

       

        public void Db_Read_Room()
        {
            //try
            //{
            //    listRoom.Clear();
            //    //listRoom.Add(new Helper_Employee { EmpId = "0", EmpNm = "ALL", cmpcode = "0" });
            //    using (MySqlConnection conn = new MySqlConnection(path_TaixinWeb))
            //    {
            //        conn.Open();
            //        using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM phongban ", conn))
            //        {
            //            using (MySqlDataReader dr = cmd.ExecuteReader())
            //            {
            //                while (dr.Read())
            //                {
            //                    if (dr[0] != null)
            //                    {
            //                        Helper_Employee item = new Helper_Employee();
            //                        item.EmpId = dr[0].ToString();
            //                        item.EmpNm = dr[1].ToString();
            //                        item.cmpcode = dr[2].ToString();
            //                        listRoom.Add(item);
            //                    }
            //                }
            //            }
            //        }
            //        conn.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            try
            {
                listRoom.Clear();
                string query = "SPGetDataCbbDept @date";
                //var listVSIPMeal = DataProvider.Instance.executeQuery(path_Ksystem25, query, new object[] { dateCheck });
                var listCbbDept = DataProvider.Instance.executeQuery(path_Ksystem20, query, new object[] { dateCheck });
                foreach (DataRow row in listCbbDept.Rows)
                {
                    Helper_Employee item = new Helper_Employee();
                    item.EmpId = row[0].ToString();
                    item.EmpNm = row[1].ToString();
                    item.cmpcode = row[2].ToString();
                    listRoom.Add(item);
                }
            }
            catch (Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Db_Read_Team()
        {
            //try
            //{
            //    listTeam.Clear();
            //    using (MySqlConnection conn = new MySqlConnection(path_TaixinWeb))
            //    {
            //        conn.Open();
            //        using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM nhom ", conn))
            //        {
            //            using (MySqlDataReader dr = cmd.ExecuteReader())
            //            {
            //                while (dr.Read())
            //                {
            //                    if (dr[0] != null)
            //                    {
            //                        Helper_Employee item = new Helper_Employee();
            //                        item.EmpId = dr[0].ToString();
            //                        item.EmpNm = dr[1].ToString();
            //                        item.cmpcode = dr[2].ToString();
            //                        listTeam.Add(item);
            //                    }
            //                }
            //            }
            //        }
            //        conn.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            try
            {
                string query = "SPGetDataCbbDeptGroup @date";
                //var listVSIPMeal = DataProvider.Instance.executeQuery(path_Ksystem25, query, new object[] { dateCheck });
                var listCbbGroup = DataProvider.Instance.executeQuery(path_Ksystem20, query, new object[] { dateCheck });
                foreach (DataRow row in listCbbGroup.Rows)
                {
                    Helper_Employee item = new Helper_Employee();
                    item.EmpId = row[0].ToString();
                    item.EmpNm = row[1].ToString();
                    item.cmpcode = row[2].ToString();
                    listTeam.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Định dạng ngày của dpk_Check thành yyyy-MM-dd và gán cho datecheck
        private void dpk_Check_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {           
            var setting1 = new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd" };           
            var dt1 = JsonConvert.SerializeObject(DateTime.Parse(dpk_Check.SelectedDate.ToString()).ToString("yyyy-MM-dd"), setting1);
            dateCheck = dt1.Substring(1, dt1.Length - 2);
            lb_Month.Content = DateTime.Parse(dateCheck).ToString("MMMM").ToUpper();
        }        

        // Gán giá trị cho shiftCheck
        private void rb_ShiftA_Checked(object sender, RoutedEventArgs e)
        {
            shiftCheck = "Ca ngày";
        }
        private void rb_ShiftB_Checked(object sender, RoutedEventArgs e)
        {
            shiftCheck = "Ca đêm";
        }
        private void rb_ShiftAll_Checked(object sender, RoutedEventArgs e)
        {
            shiftCheck = "Tất cả";
        }
        
        // Event click button Tìm kiếm
        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            if (checkWorking == false && MainWindow._checkInternet== "Success")
            {
                GetWorkingRate();
                //GetVacationLeaveRate();
                //GetVSIPMeal();
                //GetCarInfo();
                
                //ScheduleInfo();
                //GetOverTimeRate();
                ////Thông tin đào tạo
                //GetEduInfo();

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

        // Chuyển page khi nhấn vào header của gridview
        private void GridViewColumnHeader_Work(object sender, RoutedEventArgs e)
        {
            gridMain1.Visibility = Visibility.Hidden;
            gridMain2.Visibility = Visibility.Hidden;
            stackData.Visibility = Visibility.Visible;
            frameData.Visibility = Visibility.Visible;
            frameData.Navigate(MainWindow.work);
            foreach (var item in MainWindow.ListButton_Header)
            {
                if (MainWindow.language == "vi-VN")
                {
                    if (item.ContentButton == "Trang chủ")
                    {
                        item.BackGroundColor = PinValue.OFF;
                    }
                    if (item.ContentButton == "Tỷ lệ đi làm")
                    {
                        item.BackGroundColor = PinValue.ON;
                    }
                }
                else
                {
                    if (item.ContentButton == "메인")
                    {
                        item.BackGroundColor = PinValue.OFF;
                    }
                    if (item.ContentButton == "출근율")
                    {
                        item.BackGroundColor = PinValue.ON;
                    }
                }
            }
        }

        private void GridViewColumnHeader_Food(object sender, RoutedEventArgs e)
        {
            gridMain1.Visibility = Visibility.Hidden;
            gridMain2.Visibility = Visibility.Hidden;
            stackData.Visibility = Visibility.Visible;
            frameData.Visibility = Visibility.Visible;
            frameData.Navigate(MainWindow.Food);
            foreach (var item in MainWindow.ListButton_Header)
            {
                if (MainWindow.language == "vi-VN")
                {
                    if (item.ContentButton == "Trang chủ")
                    {
                        item.BackGroundColor = PinValue.OFF;
                    }
                    if (item.ContentButton == "Tỷ lệ ăn cơm VSIP")
                    {
                        item.BackGroundColor = PinValue.ON;
                    }
                }
                else
                {
                    if (item.ContentButton == "메인")
                    {
                        item.BackGroundColor = PinValue.OFF;
                    }
                    if (item.ContentButton == "VSIP 식수현황")
                    {
                        item.BackGroundColor = PinValue.ON;
                    }
                }
            }
        }


        private void GridViewColumnHeader_Edu(object sender, RoutedEventArgs e)
        {
            gridMain1.Visibility = Visibility.Hidden;
            gridMain2.Visibility = Visibility.Hidden;
            stackData.Visibility = Visibility.Visible;
            frameData.Visibility = Visibility.Visible;
            frameData.Navigate(MainWindow.Training);
            foreach (var item in MainWindow.ListButton_Header)
            {
                if (MainWindow.language == "vi-VN")
                {
                    if (item.ContentButton == "Trang chủ")
                    {
                        item.BackGroundColor = PinValue.OFF;
                    }
                    if (item.ContentButton == "Đào tạo")
                    {
                        item.BackGroundColor = PinValue.ON;
                    }
                }
                else
                {
                    if (item.ContentButton == "메인")
                    {
                        item.BackGroundColor = PinValue.OFF;
                    }
                    if (item.ContentButton == "교육")
                    {
                        item.BackGroundColor = PinValue.ON;
                    }
                }
            }
        }

        private void GridViewColumnHeader_Holiday(object sender, RoutedEventArgs e)
        {
            gridMain1.Visibility = Visibility.Hidden;
            gridMain2.Visibility = Visibility.Hidden;
            Page_Holiday holiday = new Page_Holiday();
            stackData.Visibility = Visibility.Visible;
            frameData.Visibility = Visibility.Visible;
            frameData.Navigate(holiday);
            foreach (var item in MainWindow.ListButton_Header)
            {
                if (MainWindow.language == "vi-VN")
                {
                    if (item.ContentButton == "Trang chủ")
                    {
                        item.BackGroundColor = PinValue.OFF;
                    }
                    if (item.ContentButton == "Tỷ lệ phép năm")
                    {
                        item.BackGroundColor = PinValue.ON;
                    }
                }
                else
                {
                    if (item.ContentButton == "메인")
                    {
                        item.BackGroundColor = PinValue.OFF;
                    }
                    if (item.ContentButton == "연차사용현황")
                    {
                        item.BackGroundColor = PinValue.ON;
                    }
                }
            }
        }

        private void GridViewColumnHeader_Overtime(object sender, RoutedEventArgs e)
        {

        }
    }

    // Chart
    public class DataChart
    {
        public static ChartValues<int> Values1 { get; set; }
        public static ChartValues<int> Values2 { get; set; }
        public static ChartValues<double> Values3 { get; set; }
        public static ChartValues<string> Labels { get; set; }
        public static ChartValues<double> YFormatter { get; set; }
        public static int Step { get; set; }
        public static string Title { get; set; }
        //public static ChartValues<ObservableValue> gt1 { get; set; }
        //public static ObservableValue gt2 { get; set; }
        //public static int gt3 { get; set; }
        //public static int gt4 { get; set; }
    }  
   
}
