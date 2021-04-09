using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace MeasurementService
{
    class MeasureFromManagment : Measure
    {
        public override string CpuTempTotal 
        { 
            get { return Tempretatura(); }
            set { CpuTempTotal = value; } 
        }
        public override string CpuUsageTotal
        {
            get { return Usage(); }
            set { CpuUsageTotal = value; } 
        }

       
        public override int PeriodTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public String Tempretatura() 
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
            double temp = 0;
            foreach (ManagementObject obj in searcher.Get())
            {
                temp = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                temp = (temp - 2732) / 10.0;
            }
            return Convert.ToString(temp);
        }

        public String Usage()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\WMI", "select * from Win32_PerfFormattedData_PerfOS_Processor");
            double usage = 0;
            foreach (ManagementObject obj in searcher.Get())
            {
                usage = Convert.ToDouble(obj["PercentProcessorTime"].ToString());
                usage = (usage - 2732) / 10.0;
            }
            return Convert.ToString(usage);
        }

        public override void refresh()
        {
            throw new NotImplementedException();
        }

        public override string refreshToString()
        {
            return CpuTempTotal + ";" + CpuUsageTotal;
        }
    }
}
