using CoPilot.Core.Data;
using GpsCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoPilot.Statistics.Data
{
    public class RouteStats
    {
        #region PRIVATE

        /// <summary>
        /// States
        /// </summary>
        private List<State> states = new List<State>();

        /// <summary>
        /// States
        /// </summary>
        private Statistics statistics;

        #endregion

        #region PROPERTY

        /// <summary>
        /// Uid
        /// </summary>
        private String uid = Guid.NewGuid().ToString();
        public String Uid
        {
            get
            {
                return uid;
            }
        }


        /// <summary>
        /// Start DateTime
        /// </summary>
        private DateTime startDate = DateTime.MinValue;
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
        }

        /// <summary>
        /// End DateTime
        /// </summary>
        private DateTime endDate = DateTime.MinValue;
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
        }

        /// <summary>
        /// ConsumedFuel
        /// </summary>
        public Double ConsumedFuel
        {
            get
            {
                return this.getConsumedFuel();
            }
        }

        /// <summary>
        /// TraveledDistance
        /// </summary>
        public Double TraveledDistance
        {
            get
            {
                return this.getTraveledDistance();
            }
        }

        /// <summary>
        /// Distance
        /// </summary>
        public Distance Distance
        {
            get
            {
                return this.getDistanceSetting();
            }
        }

        #endregion

        /// <summary>
        /// Route stats
        /// </summary>
        /// <param name="date"></param>
        /// <param name="states"></param>
        public RouteStats(Statistics statistics, DateTime date, List<State> states)
        {
            this.statistics = statistics;
            this.states = states;
            this.startDate = date;
            this.endDate = states.Last().Time;
        }

        /// <summary>
        /// Get states
        /// </summary>
        /// <returns></returns>
        public List<State> getStates()
        {
            return this.states;
        }

        /// <summary>
        /// Get consumed fuel
        /// </summary>
        /// <returns></returns>
        private Double getConsumedFuel()
        {
            Double distance = this.getTraveledDistance();
            Double avarage = this.statistics.getFuelStats().AverageConsumption(this.statistics.getDistanceSetting());

            return distance * avarage;
        }

        /// <summary>
        /// Traveled distance
        /// </summary>
        /// <returns></returns>
        private Double getTraveledDistance()
        {
            List<State> states = this.getStates();
            Double distance = 0;
            Geo g;

            for(var i = 0; i  < states.Count - 1; i++) 
            {
                PositionStats p1 = new PositionStats(states[i].Position);
                PositionStats p2 = new PositionStats(states[i + 1].Position);
                if (p1.Unknow == false && p2.Unknow == false) 
                {
                    g = new Geo(new GeoPosition(p1.Latitude, p1.Longitude, 0));
                    distance += g.distanceTo(new GeoPosition(p2.Latitude, p2.Longitude, 0));
                }
            }

            return distance;
        }

        /// <summary>
        /// Get distance
        /// </summary>
        /// <returns></returns>
        private Distance getDistanceSetting()
        {
            return this.statistics.getDistanceSetting();
        }
    }
}
