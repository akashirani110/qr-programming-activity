using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainStopSequence
{
    public class Station
    {
        public Station(string name, bool isStopping)
        {
            Name = name;
            IsStopping = isStopping;
        }

        public string Name { get; set; }
        public bool IsStopping  { get; set; }
    }
}
