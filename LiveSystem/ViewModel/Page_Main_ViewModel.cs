using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSystem.ViewModel
{
    public class Page_Main_ViewModel
    {
        
    }

    //====================================================================//
    public class Working_Rate_ViewModel
    {
        public string EmpNm { get; set; }
        public int Total { get; set; }
        public int WorkOn { get; set; }
        public int WorkOff { get; set; }
        public int Delay { get; set; }
        public string Rate { get; set; }
    }

    //====================================================================//
    public class Leave_Rate_ViewModel
    {
        public string MinorCd { get; set; }
        public string Division { get; set; }
        public double Total { get; set; }
        public double Used { get; set; }
        public double Remain { get; set; }
        public string Rate { get; set; }
    }

    //====================================================================//
    public class OverTime_Rate_ViewModel
    {
        public string DeptNm { get; set; }
        public int NumPer { get; set; }
        public int TotalOT { get; set; }
        public int Rate { get; set; }

        // Overtime between 40h and 52h
        public int Numper40 { get; set; }
        public int RateNumper40 { get; set; }
        // Overtime between 52h and 104h
        public int Numper104 { get; set; }
        public int RateNumper104 { get; set; }
    }

    //====================================================================//
    public class Meal_Information_ViewModel
    {
        public string Division { get; set; }
        public int Breakfast { get; set; }
        public int Lunch { get; set; }
        public int Dinner { get; set; }
        public int NightMeal { get; set; }
    }

    //====================================================================//
    public class Address_Information_ViewModel
    {
        public string No { get; set; }
        public string EmpId { get; set; }
        public string EmpNm { get; set; }
        public int SexCd { get; set; }
        public DateTime DOB { get; set; }
        public string HpTel { get; set; }
        public string ResidId { get; set; }
        public string ResidPlace { get; set; }
        public DateTime ResidDate { get; set; }
        public string Nation { get; set; }
        public string Deptlv1 { get; set; }
        public string Deptlv2 { get; set; }
        public string Deptlv3 { get; set; }
        public string Position { get; set; }
        public string Shift { get; set; }
        public string Level { get; set; }

        public string TempProv { get; set; }
        public string TempDist { get; set; }
        public string TempComm { get; set; }
        public string TempVilla { get; set; }
        
        public string PermProv { get; set; }
        public string PermDist { get; set; }
        public string PermComm { get; set; }
        public string PermVilla { get; set; }
    }

    //====================================================================//
    public class Emp_Vaccine
    {
        public int ID { get; set; }
        public string EmpId { get; set; }
        public string EmpNm { get; set; }
        public string Deptlv1 { get; set; }
        public string Deptlv2 { get; set; }
        public int Vtimes { get; set; }
    }

    public class Vaccine_Information_ViewModel
    {
        public string VaccineNo { get; set; }
        public int Qty { get; set; }
        public int Rate { get; set; }
    }

    //====================================================================//
    public class Update_Information_ViewModel
    {
        public string Status { get; set; }
        public int Qty { get; set; }
    }

    //====================================================================//
    public class Manage_Car_ViewModel
    {
        public string Color { get; set; }
        public string CarIdentityNo { get; set; }
        public string Driver { get; set; }
        public string DriverPhoneNumber { get; set; }
        public bool Status { get; set; }
        public int KMMonth { get; set; }
        public int Quota { get; set; }
        public int Remain { get; set; }
        public string Destination { get; set; }
        public string EmpUse { get; set; }
    }
}
