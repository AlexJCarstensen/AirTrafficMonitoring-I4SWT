﻿using System.Collections.Generic;
using ATMModel.Data;

namespace ATMModel.Events
{
    public interface IATMEventHandler
    {
        void Handle(ICollection<IATMTransponderData> atmTransponderDatas);
    }
}