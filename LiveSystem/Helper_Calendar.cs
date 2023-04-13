using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSystem
{
    public class Helper_CalendarData : INotifyPropertyChanged
    {
        private string _Id;
        private string _EmpId;
        private string _EmpNm;
        private string _Depatment;
        private string _NoteNo;
        private string _dateFrom;
        private string _dateTo;
        private string _timeFrom;
        private string _timeTo;
        private string _addrFrom;
        private string _addrTo;
        private string _Note;
        private string _Date;
        private string _etc1;
        private string _etc2;
        private string _etc3;
        private string _etc4;
        private string _etc5;
        private string _etc6;
        private string _etc7;
        private string _etc8;
        private string _etc9;
        private string _Insdt;
        public string Id { get { return _Id; } set { if (_Id != value) { _Id = value; NotifyPropertyChanged("Id"); } } }
        public string EmpId { get { return _EmpId; } set { if (_EmpId != value) { _EmpId = value; NotifyPropertyChanged("EmpId"); } } }
        public string EmpNm { get { return _EmpNm; } set { if (_EmpNm != value) { _EmpNm = value; NotifyPropertyChanged("EmpNm"); } } }
        public string Depatment { get { return _Depatment; } set { if (_Depatment != value) { _Depatment = value; NotifyPropertyChanged("Depatment"); } } }
        public string NoteNo { get { return _NoteNo; } set { if (_NoteNo != value) { _NoteNo = value; NotifyPropertyChanged("NoteNo"); } } }
        public string dateFrom { get { return _dateFrom; } set { if (_dateFrom != value) { _dateFrom = value; NotifyPropertyChanged("dateFrom"); } } }
        public string dateTo { get { return _dateTo; } set { if (_dateTo != value) { _dateTo = value; NotifyPropertyChanged("dateTo"); } } }
        public string timeFrom { get { return _timeFrom; } set { if (_timeFrom != value) { _timeFrom = value; NotifyPropertyChanged("timeFrom"); } } }
        public string timeTo { get { return _timeTo; } set { if (_timeTo != value) { _timeTo = value; NotifyPropertyChanged("timeTo"); } } }
        public string addrFrom { get { return _addrFrom; } set { if (_addrFrom != value) { _addrFrom = value; NotifyPropertyChanged("addrFrom"); } } }
        public string addrTo { get { return _addrTo; } set { if (_addrTo != value) { _addrTo = value; NotifyPropertyChanged("addrTo"); } } }
        public string Note { get { return _Note; } set { if (_Note != value) { _Note = value; NotifyPropertyChanged("Note"); } } }
        public string Date { get { return _Date; } set { if (_Date != value) { _Date = value; NotifyPropertyChanged("Date"); } } }
        public string etc1 { get { return _etc1; } set { if (_etc1 != value) { _etc1 = value; NotifyPropertyChanged("etc1"); } } }
        public string etc2 { get { return _etc2; } set { if (_etc2 != value) { _etc2 = value; NotifyPropertyChanged("etc2"); } } }
        public string etc3 { get { return _etc3; } set { if (_etc3 != value) { _etc3 = value; NotifyPropertyChanged("etc3"); } } }
        public string etc4 { get { return _etc4; } set { if (_etc4 != value) { _etc4 = value; NotifyPropertyChanged("etc4"); } } }
        public string etc5 { get { return _etc5; } set { if (_etc5 != value) { _etc5 = value; NotifyPropertyChanged("etc5"); } } }
        public string etc6 { get { return _etc6; } set { if (_etc6 != value) { _etc6 = value; NotifyPropertyChanged("etc6"); } } }
        public string etc7 { get { return _etc7; } set { if (_etc7 != value) { _etc7 = value; NotifyPropertyChanged("etc7"); } } }
        public string etc8 { get { return _etc8; } set { if (_etc8 != value) { _etc8 = value; NotifyPropertyChanged("etc8"); } } }
        public string etc9 { get { return _etc9; } set { if (_etc9 != value) { _etc9 = value; NotifyPropertyChanged("etc9"); } } }
        public string Insdt { get { return _Insdt; } set { if (_Insdt != value) { _Insdt = value; NotifyPropertyChanged("Insdt"); } } }
        private void NotifyPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
