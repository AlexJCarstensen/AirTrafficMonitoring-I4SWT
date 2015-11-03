using ATMModel.Data;

namespace ATMModel.Converters
{
    public interface IATMVelocityConverter
    {
        double Convert(IATMCoordinate oldCoordinate, IATMCoordinate newCoordinate, string oldTimestamp,
            string newTimestamp);
    }
}