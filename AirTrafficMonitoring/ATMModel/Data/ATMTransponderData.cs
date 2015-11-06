namespace ATMModel.Data
{
    public class ATMTransponderData : IATMTransponderData
    {
        public ATMTransponderData()
        {
        }
        public ATMTransponderData(string tag, int coordinateX, int coordinateY, int coordinateZ, string timestamp, int horizontalVelocity = 0, int compassCourse = 0)
        {
            Tag = tag;
            Coordinate = new ATMCoordinate(coordinateX, coordinateY, coordinateZ);
            Timestamp = timestamp;
            HorizontalVelocity = horizontalVelocity;
            CompassCourse = compassCourse;
        }

        public ATMTransponderData(string tag, IATMCoordinate coordinate, string timestamp, int horizontalVelocity = 0, int compassCourse = 0)
        {
            Tag = tag;
            Coordinate = coordinate;
            Timestamp = timestamp;
            HorizontalVelocity = horizontalVelocity;
            CompassCourse = compassCourse;
        }

        public int CompassCourse { get; set; }
        public IATMCoordinate Coordinate { get; set; }
        public int HorizontalVelocity { get; set; }
        public string Tag { get; set; }
        public string Timestamp { get; set; }
    }
}