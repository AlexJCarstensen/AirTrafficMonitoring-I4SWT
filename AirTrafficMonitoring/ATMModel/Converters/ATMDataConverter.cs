using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Converters
{
    public class ATMDataConverter : IATMDataConverter
    {
        private readonly IATMAngleConverter _angle;
        private readonly IATMVelocityConverter _velocity;
        private static ICollection<IATMTransponderData> _localDataItems = new List<IATMTransponderData>();
        public ATMDataConverter(IATMAngleConverter angle, IATMVelocityConverter velocity)
        {
            _angle = angle;
            _velocity = velocity;
        }

        public List<IATMTransponderData> Convert(ICollection<string> list) 
        {
            var currentItems = new List<IATMTransponderData>();
            foreach (var newItem in from data in list.Select(item => item.Split(';')) let coordinate = new ATMCoordinate(int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3])) where coordinate.Validate select new ATMTransponderData(
                data[0],
                coordinate,
                data[4]
                ))
            {
                if (_localDataItems.All(t => t.Tag != newItem.Tag))
                {
                    currentItems.Add(newItem);
                    continue;
                }

                var oldItem = _localDataItems.First(t => t.Tag == newItem.Tag);
                newItem.CompassCourse = (int)_angle.Convert(oldItem.Coordinate, newItem.Coordinate);
                newItem.HorizontalVelocity = (int)_velocity.Convert(oldItem.Coordinate, newItem.Coordinate, oldItem.Timestamp, newItem.Timestamp);
                currentItems.Add(newItem);
            }
            _localDataItems = currentItems;
            return _localDataItems.ToList();
            
        }
    }
}