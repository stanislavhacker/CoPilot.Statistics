using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoPilot.Statistics.Graph
{
    public class GraphData_DateTime
    {
        /// <summary>
        /// Y
        /// </summary>
        public List<double> Y { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public List<DateTime> X { get; set; }

        /// <summary>
        /// Legend X
        /// </summary>
        public String LegendX { get; set; }

        /// <summary>
        /// Legend Y
        /// </summary>
        public String LegendY { get; set; }

        /// <summary>
        /// Graph data
        /// </summary>
        public GraphData_DateTime()
        {
            this.X = new List<DateTime>();
            this.Y = new List<double>();
        }
 
    }
}
