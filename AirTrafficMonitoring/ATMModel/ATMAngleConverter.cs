using System;

namespace ATMModel
{
    class ATMAngleConverter : IATMAngleConverter
    {
        public double Convert(IATMCoordinate oldCoordinate, IATMCoordinate newCoordinate)
        {
            var theta = Math.Atan2(oldCoordinate.Y - newCoordinate.Y, oldCoordinate.X - newCoordinate.X);
            theta += Math.PI/2.0;
            var angle = theta*(180/Math.PI);
            if (angle < 0)
                angle += 360;
            return angle;
        }
    }
}