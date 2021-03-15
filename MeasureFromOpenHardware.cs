using System;
using System.Collections.Generic;
using System.Text;
using OpenHardwareMonitor;
using OpenHardwareMonitor.Hardware;
using OpenHardwareMonitor.Hardware.CPU;

namespace MeasurementService
{
    internal class MeasureFromOpenHardware : Measure
    {
        public override string CpuTempTotal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string CpuUsageTotal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override int PeriodTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void refresh()
        {
            throw new NotImplementedException();
        }

        public override string refreshToString()
        {
            throw new NotImplementedException();
        }
    }
}
