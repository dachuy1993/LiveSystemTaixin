using LiveSystem.DAO;
using LiveSystem.Model;
using Microsoft.Win32;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.IO;
using LiveCharts;
using LiveCharts.Wpf;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using LiveCharts.Helpers;
using LiveCharts.Wpf.Charts.Base;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Reflection.Emit;
using System.Windows.Media.Converters;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for Page_Environment.xaml
    /// </summary>
    public partial class Page_Environment : Page, INotifyPropertyChanged
    {
        #region Khai báo 
        public static string path_Ksystem20 = "Data Source=192.168.2.20;Initial Catalog=TAIXINERP;Persist Security Info=True;User ID=sa;Password= Ksystem@123";
        bool checkWorking = false;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SeriesCollection SeriesCollectionYear { get; set; }
        public SeriesCollection SeriesCollectionYearRate { get; set; }
        public SeriesCollection SeriesCollectionArea { get; set; }

        public SeriesCollection SeriesCollectionCategory { get; set; }
        public SeriesCollection SeriesCollectionColor { get; set; }

        
        public string[] Labels { get; set; }
        public string[] LabelsRate { get; set; }
        public string[] LabelsArea { get; set; }
        public string[] LabelsCate { get; set; }
        public string[] LabelsColor { get; set; }
        
        
        public Func<double, string> Formatter { get; set; }
        
        public Func<ChartPoint, string> FormaterR { get; private set; }
        #endregion
        public Page_Environment()
        {
            InitializeComponent();
            //DataContext = this;
            Loaded += Page_Environment_Loaded;
        }

        public void Page_Environment_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MainWindow.language);
            ApplyLanguage(MainWindow.language);
            GetDataCmb();
            GetListForYear();
            GetListForArea();
            GetListForCategory();
            GetListForColor();
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



        private async void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {

            Page_Environment page_Environment = new Page_Environment();
          
            SeriesCollectionYear.Clear();
            SeriesCollectionYear = new SeriesCollection();
            


            GetListForYear();
            //GetListForArea();
            //GetListForCategory();
            //GetListForColor();
        }

        private async void GetListForYear()
        {

            string queryYear = "SPGetDataForYearSafe @Year , @TimeRv , @AreaRv";
            string Year = cbbYear.Text;
            string TimeRv = cbbTimeReview.Text;
            string AreaRv = cbbAreaNm.Text;
            DataTable ListDataForYear = new DataTable();
            ListDataForYear = DataProvider.Instance.ExecuteSP(path_Ksystem20, queryYear, new object[] { Year, TimeRv, AreaRv });

            // biểu đô biến động theo năm
            ChartValues<double> listError = new ChartValues<double>();
            ChartValues<double> listImprove = new ChartValues<double>();
            ChartValues<double> listRate = new ChartValues<double>();
            List<string> listLabels = new List<string>();
            List<string> listLabelsKorea = new List<string>();
            foreach (DataRow row in ListDataForYear.Rows)
            {
                listError.Add(double.Parse(row["Error"].ToString()));
                listImprove.Add(double.Parse(row["Improve"].ToString()));
                listRate.Add(double.Parse(row["Rate"].ToString()));
                listLabels.Add((row["EvalR"].ToString()));
                listLabelsKorea.Add((row["EvalK"].ToString()));
            }

            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "vi-VN":
                    //Point point = new Point(10, 20);
                        SeriesCollectionYear = new SeriesCollection()
                        {

                            new ColumnSeries
                            {
                                Title = "Sum of Tổng lỗi KV",
                                Values = listError,
                                DataLabels = true,
                                FontSize = 15

                            },
                            new ColumnSeries(Chart.AxisYProperty.PropertyType)
                            {
                                Title = "Sum of Tổng cải tiến KV",
                                Values = listImprove,
                                DataLabels = true,
                                FontSize = 15
                            },
                            new LineSeries
                            {
                                Title = "Sum of % Cải tiến năm",
                                Values = listRate,
                                DataLabels = true,
                                FontSize = 15,
                    
                                //LabelPoint = FormaterR,
                            }
                        };
                        Labels = listLabels.ToArray();
                    break;
                default:
                    SeriesCollectionYear = new SeriesCollection()
                        {

                            new ColumnSeries
                            {
                                Title = "구역별 위반 건수",
                                Values = listError,
                                DataLabels = true,
                                FontSize = 15

                            },
                            new ColumnSeries(Chart.AxisYProperty.PropertyType)
                            {
                                Title = "구역별 개선 건수",
                                Values = listImprove,
                                DataLabels = true,
                                FontSize = 15
                            },
                            new LineSeries
                            {
                                Title = "구역별 개선 비율",
                                Values = listRate,
                                DataLabels = true,
                                FontSize = 15,
                    
                                //LabelPoint = FormaterR,
                            }
                        };
                    Labels = listLabelsKorea.ToArray();
                    break;
            }

            //SeriesCollectionYear.Select(x => x.Values).ToList(); 



            //DataContext = this;
        }


        private async void GetListForArea()
        {
            string queryArea = "SPGetDataForAreaSafe @Year , @TimeRv , @AreaRv";
            await Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    Page_LoadingData page_Loading = new Page_LoadingData();
                    page_Loading.Visibility = Visibility.Visible;
                    frameLoading.Navigate(page_Loading);
                    checkWorking = true;
                }, System.Windows.Threading.DispatcherPriority.ContextIdle);
            });
            string Year = cbbYear.Text;
            string TimeRv = cbbTimeReview.Text;
            string AreaRv = cbbAreaNm.Text;

            DataTable ListDataForArea = new DataTable();
            await Task.Run(() =>
            {
                ListDataForArea = DataProvider.Instance.ExecuteSP(path_Ksystem20, queryArea, new object[] { Year, TimeRv, AreaRv });

            });
            // biểu dô theo khu vực

            ChartValues<double> listErrorArea = new ChartValues<double>();
            ChartValues<double> listImproveArea = new ChartValues<double>();
            ChartValues<double> listRateArea = new ChartValues<double>();
            List<string> listLabelsArea = new List<string>();

            foreach (DataRow row in ListDataForArea.Rows)
            {
                listErrorArea.Add(double.Parse(row["Error"].ToString()));
                listImproveArea.Add(double.Parse(row["Improve"].ToString()));
                listRateArea.Add(double.Parse(row["Rate"].ToString()));
                listLabelsArea.Add((row["Area"].ToString()));
            }

            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "vi-VN":
                    SeriesCollectionArea = new SeriesCollection()
                    {
                        new StackedColumnSeries
                        {
                            Title = "Sum of Tổng lỗi KV",
                            Values = listErrorArea,
                            StackMode = StackMode.Values,
                            DataLabels = true,
                            FontSize = 15
                        },
                        new StackedColumnSeries
                        {
                            Title = "Sum of Tổng cải tiến KV",
                            Values = listImproveArea,
                            StackMode = StackMode.Values,
                            DataLabels = true,
                            FontSize = 15
                        },
                        new LineSeries
                        {
                            Title = "Sum of % Cải tiến năm",
                            Values = listRateArea,
                            DataLabels = true,
                            FontSize = 15
                        }
                    };
                    LabelsArea = listLabelsArea.ToArray();
                    break;
                default:
                    SeriesCollectionArea = new SeriesCollection()
                    {
                        new StackedColumnSeries
                        {
                            Title = "구역별 위반 건수",
                            Values = listErrorArea,
                            StackMode = StackMode.Values,
                            DataLabels = true,
                            FontSize = 15
                        },
                        new StackedColumnSeries
                        {
                            Title = "구역별 개선 건수",
                            Values = listImproveArea,
                            StackMode = StackMode.Values,
                            DataLabels = true,
                            FontSize = 15
                        },
                        new LineSeries
                        {
                            Title = "구역별 개선 비율",
                            Values = listRateArea,
                            DataLabels = true,
                            FontSize = 15
                        }


                    };

                    LabelsArea = listLabelsArea.ToArray();
                    break;
            }
            //DataContext = this;
            //frameChart_Year.Navigate(column);

            // Đóng Page_LoadingData
            await Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    stackLoading.Visibility = Visibility.Hidden;
                    checkWorking = false;
                }, System.Windows.Threading.DispatcherPriority.ContextIdle);
            });

        }


        private async void GetListForCategory()
        {
            string queryCategory = "SPGetDataForCategorySafe @Year , @TimeRv , @AreaRv";
            await Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    Page_LoadingData page_Loading = new Page_LoadingData();
                    page_Loading.Visibility = Visibility.Visible;
                    frameLoading.Navigate(page_Loading);
                    checkWorking = true;
                }, System.Windows.Threading.DispatcherPriority.ContextIdle);
            });
            string Year = cbbYear.Text;
            string TimeRv = cbbTimeReview.Text;
            string AreaRv = cbbAreaNm.Text;

            DataTable ListDataForCategory = new DataTable();
            await Task.Run(() =>
            {
                ListDataForCategory = DataProvider.Instance.executeQuery(path_Ksystem20, queryCategory, new object[] { Year, TimeRv, AreaRv });

            });
            // biểu dô theo khu vực

            ChartValues<double> listErrorCate = new ChartValues<double>();
            ChartValues<double> listImproveCate = new ChartValues<double>();
            ChartValues<double> listRateCate = new ChartValues<double>();
            List<string> listLabelsCate = new List<string>();
            List<string> listLabelsCateKorea = new List<string>();

            foreach (DataRow row in ListDataForCategory.Rows)
            {
                listErrorCate.Add(double.Parse(row["NumErr"].ToString()));
                listImproveCate.Add(double.Parse(row["NumImp"].ToString()));
                listRateCate.Add(double.Parse(row["Rate"].ToString()));
                listLabelsCate.Add((row["Category"].ToString()));
                listLabelsCateKorea.Add((row["CategoryK"].ToString()));
            }
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "vi-VN":

                    SeriesCollectionCategory = new SeriesCollection()
                    {
                        new ColumnSeries
                        {
                            Title = "Sum of Tổng lỗi KV",
                            Values = listErrorCate,
                            DataLabels = true,
                            FontSize = 15
                        },
                        new ColumnSeries
                        {
                            Title = "Sum of Tổng cải tiến KV",
                            Values = listImproveCate,
                            DataLabels = true,
                            FontSize = 15
                        },
                        new LineSeries
                        {
                            Title = "Sum of % Cải tiến năm",
                            Values = listRateCate,
                            DataLabels = true,
                            FontSize = 15
                        }


                    };

                    LabelsCate = listLabelsCate.ToArray();
                    break;
                default:
                    SeriesCollectionCategory = new SeriesCollection()
                    {
                        new ColumnSeries
                        {
                            Title = "구역별 위반 건수",
                            Values = listErrorCate,
                            DataLabels = true,
                            FontSize = 15
                        },
                    new ColumnSeries
                    {
                        Title = "구역별 개선 건수",
                        Values = listImproveCate,
                        DataLabels = true,
                        FontSize = 15
                    },
                    new LineSeries
                    {
                        Title = "구역별 개선 비율",
                        Values = listRateCate,
                        DataLabels = true,
                        FontSize = 15
                    }
                };

                    LabelsCate = listLabelsCateKorea.ToArray();
                    break;
            }
                    //DataContext = this;
                    //frameChart_Year.Navigate(column);

                    // Đóng Page_LoadingData
                    await Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    stackLoading.Visibility = Visibility.Hidden;
                    checkWorking = false;
                }, System.Windows.Threading.DispatcherPriority.ContextIdle);
            });

        }

        private async void GetListForColor()
        {
            string queryColor = "SPGetDataForColorSafe @Year , @TimeRv , @AreaRv";
            await Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    Page_LoadingData page_Loading = new Page_LoadingData();
                    page_Loading.Visibility = Visibility.Visible;
                    frameLoading.Navigate(page_Loading);
                    checkWorking = true;
                }, System.Windows.Threading.DispatcherPriority.ContextIdle);
            });
            string Year = cbbYear.Text;
            string TimeRv = cbbTimeReview.Text;
            string AreaRv = cbbAreaNm.Text;

            DataTable ListDataForColor = new DataTable();
            await Task.Run(() =>
            {
                ListDataForColor = DataProvider.Instance.executeQuery(path_Ksystem20, queryColor, new object[] { Year, TimeRv, AreaRv });

            });
            // biểu dô theo khu vực

            ChartValues<double> listColorRed = new ChartValues<double>();
            ChartValues<double> listColorYellow = new ChartValues<double>();
            ChartValues<double> listColorGreen = new ChartValues<double>();
            List<string> listLabelsColor = new List<string>();
            List<string> listLabelsColorKorea = new List<string>();
            foreach (DataRow row in ListDataForColor.Rows)
            {
                listColorRed.Add(double.Parse(row["ColorRed"].ToString()));
                listColorYellow.Add(double.Parse(row["ColorYellow"].ToString()));
                listColorGreen.Add(double.Parse(row["ColorGreen"].ToString()));
                listLabelsColor.Add((row["YearTime"].ToString()));
                listLabelsColorKorea.Add((row["YearTimeK"].ToString()));
            }
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "vi-VN":
                    SeriesCollectionColor = new SeriesCollection()
                        {
                        new ColumnSeries
                        {
                            Title = "Khu vực màu đỏ",
                            Values = listColorRed,
                            Fill = Brushes.Red,
                            DataLabels = true,
                            FontSize = 15
                        },
                        new ColumnSeries
                        {
                            Title = "Khu vực màu vàng",
                            Values = listColorYellow,
                            Fill = Brushes.Yellow,
                            DataLabels = true,
                            FontSize = 15
                        },
                        new ColumnSeries
                        {
                            Title = "Khu vực màu xanh",
                            Values = listColorGreen,
                            Fill = Brushes.Green,
                            DataLabels = true,
                            FontSize = 15
                        }


                    };

                    LabelsColor = listLabelsColor.ToArray();
                    break;
                default:
                    SeriesCollectionColor = new SeriesCollection()
                        {
                        new ColumnSeries
                        {
                            Title = "빨간색구역",
                            Values = listColorRed,
                            Fill = Brushes.Red,
                            DataLabels = true,
                            FontSize = 15
                        },
                        new ColumnSeries
                        {
                            Title = "노란색구역",
                            Values = listColorYellow,
                            Fill = Brushes.Yellow,
                            DataLabels = true,
                            FontSize = 15
                        },
                        new ColumnSeries
                        {
                            Title = "파란색구역",
                            Values = listColorGreen,
                            Fill = Brushes.Green,
                            DataLabels = true,
                            FontSize = 15
                        }


                    };

                    LabelsColor = listLabelsColorKorea.ToArray();
                    break;
            }
                    DataContext = this;
            //frameChart_Year.Navigate(column);

            // Đóng Page_LoadingData
            await Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    stackLoading.Visibility = Visibility.Hidden;
                    checkWorking = false;
                }, System.Windows.Threading.DispatcherPriority.ContextIdle);
            });

        }

        private async void GetDataCmb()
        {
            //lây dữ liệu lên cbb Year
            string cbYear = "";
            string queryYear = "SPGetDataCmbYearSafe @cbYear ";
            
            // Lấy dữ liệu và hiển thị
            DataTable listCmbYear = new DataTable();

            listCmbYear = DataProvider.Instance.ExecuteSP(Page_Main.path_Ksystem20, queryYear, new object[] { cbYear });
            

            List<string> listResultYear = new List<string>();
            

            foreach (DataRow Row in listCmbYear.Rows)
            {
                listResultYear.Add(Row["Name"].ToString());
            }
            cbbYear.ItemsSource = listResultYear;
            
        }

       

        private void btnCheckData_Click(object sender, RoutedEventArgs e)
        {
            Window_EnvironSave EnvironSave = new Window_EnvironSave();
            EnvironSave.Show();
        }

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

        private void btnDanhsach_Click(object sender, RoutedEventArgs e)
        {
            Window_EnvironmentData EnvironData = new Window_EnvironmentData();
            EnvironData.Show();
        }
    }
}
