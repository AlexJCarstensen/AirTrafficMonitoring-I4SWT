using System;
using System.Globalization;
using ATMModel.Data;

namespace ATMModel.Converters
{
    /// <summary>
    /// This class convert two points and time to velocity
    /// </summary>
    public class ATMVelocityConverter : IATMVelocityConverter
    {
        /// <summary>
        /// Convert velocity m/s
        /// </summary>
        /// <param name="oldCoordinate"></param>
        /// <param name="newCoordinate"></param>
        /// <param name="oldTimestamp"></param>
        /// <param name="newTimestamp"></param>
        /// <returns></returns>
        public double Convert(IATMCoordinate oldCoordinate, IATMCoordinate newCoordinate, string oldTimestamp, string newTimestamp)
        {
            // check for null reference
            if (oldCoordinate == null || newCoordinate == null || oldTimestamp == null || newTimestamp == null) throw new ArgumentNullException(nameof(newCoordinate) +", " + nameof(oldCoordinate) + ", " + nameof(oldTimestamp) + ", " + nameof(newTimestamp) + " are null");

            // calculate tha distance between points in meter
            var velocity =
                Math.Sqrt(Math.Pow((newCoordinate.X - oldCoordinate.X), 2) +
                          Math.Pow((newCoordinate.Y - oldCoordinate.Y), 2));
            // confirm that time difference is not negativ or 0
            if((double.Parse(newTimestamp, CultureInfo.CurrentCulture.NumberFormat) - double.Parse(oldTimestamp, CultureInfo.CurrentCulture.NumberFormat) <= 0) || Math.Abs(velocity) < 3)
                return 0;
            // calculate velocity
            velocity = velocity/
                       ((double.Parse(newTimestamp, CultureInfo.CurrentCulture.NumberFormat) - double.Parse(oldTimestamp, CultureInfo.CurrentCulture.NumberFormat))/1000);
            // convert 2 precision float and return it
            return Math.Round(velocity, 2);
        }
    }
}