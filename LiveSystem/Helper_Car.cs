using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSystem
{
    public class Helper_Car
    {
        private int _ID;
        private string _CarID;
        private string _CarType;
        private string _Name;
        private string _Tel;
        private string _Status;
        private string _KmNumber;
        private string _NameOrder;
        private string _DeptOrder;
        private string _FirPos;
        private string _EndPos;
        private string _TimeOn;
        private string _TimeEnd;
        private string _Date;
        private string _Index;
        private string _Color;
        private string _Km;
        private string _KmLimit;
        private string _KmUs;
        public int ID { get { return _ID; } set { if (_ID != value) { _ID = value; NotifyPropertyChanged("ID"); } } }       
        public string CarID { get { return _CarID; } set { if (_CarID != value) { _CarID = value; NotifyPropertyChanged("CarID"); } } }
        public string CarType { get { return _CarType; } set { if (_CarType != value) { _CarType = value; NotifyPropertyChanged("CarType"); } } }
        public string Name { get { return _Name; } set { if (_Name != value) { _Name = value; NotifyPropertyChanged("Name"); } } }
        public string Tel { get { return _Tel; } set { if (_Tel != value) { _Tel = value; NotifyPropertyChanged("Tel"); } } }
        public string Status { get { return _Status; } set { if (_Status != value) { _Status = value; NotifyPropertyChanged("Status"); } } }
        public string KmNumber { get { return _KmNumber; } set { if (_KmNumber != value) { _KmNumber = value; NotifyPropertyChanged("KmNumber"); } } }
        public string NameOrder { get { return _NameOrder; } set { if (_NameOrder != value) { _NameOrder = value; NotifyPropertyChanged("NameOder"); } } }
        public string DeptOrder { get { return _DeptOrder; } set { if (_DeptOrder != value) { _DeptOrder = value; NotifyPropertyChanged("DeptOder"); } } }
        public string FirPos { get { return _FirPos; } set { if (_FirPos != value) { _FirPos = value; NotifyPropertyChanged("FirPos"); } } }
        public string EndPos { get { return _EndPos; } set { if (_EndPos != value) { _EndPos = value; NotifyPropertyChanged("EndPos"); } } }
        public string TimeOn { get { return _TimeOn; } set { if (_TimeOn != value) { _TimeOn = value; NotifyPropertyChanged("TimeOn"); } } }
        public string TimeEnd { get { return _TimeEnd; } set { if (_TimeEnd != value) { _TimeEnd = value; NotifyPropertyChanged("TimeEnd"); } } }
        public string Date { get { return _Date; } set { if (_Date != value) { _Date = value; NotifyPropertyChanged("Date"); } } }
        public string Index { get { return _Index; } set { if (_Index != value) { _Index = value; NotifyPropertyChanged("Index"); } } }
        public string Color { get { return _Color; } set { if (_Color != value) { _Color = value; NotifyPropertyChanged("Color"); } } }
        public string Km { get { return _Km; } set { if (_Km != value) { _Km = value; NotifyPropertyChanged("Km"); } } }
        public string KmLimit { get { return _KmLimit; } set { if (_KmLimit != value) { _KmLimit = value; NotifyPropertyChanged("KmLimit"); } } }
        public string KmUs { get { return _KmUs; } set { if (_KmUs != value) { _KmUs = value; NotifyPropertyChanged("KmUs"); } } }

        private void NotifyPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
