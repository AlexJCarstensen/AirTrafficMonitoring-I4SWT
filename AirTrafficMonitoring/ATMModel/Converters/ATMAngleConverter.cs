using System;
using ATMModel.Data;

namespace ATMModel.Converters
{
    public class ATMAngleConverter : IATMAngleConverter
    {
        public double Convert(IATMCoordinate oldCoordinate, IATMCoordinate newCoordinate)
        {
            if (oldCoordinate == null || newCoordinate == null) throw new ArgumentNullException(nameof(oldCoordinate) + " or " + nameof(newCoordinate) + " are null");
            var theta = Math.Atan2(oldCoordinate.Y - newCoordinate.Y, oldCoordinate.X - newCoordinate.X);
            theta += Math.PI/2.0;
            var angle = theta*(180/Math.PI);
            if (angle < 0)
                angle += 360;
            return Math.Round(angle, 2);
        }
    }
}