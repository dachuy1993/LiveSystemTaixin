﻿using LiveSystem.DAO;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
    /// Interaction logic for Page_EmpIn.xaml
    /// </summary>
    public partial class Page_EmpIn : Page
    {
        bool checkWorking = false;
        public Page_EmpIn()
        {
            InitializeComponent();
            dp_Check.SelectedDate = DateTime.Now;

        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int ql1 = int.Parse(txt_Ql.Text);
                int mar1 = int.Parse(txt_Mar.Text);
                int qc1 = int.Parse(txt_Qc.Text);
                int it1 = int.Parse(txt_It.Text);
                int cocgiay1 = int.Parse(txt_Cocgiay.Text);
                int sx1 = int.Parse(txt_Sx.Text);
                int ql2 = int.Parse(txt_Real_Ql.Text);
                int mar2 = int.Parse(txt_Real_Mar.Text);
                int qc2 = int.Parse(txt_Real_Qc.Text);
                int it2 = int.Parse(txt_Real_It.Text);
                int cocgiay2 = int.Parse(txt_Real_Cocgiay.Text);
                int sx2 = int.Parse(txt_Real_Sx.Text);


                int ql1_D = int.Parse(txt_Ql_Dem.Text);
                int mar1_D = int.Parse(txt_Mar_Dem.Text);
                int qc1_D = int.Parse(txt_Qc_Dem.Text);
                int it1_D = int.Parse(txt_It_Dem.Text);
                int cocgiay1_D = int.Parse(txt_Cocgiay_Dem.Text);
                int sx1_D = int.Parse(txt_Sx_Dem.Text);
                int ql2_D = int.Parse(txt_Real_Ql_Dem.Text);
                int mar2_D = int.Parse(txt_Real_Mar_Dem.Text);
                int qc2_D = int.Parse(txt_Real_Qc_Dem.Text);
                int it2_D = int.Parse(txt_Real_It_Dem.Text);
                int cocgiay2_D = int.Parse(txt_Real_Cocgiay_Dem.Text);
                int sx2_D = int.Parse(txt_Real_Sx_Dem.Text);


                List<Dept> list_Dept = new List<Dept>();
                list_Dept.Add(new Dept() { Name = "MANAGE", QtySignDay = ql1, QtyRealDay = ql2, QtySignNight = ql1_D, QtyRealNight = ql2_D });
                list_Dept.Add(new Dept() { Name = "MAR", QtySignDay = mar1, QtyRealDay = mar2, QtySignNight = mar1_D, QtyRealNight = mar2_D });
                list_Dept.Add(new Dept() { Name = "QC", QtySignDay = qc1, QtyRealDay = qc2, QtySignNight = qc1_D, QtyRealNight = qc2_D });
                list_Dept.Add(new Dept() { Name = "IT", QtySignDay = it1, QtyRealDay = it2, QtySignNight = it1_D, QtyRealNight = it2_D });
                list_Dept.Add(new Dept() { Name = "HICUP", QtySignDay = cocgiay1, QtyRealDay = cocgiay2, QtySignNight = cocgiay1_D, QtyRealNight = cocgiay2_D });
                list_Dept.Add(new Dept() { Name = "PRO", QtySignDay = sx1, QtyRealDay = sx2, QtySignNight = sx1_D, QtyRealNight = sx2_D });

                string date = DateTime.Parse(dp_Check.ToString()).ToString("yyyy-MM-dd");

                using (SqlConnection conn = new SqlConnection(Page_Main.path_Ksystem25))
                {
                    conn.Open();
                    //var command = "Delete tmmempetc where date='" + date + "' and (Dept<>'JW' and Dept <>'SF')";
                    var command = "Delete TDAEmpETC where DateEtc='" + date + "' and (DeptNm<>'JW' and DeptNm <>'SF')";
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

                foreach (var item in list_Dept)
                {

                    using (SqlConnection conn = new SqlConnection(Page_Main.path_Ksystem25))
                    {
                        conn.Open();
                        //var command = "INSERT tmmempetc(Dept,qtySignDay,qtyRealDay,qtySignNight,qtyRealNight,date,insdt) VALUES(N'" + item.Name + "','" + item.QtySignDay + "','" + item.QtyRealDay + "','" + item.QtySignNight + "','" + item.QtyRealNight + "','" + date + "','" + DateTime.Now.ToString() + "')";
                        var command = "INSERT TDAEmpETC(DeptNm,qtySignDay,qtyRealDay,qtySignNight,qtyRealNight,DateEtc,insdt) VALUES(N'" + item.Name + "','" + item.QtySignDay + "','" + item.QtyRealDay + "','" + item.QtySignNight + "','" + item.QtyRealNight + "','" + date + "','" + DateTime.Now.ToString() + "')";
                        using (SqlCommand cmd = new SqlCommand(command, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                }
                MessageBox.Show("Đăng ký thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng kiểm tra lại số lượng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddVendor_Click(object sender, RoutedEventArgs e)
        {
            int jw1 = int.Parse(txt_Jw.Text);
            int sf1 = int.Parse(txt_Sf.Text);
            int HMP1 = int.Parse(txt_HMP.Text);
            int HES1 = int.Parse(txt_HES.Text);
            int IQ1 = int.Parse(txt_IQ.Text);
            int jw2 = int.Parse(txt_Real_Jw.Text);
            int sf2 = int.Parse(txt_Real_Sf.Text);
            int HMP2 = int.Parse(txt_Real_HMP.Text);
            int HES2 = int.Parse(txt_Real_HES.Text);
            int IQ2 = int.Parse(txt_Real_IQ.Text);

            int jw1_D = int.Parse(txt_Jw_Dem.Text);
            int sf1_D = int.Parse(txt_Sf_Dem.Text);
            int HMP1_D = int.Parse(txt_HMP_Dem.Text);
            int HES1_D = int.Parse(txt_HES_DemVSIP.Text);
            int IQ1_D = int.Parse(txt_HMP_Dem.Text);
            int jw2_D = int.Parse(txt_Real_Jw_Dem.Text);
            int sf2_D = int.Parse(txt_Real_Sf_Dem.Text);
            int HMP2_D = int.Parse(txt_Real_HMP_Dem.Text);
            int HES2_D = int.Parse(txt_Real_HES_Dem.Text);
            int IQ2_D = int.Parse(txt_Real_IQ_Dem.Text);

            List<Dept> list_Dept = new List<Dept>();

            list_Dept.Add(new Dept() { Name = "JW", QtySignDay = jw1, QtyRealDay = jw2, QtySignNight = jw1_D, QtyRealNight = jw2_D });
            list_Dept.Add(new Dept() { Name = "SF", QtySignDay = sf1, QtyRealDay = sf2, QtySignNight = sf1_D, QtyRealNight = sf2_D });
            list_Dept.Add(new Dept() { Name = "HMP", QtySignDay = HMP1, QtyRealDay = HMP2, QtySignNight = HMP1_D, QtyRealNight = HMP2_D });
            list_Dept.Add(new Dept() { Name = "HES", QtySignDay = HES1, QtyRealDay = HES2, QtySignNight = HES1_D, QtyRealNight = HES2_D });
            list_Dept.Add(new Dept() { Name = "IQ", QtySignDay = IQ1, QtyRealDay = IQ2, QtySignNight = IQ1_D, QtyRealNight = IQ2_D });


            string date = DateTime.Parse(dp_Check.ToString()).ToString("yyyy-MM-dd");

            using (SqlConnection conn = new SqlConnection(Page_Main.path_Ksystem25))
            {
                conn.Open();
                //var command = "Delete tmmempetc where date='" + date + "' and (Dept='JW' or Dept ='SF')";
                var command = "Delete TDAEmpETC where DateEtc='" + date + "' and (DeptNm='JW' or DeptNm ='SF' or DeptNm ='HMP' or DeptNm ='HES' or DeptNm ='IQ')";
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

            foreach (var item in list_Dept)
            {

                using (SqlConnection conn = new SqlConnection(Page_Main.path_Ksystem25))
                {
                    conn.Open();
                    //var command = "INSERT tmmempetc(Dept,qtySignDay,qtyRealDay,qtySignNight,qtyRealNight,date,insdt) VALUES(N'" + item.Name + "','" + item.QtySignDay + "','" + item.QtyRealDay + "','" + item.QtySignNight + "','" + item.QtyRealNight + "','" + date + "','" + DateTime.Now.ToString() + "')";
                    var command = "INSERT TDAEmpETC(DeptNm,qtySignDay,qtyRealDay,qtySignNight,qtyRealNight,DateEtc,insdt) VALUES(N'" + item.Name + "','" + item.QtySignDay + "','" + item.QtyRealDay + "','" + item.QtySignNight + "','" + item.QtyRealNight + "','" + date + "','" + DateTime.Now.ToString() + "')";
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            MessageBox.Show("Đăng ký thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);


        }

        public void Db_Read_ETC(string date)
        {
            using (SqlConnection conn = new SqlConnection(Page_Main.path_Ksystem25))
            {
                conn.Open();
                var command = "select * from  TDAEmpETC where DateEtc='" + date + "'";
                List<Dept> listDept = new List<Dept>();
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr[0] != null)
                            {
                                Dept dep = new Dept();
                                dep.Name = dr[1].ToString();
                                dep.QtySignDay = int.Parse(dr[2].ToString());
                                dep.QtyRealDay = int.Parse(dr[3].ToString());
                                dep.QtySignNight = int.Parse(dr[4].ToString());
                                dep.QtyRealNight = int.Parse(dr[5].ToString());
                                listDept.Add(dep);
                            }
                        }
                    }
                }
                foreach (var item in listDept)
                {
                    if (item.Name == "MANAGE")
                    {
                        txt_Ql.Text = item.QtySignDay.ToString();
                        txt_Real_Ql.Text = item.QtyRealDay.ToString();
                        txt_Ql_Dem.Text = item.QtySignNight.ToString();
                        txt_Real_Ql_Dem.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "MAR")
                    {
                        txt_Mar.Text = item.QtySignDay.ToString();
                        txt_Real_Mar.Text = item.QtyRealDay.ToString();
                        txt_Mar_Dem.Text = item.QtySignNight.ToString();
                        txt_Real_Mar_Dem.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "QC")
                    {
                        txt_Qc.Text = item.QtySignDay.ToString();
                        txt_Real_Qc.Text = item.QtyRealDay.ToString();
                        txt_Qc_Dem.Text = item.QtySignNight.ToString();
                        txt_Real_Qc_Dem.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "IT")
                    {
                        txt_It.Text = item.QtySignDay.ToString();
                        txt_Real_It.Text = item.QtyRealDay.ToString();
                        txt_It_Dem.Text = item.QtySignNight.ToString();
                        txt_Real_It_Dem.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "HICUP")
                    {
                        txt_Cocgiay.Text = item.QtySignDay.ToString();
                        txt_Real_Cocgiay.Text = item.QtyRealDay.ToString();
                        txt_Cocgiay_Dem.Text = item.QtySignNight.ToString();
                        txt_Real_Cocgiay_Dem.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "PRO")
                    {
                        txt_Sx.Text = item.QtySignDay.ToString();
                        txt_Real_Sx.Text = item.QtyRealDay.ToString();
                        txt_Sx_Dem.Text = item.QtySignNight.ToString();
                        txt_Real_Sx_Dem.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "JW")
                    {
                        txt_Jw.Text = item.QtySignDay.ToString();
                        txt_Real_Jw.Text = item.QtyRealDay.ToString();
                        txt_Jw_Dem.Text = item.QtySignNight.ToString();
                        txt_Real_Jw_Dem.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "SF")
                    {
                        txt_Sf.Text = item.QtySignDay.ToString();
                        txt_Real_Sf.Text = item.QtyRealDay.ToString();
                        txt_Sf_Dem.Text = item.QtySignNight.ToString();
                        txt_Real_Sf_Dem.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "HMP")
                    {
                        txt_HMP.Text = item.QtySignDay.ToString();
                        txt_Real_HMP.Text = item.QtyRealDay.ToString();
                        txt_HMP_Dem.Text = item.QtySignNight.ToString();
                        txt_Real_HMP_Dem.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "HES")
                    {
                        txt_HES.Text = item.QtySignDay.ToString();
                        txt_Real_HES.Text = item.QtyRealDay.ToString();
                        txt_HES_Dem.Text = item.QtySignNight.ToString();
                        txt_Real_HES_Dem.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "IQ")
                    {
                        txt_IQ.Text = item.QtySignDay.ToString();
                        txt_Real_IQ.Text = item.QtyRealDay.ToString();
                        txt_IQ_Dem.Text = item.QtySignNight.ToString();
                        txt_Real_IQ_Dem.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "JWVSIP")
                    {
                        txt_JwVSIP.Text = item.QtySignDay.ToString();
                        txt_Real_JwVSIP.Text = item.QtyRealDay.ToString();
                        txt_Jw_DemVSIP.Text = item.QtySignNight.ToString();
                        txt_Real_Jw_DemVSIP.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "SFVSIP")
                    {
                        txt_SfVSIP.Text = item.QtySignDay.ToString();
                        txt_Real_SfVSIP.Text = item.QtyRealDay.ToString();
                        txt_Sf_DemVSIP.Text = item.QtySignNight.ToString();
                        txt_Real_Sf_DemVSIP.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "HMPVSIP")
                    {
                        txt_HMPVSIP.Text = item.QtySignDay.ToString();
                        txt_Real_HMPVSIP.Text = item.QtyRealDay.ToString();
                        txt_HMP_DemVSIP.Text = item.QtySignNight.ToString();
                        txt_Real_HMP_DemVSIP.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "HESVSIP")
                    {
                        txt_HESVSIP.Text = item.QtySignDay.ToString();
                        txt_Real_HESVSIP.Text = item.QtyRealDay.ToString();
                        txt_HES_DemVSIP.Text = item.QtySignNight.ToString();
                        txt_Real_HES_DemVSIP.Text = item.QtyRealNight.ToString();
                    }
                    if (item.Name == "IQVSIP")
                    {
                        txt_IQVSIP.Text = item.QtySignDay.ToString();
                        txt_Real_IQVSIP.Text = item.QtyRealDay.ToString();
                        txt_IQ_DemVSIP.Text = item.QtySignNight.ToString();
                        txt_Real_IQ_DemVSIP.Text = item.QtyRealNight.ToString();
                    }
                }
                conn.Close();
            }
        }

        string dateCheck = "";
        private void dp_Check_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dateCheck = DateTime.Parse(dp_Check.SelectedDate.ToString()).ToString("yyyy-MM-dd");
            Db_Read_ETC(dateCheck);
        }

        private void btnUploadEmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "SPGetDataAutoTimekeepingDay";
                //string query2 = "select * from update_employee";
                var listInsertData = DataProvider.Instance.ExecuteSP(Page_Main.path_Taixin, query);

                string queryToMysql = "SPGetDataAutoTimekeepingDayV2";
                //string query2 = "select * from update_employee";
                var listInsertDataV2 = DataProvider.Instance.ExecuteSP(Page_Main.path_Taixin, queryToMysql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Upload Data to Mysql");
            }
        }


        public class Dept
        {
            public string Name { get; set; }
            public int QtySignDay { get; set; }
            public int QtyRealDay { get; set; }
            public int QtySignNight { get; set; }
            public int QtyRealNight { get; set; }
            public string TypeShift { get; set; }
        }

        private void BtnToKsys_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "GetDataEmpInfoFromMYSQL";
                //string query2 = "select * from update_employee";
                var listInsertData = DataProvider.Instance.ExecuteSP(Page_Main.path_Taixin, query);

                string queryToKsys = "GetDataEmpInfoToKsys";
                //string query2 = "select * from update_employee";
                var listInsertDataV2 = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, queryToKsys);

                MessageBox.Show("Xử lý thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Upload Data to Ksystem");
            }
        }

        private async void BtnTimeKeeping_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Page_LoadingData page_Loading = new Page_LoadingData();
                stackLoading.Visibility = Visibility.Visible;
                frameLoading.Navigate(page_Loading);
                checkWorking = true;
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
                await Task.Run(() =>
                {
                    string query = "SPGetDataAutoTimekeepingDay";
                    //string query2 = "select * from update_employee";
                    var listInsertData = DataProvider.Instance.ExecuteSP(Page_Main.path_Taixin, query);

                    string queryToKsys = "SPGetDataAutoTimekeepingDayV2";
                    //string query2 = "select * from update_employee";
                    var listInsertDataV2 = DataProvider.Instance.ExecuteSP(Page_Main.path_Taixin, queryToKsys);
                });

                //Đóng Page_LoadingData
                await Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        stackLoading.Visibility = Visibility.Hidden;
                        checkWorking = false;
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);
                });
                MessageBox.Show("Xử lý thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Upload Data to Web");
            }
        }

        private void btnAddVendorVSIP_Click(object sender, RoutedEventArgs e)
        {

            int jw1VSIP = int.Parse(txt_JwVSIP.Text);
            int sf1VSIP = int.Parse(txt_SfVSIP.Text);
            int HMP1VSIP = int.Parse(txt_HMPVSIP.Text);
            int HES1VSIP = int.Parse(txt_HESVSIP.Text);
            int IQ1VSIP = int.Parse(txt_IQVSIP.Text);

            int jw2VSIP = int.Parse(txt_Real_JwVSIP.Text);
            int sf2VSIP = int.Parse(txt_Real_SfVSIP.Text);
            int HMP2VSIP = int.Parse(txt_Real_HMPVSIP.Text);
            int HES2VSIP = int.Parse(txt_Real_HESVSIP.Text);
            int IQ2VSIP = int.Parse(txt_Real_IQVSIP.Text);

            int jw1_DVSIP = int.Parse(txt_Jw_DemVSIP.Text);
            int sf1_DVSIP = int.Parse(txt_Sf_DemVSIP.Text);
            int HMP1_DVSIP = int.Parse(txt_HMP_DemVSIP.Text);
            int HES1_DVSIP = int.Parse(txt_HES_DemVSIP.Text);
            int IQ1_DVSIP = int.Parse(txt_IQ_DemVSIP.Text);

            int jw2_DVSIP = int.Parse(txt_Real_Jw_DemVSIP.Text);
            int sf2_DVSIP = int.Parse(txt_Real_Sf_DemVSIP.Text);
            int HMP2_DVSIP = int.Parse(txt_Real_HMP_DemVSIP.Text);
            int HES2_DVSIP = int.Parse(txt_Real_HES_DemVSIP.Text);
            int IQ2_DVSIP = int.Parse(txt_Real_IQ_DemVSIP.Text);

            List<Dept> list_Dept = new List<Dept>();

            list_Dept.Add(new Dept() { Name = "JWVSIP", QtySignDay = jw1VSIP, QtyRealDay = jw2VSIP, QtySignNight = jw1_DVSIP, QtyRealNight = jw2_DVSIP });
            list_Dept.Add(new Dept() { Name = "SFVSIP", QtySignDay = sf1VSIP, QtyRealDay = sf2VSIP, QtySignNight = sf1_DVSIP, QtyRealNight = sf2_DVSIP });
            list_Dept.Add(new Dept() { Name = "HMPVSIP", QtySignDay = HMP1VSIP, QtyRealDay = HMP2VSIP, QtySignNight = HMP1_DVSIP, QtyRealNight = HMP2_DVSIP });
            list_Dept.Add(new Dept() { Name = "HESVSIP", QtySignDay = HES1VSIP, QtyRealDay = HES2VSIP, QtySignNight = HES1_DVSIP, QtyRealNight = HES2_DVSIP });
            list_Dept.Add(new Dept() { Name = "IQSVIP", QtySignDay = IQ1VSIP, QtyRealDay = IQ2VSIP, QtySignNight = IQ1_DVSIP, QtyRealNight = IQ2_DVSIP });


            string date = DateTime.Parse(dp_Check.ToString()).ToString("yyyy-MM-dd");

            using (SqlConnection conn = new SqlConnection(Page_Main.path_Ksystem25))
            {
                conn.Open();
                //var command = "Delete tmmempetc where date='" + date + "' and (Dept='JW' or Dept ='SF')";
                var command = "Delete TDAEmpETC where DateEtc='" + date + "' and (DeptNm='JWVSIP' or DeptNm ='SFVSIP' or DeptNm ='HMPVSIP' or DeptNm ='HESVSIP' or DeptNm ='IQVSIP')";
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

            foreach (var item in list_Dept)
            {

                using (SqlConnection conn = new SqlConnection(Page_Main.path_Ksystem25))
                {
                    conn.Open();
                    //var command = "INSERT tmmempetc(Dept,qtySignDay,qtyRealDay,qtySignNight,qtyRealNight,date,insdt) VALUES(N'" + item.Name + "','" + item.QtySignDay + "','" + item.QtyRealDay + "','" + item.QtySignNight + "','" + item.QtyRealNight + "','" + date + "','" + DateTime.Now.ToString() + "')";
                    var command = "INSERT TDAEmpETC(DeptNm,qtySignDay,qtyRealDay,qtySignNight,qtyRealNight,DateEtc,insdt) VALUES(N'" + item.Name + "','" + item.QtySignDay + "','" + item.QtyRealDay + "','" + item.QtySignNight + "','" + item.QtyRealNight + "','" + date + "','" + DateTime.Now.ToString() + "')";
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void BtnSkipVendor_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
