using System;
using System.Collections.Generic;
using System.Text;

namespace MeasurementService
{
    internal class StringState : State
    {
        public StringState() : base()
        {
            Data = new StringBuilder();
        }
        public StringBuilder Data { get; set; }

    }
}
