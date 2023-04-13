using LiveSystem.DAO;
using LiveSystem.Model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Page_Map.xaml
    /// </summary>
    public partial class Page_Map : Page
    {
        #region Khai báo
        List<Helper_Employee> listQtyXa = new List<Helper_Employee>();
        List<Helper_Employee> ListFull = new List<Helper_Employee>();
        #endregion
        public Page_Map()
        {
            InitializeComponent();
            List<string> DistName = new List<string>();
            BitmapImage image = new BitmapImage(new Uri("./Image/map_bn.png", UriKind.Relative));
            Image_Map.Source = image;

            var listAllEmp = GetAllEmpByDist("ALL");
            btnTienDu.Content = "Tiên Du : " + listAllEmp.Where(x => x.TempDist == "Huyện Tiên Du").Count().ToString();
            btnLuongTai.Content = "Lương Tài : " + listAllEmp.Where(x => x.TempDist == "Huyện Lương Tài").Count().ToString();
            btnQueVo.Content = "Quế Võ : " + listAllEmp.Where(x => x.TempDist == "Huyện Quế Võ").Count().ToString();
            btnThuanThanh.Content = "Thuận Thành : " + listAllEmp.Where(x => x.TempDist == "Huyện Thuận Thành").Count().ToString();
            btnGiaBinh.Content = "Gia Bình : " + listAllEmp.Where(x => x.TempDist == "Huyện Gia Bình").Count().ToString();
            btnYenPhong.Content = "Yên Phong : " + listAllEmp.Where(x => x.TempDist == "Huyện Yên Phong").Count().ToString();
            btnBacNinh.Content = "Bắc Ninh : " + listAllEmp.Where(x => x.TempDist == "Thành phố Bắc Ninh").Count().ToString();
            btnTuSon.Content = "Từ Sơn : " + listAllEmp.Where(x => x.TempDist == "Thành phố Từ Sơn").Count().ToString();

            txbTenXa.Content = "Huyện/Thành phố";
            Loaded += Page_Map_Loaded;
        }

        private void Page_Map_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MainWindow.language);
            ApplyLanguage(MainWindow.language);
        }

        //===============================================================================================================//
        // Lấy danh sách nhân viên theo huyện thuộc tỉnh bắc ninh
        private List<Emp> GetAllEmpByDist(string tempDist)
        {
            // Lấy dữ liệu thông tin nhân viên
            string query1 = "select * from update_employee where TempProv = N'Tỉnh Bắc Ninh'";
            var listEmpInformation = DataProvider.Instance.MySqlExecuteQuery(Page_Main.path_TaixinWeb, query1);
            // Lấy dữ liệu nhân viên thực tế từ Ksystem
            string query2 = "SELECT * FROM TDAEmpMaster where RetDate >= @date and len(EmpId) > 4 and len(EmpId) < 8";
            var listEmp = DataProvider.Instance.executeQuery(Page_Main.path_Ksystem20, query2, new object[] { Page_Main.dateCheck });

            List<Emp> listAllEmp = new List<Emp>();

            // Lấy dữ liệu nhân viên đã được update thông tin trên taixin web
            listAllEmp = listEmp.AsEnumerable().Join(listEmpInformation.AsEnumerable(), x => x["EmpId"].ToString().Trim().ToUpper(), y => y["EmpId"].ToString().Trim().ToUpper(), (x, y) => new { x, y })
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
            }).ToList();

            // Lọc theo huyện
            if(tempDist != "ALL")
            {
                listAllEmp = listAllEmp.Where(x => x.TempDist == tempDist).ToList();
            }

            return listAllEmp;
        }

        // Lấy danh sách nhân viên theo xã từ danh sách nhân viên theo huyện
        private List<Emp> GetAllEmpByComm(List<Emp> listAllEmpByDist, string tempComm)
        {
            var listAllEmpByComm = listAllEmpByDist.Where(x => x.TempComm == tempComm).OrderBy(x => x.EmpId).ToList();

            // Thêm STT và sửa Giới tính
            int i = 1;
            listAllEmpByComm.ForEach(x =>
            {
                x.ID = i;
                if (x.SexCd.Contains("001")) { x.SexCd = "Nam"; } else { x.SexCd = "Nữ"; }
                i++;
            });
            return listAllEmpByComm;
        }
        //===============================================================================================================//


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

        private void btnYenPhong_Click(object sender, RoutedEventArgs e)
        {
            txbTenXa.Content = "Huyện Yên Phong";
            var listAllEmpByDist = GetAllEmpByDist(txbTenXa.Content.ToString()).GroupBy(x => x.TempComm).Select(x => new {CommName = x.Key, EmpQty = x.Count()}).ToList();
            
            lvQtyXa.ItemsSource = listAllEmpByDist;
        }

        private void btnBacNinh_Click(object sender, RoutedEventArgs e)
        {
            txbTenXa.Content = "Thành phố Bắc Ninh";
            var listAllEmpByDist = GetAllEmpByDist(txbTenXa.Content.ToString()).GroupBy(x => x.TempComm).Select(x => new { CommName = x.Key, EmpQty = x.Count() }).ToList();
            lvQtyXa.ItemsSource = listAllEmpByDist;
        }

        private void btnQueVo_Click(object sender, RoutedEventArgs e)
        {
            txbTenXa.Content = "Huyện Quế Võ";
            var listAllEmpByDist = GetAllEmpByDist(txbTenXa.Content.ToString()).GroupBy(x => x.TempComm).Select(x => new { CommName = x.Key, EmpQty = x.Count() }).ToList();
            lvQtyXa.ItemsSource = listAllEmpByDist;
        }

        private void btnGiaBinh_Click(object sender, RoutedEventArgs e)
        {
            txbTenXa.Content = "Huyện Gia Bình";
            var listAllEmpByDist = GetAllEmpByDist(txbTenXa.Content.ToString()).GroupBy(x => x.TempComm).Select(x => new { CommName = x.Key, EmpQty = x.Count() }).ToList();
            lvQtyXa.ItemsSource = listAllEmpByDist;
        }

        private void btnLuongTai_Click(object sender, RoutedEventArgs e)
        {
            txbTenXa.Content = "Huyện Lương Tài";
            var listAllEmpByDist = GetAllEmpByDist(txbTenXa.Content.ToString()).GroupBy(x => x.TempComm).Select(x => new { CommName = x.Key, EmpQty = x.Count() }).ToList();
            lvQtyXa.ItemsSource = listAllEmpByDist;
        }

        private void btnThuanThanh_Click(object sender, RoutedEventArgs e)
        {
            txbTenXa.Content = "Huyện Thuận Thành";
            var listAllEmpByDist = GetAllEmpByDist(txbTenXa.Content.ToString()).GroupBy(x => x.TempComm).Select(x => new { CommName = x.Key, EmpQty = x.Count() }).ToList();
            lvQtyXa.ItemsSource = listAllEmpByDist;
        }

        private void btnTienDu_Click(object sender, RoutedEventArgs e)
        {
            txbTenXa.Content = "Huyện Tiên Du";
            var listAllEmpByDist = GetAllEmpByDist(txbTenXa.Content.ToString()).GroupBy(x => x.TempComm).Select(x => new { CommName = x.Key, EmpQty = x.Count() }).ToList();
            lvQtyXa.ItemsSource = listAllEmpByDist;
        }

        private void btnTuSon_Click(object sender, RoutedEventArgs e)
        {
            txbTenXa.Content = "Thành phố Từ Sơn";
            var listAllEmpByDist = GetAllEmpByDist(txbTenXa.Content.ToString()).GroupBy(x => x.TempComm).Select(x => new { CommName = x.Key, EmpQty = x.Count() }).ToList();
            lvQtyXa.ItemsSource = listAllEmpByDist;
        }

        private void lvQtyXa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic comm = lvQtyXa.SelectedItem;
            string commName = "";
            if(comm != null) { commName = comm.CommName; }
            
            lvThongTin.ClearValue(ListView.ItemsSourceProperty);
            if (commName != null)
            {
                try
                {
                    var listAllEmpByDist = GetAllEmpByDist(txbTenXa.Content.ToString()).ToList();
                    lvThongTin.ItemsSource = GetAllEmpByComm(listAllEmpByDist, commName);
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
                
            }
        }
    }
}
