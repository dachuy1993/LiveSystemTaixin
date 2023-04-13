using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSystem.Model
{
    public class EmpVacationLeave
    {
        public int ID { get; set; }
        public string Division { get; set; }
        public string DeptNm { get; set; }
        public string GroupNm { get; set; }
        public string EmpId { get; set; }
        public string EmpNm { get; set; }
        public double Old { get; set; }
        public double Total { get; set; }
        public double Used { get; set; }
        public double Remain { get; set; }
    }
}
