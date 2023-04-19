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
    /// Interaction logic for Window_CheckUser.xaml
    /// </summary>
    public partial class Window_CheckUser : Window
    {
        public Window_CheckUser()
        {
            InitializeComponent();
            frameEmp.Navigate(Page_CheckPermission);
        }
        Page_CheckPermission Page_CheckPermission = new Page_CheckPermission();

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
