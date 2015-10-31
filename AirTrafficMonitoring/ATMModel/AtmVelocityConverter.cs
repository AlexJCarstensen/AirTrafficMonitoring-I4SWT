using System;

namespace ATMModel
{
    public class ATMVelocityConverter : IATMVelocityConverter
    {
        public ATMVelocityConverter(){}
        public double Convert(IATMCoordinate oldCoordinate, IATMCoordinate newCoordinate, IATMTransponderData oldTimestamp, IATMTransponderData newTimestamp)
        {
            var velocity =
                Math.Sqrt(Math.Pow((newCoordinate.X - oldCoordinate.X), 2) +
                          Math.Pow((newCoordinate.Y - oldCoordinate.Y), 2));
            velocity = velocity/
                       ((double.TryParse(newTimestamp.Timestamp) - double.TryParse(oldTimestamp.Timestamp))/1000);
            return velocity;
        }
    }
}