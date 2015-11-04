using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using static System.Double;

namespace ATMViewModel
{
    public class LocationMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double width = System.Convert.ToDouble(values[0]);
            double height = System.Convert.ToDouble(values[1]);
            double pointx;
            if (!TryParse(values[2].ToString(), out pointx)) pointx = 0;
            pointx = (pointx >= 10000 || pointx <= 90000) ? (pointx - 10000) : 0;
            double pointy;
            if (!TryParse(values[3].ToString(), out pointy)) pointy = 0;
            pointy = (pointy >= 10000 || pointy <= 90000) ? (pointy - 10000) : 0;
            if (pointx < 1  || pointy < 1) return new Thickness(-100, -100, 0, 0);
            
            var thicknes = new Thickness((int)(width / 80000 * pointx), (int)(height / 80000 * pointy), 0, 0);

            return thicknes;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}