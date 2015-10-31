using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
