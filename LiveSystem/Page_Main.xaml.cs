using LiveCharts;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using OfficeOpenXml;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
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
using OfficeOpenXml.FormulaParsing.Utilities;
using LiveSystem.Model;

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
        //public static string path_TaixinYP = "Data Source=113.160.208.231,1433;Initial Catalog=ChamCom;Persist Security Info=True;User ID=WiseEyeOn39;Password= cNca@123#!";
        //public static string path_Taixin = "Data Source=192.168.2.10;Initial Catalog=taixin_HR;Persist Security Info=True;User ID=sa;Password=oneuser1!";


        bool checkWorking = false;
        public static string dateCheck = DateTime.Now.ToString("yyyyMMdd");
        public static string shiftCheck = "Ca ngày";
        public int s = 0;
        public int NumSafe;


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
                string query = "SPGetDataRateWorkMainM @date";
                //string query = "SELECT * from tmmwrate where Shift = @shift and Insdt = @date";
                Page_LoadingData page_Loading = new Page_LoadingData();
                stackLoading.Visibility = Visibility.Visible;
                frameLoading.Navigate(page_Loading);
                checkWorking = true;
                if (dateCheck.Count() != 8)
                    dateCheck = DateTime.Parse(dateCheck).ToString("yyyyMMdd");
                // Hiển thị Page_LoadingData
                await Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        //Page_LoadingData page_Loading = new Page_LoadingData();
                        stackLoading.Visibility = Visibility.Visible;
                        frameLoading.Navigate(page_Loading);
                        checkWorking = true;
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);

                });

                // Lấy dữ liệu và hiển thị
                await Task.Run(() =>
                {
                    
                        // Tỷ lệ đi làm
                        
                        listWorkingRate = DataProvider.Instance.ExecuteSP(path_Ksystem20, query, new object[] { dateCheck });
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
                        // Tỷ lệ tuyển dụng
                        GetRecruitmentRate();
                        GetDataSafe();


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

                //Đóng Page_LoadingData
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
                
                string query = "SPGetDataOverTimeMainM @date";
                var listOTRate = DataProvider.Instance.executeQuery(path_Ksystem20, query, new object[] { dateCheck });

                lvOverTime.ItemsSource = listOTRate.DefaultView;
            }
            catch (Exception)
            {
                MessageBox.Show("Error when processing overtime data", "Error", MessageBoxButton.OK);
            }
            
        }

        // Tỷ lệ tuyển dụng
        private void GetRecruitmentRate()
        {
            try
            {
                
                string query = "SPGetDataRecruitmentMain @date";
                var listOTRate = DataProvider.Instance.executeQuery(path_Ksystem20, query, new object[] { dateCheck });

                

                lvRecruitment.ItemsSource = listOTRate.DefaultView;
            }
            catch (Exception)
            {
                MessageBox.Show("Error when processing recruitment data", "Error", MessageBoxButton.OK);
            }

        }

        private void GetDataSafe()
        {
            try
            {
                
                // Lấy dữ liệu nhân viên thực tế từ Ksystem
                string query2 = "SPGetDataSafe @date";

                var listEmp = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query2, new object[] { Page_Main.dateCheck });
                
                foreach (DataRow row in listEmp.Rows)
                {
                     NumSafe = int.Parse(row["NumSafe"].ToString());
                }
                lbNumTN.Content = NumSafe;


            }
            catch (Exception)
            {
                MessageBox.Show("Error in processing safe data", "Error", MessageBoxButton.OK);
            }

        }


        // Thông tin suất ăn VSIP
        private void GetVSIPMeal()
        {
            try
            {
                
                string query = "SPGetDateFoodMainM @date";
                var listVSIPMeal = DataProvider.Instance.ExecuteSP(path_TaixinAccessManager, query, new object[] { dateCheck });

                string query1 = "SPGetDataRateWorkMainFoodM @date";
                var listEmpWork = DataProvider.Instance.ExecuteSP(path_Ksystem20,query1, new object[] { dateCheck });

                string queryYP = "SPGetDateFoodMainYenPhong @date";
                //var listYPMealYenPhong = DataProvider.Instance.ExecuteSP(path_TaixinYP, queryYP, new object[] { dateCheck });
                int SangTx = 0;
                int TruaTx = 0;
                int ChieuTx = 0;
                int DemTx = 0;

                int SangWYP = 0;
                int TruaWYP = 0;
                int ChieuWYP = 0;
                int DemWYP = 0;

                int SangFYP = 0;
                int TruaFYP = 0;
                int ChieuFYP = 0;
                int DemFYP = 0;


                foreach ( DataRow item in listEmpWork.Rows)
                {
                    if (item["ID"].ToString() == "1")
                    {
                        SangTx = int.Parse(item["EmpNum"].ToString());
                    }
                    else if(item["ID"].ToString() == "2")
                    {
                        SangWYP = int.Parse(item["EmpNum"].ToString());
                    }
                    else if (item["ID"].ToString() == "3")
                    {
                        TruaTx = int.Parse(item["EmpNum"].ToString());
                    }
                    else if (item["ID"].ToString() == "4")
                    {
                        TruaWYP = int.Parse(item["EmpNum"].ToString());
                    }
                    else if (item["ID"].ToString() == "5")
                    {
                        ChieuTx = int.Parse(item["EmpNum"].ToString());
                    }
                    else if (item["ID"].ToString() == "6")
                    {
                        ChieuWYP = int.Parse(item["EmpNum"].ToString());
                    }
                    else if (item["ID"].ToString() == "7")
                    {
                        DemTx = int.Parse(item["EmpNum"].ToString());
                    }
                    else if (item["ID"].ToString() == "8")
                    {
                        DemWYP = int.Parse(item["EmpNum"].ToString());
                    }
                }

                foreach (DataRow item  in listVSIPMeal.Rows)
                {
                    if (item["ID"].ToString() == "1")
                    {
                        item["EmpNum"] = SangTx;
                    }
                    else if (item["ID"].ToString() == "2")
                    {
                        item["EmpNum"] = SangWYP;
                    }
                    else if (item["ID"].ToString() == "3")
                    {
                        item["EmpNum"] = TruaTx;
                    }
                    else if (item["ID"].ToString() == "4")
                    {
                        item["EmpNum"] = TruaWYP;
                    }
                    else if (item["ID"].ToString() == "5")
                    {
                        item["EmpNum"] = ChieuTx;
                    }
                    else if (item["ID"].ToString() == "6")
                    {
                        item["EmpNum"] = ChieuWYP;
                    }
                    else if (item["ID"].ToString() == "7")
                    {
                        item["EmpNum"] = DemTx;
                    }
                    else if (item["ID"].ToString() == "8")
                    {
                        item["EmpNum"] = DemWYP;
                    }

                }

                foreach (DataRow item in listVSIPMeal.Rows)
                {
                    if(int.Parse(item["EmpNum"].ToString()) == 0)
                    {
                        item["Rate"] = "0";
                    }   
                    else
                    {
                        item["Rate"] = int.Parse(item["EmpFood"].ToString()) * 100 / int.Parse(item["EmpNum"].ToString());
                    }    
                    

                }
                foreach (DataRow item in listVSIPMeal.Rows)
                {
                    item["Rate"] = item["Rate"].ToString() + "%";
                }



                    DataView dv = listVSIPMeal.DefaultView;
                dv.Sort = "ID ASC";


                lvVSIPMeal.ItemsSource = dv;
                
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
                //lvCar.ItemsSource = null;
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

                string queryV2 = "Call SPGetListCarForDateV2( @date1 , @dateKM1 , @dateKM2 )";

                var listCarV2 = DataProvider.Instance.MySqlExecuteQuery(path_TaixinWeb, queryV2, new object[] { date1, dateKM1, dateKM2 });
                List<ListCarModel> listAll = new List<ListCarModel>();
                listCar.Columns.Add("Color", typeof(string));
                listCarV2.Columns.Add("Color", typeof(string));
                foreach (DataRow row in listCar.Rows)
                {
                    if (row["Status"].ToString() == "Finish")
                    {
                        row["Color"] = "DodgerBlue";
                        row["Destination"] = "";
                        row["OtherDestination"] = "";
                    }
                    else if (row["Status"].ToString() == "Order")
                    {
                        row["Color"] = "Yellow";
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

                foreach (DataRow row in listCarV2.Rows)
                {
                    if (row["Status"].ToString() == "Finish")
                    {
                        row["Color"] = "DodgerBlue";
                        row["Destination"] = "";
                        row["OtherDestination"] = "";
                    }
                    else if (row["Status"].ToString() == "Order")
                    {
                        row["Color"] = "Yellow";
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

                 foreach (DataRow row in listCar.Rows)
                {
                    ListCarModel carModel = new ListCarModel();
                    carModel.CarType = row["Cartype"].ToString();
                    carModel.EmpNm = row["EmpNm"].ToString();
                    carModel.EmpTel = row["EmpTel"].ToString();
                    carModel.Status = row["Status"].ToString();
                    carModel.KMMonth = int.Parse(row["KMMonth"].ToString());
                    carModel.Quota = int.Parse(row["Quota"].ToString());
                    carModel.Remain = int.Parse(row["Remain"].ToString());
                    carModel.Destination = row["Destination"].ToString();
                    carModel.OtherDestination = row["OtherDestination"].ToString();
                    carModel.User = row["User"].ToString();
                    carModel.OtherOrderer = row["OtherOrderer"].ToString();
                    carModel.Color = row["Color"].ToString();
                    carModel.ID = int.Parse(row["ID"].ToString());
                    carModel.Order1 = row["Order1"].ToString();
                    listAll.Add(carModel);

                }

                foreach (DataRow row in listCarV2.Rows)
                {
                    ListCarModel carModel = new ListCarModel();
                    carModel.CarType = row["Cartype"].ToString();
                    carModel.EmpNm = row["EmpNm"].ToString();
                    carModel.EmpTel = row["EmpTel"].ToString();
                    carModel.Status = row["Status"].ToString();
                    carModel.KMMonth = int.Parse(row["KMMonth"].ToString());
                    carModel.Quota = int.Parse(row["Quota"].ToString());
                    carModel.Remain = int.Parse(row["Remain"].ToString());
                    carModel.Destination = row["Destination"].ToString();
                    carModel.OtherDestination = row["OtherDestination"].ToString();
                    carModel.User = row["User"].ToString();
                    carModel.OtherOrderer = row["OtherOrderer"].ToString();
                    carModel.Color = row["Color"].ToString();
                    carModel.ID = int.Parse(row["ID"].ToString());
                    carModel.Order1 = row["Order1"].ToString();
                    listAll.Add(carModel);

                }

                

                lvCar.ItemsSource = listAll.OrderBy(x => x.ID);
                
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
            //lb.Content = DateTime.Now.Second;
            if(DateTime.Now.Second == 59)
            {
                GetCarInfo();
                //GetWorkingRate();
            }    
            if(DateTime.Now.Hour == 8 && DateTime.Now.Minute == 1 && DateTime.Now.Second == 01)
            {
                dpk_Check.SelectedDate = DateTime.Now;
                GetWorkingRate();
                

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
            //lb_Month.Content = DateTime.Parse(dateCheck).ToString("MMMM").ToUpper();
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
