using LiveSystem.DAO;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for Window_EnvironSave.xaml
    /// </summary>
    public partial class Window_EnvironSave : Window
    {
        public Window_EnvironSave()
        {
            InitializeComponent();
            GetDataCmbYear();
            GetDataCmbColor();
            //GetDataCmbTimes();
        }

        #region Khai báo 
        public static string path_Ksystem20 = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";
        bool checkWorking = false;
        string PCCCErr ;
        string ElecErr;
        string SafetyErr;
        string HealthErr;
        string EnviroErr;
        string PCCCImp;
        string ElecImp;
        string SafetyImp;
        string HealthImp;
        string EnviroImp;
        string areaN;
        string areaS;
        string EvalD;
        string Color;
        string PICReview;
        string IPEmp;
        string Point;
        string Rate;

        #endregion


        private void cbbYearChange(object sender, SelectionChangedEventArgs e)
        {
            var click = sender as ComboBox;
            var clickItem = click.SelectedItem as ComboBoxItem;
            string queryTimes = "SPGetDataCmbTimesSafe @cbYear";
            string queryArea = "SPGetDataCmbAreaSafe @cbYear";
            //lấy dữ liệu cbb times
            string Year = "ALL";
            //Year = clickItem.Content.ToString();

            DataTable listCmbTimes = new DataTable();
            DataTable listCmbArea = new DataTable();
            listCmbTimes = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, queryTimes, new object[] { Year });
            listCmbArea = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, queryArea, new object[] { Year });

            List<string> listResultTimes = new List<string>();
            List<string> listResultArea = new List<string>();
            foreach (DataRow Row in listCmbTimes.Rows)
            {
                listResultTimes.Add(Row["Name"].ToString());
            }
            cbbTimeReview.ItemsSource = listResultTimes;

            foreach (DataRow Row in listCmbArea.Rows)
            {
                listResultArea.Add(Row["Name"].ToString());
            }
            cbbAreaNm.ItemsSource = listResultArea;
        }


        public string GetIPAddress()
        {
            string IPAddress = string.Empty;
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }

        private async void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {

            //Loaded += Page_Environment_Loaded;
            if (checkWorking == false && MainWindow._checkInternet == "Success")
            {
                await Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Page_LoadingData page_Loading = new Page_LoadingData();
                        stackLoading.Visibility = Visibility.Visible;
                        frameLoading.Navigate(page_Loading);
                        //lvThongTin.ClearValue(ListView.ItemsSourceProperty);
                        checkWorking = true;
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);
                });




                await Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        GetListDataSafeFind();
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

        private async void GetListDataSafeFind()
        {
            string YearFind = cbbYear.Text;
            string TimesFind = cbbTimeReview.Text;
            string AreaFind = cbbAreaNm.Text;

            string query = "SPGetDataFindSafe @Year , @Time , @Area";

            
            //// Lấy dữ liệu và hiển thị
            DataTable FindDataSafe = new DataTable();
            
            if (YearFind == "")
                    MessageBox.Show("Bạn phải chọn năm đánh giá", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                if (TimesFind == "")
                    MessageBox.Show("Bạn phải chọn năm đánh giá", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                if (AreaFind == "")
                    MessageBox.Show("Bạn phải chọn năm đánh giá", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);


                FindDataSafe = DataProvider.Instance.ExecuteSP(path_Ksystem20, query, new object[] { YearFind, TimesFind, AreaFind });


                foreach (DataRow row in FindDataSafe.Rows)
                {
                    txt_PCCC.Text = row["fSafe1"].ToString();
                    txt_Elec.Text = row["Elec1"].ToString();
                    txt_Safe.Text = row["Safety1"].ToString();
                    txt_Health.Text = row["Health1"].ToString();
                    txt_Envico.Text = row["sEnviro1"].ToString();
                    txt_Imp_PCCC.Text = row["fSafe2"].ToString();
                    txt_Imp_Elec.Text = row["Elec2"].ToString();
                    txt_Imp_Safe.Text = row["Safety2"].ToString();
                    txt_Imp_Heal.Text = row["Health2"].ToString();
                    txt_Imp_Envico.Text = row["sEnviro2"].ToString();
                    txt_AreaRv.Text = row["areaN"].ToString();
                    txt_AreaCharge.Text = row["areaS"].ToString();
                    txt_DayReview.Text = row["EvalD"].ToString();
                    txt_Point.Text = row["score"].ToString();
                    txt_RateRv.Text = row["Rate"].ToString();
                    cbbColor.Text = row["zColor2"].ToString();
                    txt_PICReview.Text = row["PICReview"].ToString();
            }
            //});


        }

        private async void GetDataCmbYear()
        {
            //lây dữ liệu lên cbb Year
            string cbYear = "";
            string queryYear = "SPGetDataCmbYearSafe @cbYear ";
            string queryTimes = "SPGetDataCmbTimeSafe @cbYear ";
            //string queryArea = "SPGetDataCmbAreaSafe @cbYear ";

            // Lấy dữ liệu và hiển thị
            DataTable listCmbYear = new DataTable();
            DataTable listCmbTimes = new DataTable();
            DataTable listCmbArea = new DataTable();

            listCmbYear = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, queryYear, new object[] { cbYear });
            listCmbTimes = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, queryTimes, new object[] { cbYear });
            //listCmbArea = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, queryArea, new object[] { cbYear });


            List<string> listResultYear = new List<string>();
            List<string> listResultTimes = new List<string>();
            List<string> listResultArea = new List<string>();


            foreach (DataRow Row in listCmbYear.Rows)
            {
                listResultYear.Add(Row["Name"].ToString());
            }
            cbbYear.ItemsSource = listResultYear;

            foreach (DataRow Row in listCmbTimes.Rows)
            {
                listResultTimes.Add(Row["Name"].ToString());
            }
            cbbTimeReview.ItemsSource = listResultTimes;

            //foreach (DataRow Row in listCmbArea.Rows)
            //{
            //    listResultArea.Add(Row["Name"].ToString());
            //}
            //cbbAreaNm.ItemsSource = listResultArea;

        }


        private async void GetDataCmbColor()
        {
            //lây dữ liệu lên cbb Year
            string cbYear = "";
            string queryCbbColor = "SPGetDataCmbColorSafe @cbYear ";

            // Lấy dữ liệu và hiển thị
            DataTable listCmbColor = new DataTable();

            listCmbColor = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, queryCbbColor, new object[] { cbYear });


            List<string> listResultColor = new List<string>();


            foreach (DataRow Row in listCmbColor.Rows)
            {
                listResultColor.Add(Row["Name"].ToString());
            }
            cbbColor.ItemsSource = listResultColor;


        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(Txt_YearReview.Text == "" )
            {
                MessageBox.Show("Bạn chưa nhập năm đánh giá nên không thể lưu được", "Thông báo", MessageBoxButton.OK);
                return;
            }
            if (Txt_TimeReview.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập lần đánh giá nên không thể lưu được", "Thông báo", MessageBoxButton.OK);
                return;
            }
            if (Txt_AreaNm.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập khu vực đánh giá nên không thể lưu được", "Thông báo", MessageBoxButton.OK);
                return;
            }
            string YearFind = Txt_YearReview.Text;
            string TimesFind = Txt_TimeReview.Text;
            string AreaFind = Txt_AreaNm.Text;
            string Result = "";

            PCCCErr = txt_PCCC.Text;
            ElecErr = txt_Elec.Text;
            SafetyErr = txt_Safe.Text;
            HealthErr = txt_Health.Text;
            EnviroErr = txt_Envico.Text;
            PCCCImp = txt_Imp_PCCC.Text;
            ElecImp = txt_Imp_Elec.Text;
            SafetyImp = txt_Imp_Safe.Text;
            HealthImp = txt_Imp_Heal.Text;
            EnviroImp = txt_Imp_Envico.Text;
            areaN = txt_AreaRv.Text;
            areaS = txt_AreaCharge.Text;
            EvalD = txt_DayReview.Text;
            Color = cbbColor.Text;
            PICReview = txt_PICReview.Text;
            IPEmp = GetIPAddress();
            Point = txt_Point.Text;
            Rate = txt_RateRv.Text;

            string SafeEdit = "SPDataSafeAdd @Year , @Time , @Area , @PCCCErr , @ElecErr , @SafeErr , @HealErr , @EnviErr ," +
                    " @PCCCImp , @ElecImp , @SafeImp , @HealImp , @EnviImp , @AreaN , @AreaS , @EvalDay , @Point , @Rate , @Color , @PICReview , @IPAddress";

            DataTable FindDataSafe = new DataTable();


            FindDataSafe = DataProvider.Instance.ExecuteSP(path_Ksystem20, SafeEdit, new object[] { YearFind, TimesFind, AreaFind, PCCCErr, ElecErr, SafetyErr, HealthErr, EnviroErr, PCCCImp, ElecImp, SafetyImp, HealthImp, EnviroImp, areaN, areaS, EvalD, Point, Rate, Color, PICReview, IPEmp });


            foreach (DataRow row in FindDataSafe.Rows)
            {
                txt_PCCC.Text = row["fSafe1"].ToString();
                txt_Elec.Text = row["Elec1"].ToString();
                txt_Safe.Text = row["Safety1"].ToString();
                txt_Health.Text = row["Health1"].ToString();
                txt_Envico.Text = row["sEnviro1"].ToString();
                txt_Imp_PCCC.Text = row["fSafe2"].ToString();
                txt_Imp_Elec.Text = row["Elec2"].ToString();
                txt_Imp_Safe.Text = row["Safety2"].ToString();
                txt_Imp_Heal.Text = row["Health2"].ToString();
                txt_Imp_Envico.Text = row["sEnviro2"].ToString();
                txt_AreaRv.Text = row["areaN"].ToString();
                txt_AreaCharge.Text = row["areaS"].ToString();
                txt_DayReview.Text = row["EvalD"].ToString();
                txt_Point.Text = row["score"].ToString();
                txt_RateRv.Text = row["Rate"].ToString();
                cbbColor.Text = row["zColor2"].ToString();
                txt_PICReview.Text = row["PICReview"].ToString();
                Result = row["Result"].ToString();
            }

            MessageBox.Show(Result, "Thông báo", MessageBoxButton.OK);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            string YearFind = cbbYear.Text;
            string TimesFind = cbbTimeReview.Text;
            string AreaFind = cbbAreaNm.Text;
            string Result = "";

            PCCCErr = txt_PCCC.Text;
            ElecErr = txt_Elec.Text;
            SafetyErr = txt_Safe.Text;
            HealthErr = txt_Health.Text;
            EnviroErr = txt_Envico.Text;
            PCCCImp = txt_Imp_PCCC.Text;
            ElecImp = txt_Imp_Elec.Text;
            SafetyImp = txt_Imp_Safe.Text;
            HealthImp = txt_Imp_Heal.Text;
            EnviroImp = txt_Imp_Envico.Text;
            areaN = txt_AreaRv.Text;
            areaS = txt_AreaCharge.Text;
            EvalD = txt_DayReview.Text;
            Color = cbbColor.Text;
            PICReview = txt_PICReview.Text;
            IPEmp = GetIPAddress();
            Point = txt_Point.Text;
            Rate = txt_RateRv.Text;


            string SafeEdit = "SPDataSafeEdit @Year , @Time , @Area , @PCCCErr , @ElecErr , @SafeErr , @HealErr , @EnviErr ," +
                    " @PCCCImp , @ElecImp , @SafeImp , @HealImp , @EnviImp , @AreaN , @AreaS , @EvalDay , @Point , @Rate , @Color , @PICReview , @IPAddress";

            DataTable FindDataSafe = new DataTable();


            FindDataSafe = DataProvider.Instance.ExecuteSP(path_Ksystem20, SafeEdit, new object[] { YearFind, TimesFind, AreaFind, PCCCErr, ElecErr, SafetyErr, HealthErr, EnviroErr, PCCCImp, ElecImp, SafetyImp, HealthImp, EnviroImp, areaN, areaS, EvalD, Point, Rate, Color, PICReview, IPEmp });


            foreach (DataRow row in FindDataSafe.Rows)
            {
                txt_PCCC.Text = row["fSafe1"].ToString();
                txt_Elec.Text = row["Elec1"].ToString();
                txt_Safe.Text = row["Safety1"].ToString();
                txt_Health.Text = row["Health1"].ToString();
                txt_Envico.Text = row["sEnviro1"].ToString();
                txt_Imp_PCCC.Text = row["fSafe2"].ToString();
                txt_Imp_Elec.Text = row["Elec2"].ToString();
                txt_Imp_Safe.Text = row["Safety2"].ToString();
                txt_Imp_Heal.Text = row["Health2"].ToString();
                txt_Imp_Envico.Text = row["sEnviro2"].ToString();
                txt_AreaRv.Text = row["areaN"].ToString();
                txt_AreaCharge.Text = row["areaS"].ToString();
                txt_DayReview.Text = row["EvalD"].ToString();
                txt_Point.Text = row["score"].ToString();
                txt_RateRv.Text = row["Rate"].ToString();
                cbbColor.Text = row["zColor2"].ToString();
                txt_PICReview.Text = row["PICReview"].ToString();
                Result = row["Result"].ToString();
            }
            MessageBox.Show(Result, "Thông báo", MessageBoxButton.OK);

        }
        
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            string YearFind = cbbYear.Text;
            string TimesFind = cbbTimeReview.Text;
            string AreaFind = cbbAreaNm.Text;
            IPEmp = GetIPAddress();
            int check = 0;
            string result = "";

            string SafeDel = "SPDataSafeDel @Year , @Time , @Area , @IPAddress";

            

            DataTable FindDataSafe = new DataTable();
            
            MessageBoxResult dlr =  MessageBox.Show("Bạn có chắc chắn muốn xoá dữ liệu không?","Thông báo",MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch(dlr)
            {
                case MessageBoxResult.Yes:
                    check = 1;
                    break;
                case MessageBoxResult.No:
                    check = 0;
                    break;
            }    
            if(check == 1)
            {
                FindDataSafe = DataProvider.Instance.ExecuteSP(path_Ksystem20, SafeDel, new object[] { YearFind, TimesFind, AreaFind, IPEmp });
            }    
            
            foreach(DataRow row in FindDataSafe.Rows)
            {
                result = row["Result"].ToString();
            }
            MessageBox.Show(result, "Thông báo");
            txt_PCCC.Text = "";
            txt_Elec.Text = "";
            txt_Safe.Text = "";
            txt_Health.Text = "";
            txt_Envico.Text = "";
            txt_Imp_PCCC.Text = "";
            txt_Imp_Elec.Text = "";
            txt_Imp_Safe.Text = "";
            txt_Imp_Heal.Text = "";
            txt_Imp_Envico.Text = "";
            txt_AreaRv.Text = "";
            txt_AreaCharge.Text = "";
            txt_DayReview.Text = "";
            txt_Point.Text = "";
            txt_RateRv.Text = "";
            cbbColor.Text = "";
            txt_PICReview.Text = "";

        }

        private void btnDanhsach_Click(object sender, RoutedEventArgs e)
        {
            Window_EnvironmentData EnvironData = new Window_EnvironmentData();
            EnvironData.Show();
        }
    }
}
