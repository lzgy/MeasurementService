using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace MeasurementService
{
    class MeasureFromManagment : Measure
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
