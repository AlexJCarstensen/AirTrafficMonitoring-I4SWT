using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ATMModel.Data;

namespace ATMModel.Events
{
    public interface IATMEventHandler
    {
        void Handle(List<IATMTransponderData> atmTransponderDatas);
    }
}