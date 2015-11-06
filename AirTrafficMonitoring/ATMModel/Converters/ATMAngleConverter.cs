using System;
using ATMModel.Data;

namespace ATMModel.Converters
{
    /// <summary>
    /// This class convert two points to angle
    /// </summary>
    public class ATMAngleConverter : IATMAngleConverter
    {
        /// <summary>
        /// Take two point and return angle of it.
        /// 0 in north
        /// 270 in east
        /// </summary>
        /// <param name="oldCoordinate"></param>
        /// <param name="newCoordinate"></param>
        /// <returns></returns>
        public double Convert(IATMCoordinate oldCoordinate, IATMCoordinate newCoordinate)
        {
            // check for null reference
            if (oldCoordinate == null || newCoordinate == null) throw new ArgumentNullException(oldCoordinate == null ? nameof(oldCoordinate) : nameof(newCoordinate), nameof(oldCoordinate) + " or " + nameof(newCoordinate) + " are null");

            // calculate teta
            var theta = Math.Atan2(oldCoordinate.Y - newCoordinate.Y, oldCoordinate.X - newCoordinate.X);
            // rotate half pi degree
            theta += Math.PI/2.0;
            //convert to degree
            var angle = theta*(180/Math.PI);
            if (angle < 0)
                angle += 360;
            // convert to 2 float precision
            return Math.Round(angle, 2);
        }
    }
}