using LiveSystem.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Window_EnvironTabSave.xaml
    /// </summary>
    public partial class Window_EnvironTabSave : Window
    {

        public Window_EnvironTabSave()
        {
            InitializeComponent();
            GetDataCmbYearTab3();
            GetDataCmbWeekTab3();
            //GetDataCmbTimes();
        }

        #region Khai báo 
        public static string path_Ksystem20 = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";
        bool checkWorking = false;
        string PCCCErr;
        string ElecErr;
        string SafetyErr;
        string HealthErr;
        string EnviroErr;
        string ULErr;
        string PCCCImp;
        string ElecImp;
        string SafetyImp;
        string HealthImp;
        string EnviroImp;
        string ULImp;
        string IPEmp;


        #endregion



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
            string YearFind = cbbYearTab3.Text;
            string WeekFind = cbbWeekTab3.Text;

            string query = "SPGetDataFindSafe @Year , @Time , @Area";


            //// Lấy dữ liệu và hiển thị
            DataTable FindDataSafe = new DataTable();

            if (YearFind == "")
                MessageBox.Show("Bạn phải chọn năm đánh giá", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            if (WeekFind == "")
                MessageBox.Show("Bạn phải chọn tuần đánh giá", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);


            FindDataSafe = DataProvider.Instance.ExecuteSP(path_Ksystem20, query, new object[] { YearFind, WeekFind });


            foreach (DataRow row in FindDataSafe.Rows)
            {
                txt_PCCC.Text = row["fSafe1"].ToString();
                txt_Elec.Text = row["Elec1"].ToString();
                txt_Safe.Text = row["Safety1"].ToString();
                txt_Health.Text = row["Health1"].ToString();
                txt_Envico.Text = row["sEnviro1"].ToString();
                txt_UL.Text = row["UL1"].ToString();
                txt_Imp_PCCC.Text = row["fSafe2"].ToString();
                txt_Imp_Elec.Text = row["Elec2"].ToString();
                txt_Imp_Safe.Text = row["Safety2"].ToString();
                txt_Imp_Heal.Text = row["Health2"].ToString();
                txt_Imp_Envico.Text = row["sEnviro2"].ToString();
                txt_Imp_UL.Text = row["UL2"].ToString();
                txt_Week.Text = row["Week"].ToString();
            }
            //});


        }

        private async void GetDataCmbYearTab3()
        {
            string query = "SPGetDataCmbYearEnviroTab3 ";
            // Lấy dữ liệu và hiển thị
            DataTable listCmb = new DataTable();

            listCmb = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query);


            List<string> listResult = new List<string>();
            foreach (DataRow Row in listCmb.Rows)
            {
                listResult.Add(Row["CbbYear"].ToString());
            }
            cbbYearTab3.ItemsSource = listResult;
        }

        private async void GetDataCmbWeekTab3()
        {
            string year = cbbYearTab3.Text;
            string query = "SPGetDataCmbWeekEnviroTab3 @Year ";
            // Lấy dữ liệu và hiển thị
            DataTable listCmb = new DataTable();

            listCmb = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, query, new object[] { year });


            List<string> listResult = new List<string>();
            foreach (DataRow Row in listCmb.Rows)
            {
                listResult.Add(Row["CbbYear"].ToString());
            }
            cbbWeekTab3.ItemsSource = listResult;
        }



        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txt_Week.Text == "" || txt_Week.Text == "0")
            {
                MessageBox.Show("Bạn chưa nhập tuần đánh giá nên không thể lưu được", "Thông báo", MessageBoxButton.OK);
                return;
            }
            if (int.Parse(txt_Week.Text) > 53 || int.Parse(txt_Week.Text) < 1)
            {
                MessageBox.Show("Bạn nhập tuần đánh giá chưa đúng định dạng", "Thông báo", MessageBoxButton.OK);
                return;
            }

            string Week = txt_Week.Text;
            string Result = "";

            PCCCErr = txt_PCCC.Text;
            ElecErr = txt_Elec.Text;
            SafetyErr = txt_Safe.Text;
            HealthErr = txt_Health.Text;
            EnviroErr = txt_Envico.Text;
            ULErr = txt_UL.Text;
            PCCCImp = txt_Imp_PCCC.Text;
            ElecImp = txt_Imp_Elec.Text;
            SafetyImp = txt_Imp_Safe.Text;
            HealthImp = txt_Imp_Heal.Text;
            EnviroImp = txt_Imp_Envico.Text;
            ULImp = txt_Imp_UL.Text;
            IPEmp = GetIPAddress();

            string SafeEdit = "SPDataSafeTabAdd @Week , @PCCCErr , @ElecErr , @SafeErr , @HealErr , @EnviErr , @ULErr ," +
                    " @PCCCImp , @ElecImp , @SafeImp , @HealImp , @EnviImp , @ULImp , @IPAddress";

            DataTable FindDataSafe = new DataTable();


            FindDataSafe = DataProvider.Instance.ExecuteSP(path_Ksystem20, SafeEdit, new object[] { Week,  PCCCErr, ElecErr, SafetyErr, HealthErr, EnviroErr, ULErr, PCCCImp, ElecImp, SafetyImp, HealthImp, EnviroImp, ULImp, IPEmp });


            foreach (DataRow row in FindDataSafe.Rows)
            {
                txt_PCCC.Text = row["fSafe1"].ToString();
                txt_Elec.Text = row["Elec1"].ToString();
                txt_Safe.Text = row["Safety1"].ToString();
                txt_Health.Text = row["Health1"].ToString();
                txt_Envico.Text = row["sEnviro1"].ToString();
                txt_UL.Text = row["ULErr"].ToString();
                txt_Imp_PCCC.Text = row["fSafe2"].ToString();
                txt_Imp_Elec.Text = row["Elec2"].ToString();
                txt_Imp_Safe.Text = row["Safety2"].ToString();
                txt_Imp_Heal.Text = row["Health2"].ToString();
                txt_Imp_Envico.Text = row["sEnviro2"].ToString();
                txt_Imp_UL.Text = row["ULImp"].ToString();
                Result = row["Result"].ToString();
            }

            MessageBox.Show(Result, "Thông báo", MessageBoxButton.OK);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            string year = cbbYearTab3.Text;
            string Week = cbbWeekTab3.Text;
            string Result = "";

            PCCCErr = txt_PCCC.Text;
            ElecErr = txt_Elec.Text;
            SafetyErr = txt_Safe.Text;
            HealthErr = txt_Health.Text;
            EnviroErr = txt_Envico.Text;
            ULErr = txt_UL.Text;
            PCCCImp = txt_Imp_PCCC.Text;
            ElecImp = txt_Imp_Elec.Text;
            SafetyImp = txt_Imp_Safe.Text;
            HealthImp = txt_Imp_Heal.Text;
            EnviroImp = txt_Imp_Envico.Text;
            ULImp = txt_Imp_UL.Text;
            IPEmp = GetIPAddress();


            string SafeEdit = "SPDataSafeTabEdit @Year , @Week , @PCCCErr , @ElecErr , @SafeErr , @HealErr , @EnviErr , @ULErr ," +
                    " @PCCCImp , @ElecImp , @SafeImp , @HealImp , @EnviImp , @ULImp , @IPAddress";

            DataTable FindDataSafe = new DataTable();


            FindDataSafe = DataProvider.Instance.ExecuteSP(path_Ksystem20, SafeEdit, new object[] { year, Week, PCCCErr, ElecErr, SafetyErr, HealthErr, EnviroErr, ULErr, PCCCImp, ElecImp, SafetyImp, HealthImp, EnviroImp,ULImp,  IPEmp });


            foreach (DataRow row in FindDataSafe.Rows)
            {
                txt_PCCC.Text = row["fSafe1"].ToString();
                txt_Elec.Text = row["Elec1"].ToString();
                txt_Safe.Text = row["Safety1"].ToString();
                txt_Health.Text = row["Health1"].ToString();
                txt_Envico.Text = row["sEnviro1"].ToString();
                txt_UL.Text = row["ULErr"].ToString();
                txt_Imp_PCCC.Text = row["fSafe2"].ToString();
                txt_Imp_Elec.Text = row["Elec2"].ToString();
                txt_Imp_Safe.Text = row["Safety2"].ToString();
                txt_Imp_Heal.Text = row["Health2"].ToString();
                txt_Imp_Envico.Text = row["sEnviro2"].ToString();
                txt_Imp_UL.Text = row["ULImp"].ToString();

                Result = row["Result"].ToString();
            }
            MessageBox.Show(Result, "Thông báo", MessageBoxButton.OK);

        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            string year = cbbYearTab3.Text;
            string Week = cbbYearTab3.Text;
            IPEmp = GetIPAddress();
            int check = 0;
            string result = "";

            string SafeDel = "SPDataSafeTabDel @Year , @Week , @IPAddress";



            DataTable FindDataSafe = new DataTable();

            MessageBoxResult dlr = MessageBox.Show("Bạn có chắc chắn muốn xoá dữ liệu không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (dlr)
            {
                case MessageBoxResult.Yes:
                    check = 1;
                    break;
                case MessageBoxResult.No:
                    check = 0;
                    break;
            }
            if (check == 1)
            {
                FindDataSafe = DataProvider.Instance.ExecuteSP(path_Ksystem20, SafeDel, new object[] { year , Week,  IPEmp });
            }

            foreach (DataRow row in FindDataSafe.Rows)
            {
                result = row["Result"].ToString();
            }
            MessageBox.Show(result, "Thông báo");
            txt_PCCC.Text = "";
            txt_Elec.Text = "";
            txt_Safe.Text = "";
            txt_Health.Text = "";
            txt_Envico.Text = "";
            txt_UL.Text = "";
            txt_Imp_PCCC.Text = "";
            txt_Imp_Elec.Text = "";
            txt_Imp_Safe.Text = "";
            txt_Imp_Heal.Text = "";
            txt_Imp_Envico.Text = "";
            txt_Imp_UL.Text = "";
        }

        private void btnDanhsach_Click(object sender, RoutedEventArgs e)
        {
            Window_EnvironmentData EnvironData = new Window_EnvironmentData();
            EnvironData.Show();
        }

        private void cbbYearChange(object sender, SelectionChangedEventArgs e)
        {
            var click = sender as ComboBox;
            var clickItem = click.SelectedItem as ComboBoxItem;
            string queryTimes = "SPGetDataCmbWeekSafeTab @cbYear";
            //lấy dữ liệu cbb times
            string Year = "ALL";
            //Year = clickItem.Content.ToString();

            DataTable listCmbTimes = new DataTable();
            DataTable listCmbArea = new DataTable();
            listCmbTimes = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, queryTimes, new object[] { Year });

            List<string> listResultTimes = new List<string>();
            foreach (DataRow Row in listCmbTimes.Rows)
            {
                listResultTimes.Add(Row["Name"].ToString());
            }
            cbbWeekTab3.ItemsSource = listResultTimes;

        }

        private void btnTimKiemTab3_Click(object sender, RoutedEventArgs e)
        {
            string query = "SPGetDataSafeSaveQueryTab3 @cbYear , @cbWeek ";
            var result = DataProvider.Instance.ExecuteSP(path_Ksystem20, query, new object[]
            {
                cbbYearTab3.Text,
                cbbWeekTab3.Text,
            });

            foreach(DataRow Row in result.Rows)
            {
                txt_Elec.Text = Row["Elec1"].ToString();
                txt_PCCC.Text = Row["fSafe1"].ToString();
                txt_Safe.Text = Row["Safety1"].ToString();
                txt_Health.Text = Row["Health1"].ToString();
                txt_Envico.Text = Row["sEnviro1"].ToString();
                txt_UL.Text = Row["ULErr"].ToString();

                txt_Imp_Elec.Text = Row["Elec2"].ToString();
                txt_Imp_PCCC.Text = Row["fSafe2"].ToString();
                txt_Imp_Safe.Text = Row["Safety2"].ToString();
                txt_Imp_Heal.Text = Row["Health2"].ToString();
                txt_Imp_Envico.Text = Row["sEnviro2"].ToString();
                txt_Imp_UL.Text = Row["ULImp"].ToString();

                txt_Week.Text = Row["EvalW"].ToString();
            }

        }

        private void txt_Week_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_PCCC_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_Elec_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_Safe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_Health_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_Envico_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_UL_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_Imp_PCCC_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_Imp_Elec_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_Imp_Safe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_Imp_Heal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_Imp_Envico_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
