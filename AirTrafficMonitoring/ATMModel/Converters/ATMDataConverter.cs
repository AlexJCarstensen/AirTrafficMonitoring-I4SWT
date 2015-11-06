using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Converters
{
    /// <summary>
    /// This class convert list of string to list of transponderdata
    /// Contructor take angle converter and velocity converter to convert data 
    /// </summary>
    public class ATMDataConverter : IATMDataConverter
    {
        private readonly IATMAngleConverter _angle;
        private readonly IATMVelocityConverter _velocity;
        private static ICollection<IATMTransponderData> _globalTransponderDatas = new List<IATMTransponderData>();
        public ATMDataConverter(IATMAngleConverter angle, IATMVelocityConverter velocity)
        {
            _angle = angle;
            _velocity = velocity;
        }

        /// <summary>
        /// Function take collection of string and convert to collection of track
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<IATMTransponderData> Convert(ICollection<string> list) 
        {
            // local variable that holds current track we converting
            var currentItems = new List<IATMTransponderData>();
            // convert all string to tracks
            foreach (var newItem in from data in list.Select(item => item.Split(';'))
                                    let coordinate = new ATMCoordinate(int.Parse(data[1], CultureInfo.CurrentCulture.NumberFormat), int.Parse(data[2], CultureInfo.CurrentCulture.NumberFormat), int.Parse(data[3], CultureInfo.CurrentCulture.NumberFormat))
                                    where coordinate.Validate
                                    select new ATMTransponderData(
                                                                data[0],
                                                                coordinate,
                                                                data[4]
                                                                    ))
            {
                // if track was not in the airspace adding it to local collection without calculate angle and velocity
                if (_globalTransponderDatas.All(t => t.Tag != newItem.Tag))
                {
                    currentItems.Add(newItem);
                    continue;
                }
                // if track was in airspace, we find that track and calculate angle and velocity compared to last position and adding it to local track collection
                var oldItem = _globalTransponderDatas.First(t => t.Tag == newItem.Tag);
                newItem.CompassCourse = (int)_angle.Convert(oldItem.Coordinate, newItem.Coordinate);
                newItem.HorizontalVelocity = (int)_velocity.Convert(oldItem.Coordinate, newItem.Coordinate, oldItem.Timestamp, newItem.Timestamp);
                currentItems.Add(newItem);
            }
            // before we return our current (local collection), our local collection is being assigned to our global collection
            _globalTransponderDatas = currentItems;
            // now we return _globalTransponderDatas
            return _globalTransponderDatas.ToList();
            
        }
    }
}