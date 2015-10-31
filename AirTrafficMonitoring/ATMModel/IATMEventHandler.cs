using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace ATMModel
{
    public interface IATMEventHandler
    {
        void Handle(IEnumerable<IATMTransponderData> data);
    }
}