using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSystem
{
    public class Helper_Employee
    {
        private int _ID;
        private string _cmpcode;
        private string _bizdiv;
        private string _EmpId;
        private string _EmpNm;
        private string _SexCd;
        private string _BOD;
        private string _Deptlv1;
        private string _Deptlv2;
        private string _Deptlv3;
        private string _Level;
        private string _ResidId;
        private string _ResidDate;
        private string _ResidPlace;
        private string _Nation;
        private string _PermProv;
        private string _PermProv2;
        private string _PermDist;
        private string _PermComm;
        private string _PermVilla;
        private string _TempProv;
        private string _TempProv2;
        private string _TempDist;
        private string _TempComm;
        private string _TempVilla;
        private string _HpTel;
        private string _Shift;
        private string _Position;
        private string _UdYear;
        private string _UdMonth;
        private string _UdDay;
        private int _QtyTinh;
        private int _QtyHuyen;
        private int _QtyXa;
        private string _TenTinh;
        private string _TenHuyen;
        private string _TenXa;       
        private string _TypeTinh;       
        private string _TypeHuyen;       
        private string _TypeXa;
        private string _TenBoPhan;
        private string _TenPhongBan;
        private string _TenNhom;
        private string _Perm_TenTinh;
        private string _Perm_TenHuyen;
        private string _Perm_TenXa;
        private string _Temp_TenTinh;
        private string _Temp_TenHuyen;
        private string _Temp_TenXa;
        private string _NumberVaccine;
        private int _DayShiftOn;
        private int _DayShiftOff;
        private int _NightShiftOn;
        private int _NightShiftOff;       
        private int _NightShiftAll;       
        private int _DayShiftAll;
        private int _NightShiftDelay;
        private int _DayShiftDelay;
        private string _RateDayOn;
        private string _RateNightOn;
        
        private string _AnSang;
        private string _AnTrua;
        private string _AnDem;
        private string _AnToi;
        private string _RateSang;
        private string _RateTrua;
        private string _RateToi;
        private string _RatePhu;
        private int _QtyEmployerEtc_R_D;
        private int _QtyEmployerEtc_S_D;
        private int _QtyEmployerEtc_R_N;
        private int _QtyEmployerEtc_S_N;
        private string _NameFood;
        private int _QtyFood;
        private int _RateFood;
        private double _OldHoliday;
        private double _TotalHoliday;
        private double _UsHoliday;
        private double _PayHoliday;
        private string _RateHoliday;
        private double _OverTime;
        private double _OverTime6;
        private double _OverTime12;
        private string _OtRate;
        private string _OtRate6;
        private string _OtRate12;
        private string _Time;
        private string _TimeType;


        public int ID { get { return _ID; } set { if (_ID != value) { _ID = value; NotifyPropertyChanged("ID"); } } }
        public string cmpcode { get { return _cmpcode; } set { if (_cmpcode != value) { _cmpcode = value; NotifyPropertyChanged("cmpcode"); } } }
        public string bizdiv { get { return _bizdiv; } set { if (_bizdiv != value) { _bizdiv = value; NotifyPropertyChanged("bizdiv"); } } }
        public string EmpId { get { return _EmpId; } set { if (_EmpId != value) { _EmpId = value; NotifyPropertyChanged("EmpId"); } } }
        public string EmpNm { get { return _EmpNm; } set { if (_EmpNm != value) { _EmpNm = value; NotifyPropertyChanged("EmpNm"); } } }
        public string SexCd { get { return _SexCd; } set { if (_SexCd != value) { _SexCd = value; NotifyPropertyChanged("SexCd"); } } }
        public string BOD { get { return _BOD; } set { if (_BOD != value) { _BOD = value; NotifyPropertyChanged("BOD"); } } }
        public string Deptlv1 { get { return _Deptlv1; } set { if (_Deptlv1 != value) { _Deptlv1 = value; NotifyPropertyChanged("Deptlv1"); } } }
        public string Deptlv2 { get { return _Deptlv2; } set { if (_Deptlv2 != value) { _Deptlv2 = value; NotifyPropertyChanged("Deptlv2"); } } }
        public string Deptlv3 { get { return _Deptlv3; } set { if (_Deptlv3 != value) { _Deptlv3 = value; NotifyPropertyChanged("Deptlv3"); } } }
        public string Level { get { return _Level; } set { if (_Level != value) { _Level = value; NotifyPropertyChanged("Level"); } } }
        public string ResidId { get { return _ResidId; } set { if (_ResidId != value) { _ResidId = value; NotifyPropertyChanged("ResidId"); } } }
        public string ResidDate { get { return _ResidDate; } set { if (_ResidDate != value) { _ResidDate = value; NotifyPropertyChanged("ResidDate"); } } }
        public string ResidPlace { get { return _ResidPlace; } set { if (_ResidPlace != value) { _ResidPlace = value; NotifyPropertyChanged("ResidPlace"); } } }
        public string Nation { get { return _Nation; } set { if (_Nation != value) { _Nation = value; NotifyPropertyChanged("Nation"); } } }
        public string PermProv { get { return _PermProv; } set { if (_PermProv != value) { _PermProv = value; NotifyPropertyChanged("PermProv"); } } }
        public string PermProv2 { get { return _PermProv2; } set { if (_PermProv2 != value) { _PermProv2 = value; NotifyPropertyChanged("PermProv2"); } } }
        public string PermDist { get { return _PermDist; } set { if (_PermDist != value) { _PermDist = value; NotifyPropertyChanged("PermDist"); } } }
        public string PermComm { get { return _PermComm; } set { if (_PermComm != value) { _PermComm = value; NotifyPropertyChanged("PermComm"); } } }
        public string PermVilla { get { return _PermVilla; } set { if (_PermVilla != value) { _PermVilla = value; NotifyPropertyChanged("PermVilla"); } } }
        public string TempProv { get { return _TempProv; } set { if (_TempProv != value) { _TempProv = value; NotifyPropertyChanged("TempProv"); } } }
        public string TempProv2 { get { return _TempProv2; } set { if (_TempProv2 != value) { _TempProv2 = value; NotifyPropertyChanged("TempProv2"); } } }
        public string TempDist { get { return _TempDist; } set { if (_TempDist != value) { _TempDist = value; NotifyPropertyChanged("TempDist"); } } }
        public string TempComm { get { return _TempComm; } set { if (_TempComm != value) { _TempComm = value; NotifyPropertyChanged("TempComm"); } } }
        public string TempVilla { get { return _TempVilla; } set { if (_TempVilla != value) { _TempVilla = value; NotifyPropertyChanged("TempVilla"); } } }
        public string HpTel { get { return _HpTel; } set { if (_HpTel != value) { _HpTel = value; NotifyPropertyChanged("HpTel"); } } }
        public string Shift { get { return _Shift; } set { if (_Shift != value) { _Shift = value; NotifyPropertyChanged("Shift"); } } }
        public string Position { get { return _Position; } set { if (_Position != value) { _Position = value; NotifyPropertyChanged("Position"); } } }
        public string UdYear { get { return _UdYear; } set { if (_UdYear != value) { _UdYear = value; NotifyPropertyChanged("UdYear"); } } }
        public string UdMonth { get { return _UdMonth; } set { if (_UdMonth != value) { _UdMonth = value; NotifyPropertyChanged("UdMonth"); } } }
        public string UdDay { get { return _UdDay; } set { if (_UdDay != value) { _UdDay = value; NotifyPropertyChanged("UdDay"); } } }
        public string MaTinh { get { return _TenTinh; } set { if (_TenTinh != value) { _TenTinh = value; NotifyPropertyChanged("TenTinh"); } } }
        public string MaHuyen { get { return _TenHuyen; } set { if (_TenHuyen != value) { _TenHuyen = value; NotifyPropertyChanged("TenHuyen"); } } }
        public string MaXa { get { return _TenXa; } set { if (_TenXa != value) { _TenXa = value; NotifyPropertyChanged("TenXa"); } } }
        public string TypeTinh { get { return _TypeTinh; } set { if (_TypeTinh != value) { _TypeTinh = value; NotifyPropertyChanged("TypeTinh"); } } }
        public string TypeHuyen { get { return _TypeHuyen; } set { if (_TypeHuyen != value) { _TypeHuyen = value; NotifyPropertyChanged("TypeHuyen"); } } }
        public string TypeXa { get { return _TypeXa; } set { if (_TypeXa != value) { _TypeXa = value; NotifyPropertyChanged("TypeXa"); } } }
        public int QtyTinh { get { return _QtyTinh; } set { if (_QtyTinh != value) { _QtyTinh = value; NotifyPropertyChanged("QtyTinh"); } } }
        public int QtyHuyen { get { return _QtyHuyen; } set { if (_QtyHuyen != value) { _QtyHuyen = value; NotifyPropertyChanged("QtyHuyen"); } } }
        public int QtyXa { get { return _QtyXa; } set { if (_QtyXa != value) { _QtyXa = value; NotifyPropertyChanged("QtyXa"); } } }

        public string TenBoPhan { get { return _TenBoPhan; } set { if (_TenBoPhan != value) { _TenBoPhan = value; NotifyPropertyChanged("TenBoPhan"); } } }
        public string TenPhongBan { get { return _TenPhongBan; } set { if (_TenPhongBan != value) { _TenPhongBan = value; NotifyPropertyChanged("TenPhongBan"); } } }
        public string TenNhom { get { return _TenNhom; } set { if (_TenNhom != value) { _TenNhom = value; NotifyPropertyChanged("TenNhom"); } } }
        public string Perm_TenTinh { get { return _Perm_TenTinh; } set { if (_Perm_TenTinh != value) { _Perm_TenTinh = value; NotifyPropertyChanged("Perm_TenTinh"); } } }
        public string Perm_TenHuyen { get { return _Perm_TenHuyen; } set { if (_Perm_TenHuyen != value) { _Perm_TenHuyen = value; NotifyPropertyChanged("Perm_TenHuyen"); } } }
        public string Perm_TenXa { get { return _Perm_TenXa; } set { if (_Perm_TenXa != value) { _Perm_TenXa = value; NotifyPropertyChanged("Perm_TenXa"); } } }
        public string Temp_TenTinh { get { return _Temp_TenTinh; } set { if (_Temp_TenTinh != value) { _Temp_TenTinh = value; NotifyPropertyChanged("Temp_TenTinh"); } } }
        public string Temp_TenHuyen { get { return _Temp_TenHuyen; } set { if (_Temp_TenHuyen != value) { _Temp_TenHuyen = value; NotifyPropertyChanged("Temp_TenHuyen"); } } }
        public string Temp_TenXa { get { return _Temp_TenXa; } set { if (_Temp_TenXa != value) { _Temp_TenXa = value; NotifyPropertyChanged("Temp_TenXa"); } } }
        public string NumberVaccine { get { return _NumberVaccine; } set { if (_NumberVaccine != value) { _NumberVaccine = value; NotifyPropertyChanged("NumberVaccine"); } } }

        public int DayShiftOn { get { return _DayShiftOn; } set { if (_DayShiftOn != value) { _DayShiftOn = value; NotifyPropertyChanged("DayShiftOn"); } } }
        public int DayShiftOff { get { return _DayShiftOff; } set { if (_DayShiftOff != value) { _DayShiftOff = value; NotifyPropertyChanged("DayShiftOff"); } } }
        public int NightShiftOn { get { return _NightShiftOn; } set { if (_NightShiftOn != value) { _NightShiftOn = value; NotifyPropertyChanged("NightShiftOn"); } } }
        public int NightShiftOff { get { return _NightShiftOff; } set { if (_NightShiftOff != value) { _NightShiftOff = value; NotifyPropertyChanged("NightShiftOff"); } } }
        public int NightShiftAll { get { return _NightShiftAll; } set { if (_NightShiftAll != value) { _NightShiftAll = value; NotifyPropertyChanged("NightShiftAll"); } } }
        public int DayShiftAll { get { return _DayShiftAll; } set { if (_DayShiftAll != value) { _DayShiftAll = value; NotifyPropertyChanged("DayShiftAll"); } } }
        public int NightShiftDelay { get { return _NightShiftDelay; } set { if (_NightShiftDelay != value) { _NightShiftDelay = value; NotifyPropertyChanged("NightShiftDelay"); } } }
        public int DayShiftDelay { get { return _DayShiftDelay; } set { if (_DayShiftDelay != value) { _DayShiftDelay = value; NotifyPropertyChanged("DayShiftDelay"); } } }
        public string RateDayOn { get { return _RateDayOn; } set { if (_RateDayOn != value) { _RateDayOn = value; NotifyPropertyChanged("RateDayOn"); } } }
        public string RateNightOn { get { return _RateNightOn; } set { if (_RateNightOn != value) { _RateNightOn = value; NotifyPropertyChanged("RateNightOn"); } } }

        public string AnSang { get { return _AnSang; } set { if (_AnSang != value) { _AnSang = value; NotifyPropertyChanged("AnSang"); } } }
        public string AnTrua { get { return _AnTrua; } set { if (_AnTrua != value) { _AnTrua = value; NotifyPropertyChanged("AnTrua"); } } }
        public string AnToi { get { return _AnToi; } set { if (_AnToi != value) { _AnToi = value; NotifyPropertyChanged("AnToi"); } } }
        public string AnDem { get { return _AnDem; } set { if (_AnDem != value) { _AnDem = value; NotifyPropertyChanged("AnDem"); } } }
        public string RateSang { get { return _RateSang; } set { if (_RateSang != value) { _RateSang = value; NotifyPropertyChanged("RateSang"); } } }
        public string RateTrua { get { return _RateTrua; } set { if (_RateTrua != value) { _RateTrua = value; NotifyPropertyChanged("RateTrua"); } } }
        public string RateDem { get { return _RateToi; } set { if (_RateToi != value) { _RateToi = value; NotifyPropertyChanged("RateToi"); } } }
        public string RateToi { get { return _RatePhu; } set { if (_RatePhu != value) { _RatePhu = value; NotifyPropertyChanged("RatePhu"); } } }

        public int QtyEmployerEtcR_D { get { return _QtyEmployerEtc_R_D; } set { if (_QtyEmployerEtc_R_D != value) { _QtyEmployerEtc_R_D = value; NotifyPropertyChanged("QtyEmployerEtcR_D"); } } }
        public int QtyEmployerEtcS_D { get { return _QtyEmployerEtc_S_D; } set { if (_QtyEmployerEtc_S_D != value) { _QtyEmployerEtc_S_D = value; NotifyPropertyChanged("QtyEmployerEtcS_D"); } } }
        public int QtyEmployerEtcS_N { get { return _QtyEmployerEtc_S_N; } set { if (_QtyEmployerEtc_S_N != value) { _QtyEmployerEtc_S_N = value; NotifyPropertyChanged("QtyEmployerEtcS_D"); } } }
        public int QtyEmployerEtcR_N { get { return _QtyEmployerEtc_R_N; } set { if (_QtyEmployerEtc_R_N != value) { _QtyEmployerEtc_R_N = value; NotifyPropertyChanged("QtyEmployerEtcR_D"); } } }

        public string NameFood { get { return _NameFood; } set { if (_NameFood != value) { _NameFood = value; NotifyPropertyChanged("NameFood"); } } }
        public int QtyFood { get { return _QtyFood; } set { if (_QtyFood != value) { _QtyFood = value; NotifyPropertyChanged("QtyFood"); } } }
        public int RateFood { get { return _RateFood; } set { if (_RateFood != value) { _RateFood = value; NotifyPropertyChanged("RateFood"); } } }
        public double OldHoliday { get { return _OldHoliday; } set { if (_OldHoliday != value) { _OldHoliday = value; NotifyPropertyChanged("OldHoliday"); } } }
        public double TotalHoliday { get { return _TotalHoliday; } set { if (_TotalHoliday != value) { _TotalHoliday = value; NotifyPropertyChanged("TotalHoliday"); } } }
        public double UsHoliday { get { return _UsHoliday; } set { if (_UsHoliday != value) { _UsHoliday = value; NotifyPropertyChanged("UsHoliday"); } } }
        public double PayHoliday { get { return _PayHoliday; } set { if (_PayHoliday != value) { _PayHoliday = value; NotifyPropertyChanged("PayHoliday"); } } }
        public string RateHoliday { get { return _RateHoliday; } set { if (_RateHoliday != value) { _RateHoliday = value; NotifyPropertyChanged("RateHoliday"); } } }
        public double OverTime { get { return _OverTime; } set { if (_OverTime != value) { _OverTime = value; NotifyPropertyChanged("OverTime"); } } }
        public double OverTime6 { get { return _OverTime6; } set { if (_OverTime6 != value) { _OverTime6 = value; NotifyPropertyChanged("OverTime6"); } } }
        public double OverTime12 { get { return _OverTime12; } set { if (_OverTime12 != value) { _OverTime12 = value; NotifyPropertyChanged("OverTime12"); } } }
        public string OtRate { get { return _OtRate; } set { if (_OtRate != value) { _OtRate = value; NotifyPropertyChanged("OtRate"); } } }
        public string OtRate6 { get { return _OtRate6; } set { if (_OtRate6 != value) { _OtRate6 = value; NotifyPropertyChanged("OtRate6"); } } }
        public string OtRate12 { get { return _OtRate12; } set { if (_OtRate12 != value) { _OtRate12 = value; NotifyPropertyChanged("OtRate12"); } } }
        public string Time { get { return _Time; } set { if (_Time != value) { _Time = value; NotifyPropertyChanged("Time"); } } }
        public string TimeType { get { return _TimeType; } set { if (_TimeType != value) { _TimeType = value; NotifyPropertyChanged("TimeType"); } } }

        private void NotifyPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
