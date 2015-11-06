namespace ATMModel.Data
{
    public class ATMCoordinate : IATMCoordinate
    {
        public ATMCoordinate(int coordinateX, int coordinateY, int coordinateZ)
        {
            X = coordinateX;
            Y = coordinateY;
            Z = coordinateZ;
        }
        public bool Validate => X >= 10000 && X <= 90000 && Y >= 10000 && Y <= 90000 && Z >= 500 && Z <= 20000;
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
}