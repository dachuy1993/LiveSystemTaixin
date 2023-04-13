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
using System.Windows.Threading;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for DelayLoadingData.xaml
    /// </summary>
    public partial class Page_LoadingData : Page
    {
        public Page_LoadingData()
        {
            InitializeComponent();
            if (MainWindow.language == "vi-VN")
            {
                txb_Loading.Text = "Đang tải dữ liệu vui lòng chờ";

            }
            else
            {
                txb_Loading.Text = "로딩 중 기다려주세요";
            }
        }
    }
}
