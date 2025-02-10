using LiveSystem.DAO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Page_CheckPermission.xaml
    /// </summary>
    public partial class Page_CheckPermission : Page
    {
        bool checkLogin = false;
        bool Check_user = false;
        bool CheckRememberLogin = false;
        bool CheckShowPass = false;
        public Page_CheckPermission()
        {
            InitializeComponent();
            pb_Pass.Visibility = Visibility.Visible;
            txtPass.Visibility = Visibility.Hidden;
            btnShowPass.Visibility = Visibility.Hidden;
            btnHidenPass.Visibility = Visibility.Visible;
        }

        private void Btn_Confirm(object sender, RoutedEventArgs e)
        {
            User_Login();
            User_Check();
            if (checkLogin == true && Check_user == true)
            {
                Window_EmployerEtc emp = new Window_EmployerEtc();
                emp.Show();

            }
            else
            {
                MessageBox.Show("Bạn không có quyền để mở chức năng này", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }    
        }

        public void User_Login()
        {
            try
            {
                Db_Read_Employee();
            }
            catch (Exception ex)
            {
                MessageBox.Show("CheckLoginInput : " + ex.Message, "Login/MainWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void User_Check()
        {
            try
            {
                Check_Employee();
            }
            catch (Exception ex)
            {
                MessageBox.Show("CheckLoginInput : " + ex.Message, "Login/MainWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Check_Employee()
        {
            try
            {
                Check_user = false;
                string path_Ksystem20 = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";
                List<string> list = new List<string>();
                using (SqlConnection conn = new SqlConnection(path_Ksystem20))
                {
                    conn.Open();
                    {
                        var command = "select RTRIM(EmpId) from TDAEmpMaster where (DeptCd = N'00104' AND RetDate = '99991231') or EmpId = 'P03002'";
                        using (SqlCommand cmd = new SqlCommand(command, conn))
                        {
                            using (IDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    list.Add(dr[0].ToString());
                                    if (dr[0] != null)
                                    {
                                        if (txtUser.Text.ToUpper() == dr[0].ToString().Trim().ToUpper())
                                        {
                                            Check_user = true;
                                        }
                                    }
                                }

                            }
                        }

                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ReadVersion_SQLserver" + ex.Message, "Login/MainWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Db_Read_Employee()
        {
            try
            {
                checkLogin = false;
                string path_TaixinWeb = "server=192.168.2.40;Port=3307;user id=txadmin;database=LiveSystem;password=Taixinweb1!";
                List<string> list = new List<string>();
                using (MySqlConnection conn = new MySqlConnection(path_TaixinWeb))
                {
                    conn.Open();
                    {
                        var command = "SELECT * FROM employee";
                        using (MySqlCommand cmd = new MySqlCommand(command, conn))
                        {
                            using (IDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    list.Add(dr[0].ToString());
                                    if (dr[0] != null)
                                    {
                                        if (txtUser.Text.ToUpper() == dr[0].ToString().Trim().ToUpper() && (txtPass.Text.ToUpper() == dr[1].ToString().Trim().ToUpper() || pb_Pass.Password.ToUpper() == dr[1].ToString().Trim().ToUpper()))
                                        {
                                            checkLogin = true;
                                        }
                                    }
                                }

                            }
                        }

                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ReadVersion_SQLserver" + ex.Message, "Login/MainWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Pb_Pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                User_Login();
            }
        }

        private void CkbRemember_Checked(object sender, RoutedEventArgs e)
        {
            txtUser.IsEnabled = false;
            txtPass.IsEnabled = false;
            pb_Pass.IsEnabled = false;
        }

        private void TxtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                User_Login();
            }
        }

        private void Txt_User_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pb_Pass.Focus();
            }
        }

        private void Btn_ShowPass_Click(object sender, RoutedEventArgs e)
        {
            pb_Pass.Password = txtPass.Text;
            pb_Pass.Visibility = Visibility.Visible;
            txtPass.Visibility = Visibility.Hidden;
            btnShowPass.Visibility = Visibility.Hidden;
            btnHidenPass.Visibility = Visibility.Visible;
            CheckShowPass = false;
        }

        private void CkbRemember_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckRememberLogin = false;
            txtPass.Text = "";
            txtUser.Text = "";
            pb_Pass.Password = "";
            ckbRemember.IsChecked = false;
            txtUser.IsEnabled = true;
            txtPass.IsEnabled = true;
            pb_Pass.IsEnabled = true;
            btnHidenPass.IsEnabled = true;
        }

        private void Btn_HidenPass_Click(object sender, RoutedEventArgs e)
        {
            txtPass.Text = pb_Pass.Password;
            pb_Pass.Visibility = Visibility.Hidden;
            txtPass.Visibility = Visibility.Visible;
            btnShowPass.Visibility = Visibility.Visible;
            btnHidenPass.Visibility = Visibility.Hidden;
            CheckShowPass = true;
        }


    }
}
