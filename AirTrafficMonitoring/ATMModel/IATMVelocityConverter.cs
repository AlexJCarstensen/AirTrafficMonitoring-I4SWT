namespace ATMModel
{
    public interface IATMVelocityConverter
    {
        double Convert(IATMCoordinate oldCoordinate, IATMCoordinate newCoordinate, IATMTransponderData oldTimestamp, IATMTransponderData newTimestamp)
    }
}