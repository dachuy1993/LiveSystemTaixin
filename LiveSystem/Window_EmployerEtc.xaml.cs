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
using System.Windows.Shapes;

namespace LiveSystem
{
    /// <summary>
    /// Interaction logic for Window_EmployerEtc.xaml
    /// </summary>
    public partial class Window_EmployerEtc : Window
    {
        public Window_EmployerEtc()
        {
            InitializeComponent();
            frameEmp.Navigate(pageIn);
        }
        Page_EmpIn pageIn = new Page_EmpIn();
        Page_EmpOut pageOut = new Page_EmpOut();
        private void rb_In_Checked(object sender, RoutedEventArgs e)
        {
            //frameEmp.Navigate(pageIn);
        }

        private void rb_Out_Checked(object sender, RoutedEventArgs e)
        {
            //frameEmp.Navigate(pageOut);
        }
    }
}

