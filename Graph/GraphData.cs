using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoPilot.Statistics.Graph
{
    public class GraphData
    {
        /// <summary>
        /// Y
        /// </summary>
        public List<double> Y { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public List<string> X { get; set; }

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
        public GraphData()
        {
            this.X = new List<string>();
            this.Y = new List<double>();
        }
 
    }
}
