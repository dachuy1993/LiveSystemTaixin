﻿using LiveCharts;
using LiveCharts.Defaults;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow
    {


        #region Khái báo biến
        public static List<Helper_DataButton> ListButton_Header = new List<Helper_DataButton>();
        DispatcherTimer dt = new DispatcherTimer();
        Page_Main main = new Page_Main();
        Page_Covid vac = new Page_Covid();
        Page_Address tt = new Page_Address();
        Page_Covid yte = new Page_Covid();
        Page_Map map = new Page_Map();
        Page_Car car = new Page_Car();        
        public static Page_WorkingRate work = new Page_WorkingRate();
        Page_OverTime OT = new Page_OverTime();
        Page_Note Note = new Page_Note();
        public static Page_Training Training = new Page_Training();
        public static Page_Food Food = new Page_Food();
        Page_Holiday holiday  = new Page_Holiday();
        int _indexCheckInternet = 0;
        string pathFileIni = "";
        int checkID = 0;
        public static string language = "vi-VN";
        public static string _checkInternet = "Success";
        public static string EmpId = "";
        #endregion
        public static string Ver = "V9.11";
        public static bool checkOne = false;
        public static bool checkNote = false;
        bool checkWorking = false;
        public MainWindow()
        {
            InitializeComponent();     
            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(language);
            ApplyLanguage(language);
            frameMain.Navigate(main);         
            lb_Version.Content = "Version : " + Ver;
        }
      
        private void ApplyLanguage(string cultureName = null)
        {
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
            CreatButton_Header();
        }   
       
        public void CreatButton_Header()
        {
            lvButtonTop.ClearValue(ListView.ItemsSourceProperty);
            ListButton_Header.Clear();
            if (language == "vi-VN")
            {
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 1,
                    ContentButton = "Trang chủ",
                    ImageSource = "Image/Dep/Home.png",
                    BackGroundColor = PinValue.ON
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 2,
                    ContentButton = "Thông Tin",
                    ImageSource = "Image/Dep/HR.png",
                    BackGroundColor = PinValue.OFF
                });
                //ListButton_Header.Add(new Helper_DataButton
                //{
                //    ID = 3,
                //    ContentButton = "Y tế",
                //    ImageSource = "Image/Dep/yte.png",
                //    BackGroundColor = PinValue.OFF
                //});
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 4,
                    ContentButton = "Vacxin",
                    ImageSource = "Image/Dep/vaccine1.png",
                    BackGroundColor = PinValue.OFF
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 5,
                    ContentButton = "Bản đồ",
                    ImageSource = "Image/Dep/map.png",
                    BackGroundColor = PinValue.OFF
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 6,
                    ContentButton = "Tỷ lệ đi làm",
                    ImageSource = "Image/Dep/Time2.png",
                    BackGroundColor = PinValue.OFF
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 7,
                    ContentButton = "Tỷ lệ ăn cơm VSIP",
                    ImageSource = "Image/Dep/Restaurant.png",
                    BackGroundColor = PinValue.OFF
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 8,
                    ContentButton = "Tỷ lệ phép năm",
                    ImageSource = "Image/Dep/Holiday.png",
                    BackGroundColor = PinValue.OFF
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 9,
                    ContentButton = "Tỷ lệ tăng ca",
                    ImageSource = "Image/Dep/overtime.png",
                    BackGroundColor = PinValue.OFF
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 10,
                    ContentButton = "Lịch trình",
                    ImageSource = "Image/Dep/note.png",
                    BackGroundColor = PinValue.OFF
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 11,
                    ContentButton = "Đào tạo",
                    ImageSource = "Image/Dep/training.png",
                    BackGroundColor = PinValue.OFF
                });

            }
            else
            {
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 1,
                    ContentButton = "메인",
                    ImageSource = "Image/Dep/Home.png",
                    BackGroundColor = PinValue.ON
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 2,
                    ContentButton = "사원정보",
                    ImageSource = "Image/Dep/HR.png",
                    BackGroundColor = PinValue.OFF
                });
                //ListButton_Header.Add(new Helper_DataButton
                //{
                //    ID = 3,
                //    ContentButton = "의료신고",
                //    ImageSource = "Image/Dep/yte.png",
                //    BackGroundColor = PinValue.OFF
                //});
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 4,
                    ContentButton = "백신 정보",
                    ImageSource = "Image/Dep/vaccine1.png",
                    BackGroundColor = PinValue.OFF
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 5,
                    ContentButton = "지도",
                    ImageSource = "Image/Dep/map.png",
                    BackGroundColor = PinValue.OFF
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 6,
                    ContentButton = "출근율",
                    ImageSource = "Image/Dep/Time2.png",
                    BackGroundColor = PinValue.OFF
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 7,
                    ContentButton = "VSIP 식수현황",
                    ImageSource = "Image/Dep/Restaurant.png",
                    BackGroundColor = PinValue.OFF
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 8,
                    ContentButton = "연차사용현황",
                    ImageSource = "Image/Dep/Holiday.png",
                    BackGroundColor = PinValue.OFF
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 9,
                    ContentButton = "잔업비율",
                    ImageSource = "Image/Dep/overtime.png",
                    BackGroundColor = PinValue.OFF
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 10,
                    ContentButton = "일정",
                    ImageSource = "Image/Dep/note.png",
                    BackGroundColor = PinValue.OFF
                });
                ListButton_Header.Add(new Helper_DataButton
                {
                    ID = 11,
                    ContentButton = "교육",
                    ImageSource = "Image/Dep/training.png",
                    BackGroundColor = PinValue.OFF
                });


            }
            //huynd 20230313 thêm đổi ngôn ngư
            foreach (var button in ListButton_Header)
            {

                button.BackGroundColor = PinValue.OFF;
                if (button.ID == checkID)
                {
                    button.BackGroundColor = PinValue.ON;
                }
                if (checkID == 0 && button.ID == 1)
                {
                    button.BackGroundColor = PinValue.ON;
                }
            }
            //end
            lvButtonTop.ItemsSource = ListButton_Header;           
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var click = sender as Button;
            var clickItem = click.DataContext as Helper_DataButton;
            if (clickItem != null && language == "vi-VN")
            {
                switch (clickItem.ContentButton)
                {
                    case "Trang chủ":
                        {                            
                            Page_Main main = new Page_Main();                           
                            frameMain.Navigate(main);                           
                            break;
                        }
                    case "Thông Tin":
                        {
                            frameMain.Navigate(tt);
                            //frameMain.Navigate(OT);
                            //frameMain.Navigate(safe);
                            break;
                        }
                    case "Y tế":
                        {
                            frameMain.Navigate(yte);
                            break;
                        }
                    case "Vacxin":
                        {
                            frameMain.Navigate(vac);
                            break;
                        }

                    case "Bản đồ":
                        {
                            frameMain.Navigate(map);
                            break;
                        }
                    case "Điều xe":
                        {
                            frameMain.Navigate(car);
                            break;
                        }
                  
                    case "Tỷ lệ đi làm":
                        {
                            frameMain.Navigate(work);
                            break;
                        }
                    case "Tỷ lệ ăn cơm VSIP":
                        {
                            frameMain.Navigate(Food);                            
                            break;
                        }
                    case "Tỷ lệ phép năm":
                        {
                            frameMain.Navigate(holiday);
                            break;
                        }
                    case "Tỷ lệ tăng ca":
                        {
                            frameMain.Navigate(OT);
                            break;
                        }
                    case "Lịch trình":
                        {
                            frameMain.Navigate(Note);
                            break;
                        }
                    case "Đào tạo":
                        {
                            frameMain.Navigate(Training);
                            break;
                        }
                }
                foreach (var button in ListButton_Header)
                {
                    button.BackGroundColor = PinValue.OFF;
                    if (button.ContentButton == clickItem.ContentButton)
                    {
                        button.BackGroundColor = PinValue.ON;
                        checkID = button.ID;
                    }
                }

            }
            else
            {
                if (clickItem != null && language == "kr-KR")
                {
                    switch (clickItem.ContentButton)
                    {
                        case "메인":
                            {
                                Page_Main main = new Page_Main();
                                frameMain.Navigate(main);
                                break;
                            }
                        case "사원정보":
                            {
                                frameMain.Navigate(tt);
                                break;
                            }
                        case "의료신고":
                            {
                                frameMain.Navigate(yte);
                                break;
                            }
                        case "백신 정보":
                            {
                                frameMain.Navigate(vac);
                                break;
                            }
                        case "지도":
                            {
                                frameMain.Navigate(map);
                                break;
                            }
                        case "자동차 매니저":
                            {
                                frameMain.Navigate(car);
                                break;
                            }
                       
                        case "출근율":
                            {
                                frameMain.Navigate(work);
                                break;
                            }
                        case "VSIP 식수현황":
                            {
                                frameMain.Navigate(Food);
                                break;
                            }
                        case "연차사용현황":
                            {
                                frameMain.Navigate(holiday);
                                break;
                            }
                        case "잔업비율":
                            {
                                frameMain.Navigate(OT);
                                break;
                            }
                        case "일정":
                            {
                                frameMain.Navigate(Note);
                                break;
                            }
                        case "교육":
                            {
                                frameMain.Navigate(Training);
                                break;
                            }

                    }
                    foreach (var button in ListButton_Header)
                    {
                        button.BackGroundColor = PinValue.OFF;
                        if (button.ContentButton == clickItem.ContentButton)
                        {
                            button.BackGroundColor = PinValue.ON;
                            checkID = button.ID;
                        }
                    }

                }
            }
        }


        //add HUYND 20230311 load lại dư liệu khi thay đổi ngôn ngữ
        private void Button_Click()
        {
            //lvButtonTop.BackgroundColor


            //Button click = new Button();
            //var clickItem = click.DataContext as Helper_DataButton;
            foreach (var button in ListButton_Header)
            {

                if (button.BackGroundColor == PinValue.ON)
                {
                    if (button.ContentButton != null && language == "vi-VN")
                    {
                        switch (button.ContentButton)
                        {
                            case "Trang chủ":
                                {
                                    Page_Main main = new Page_Main();
                                    frameMain.Navigate(main);
                                    break;
                                }
                            case "Thông Tin":
                                {
                                    Page_Address tt = new Page_Address();
                                    frameMain.Navigate(tt);
                                    //frameMain.Navigate(OT);
                                    //frameMain.Navigate(safe);
                                    break;
                                }
                            case "Y tế":
                                {
                                    Page_Covid yte = new Page_Covid();
                                    frameMain.Navigate(yte);
                                    break;
                                }
                            case "Vacxin":
                                {
                                    Page_Covid vac = new Page_Covid();
                                    frameMain.Navigate(vac);
                                    break;
                                }

                            case "Bản đồ":
                                {
                                    Page_Map map = new Page_Map();
                                    frameMain.Navigate(map);
                                    break;
                                }
                            case "Điều xe":
                                {
                                    Page_Car car = new Page_Car();
                                    frameMain.Navigate(car);
                                    break;
                                }

                            case "Tỷ lệ đi làm":
                                {
                                    Page_WorkingRate work = new Page_WorkingRate();
                                    frameMain.Navigate(work);
                                    break;
                                }
                            case "Tỷ lệ ăn cơm VSIP":
                                {
                                    Page_Food Food = new Page_Food();
                                    frameMain.Navigate(Food);
                                    break;
                                }
                            case "Tỷ lệ phép năm":
                                {
                                    Page_Holiday holiday = new Page_Holiday();
                                    frameMain.Navigate(holiday);
                                    break;
                                }
                            case "Tỷ lệ tăng ca":
                                {
                                    Page_OverTime OT = new Page_OverTime();
                                    frameMain.Navigate(OT);
                                    break;
                                }
                            case "Lịch trình":
                                {
                                    Page_Note Note = new Page_Note();
                                    frameMain.Navigate(Note);
                                    break;
                                }
                            case "Đào tạo":
                                {
                                    Page_Training Training = new Page_Training();
                                    frameMain.Navigate(Training);
                                    break;
                                }
                        }
                        //foreach (var button1 in ListButton_Header)
                        //{
                        //    button1.BackGroundColor = PinValue.OFF;
                        //    if (button1.ContentButton == clickItem.ContentButton)
                        //    {
                        //        button1.BackGroundColor = PinValue.ON;
                        //    }
                        //}

                    }
                    else
                    {
                        if (button.ContentButton != null && language == "kr-KR")
                        {
                            switch (button.ContentButton)
                            {
                                case "메인":
                                    {
                                        Page_Main main = new Page_Main();
                                        frameMain.Navigate(main);
                                        break;
                                    }
                                case "사원정보":
                                    {
                                        Page_Address tt = new Page_Address();
                                        frameMain.Navigate(tt);
                                        break;
                                    }
                                case "의료신고":
                                    {
                                        Page_Covid yte = new Page_Covid();
                                        frameMain.Navigate(yte);
                                        break;
                                    }
                                case "백신 정보":
                                    {
                                        Page_Covid vac = new Page_Covid();
                                        frameMain.Navigate(vac);
                                        break;
                                    }
                                case "지도":
                                    {
                                        Page_Map map = new Page_Map();
                                        frameMain.Navigate(map);
                                        break;
                                    }
                                case "자동차 매니저":
                                    {
                                        Page_Car car = new Page_Car();
                                        frameMain.Navigate(car);
                                        break;
                                    }

                                case "출근율":
                                    {
                                        Page_WorkingRate work = new Page_WorkingRate();
                                        frameMain.Navigate(work);
                                        break;
                                    }
                                case "VSIP 식수현황":
                                    {
                                        Page_Food Food = new Page_Food();
                                        frameMain.Navigate(Food);
                                        break;
                                    }
                                case "연차사용현황":
                                    {
                                        Page_Holiday holiday = new Page_Holiday();
                                        frameMain.Navigate(holiday);
                                        break;
                                    }
                                case "잔업비율":
                                    {
                                        Page_OverTime OT = new Page_OverTime();
                                        frameMain.Navigate(OT);
                                        break;
                                    }
                                case "일정":
                                    {
                                        Page_Note Note = new Page_Note();
                                        frameMain.Navigate(Note);
                                        break;
                                    }
                                case "교육":
                                    {
                                        Page_Training Training = new Page_Training();
                                        frameMain.Navigate(Training);
                                        break;
                                    }

                            }
                            //foreach (var button1 in ListButton_Header)
                            //{
                            //    button1.BackGroundColor = PinValue.OFF;
                            //    if (button1.ContentButton == clickItem.ContentButton)
                            //    {
                            //        button1.BackGroundColor = PinValue.ON;
                            //    }
                            //}

                        }
                    }
                }
            }

        }

        //END

        private void rb_langKr_Click(object sender, RoutedEventArgs e)
        {
            language = "kr-KR";
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(language);
            ApplyLanguage(language);   
            CreatButton_Header();
            Button_Click();
        }

        private void rb_langVn_Click(object sender, RoutedEventArgs e)
        {
            language = "vi-VN";
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(language);
            ApplyLanguage(language);          
            CreatButton_Header();
            Button_Click();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }   

}