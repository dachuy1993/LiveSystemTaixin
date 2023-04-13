using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for Page_Note.xaml
    /// </summary>
    public partial class Page_Note : Page
    {
        Grid GridMain = new Grid();
        Grid GridLeft = new Grid();
        StackPanel GridHead = new StackPanel();
        Grid GridWeekHead = new Grid();
        public static Grid GridCalenda = new Grid();
        DatePicker datePicker = new DatePicker();
        static string[] week = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        string[] month = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public static Thickness margin = new Thickness();
        public static Thickness margin1 = new Thickness();
        List<Button> ListButton_Month = new List<Button>();
        public static string dateSelect = "";
        public Page_Note()
        {
            InitializeComponent();
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Thread.CurrentThread.CurrentCulture = ci;
            Loaded += Page_Note_Loaded;
        }

        private void Page_Note_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MainWindow.language);
            ApplyLanguage(MainWindow.language);
            //Db_Read_HolidayList();            
            if (MainWindow.checkNote == false)
            {
                //Thiết kế giao diện hiển thị==========================================================
                margin.Top = 3;
                margin.Bottom = 3;
                margin.Right = 3;
                margin.Left = 3;
                margin1.Top = 5;
                margin1.Bottom = 2;
                margin1.Right = 2;
                margin1.Left = 5;
                RowDefinition row1 = new RowDefinition();
                RowDefinition row2 = new RowDefinition();
                RowDefinition row3 = new RowDefinition();
                ColumnDefinition col1 = new ColumnDefinition();
                ColumnDefinition col2 = new ColumnDefinition();
                col1.Width = new GridLength(150);
                row1.Height = new GridLength(0);
                row2.Height = new GridLength(40);
                GridMain.RowDefinitions.Add(row1);
                GridMain.RowDefinitions.Add(row2);
                GridMain.RowDefinitions.Add(row3);
                GridMain.ColumnDefinitions.Add(col1);
                GridMain.ColumnDefinitions.Add(col2);
                GridHead.HorizontalAlignment = HorizontalAlignment.Left;
                GridHead.Orientation = Orientation.Horizontal;
                datePicker.Height = 0;
                datePicker.Width = 0;
                datePicker.HorizontalContentAlignment = HorizontalAlignment.Center;
                datePicker.VerticalContentAlignment = VerticalAlignment.Center;
                datePicker.Margin = margin;
                datePicker.SelectedDateChanged += DateCheck_SelectedDateChanged;
                GridHead.Children.Add(datePicker);
                Button btn_Update = new Button();
                btn_Update.Width = 100;
                btn_Update.Height = 30;
                btn_Update.Content = "Update";
                btn_Update.Click += BtnTimKiem_Click;
                GridHead.Children.Add(btn_Update);
                datePicker.SelectedDate = DateTime.Now;
                for (int i = 0; i <= 5; i++)
                {
                    RowDefinition row = new RowDefinition();
                    GridCalenda.RowDefinitions.Add(row);
                }
                for (int i = 0; i <= 6; i++)
                {
                    ColumnDefinition colum = new ColumnDefinition();
                    GridCalenda.ColumnDefinitions.Add(colum);
                }
                Grid.SetRow(GridHead, 0);
                Grid.SetRow(GridWeekHead, 1);
                Grid.SetRow(GridCalenda, 2);
                Grid.SetColumn(GridHead, 1);
                Grid.SetColumn(GridWeekHead, 1);
                Grid.SetColumn(GridCalenda, 1);


                //Button tăng giảm tháng cần xem=======================================================

                Button btnInc = new Button();
                Button btnDec = new Button();
                btnInc.Click += BtnInc_Click;
                btnDec.Click += BtnDec_Click;
                btnInc.Width = 30;
                btnDec.Width = 30;
                btnDec.Background = Brushes.White;
                btnInc.Background = Brushes.White;
                btnDec.BorderThickness = new Thickness(0, 0, 0, 0);
                btnInc.BorderThickness = new Thickness(0, 0, 0, 0);
                btnDec.FontSize = 16;
                btnInc.FontSize = 16;
                btnDec.FontWeight = FontWeights.Bold;
                btnInc.FontWeight = FontWeights.Bold;
                btnDec.Content = "<";
                btnInc.Content = ">";
                btnDec.Margin = new Thickness(10, 0, 0, 0);
                lbl.Content = year;
                lbl.Width = 70;
                lbl.FontSize = 18;
                lbl.FontWeight = FontWeights.Bold;
                lbl.VerticalContentAlignment = VerticalAlignment.Center;
                lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                StackPanel stackYear = new StackPanel();
                stackYear.Orientation = Orientation.Horizontal;
                stackYear.Children.Add(btnDec);
                stackYear.Children.Add(lbl);
                stackYear.Children.Add(btnInc);
                Grid.SetRow(stackYear, 1);
                GridMain.Children.Add(stackYear);

                //Tên các tháng trong năm=======================================================

                StackPanel stackPanelMonth = new StackPanel();
                foreach (var item in month)
                {
                    Button btnMonth = new Button();
                    btnMonth.Background = Brushes.Lavender;
                    DateTimeFormatInfo mfi = new DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(DateTime.Now.Month).ToUpper();
                    if (item.ToUpper() == strMonthName)
                    {
                        btnMonth.Background = Brushes.LightGreen;
                    }
                    btnMonth.Foreground = Brushes.Black;
                    btnMonth.Content = item;
                    btnMonth.FontSize = 15;
                    btnMonth.FontWeight = FontWeights.Bold;
                    //btnMonth.Width = 120;
                    btnMonth.Height = 35;
                    btnMonth.Margin = new Thickness(5, 5, 5, 5);
                    btnMonth.Click += BtnMonth_Click;
                    stackPanelMonth.Children.Add(btnMonth);
                    ListButton_Month.Add(btnMonth);
                }
                Grid.SetRow(stackPanelMonth, 2);
                Grid.SetColumn(stackPanelMonth, 0);
                GridMain.Children.Add(stackPanelMonth);
                GridMain.Children.Add(GridHead);
                GridMain.Children.Add(GridWeekHead);
                GridMain.Children.Add(GridCalenda);
                CreateWeekHead();
                CreateCalendar(DateTime.Now.ToString("yyyy-MM-dd"));
                this.Content = GridMain;
                MainWindow.checkNote = true;
            }
        }

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

        private void BtnMonth_Click(object sender, RoutedEventArgs e)
        {
            int _index = 0;
            var click = sender as Button;
            foreach (var item in ListButton_Month)
            {
                item.Background = Brushes.Lavender;
            }
            if (click != null)
            {
                click.Background = Brushes.LightGreen;
            }
            foreach (var item in month)
            {
                _index++;
                if (click.Content.ToString() == item)
                {
                    break;
                }
            }
            dateSelect = DateTime.Parse(year.ToString() + "-" + _index.ToString("00") + "-01").ToString("yyyy-MM-dd");
            CreateCalendar(dateSelect);
        }

        Label lbl = new Label();
        int year = DateTime.Now.Year;
        private void BtnDec_Click(object sender, RoutedEventArgs e)
        {
            year--;
            lbl.Content = year.ToString();
        }

        private void BtnInc_Click(object sender, RoutedEventArgs e)
        {
            year++;
            lbl.Content = year.ToString();
        }

        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            CreateCalendar(dateSelect);
        }

        static string path_SQL = @"Data Source=192.168.2.5;Initial Catalog=LiveSystem;Persist Security Info=True;User ID=sa;Password=oneuser1!;Connect Timeout=60";

        public static List<Helper_CalendarData> ListNoteData = new List<Helper_CalendarData>();
        public static List<Helper_CalendarData> ListHoliday = new List<Helper_CalendarData>();

        //Read data theo từng ngày=============================================================================
        public static void Db_Read_Note()
        {
            ListNoteData.Clear();
            string _date1 = dateSelect.Substring(0, 8) + "00";
            string _date2 = dateSelect.Substring(0, 8) + "31";
            using (SqlConnection conn = new SqlConnection(path_SQL))
            {
                conn.Open();
                var command = "select * from tmmcalnote where Date >= '" + _date1 + "' and Date <= '" + _date2 + "'";
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
                                ListNoteData.Add(item);

                            }
                        }
                    }
                }
                conn.Close();
            }
        }


        //Thông tin những ngày đặc biệt========================================================================
        public void Db_Read_HolidayList()
        {
            ListHoliday.Clear();
            using (SqlConnection conn = new SqlConnection(path_SQL))
            {
                conn.Open();
                var command = "select * from tmmhdes where etc1=N'Dương lịch'";
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr[0] != null)
                            {
                                Helper_CalendarData item = new Helper_CalendarData();
                                string date = dr[1].ToString().Trim().Substring(0, 2);
                                string month = dr[1].ToString().Trim().Substring(3, 2);
                                item.etc1 = date;
                                item.etc2 = month;
                                item.Note = dr[2].ToString();
                                ListHoliday.Add(item);
                            }
                        }
                    }
                }
                conn.Close();
            }
        }

        //Create UI ngày trong tuần============================================================================
        public void CreateWeekHead()
        {
            string[] week = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            for (int i = 0; i <= 6; i++)
            {
                ColumnDefinition colum = new ColumnDefinition();
                GridWeekHead.ColumnDefinitions.Add(colum);
            }

            for (int i = 0; i <= 6; i++)
            {
                Grid grid = new Grid();
                Border boder = new Border();
                boder.Background = Brushes.Lavender;
                boder.Height = 30;
                boder.BorderBrush = Brushes.Gray;
                boder.BorderThickness = new Thickness(0.5);
                boder.Margin = margin;
                boder.CornerRadius = new CornerRadius(3, 3, 3, 3);
                grid.Children.Add(boder);
                TextBlock txb = new TextBlock();
                txb.FontSize = 15;
                txb.Foreground = Brushes.Black;
                txb.FontWeight = FontWeights.Bold;
                txb.VerticalAlignment = VerticalAlignment.Center;
                txb.HorizontalAlignment = HorizontalAlignment.Center;
                txb.TextAlignment = TextAlignment.Center;
                txb.Text = week[i].ToString();
                grid.Children.Add(txb);
                Grid.SetColumn(grid, i);
                Grid.SetRow(grid, 1);
                GridWeekHead.Children.Add(grid);
            }
        }

        //Create UI ngày trong tháng hiển thị chi tiết các ngày trong tháng kèm theo dữ liệu từng ngày=========
        public static void CreateCalendar(string date)
        {
            Db_Read_Note();
            GridCalenda.Children.Clear();
            List<Helper_Calendar> ListDayMonth = new List<Helper_Calendar>();
            int posWeek = 0;
            int id = 0;
            int index = 0;
            string year = DateTime.Parse(date).Year.ToString("0000"); //DateTime.Now.Month.ToString("00");
            string month = DateTime.Parse(date).Month.ToString("00"); //DateTime.Now.Month.ToString("00");
            string dayOfWeek = DateTime.Parse(date).Year.ToString("0000") + month + "01";
            var dateValue = DateTime.ParseExact(dayOfWeek, "yyyyMMdd", CultureInfo.InvariantCulture);
            int days = DateTime.DaysInMonth(int.Parse(year), int.Parse(month));
            foreach (var item in week)
            {
                posWeek++;
                if (dateValue.DayOfWeek.ToString() == item)
                {
                    posWeek = posWeek - 1;
                    break;
                }
            }
            int _qtyHang = (days + posWeek - 1) / 7;

            foreach (var item in ListDayMonth)
            {
                item.TextBoxDay.Text = "";
                item.TextBoxNote.Text = "";
            }
            for (int hang = 0; hang <= 5; hang++)
            {
                for (int cot = 0; cot <= 6; cot++)
                {
                    Helper_Calendar cal = new Helper_Calendar();
                    if (index == days) index = 0;
                    index++;
                    id++;
                    Grid grid = new Grid();
                    RowDefinition r1 = new RowDefinition();
                    RowDefinition r2 = new RowDefinition();
                    RowDefinition r3 = new RowDefinition();
                    r1.Height = new GridLength(20);
                    r2.Height = new GridLength(0);
                    grid.RowDefinitions.Add(r1);
                    grid.RowDefinitions.Add(r2);
                    grid.RowDefinitions.Add(r3);
                    Border boder = new Border();
                    boder.BorderBrush = Brushes.Gray;
                    boder.Background = Brushes.Linen;
                    boder.BorderThickness = new Thickness(1);
                    boder.Margin = margin;
                    boder.CornerRadius = new CornerRadius(3, 3, 3, 3);
                    TextBlock txb_Day = new TextBlock();
                    txb_Day.Height = 30;
                    txb_Day.FontSize = 15;
                    txb_Day.Foreground = new SolidColorBrush(Color.FromArgb(255, 54, 54, 54));
                    txb_Day.FontWeight = FontWeights.Bold;
                    txb_Day.VerticalAlignment = VerticalAlignment.Top;
                    txb_Day.HorizontalAlignment = HorizontalAlignment.Left;
                    txb_Day.TextAlignment = TextAlignment.Center;
                    txb_Day.Margin = new Thickness(7, 2, 0, 0);
                    //txb.Name = "A"+id.ToString();
                    txb_Day.MouseDown += Txb_MouseDown;
                    Grid.SetRow(txb_Day, 0);


                    TextBlock txb_HolidayNote = new TextBlock();
                    txb_HolidayNote.Height = 20;
                    txb_HolidayNote.FontSize = 10;
                    txb_HolidayNote.Foreground = new SolidColorBrush(Color.FromArgb(255, 69, 69, 69));
                    txb_HolidayNote.VerticalAlignment = VerticalAlignment.Top;
                    txb_HolidayNote.HorizontalAlignment = HorizontalAlignment.Left;
                    txb_HolidayNote.TextAlignment = TextAlignment.Center;
                    txb_HolidayNote.TextWrapping = TextWrapping.Wrap;
                    txb_HolidayNote.Margin = new Thickness(5, 0, 0, 0);
                    Grid.SetRow(txb_HolidayNote, 1);

                    TextBlock txb_DataNote = new TextBlock();
                    txb_DataNote.Background = Brushes.AliceBlue;
                    txb_DataNote.Margin = new Thickness(5);
                    txb_DataNote.TextWrapping = TextWrapping.Wrap;
                    txb_DataNote.MouseDown += Txb_MouseDown;
                    grid.Children.Add(boder);
                    grid.Children.Add(txb_DataNote);
                    grid.Children.Add(txb_Day);
                    grid.Children.Add(txb_HolidayNote);
                    Grid.SetRow(txb_DataNote, 2);
                    Grid.SetRowSpan(boder, 3);
                    Grid.SetColumn(grid, cot);
                    Grid.SetRow(grid, hang);
                    cal.Border = boder;
                    cal.TextBoxDay = txb_Day;
                    cal.TextBoxHoliday = txb_HolidayNote;
                    cal.TextBoxNote = txb_DataNote;
                    ListDayMonth.Add(cal);
                    GridCalenda.Children.Add(grid);
                }

            }

            foreach (var item in ListDayMonth)
            {
                item.TextBoxDay.Text = "";
                item.TextBoxNote.Text = "";
            }
            for (int i = posWeek; i < days + posWeek; i++)
            {
                ListDayMonth[i].ID = i;
                ListDayMonth[i].TextBoxDay.Text = (i - posWeek + 1).ToString();
                ListDayMonth[i].TextBoxNote.Name = "A" + (i - posWeek + 1).ToString();
                ListDayMonth[i].TextBoxDay.Name = "A" + (i - posWeek + 1).ToString();
                foreach (var item in ListNoteData)
                {
                    if (int.Parse(item.Date.Substring(8, 2)) == (i - posWeek + 1))
                    {
                        //ListDayMonth[i].TextBoxNote.Text = (item.dateFrom + "->" + item.dateTo + "\r\n" + item.Note);
                        ListDayMonth[i].TextBoxNote.Text = (item.Note);
                        ListDayMonth[i].TextBoxNote.Background = Brushes.GreenYellow;
                    }
                }
            }
            foreach (var item1 in ListDayMonth)
            {
                foreach (var item2 in ListHoliday)
                {
                    if (item1.TextBoxDay.Text.Length > 0)
                    {
                        if (int.Parse(item1.TextBoxDay.Text.ToString().Trim()).ToString("00") == item2.etc1 && month == item2.etc2)
                        {
                            //if (item2.Note.Length < 45)
                            //item1.TextBoxHoliday.Text = item2.Note;
                        }
                    }
                }
            }

            foreach (var item in ListDayMonth)
            {
                if (item.TextBoxDay.Text == "")
                {
                    item.Border.Background = Brushes.White;
                    item.TextBoxDay.Background = Brushes.White;
                    item.TextBoxHoliday.Background = Brushes.White;
                    item.TextBoxNote.Background = Brushes.White;
                    item.Border.BorderThickness = new Thickness(0);

                }
            }
            foreach (var item in ListDayMonth)
            {
                if (item.TextBoxDay.Text == DateTime.Now.Day.ToString())
                {
                    item.TextBoxNote.Background = Brushes.Orange;
                }
            }
        }

        public static string dateView = "";

        private void DateCheck_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dateSelect = DateTime.Parse(datePicker.SelectedDate.ToString()).ToString("yyyy-MM-dd");
            CreateCalendar(dateSelect);
        }

        public static int NoteIndex = 0;
        private static void Txb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var click = sender as TextBlock;
            if (click.Name.Length > 0)
            {
                int day = int.Parse(click.Name.Substring(1, click.Name.Length - 1));
                dateView = dateSelect.Substring(0, 8) + day.ToString("00");
                if (NoteIndex == 0)
                {
                    Page_NoteDetail note = new Page_NoteDetail();
                    note.Show();
                }
            }

        }
    }
    public class Helper_Calendar
    {
        public int ID { get; set; }
        public TextBlock TextBoxDay { get; set; }
        public TextBlock TextBoxNote { get; set; }
        public TextBlock TextBoxHoliday { get; set; }
        public Border Border { get; set; }
    }
}
