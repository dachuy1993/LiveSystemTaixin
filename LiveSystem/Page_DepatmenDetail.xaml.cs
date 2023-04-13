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
    /// Interaction logic for Page_DepatmenDetail.xaml
    /// </summary>
    public partial class Page_DepatmenDetail : Page
    {
        public Page_DepatmenDetail()
        {
            InitializeComponent();
            Loaded += Page_DepatmenDetail_Loaded;
        }

        private void Page_DepatmenDetail_Loaded(object sender, RoutedEventArgs e)
        {
            List<Helper_Employee> listEmp = new List<Helper_Employee>();
            List<Helper_Employee> listTemp = new List<Helper_Employee>();
            lvThongTin.ClearValue(ListView.ItemsSourceProperty);
            string date1 = Page_Main.dateCheck;
            string date2 = DateTime.Parse(Page_Main.dateCheck.ToString()).ToString("yyyyMMdd");
            if (Page_Main.shiftCheck =="Ca ngày" || Page_Main.shiftCheck == "Tất cả")
            {
                using (SqlConnection conn = new SqlConnection(Page_Main.path_Ksystem20))
                {
                    conn.Open();
                    var command1 = "SELECT DISTINCT(a.cID),b.EmpNm from tblTimeLog as a JOIN TDAEmpMaster as b ON a.cID = b.EmpId JOIN TDAMinor as c ON b.DeptCd = c.Item1 JOIN TYWEmpGroupReg_VN as d on a.cID = d.EmpId JOIN TYWGroupToShiftReg_VN as e on d.GroupCd = e.GroupCd where a.cDateTime BETWEEN '" + date1 + " 00:00:00.000' and '" + date1 + " 07:59:59.000' and c.MinorCd like '" + Page_Main.Depatmen_Code + "' and '" + date2 + "' BETWEEN d.FromDate AND d.ToDate AND (e.Remark=N'Ca ngày' or e.Remark=N'ca 1 (6h-14h)' or e.Remark=N'ca 2 (14h-22h)') AND b.RetDate>'" + Page_Main.dateCheck + "'";
                    using (SqlCommand cmd = new SqlCommand(command1, conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0].ToString() != null)
                                {
                                    Helper_Employee per = new Helper_Employee();
                                    per.EmpId = dr[0].ToString();
                                    per.EmpNm = dr[1].ToString();
                                    per.UdDay = "OK";
                                    listEmp.Add(per);
                                }
                            }
                        }

                    }
                    var command2 = "SELECT DISTINCT(a.cID),b.EmpNm from tblTimeLog as a JOIN TDAEmpMaster as b ON a.cID = b.EmpId JOIN TDAMinor as c ON b.DeptCd = c.Item1 JOIN TYWEmpGroupReg_VN as d on a.cID = d.EmpId JOIN TYWGroupToShiftReg_VN as e on d.GroupCd = e.GroupCd where a.cDateTime BETWEEN '" + date1 + " 08:00:00.000' and '" + date1 + " 15:59:59.000' and c.MinorCd like '" + Page_Main.Depatmen_Code + "' and '" + date2 + "' BETWEEN d.FromDate AND d.ToDate AND (e.Remark=N'Ca ngày' or e.Remark=N'ca 1 (6h-14h)' or e.Remark=N'ca 2 (14h-22h)') AND b.RetDate>'" + Page_Main.dateCheck + "'";
                    using (SqlCommand cmd = new SqlCommand(command2, conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0].ToString() != null)
                                {
                                    Helper_Employee per = new Helper_Employee();
                                    per.EmpId = dr[0].ToString();
                                    per.EmpNm = dr[1].ToString();
                                    per.UdDay = "NG";
                                    listEmp.Add(per);
                                }
                            }
                        }

                    }
                    conn.Close();
                }
            }
            if (Page_Main.shiftCheck == "Ca đêm" || Page_Main.shiftCheck == "Tất cả")
            {
                using (SqlConnection conn = new SqlConnection(Page_Main.path_Ksystem20))
                {
                    conn.Open();
                    var command1 = "SELECT DISTINCT(a.cID),b.EmpNm from tblTimeLog as a JOIN TDAEmpMaster as b ON a.cID = b.EmpId JOIN TDAMinor as c ON b.DeptCd = c.Item1 JOIN TYWEmpGroupReg_VN as d on a.cID = d.EmpId JOIN TYWGroupToShiftReg_VN as e on d.GroupCd = e.GroupCd where a.cDateTime BETWEEN '" + date1 + " 18:00:00.000' and '" + date1 + " 19:59:59.000' and c.MinorCd like '" + Page_Main.Depatmen_Code + "' and '" + date2 + "' BETWEEN d.FromDate AND d.ToDate AND (e.Remark=N'Ca đêm' or e.Remark=N'ca 3 (22h-6h)') AND b.RetDate>'" + Page_Main.dateCheck + "'";
                    using (SqlCommand cmd = new SqlCommand(command1, conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0].ToString() != null)
                                {
                                    Helper_Employee per = new Helper_Employee();
                                    per.EmpId = dr[0].ToString();
                                    per.EmpNm = dr[1].ToString();
                                    per.UdDay = "OK";
                                    listEmp.Add(per);
                                }
                            }
                        }

                    }
                    var command2 = "SELECT DISTINCT(a.cID),b.EmpNm from tblTimeLog as a JOIN TDAEmpMaster as b ON a.cID = b.EmpId JOIN TDAMinor as c ON b.DeptCd = c.Item1 JOIN TYWEmpGroupReg_VN as d on a.cID = d.EmpId JOIN TYWGroupToShiftReg_VN as e on d.GroupCd = e.GroupCd where a.cDateTime BETWEEN '" + date1 + " 20:00:00.000' and '" + date1 + " 23:59:59.000' and c.MinorCd like '" + Page_Main.Depatmen_Code + "' and '" + date2 + "' BETWEEN d.FromDate AND d.ToDate AND (e.Remark=N'Ca đêm' or e.Remark=N'ca 3 (22h-6h)') AND b.RetDate>'" + Page_Main.dateCheck + "'";
                    using (SqlCommand cmd = new SqlCommand(command2, conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0].ToString() != null)
                                {
                                    Helper_Employee per = new Helper_Employee();
                                    per.EmpId = dr[0].ToString();
                                    per.EmpNm = dr[1].ToString();
                                    per.UdDay = "NG";
                                    listEmp.Add(per);
                                }
                            }
                        }

                    }
                    conn.Close();
                }
            }           

            listTemp = listTemp.OrderBy(x=>x.EmpId).ToList();
            listTemp = listEmp.OrderBy(x => x.UdDay).ToList();
            int index = 0;
            foreach (var item in listTemp)
            {
                item.ID = index;
                index++;
            }
            lvThongTin.ItemsSource = listTemp;
        }
    }
}
