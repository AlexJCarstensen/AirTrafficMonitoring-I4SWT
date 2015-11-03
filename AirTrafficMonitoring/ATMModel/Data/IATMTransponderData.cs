namespace ATMModel.Data
{
    public interface IATMTransponderData
    {
        int CompassCourse { get; set; }
        IATMCoordinate Coordinate { get; set; }
        int HorizontalVelocity { get; set; }
        string Tag { get; set; }
        string Timestamp { get; set; }
    }
}