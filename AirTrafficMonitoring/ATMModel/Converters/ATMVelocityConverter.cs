using System;
using ATMModel.Data;

namespace ATMModel.Converters
{
    public class ATMVelocityConverter : IATMVelocityConverter
    {
        public double Convert(IATMCoordinate oldCoordinate, IATMCoordinate newCoordinate, string oldTimestamp, string newTimestamp)
        {
            var velocity =
                Math.Sqrt(Math.Pow((newCoordinate.X - oldCoordinate.X), 2) +
                          Math.Pow((newCoordinate.Y - oldCoordinate.Y), 2));
            velocity = velocity/
                       ((double.Parse(newTimestamp) - double.Parse(oldTimestamp))/1000);
            return Math.Round(velocity, 2);
        }
    }
}