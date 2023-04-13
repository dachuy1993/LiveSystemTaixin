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
using System.Windows.Shapes;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for Window_Car.xaml
    /// </summary>
    public partial class Window_Car : Window
    {
        public Window_Car()
        {
            InitializeComponent();
            dpk_CheckFrom.SelectedDate = DateTime.Now;
            dpk_CheckTo.SelectedDate = DateTime.Now;
            MessageBox.Show(Page_Car.carID);
        }

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
