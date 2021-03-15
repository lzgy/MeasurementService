using System;
using System.Collections.Generic;
using System.Text;

namespace MeasurementService
{
    /// <summary>
    /// Abstract class for measuring
    /// </summary>
    internal abstract class Measure
    {
        /// <summary>
        /// 
        /// </summary>
        abstract public string CpuTempTotal { get; set; }
        abstract public string CpuUsageTotal { get; set; }
        abstract public int PeriodTime { get; set; }
        abstract public void refresh();

        abstract public String refreshToString();
    }
}
