using GpsCalculation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoPilot.Statistics.Data
{
    public class PositionStats
    {
        /// <summary>
        /// Latitude
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public Boolean Unknow { get; set; }

        /// <summary>
        /// Position stats
        /// </summary>
        /// <param name="position"></param>
        public PositionStats(String position)
        {
            if (position != null && position != "Unknown")
            {
                var pos = position.Split(',');

                this.Latitude = Double.Parse(pos[0], CultureInfo.InvariantCulture);
                this.Longitude = Double.Parse(pos[1], CultureInfo.InvariantCulture);
                this.Unknow = false;
            }
            else
            {
                this.Unknow = true;
            }
        }

        /// <summary>
        /// Position stats
        /// </summary>
        public GeoPosition getPosition()
        {
            if (!this.Unknow)
            {
                return new GeoPosition(this.Latitude, this.Longitude, 10);
            }
            else
            {
                return new GeoPosition(0, 0, 0);
            }
        }
    }
}
