namespace ATMModel
{
    public interface IATMVelocityConverter
    {
        double Convert(IATMCoordinate oldCoordinate, IATMCoordinate newCoordinate, string oldTimestamp,
            string newTimestamp);
    }
}