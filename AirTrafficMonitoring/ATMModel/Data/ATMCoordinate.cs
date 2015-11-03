namespace ATMModel.Data
{
    public class ATMCoordinate : IATMCoordinate
    {
        public ATMCoordinate(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public bool Validate => X >= 10000 && X <= 90000 && Y >= 10000 && Y <= 90000 && Z >= 500 && Z <= 20000;
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
}