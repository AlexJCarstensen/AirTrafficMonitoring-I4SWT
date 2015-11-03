using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace ATMModel.Events
{
    public interface IATMEventHandler
    {
        void Handle(List<IATMTransponderData> atmTransponderDatas);
    }
}