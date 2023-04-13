using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
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
using System.Windows.Shapes;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for Page_NoteDetail.xaml
    /// </summary>
    public partial class Page_NoteDetail : Window
    {
        public Page_NoteDetail()
        {
            InitializeComponent();
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Thread.CurrentThread.CurrentCulture = ci;
            CreatAllButtonEdit();
            Db_ReadNote();
            Page_Note.NoteIndex = 1;
        }
        List<Helper_DataButton> listButtonTop = new List<Helper_DataButton>();
        string dateFromTo = "";
        public void CreatAllButtonEdit()
        {
            lvButtonTop.Items.Clear();
            listButtonTop.Clear();
            //listButtonTop.Add(new Helper_DataButton
            //{
            //    ID = 1,
            //    ContentButton = "Add",
            //    ImageSource = "Image/Edit/add.png",
            //    BackGroundColor = PinValue.OFF
            //});            
            listButtonTop.Add(new Helper_DataButton
            {
                ID = 4,
                ContentButton = "Create",
                ImageSource = "Image/Edit/save.png",
                BackGroundColor = PinValue.OFF
            });
            listButtonTop.Add(new Helper_DataButton
            {
                ID = 3,
                ContentButton = "Edit",
                ImageSource = "Image/Edit/edit.png",
                BackGroundColor = PinValue.OFF
            });
            listButtonTop.Add(new Helper_DataButton
            {
                ID = 5,
                ContentButton = "Del",
                ImageSource = "Image/Edit/delete.png",
                BackGroundColor = PinValue.OFF
            });
            listButtonTop.Add(new Helper_DataButton
            {
                ID = 5,
                ContentButton = "Date",
                ImageSource = "Image/Edit/add.png",
                BackGroundColor = PinValue.OFF
            });
            //listButtonTop.Add(new Helper_DataButton
            //{
            //    ID = 6,
            //    ContentButton = "Check",
            //    ImageSource = "Image/Edit/check.png",
            //    BackGroundColor = PinValue.OFF
            //});
            foreach (var button in listButtonTop)
            {
                lvButtonTop.Items.Add(button);
            }

        }

        private void ButtonTop_Click(object sender, RoutedEventArgs e)
        {
            var click = sender as Button;
            var clickItem = click.DataContext as Helper_DataButton;
            if (clickItem != null)
            {
                switch (clickItem.ContentButton)
                {
                    //case "Add":
                    //    {
                    //        processButton = "Add";
                    //        ProcessButtonEdit_Add();
                    //        break;
                    //    }
                    case "Del":
                        {                          
                            ProcessButtonEdit_Del();
                            break;
                        }
                    case "Edit":
                        {
                            ProcessButtonEdit_Edit();
                            break;
                        }
                    case "Create":
                        {
                            ProcessButtonEdit_Save();
                            break;
                        }
                    case "Date":
                        {                           
                            ProcessButtonEdit_Date();
                            break;
                        }
                        //case "Run":
                        //    {
                        //        processButton = "Run";
                        //        ProcessButtonEdit_Run();
                        //        break;
                        //    }
                }
                foreach (var button in listButtonTop)
                {
                    button.BackGroundColor = PinValue.OFF;
                    if (button.ContentButton == clickItem.ContentButton)
                    {
                        button.BackGroundColor = PinValue.ON;
                    }
                }

            }
        }

       

        string path_SQL = @"Data Source=192.168.2.5;Initial Catalog=LiveSystem;Persist Security Info=True;User ID=sa;Password=oneuser1!;Connect Timeout=60";

        bool checkData = false;
        public void Db_ReadNote()
        {
            txb_Note.Text = "";
            txt_Note.Text = "";
            string dateSelect = Page_Note.dateView;
            using (SqlConnection conn = new SqlConnection(path_SQL))
            {
                conn.Open();
                var command = "select * from tmmcalnote where Date = '" + dateSelect + "'";
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr[0] != null)
                            {
                                Helper_CalendarData item = new Helper_CalendarData();
                                item.Id = dr[0].ToString();
                                item.EmpId = dr[1].ToString();
                                item.EmpNm = dr[2].ToString();
                                item.Depatment = dr[3].ToString();
                                item.NoteNo = dr[4].ToString();
                                item.dateFrom = dr[5].ToString();
                                item.dateTo = dr[6].ToString();
                                item.timeFrom = dr[7].ToString();
                                item.timeTo = dr[8].ToString();
                                item.addrFrom = dr[9].ToString();
                                item.addrTo = dr[10].ToString();
                                item.Note = dr[11].ToString();
                                item.Date = dr[12].ToString();
                                item.etc1 = dr[13].ToString();
                                item.etc2 = dr[14].ToString();
                                item.etc3 = dr[15].ToString();
                                item.etc4 = dr[16].ToString();
                                item.etc5 = dr[17].ToString();
                                item.etc6 = dr[18].ToString();
                                item.etc7 = dr[19].ToString();
                                item.etc8 = dr[20].ToString();
                                item.etc9 = dr[21].ToString();
                                item.Insdt = dr[22].ToString();
                                dpk_From.SelectedDate = DateTime.Parse(item.dateFrom);
                                dpk_To.SelectedDate = DateTime.Parse(item.dateTo);
                                txb_Note.Text = "Date : " + item.dateFrom + "->" + item.dateTo;
                                txt_Note.Text = item.Note;
                                checkData = true;
                            }
                        }
                    }
                }
                conn.Close();
            }
            if (checkData == false)
            {
                dpk_From.SelectedDate = DateTime.Now;
                dpk_To.SelectedDate = DateTime.Now;
            }


        }
        string dateCreate = "";
        public void Db_CheckCreate()
        {
            using (SqlConnection conn = new SqlConnection(path_SQL))
            {
                conn.Open();
                var command = "SELECT COUNT(Date) FROM tmmcalnote where Date = '" + Page_Note.dateView + "'";
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    dateCreate =  cmd.ExecuteScalar().ToString();
                }
                conn.Close();
            }
        }

        public void ProcessButtonEdit_Save()
        {
            Db_CheckCreate();
            if (dateCreate == "0")
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Bạn có muốn lưu công việc không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question))
                {
                    string NoteNo = "20220930-001";
                    string dateSelect = Page_Note.dateView;
                    string dateFrom = DateTime.Parse(dpk_From.SelectedDate.ToString()).ToString("yyyy-MM-dd");
                    string dateTo = DateTime.Parse(dpk_To.SelectedDate.ToString()).ToString("yyyy-MM-dd");
                    string note = txt_Note.Text;
                    string insdt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    using (SqlConnection conn = new SqlConnection(path_SQL))
                    {
                        conn.Open();
                        var command = "Insert tmmcalnote(NoteNo,dateFrom,dateTo,Note,Date,OpType,UserId,Insdt) Values('" + NoteNo + "','" + dateFrom + "','" + dateTo + "',N'" + note + "','" + dateSelect + "','Save','" + MainWindow.EmpId + "','" + insdt + "')";
                        using (SqlCommand cmd = new SqlCommand(command, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                    MessageBox.Show("Lưu công việc thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Công việc đã được tạo. Chỉ có thể chỉnh sửa", "통보", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void ProcessButtonEdit_Del()
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Bạn có muốn xoá dữ liệu không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {               
                string dateSelect = Page_Note.dateView;              
                using (SqlConnection conn = new SqlConnection(path_SQL))
                {
                    conn.Open();
                    var command = "Update tmmcalnote SET Date='9999-01-01',OpType='Delete',UserId='" + MainWindow.EmpId+"',Insdt='"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Date='" + dateSelect + "'";
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                Db_ReadNote();
                MessageBox.Show("Xoá dữ liệu thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void ProcessButtonEdit_Edit()
        {
            if (checkData == true)
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Bạn có muốn chỉnh sửa dữ liệu không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question))
                {
                    string NoteNo = "20220930-001";
                    string dateSelect = Page_Note.dateView;
                    string dateFrom = DateTime.Parse(dpk_From.SelectedDate.ToString()).ToString("yyyy-MM-dd");
                    string dateTo = DateTime.Parse(dpk_To.SelectedDate.ToString()).ToString("yyyy-MM-dd");
                    string note = txt_Note.Text;
                    string insdt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    using (SqlConnection conn = new SqlConnection(path_SQL))
                    {
                        conn.Open();
                        var command = "UPDATE tmmcalnote SET dateFrom='" + dateFrom + "',dateTo='" + dateTo + "',Note=N'" + note + "',Date='" + dateSelect + "',OpType='Update',UserId='" + MainWindow.EmpId+"',Insdt='" + insdt + "' where date='" + dateSelect + "'";
                        using (SqlCommand cmd = new SqlCommand(command, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                    Db_ReadNote();
                    MessageBox.Show("Sửa dữ liệu thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Cần tạo dữ liệu trước khi chỉnh sửa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void ProcessButtonEdit_Date()
        {
            string dateFrom = DateTime.Parse(dpk_From.SelectedDate.ToString()).ToString("MM-dd");
            string dateTo = DateTime.Parse(dpk_To.SelectedDate.ToString()).ToString("MM-dd");
            dateFromTo = dateFrom + " -> " + dateTo + " : ";
            txt_Note.Text = txt_Note.Text + dateFromTo;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Page_Note.NoteIndex = 0;
            Page_Note.CreateCalendar(Page_Note.dateSelect);

        }

        private void dpk_From_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            txb_Note.Text = "";
            //txb_Note.Text = "Date : "+DateTime.Parse(dpk_From.SelectedDate.ToString()).ToString("yyyy-MM-dd") + "->" + DateTime.Parse(dpk_To.SelectedDate.ToString()).ToString("yyyy-MM-dd");
        }

        private void dpk_To_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            txb_Note.Text = "";
            txb_Note.Text = "Date : " + DateTime.Parse(dpk_From.SelectedDate.ToString()).ToString("yyyy-MM-dd") + "->" + DateTime.Parse(dpk_To.SelectedDate.ToString()).ToString("yyyy-MM-dd");
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {

        }
    }
}
