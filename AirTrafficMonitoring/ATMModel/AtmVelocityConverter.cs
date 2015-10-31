using System;

namespace ATMModel
{
    public class ATMVelocityConverter : IATMVelocityConverter
    {
        public double Convert(IATMCoordinate oldCoordinate, IATMCoordinate newCoordinate, IATMTransponderData oldTimestamp, IATMTransponderData newTimestamp)
        {
            var velocity =
                Math.Sqrt(Math.Pow((newCoordinate.X - oldCoordinate.X), 2) +
                          Math.Pow((newCoordinate.Y - oldCoordinate.Y), 2));
            velocity = velocity/
                       ((double.Parse(newTimestamp.Timestamp) - double.Parse(oldTimestamp.Timestamp))/1000);
            return velocity;
        }
    }
}