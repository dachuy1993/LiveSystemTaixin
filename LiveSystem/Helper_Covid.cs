using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSystem
{
    public class Helper_Covid
    {
        private int _ID;
        private string _EmpId;
        private string _EmpNm;
        private string _Dept;
        private string _ContactF;
        private string _StatusContact;
        private string _HistoryTravel;
        private string _StatusHealth;
        private string _Year;
        private string _Month;
        private string _Day;
        private string _Date;
        public int ID { get { return _ID; } set { if (_ID != value) { _ID = value; NotifyPropertyChanged("ID"); } } }
        public string EmpId { get { return _EmpId; } set { if (_EmpId != value) { _EmpId = value; NotifyPropertyChanged("EmpId"); } } }
        public string EmpNm { get { return _EmpNm; } set { if (_EmpNm != value) { _EmpNm = value; NotifyPropertyChanged("EmpNm"); } } }
        public string Dept { get { return _Dept; } set { if (_Dept != value) { _Dept = value; NotifyPropertyChanged("Dept"); } } }
        public string ContactF { get { return _ContactF; } set { if (_ContactF != value) { _ContactF = value; NotifyPropertyChanged("ContactF"); } } }
        public string StatusContact { get { return _StatusContact; } set { if (_StatusContact != value) { _StatusContact = value; NotifyPropertyChanged("StatusContact"); } } }
        public string HistoryTravel { get { return _HistoryTravel; } set { if (_HistoryTravel != value) { _HistoryTravel = value; NotifyPropertyChanged("HistoryTravel"); } } }
        public string StatusHealth { get { return _StatusHealth; } set { if (_StatusHealth != value) { _StatusHealth = value; NotifyPropertyChanged("StatusHealth"); } } }
        public string Year { get { return _Year; } set { if (_Year != value) { _Year = value; NotifyPropertyChanged("Year"); } } }
        public string Month { get { return _Month; } set { if (_Month != value) { _Month = value; NotifyPropertyChanged("Month"); } } }
        public string Day { get { return _Day; } set { if (_Day != value) { _Day = value; NotifyPropertyChanged("Day"); } } }
        public string Date { get { return _Date; } set { if (_Date != value) { _Date = value; NotifyPropertyChanged("Date"); } } }

        private void NotifyPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
