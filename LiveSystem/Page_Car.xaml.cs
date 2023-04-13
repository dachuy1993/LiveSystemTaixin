using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Page_Car.xaml
    /// </summary>
    public partial class Page_Car : Page
    {
        public Page_Car()
        {
            InitializeComponent();
            //dpk_Check.SelectedDate = DateTime.Now;
            Db_Read_KilometCar();
            Loaded += Page_Car_Loaded;
        }

        private void Page_Car_Loaded(object sender, RoutedEventArgs e)
        {
            //Db_Read_KilometCar();
        }

        public static string carID = "";
        public static List<Helper_Car> List_Car = new List<Helper_Car>();
        List<Helper_Car> List_Car_Status = new List<Helper_Car>();
        List<Helper_Car> List_Car_Kilomet = new List<Helper_Car>();

        public void Db_Read_List_Car()
        {
            try
            {
                List_Car.Clear();
                using (MySqlConnection conn = new MySqlConnection(Page_Main.path_TaixinWeb))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM LiveSystem.emp_extra", conn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0] != null)
                                {
                                    Helper_Car car = new Helper_Car();
                                    car.CarID = dr[2].ToString();
                                    car.CarType = dr[3].ToString();
                                    car.Name = dr[4].ToString();
                                    car.Tel = dr[5].ToString();
                                    car.KmLimit = dr[7].ToString();
                                    car.Index = dr[8].ToString();
                                    car.Status = "Finish";
                                    car.Color = "Red";
                                    List_Car.Add(car);
                                }
                            }
                        }
                    }
                    conn.Close();
                }
                qtyCar.Content = "Số lượng xe : " + List_Car.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void Db_Read_StatusCar()
        {
            try
            {
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                List_Car = List_Car.OrderBy(x => x.Index).ToList();
                foreach (var item1 in List_Car)
                {
                    using (MySqlConnection conn = new MySqlConnection(Page_Main.path_TaixinWeb))
                    {
                        conn.Open();
                        var command = "SELECT * FROM LiveSystem.dieu_xe where Date >= '" + date + "' AND CarNo='" + item1.CarID + "' order by Date desc,StartTime desc limit 1";
                        using (MySqlCommand cmd = new MySqlCommand(command, conn))
                        {
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    if (dr[0] != null)
                                    {
                                        item1.CarID = dr[2].ToString();
                                        item1.Name = dr[3].ToString();
                                        item1.Tel = dr[4].ToString();
                                        item1.FirPos = dr[5].ToString();
                                        item1.EndPos = dr[7].ToString();
                                        item1.TimeOn = dr[13].ToString();
                                        item1.TimeEnd = dr[14].ToString();
                                        item1.NameOrder = dr[10].ToString() + dr[11].ToString();
                                        item1.DeptOrder = dr[16].ToString();
                                        item1.Status = dr[17].ToString();
                                        item1.Km = dr[18].ToString();
                                        if (item1.Status == "Running")
                                        {
                                            item1.Color = "DodgerBlue";
                                        }
                                        if (item1.Status == "Order")
                                        {
                                            item1.Color = "Orange";
                                        }
                                        if (item1.Status == "Finish")
                                        {
                                            item1.Color = "Red";
                                        }
                                    }
                                }
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void Db_Read_KilometCar()
        {
            try
            {
                Db_Read_List_Car();
                Db_Read_StatusCar();
                lvCar.ClearValue(ListView.ItemsSourceProperty);
                string date1 = "";
                string date2 = "";
                if(DateTime.Now.Month==1)
                {
                    if (int.Parse(DateTime.Now.Day.ToString()) <= 25)
                    {
                        date1 = DateTime.Now.ToString("yyyy-MM") + "-25";
                        date2 = DateTime.Now.AddYears(-1).ToString("yyyy") +"-12-26";
                        
                    }
                    else
                    {
                        date1 = DateTime.Now.AddYears(-1).ToString("yyyy") + "-12-31";
                        date2 = DateTime.Now.AddYears(-1).ToString("yyyy") + "-12-26";
                    }
                }  
                else
                {
                    if (int.Parse(DateTime.Now.Day.ToString()) <= 25)
                    {
                        date1 = DateTime.Now.ToString("yyyy-MM") + "-25";
                        date2 = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.AddMonths(-1).ToString("MM") + "-26";
                    }
                    else
                    {
                        date1 = DateTime.Now.ToString("yyyy-MM") + "-31";
                        date2 = DateTime.Now.ToString("yyyy-MM") + "-26";
                    }
                }                

                using (MySqlConnection conn = new MySqlConnection(Page_Main.path_TaixinWeb))
                {
                    conn.Open();
                    foreach (var item in List_Car)
                    {
                        using (MySqlCommand cmd = new MySqlCommand("SELECT sum(ResultKm) FROM LiveSystem.ql_km where CarNo='" + item.CarID + "' and Date>='" + date2 + "' and Date <= '" + date1 + "'", conn))
                        {
                            item.KmNumber = cmd.ExecuteScalar().ToString();
                            if (item.KmNumber == null || item.KmNumber == "")
                            {
                                item.KmNumber = "0";
                            }
                            item.KmUs = (int.Parse(item.KmLimit.ToString()) - int.Parse(item.KmNumber.ToString())).ToString();
                        }
                    }
                    conn.Close();
                }
                List_Car = List_Car.OrderBy(x => x.Index).ToList();
                int index = 0;
                foreach (var item in List_Car)
                {
                    item.ID = index++;
                }
                lvCar.ItemsSource = List_Car;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        public async void Process_Car()
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
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);

                });
                await Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Db_Read_KilometCar();
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);

                });
                await Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        stackLoading.Visibility = Visibility.Hidden;
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            lvCar.ClearValue(ListView.ItemsSourceProperty);
            List<Helper_Car> temp = new List<Helper_Car>();
            switch (status)
            {
                case "Run":
                    {
                        foreach (var item in List_Car)
                        {
                            if (item.Status == "Running")
                            {
                                temp.Add(item);
                            }
                        }
                        break;
                    }
                case "Stop":
                    {
                        foreach (var item in List_Car)
                        {
                            if (item.Status == "Finish")
                            {
                                temp.Add(item);
                            }
                        }
                        break;
                    }
                case "Order":
                    {
                        foreach (var item in List_Car)
                        {
                            if (item.Status == "Order")
                            {
                                temp.Add(item);
                            }
                        }
                        break;
                    }
                case "All":
                    {
                        foreach (var item in List_Car)
                        {
                            temp.Add(item);
                        }
                        break;
                    }
            }
            int index = 0;
            foreach (var item in temp)
            {
                item.ID = index++;
            }
            if (txt_CarID.Text != "")
            {
                temp = temp.Where(x => x.CarID.ToUpper().Contains(txt_CarID.Text.ToUpper())).ToList();
            }
            lvCar.ItemsSource = temp;
        }
        string status = "";
        private void rb_On_Checked(object sender, RoutedEventArgs e)
        {
            status = "Run";
        }

        private void rb_Off_Checked(object sender, RoutedEventArgs e)
        {
            status = "Stop";
        }

        private void rb_Order_Checked(object sender, RoutedEventArgs e)
        {
            status = "Order";
        }
        private void rb_All_Checked(object sender, RoutedEventArgs e)
        {
            status = "All";
        }

        private void lvCar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var click = sender as ListView;
            var clickItem = click.SelectedItem as Helper_Car;
            if (clickItem != null)
            {
                carID = clickItem.CarID;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var click = sender as Button;
            //var clickItem = click.DataContext as Helper_Car;
            //carID=clickItem.CarID;
            //Window_Car  car = new Window_Car();
            //car.Show();
        }


    }
}

