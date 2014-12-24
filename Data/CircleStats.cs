using CoPilot.Core.Data;
using CoPilot.Core.Utils;
using GpsCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoPilot.Statistics.Data
{
    public class CircleStats
    {
        #region PRIVATE

        /// <summary>
        /// Records
        /// </summary>
        private Records Records { get; set; }

        /// <summary>
        /// Circuit
        /// </summary>
        public Circuit Circuit { get; set; }

        #endregion
        
        /// <summary>
        /// Fill stats
        /// </summary>
        /// <param name="records"></param>
        /// <param name="video"></param>
        public CircleStats(Records records, Circuit circuit)
        {
            this.Circuit = circuit;
            this.Records = records;
        }

        /// <summary>
        /// Get circuit
        /// </summary>
        /// <returns></returns>
        public Circuit getCircuit()
        {
            return this.Circuit;
        }

        /// <summary>
        /// get length
        /// </summary>
        /// <returns></returns>
        public Double getLength(Distance unit)
        {
            var positions = this.getPositions();
            var distance = 0.0;

            for (var i = 0; i < positions.Count - 1; i++)
            {
                var g = new Geo(positions[i].getPosition());
                distance += g.distanceTo(positions[i + 1].getPosition());
            }

            //recalculate
            var odometer = new Odometer(distance, Distance.Km);
            DistanceExchange.CurrentDistance = unit;
            return DistanceExchange.GetOdometerWithRightDistance(odometer);
        }

        /// <summary>
        /// Get positions
        /// </summary>
        /// <returns></returns>
        public List<PositionStats> getPositions()
        {
            return this.Circuit.States
                .OrderBy(e => e.Time)
                .Select<State, PositionStats>((e) => { return new PositionStats(e.Position); })
                .ToList();
        }
    }
}
