using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSystem.Model
{
    public class Emp
    {
        public int ID { get; set; }
        public string EmpId { get; set; }
        public string EmpNm { get; set; }
        public string SexCd { get; set; }
        public string BOD { get; set; }
        public string HpTel { get; set; }
        public string ResidId { get; set; }
        public string ResidPlace { get; set; }
        public string ResidDate { get; set; }
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
        public string TaxCode { get; set; } 
    }
}
