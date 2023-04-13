using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSystem
{
    public class Helper_DataButton : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string ContentButton { get; set; }
        public string _ImageSource { get; set; }
        public PinValue _CheckCreatTab;
        public PinValue _BackGround { get; set; }

        public string ImageSource
        {
            get { return this._ImageSource; }
            set
            {
                if (this._ImageSource != value)
                {
                    this._ImageSource = value;
                    NotifyPropertyChanged("ImageSource");
                }
            }
        }

        public PinValue CheckCreatTab
        {
            get { return this._CheckCreatTab; }
            set
            {
                if (this._CheckCreatTab != value)
                {
                    this._CheckCreatTab = value;
                    NotifyPropertyChanged("CheckCreatTab");
                }
            }
        }

        public PinValue BackGroundColor
        {
            get { return this._BackGround; }
            set
            {
                if (this._BackGround != value)
                {
                    this._BackGround = value;
                    NotifyPropertyChanged("BackGroundColor");
                }
            }
        }
        private void NotifyPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
