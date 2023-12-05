using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainStopSequence
{
    public class StopSequenceExpressSection
    {
        public StopSequenceExpressSection(Station firstStation, Station lastStation)
        {
            FirstStation = firstStation;
            LastStation = lastStation;
        }

        public Station FirstStation { get; set; }
        public Station LastStation { get; set; }
    }
}
