﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergyContracts.Events
{
    public class RevertRemoveDeviceFromIncidentEvent
    {
        public int IncidentID { get; set; }
        public int DeviceID { get; set; }
    }
}
