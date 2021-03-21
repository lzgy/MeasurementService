using System;
using System.Collections.Generic;
using System.Text;
using OpenHardwareMonitor;
using OpenHardwareMonitor.Hardware;
using OpenHardwareMonitor.Hardware.CPU;

namespace MeasurementService
{
    class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
    internal class MeasureFromOpenHardware : Measure
    {
        UpdateVisitor updateVisitor;
        Computer computer ;

        public override string CpuTempTotal 
        {
            get 
            {
                computer.Accept(updateVisitor);
                return computer.Hardware[0].Sensors[18].Value.ToString(); 
            }
            set { CpuTempTotal = value; } 
        }
        public override string CpuUsageTotal 
        { 
            get 
            {
                computer.Accept(updateVisitor);
                return computer.Hardware[0].Sensors[16].Value.ToString(); 
            }
            set { CpuUsageTotal = value; }
        }
        public override int PeriodTime 
        { get; set; }

        public MeasureFromOpenHardware() 
        {
            updateVisitor = new UpdateVisitor();
            computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.Accept(updateVisitor);
        }

        /// <summary>
        /// Nincs implementálva
        /// </summary>
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
