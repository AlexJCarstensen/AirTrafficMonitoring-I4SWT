using System;
using System.Globalization;
using ATMModel.Data;

namespace ATMModel.Converters
{
    public class ATMVelocityConverter : IATMVelocityConverter
    {
        public double Convert(IATMCoordinate oldCoordinate, IATMCoordinate newCoordinate, string oldTimestamp, string newTimestamp)
        {
            if (oldCoordinate == null || newCoordinate == null || oldTimestamp == null || newTimestamp == null) throw new ArgumentNullException();

            var velocity =
                Math.Sqrt(Math.Pow((newCoordinate.X - oldCoordinate.X), 2) +
                          Math.Pow((newCoordinate.Y - oldCoordinate.Y), 2));
            if((double.Parse(newTimestamp, CultureInfo.CurrentCulture.NumberFormat) - double.Parse(oldTimestamp, CultureInfo.CurrentCulture.NumberFormat) <= 0) || Math.Abs(velocity) < 3)
                return 0;
            velocity = velocity/
                       ((double.Parse(newTimestamp, CultureInfo.CurrentCulture.NumberFormat) - double.Parse(oldTimestamp, CultureInfo.CurrentCulture.NumberFormat))/1000);
            return Math.Round(velocity, 2);
        }
    }
}