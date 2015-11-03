using ATMModel.Data;

namespace ATMModel.Converters
{
    public interface IATMAngleConverter
    {
        double Convert(IATMCoordinate oldCoordinate, IATMCoordinate newCoordinate);
    }
}