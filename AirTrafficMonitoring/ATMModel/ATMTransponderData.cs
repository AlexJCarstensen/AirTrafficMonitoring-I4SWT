namespace ATMModel
{
    public class ATMTransponderData : IATMTransponderData
    {
        public ATMTransponderData()
        {
        }
        public ATMTransponderData(string tag, int x, int y, int z, string timestamp, int hv = 0, int cc = 0)
        {
            Tag = tag;
            Coordinate = new ATMCoordinate(x, y, z);
            Timestamp = timestamp;
            HorizontalVelocity = hv;
            CompassCourse = cc;
        }

        public int CompassCourse { get; set; }
        public IATMCoordinate Coordinate { get; set; }
        public int HorizontalVelocity { get; set; }
        public string Tag { get; set; }
        public string Timestamp { get; set; }
    }
}