using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace LiveSystem
{

    public enum PinValue { ON, OFF, NotRun };
    public class ConverterColor_LightGray_Green : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            if ((PinValue)value == PinValue.ON)
            {
                //return new SolidColorBrush(Color.FromRgb(200, 230, 250));//Màu xanh nước biển nhẹ

                return new SolidColorBrush(Colors.LightGreen);//Màu xanh lá
            }
            else
            {
                return new SolidColorBrush(Colors.LightGray);//Màu xám hơi đậm
            }
            //else if ((PinValue)value == PinValue.OFF)
            //{
            //    return new SolidColorBrush(Colors.LightGray);//Màu xám
            //}
            //else return new SolidColorBrush(Colors.Red);//Màu đỏ nhẹ

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterColor_DimGray_LightGreen : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)ColorConverter.ConvertFromString("#BEBEBE");
            if ((PinValue)value == PinValue.ON)
            {
                //return new SolidColorBrush(Color.FromRgb(200, 230, 250));//Màu xanh nước biển nhẹ

                return new SolidColorBrush(Colors.LightGreen);//Màu xanh lá
            }
            else
            {
                return new SolidColorBrush(color);//Màu xám hơi đậm
            }
            //else if ((PinValue)value == PinValue.OFF)
            //{
            //    return new SolidColorBrush(Colors.LightGray);//Màu xám
            //}
            //else return new SolidColorBrush(Colors.Red);//Màu đỏ nhẹ

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConverterColor_Red : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((PinValue)value == PinValue.ON)
            {
                return new SolidColorBrush(Colors.Red);
            }
            else return new SolidColorBrush(Colors.LightGray);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConverterColor_Yellow : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((PinValue)value == PinValue.ON || (PinValue)value == PinValue.NotRun)
            {
                return new SolidColorBrush(Colors.Yellow);
            }
            return new SolidColorBrush(Colors.LightGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConverterColor_Gray: IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((PinValue)value == PinValue.ON || (PinValue)value == PinValue.NotRun)
            {
                return new SolidColorBrush(Colors.LightGray);
            }
            return new SolidColorBrush(Colors.LightGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterColor_RowGray : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((PinValue)value == PinValue.ON)
            {
                return new SolidColorBrush(Colors.AliceBlue);
            }
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #region Đổi màu các hàng trong ListView

    public class ConverColorRowListView : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double number;
            if (double.TryParse(value.ToString(), out number))
            {
                //return Math.Sqrt(number);
                if (number % 2 != 0) return -1;
                if (number % 2 == 0) return +1;
                //if (number == 0) return 0;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
