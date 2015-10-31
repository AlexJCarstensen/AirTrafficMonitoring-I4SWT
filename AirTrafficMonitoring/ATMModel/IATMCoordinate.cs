namespace ATMModel
{
    public interface IATMCoordinate
    {
        bool Validate { get; }
        int X { get; set; }
        int Y { get; set; }
        int Z { get; set; }

    }
}
