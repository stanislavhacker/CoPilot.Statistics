using CoPilot.Core.Data;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoPilot.Statistics.Data
{
    public class StateStats
    {
        #region PRIVATE

        /// <summary>
        /// Records
        /// </summary>
        private Records Records { get; set; }

        #endregion

        #region DATE

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        public DateTime EndDate { get; set; }

        #endregion

        /// <summary>
        /// State stats
        /// </summary>
        /// <param name="records"></param>
        /// <param name="video"></param>
        public StateStats(Records records, DateTime start, DateTime end)
        {
            this.Records = records;
            this.StartDate = start;
            this.EndDate = end;
        }

        #region DATA

        /// <summary>
        /// Avarage speed
        /// </summary>
        /// <returns></returns>
        public Double AvarageSpeed()
        {
            if (this.StartDate != null && this.EndDate != null)
            {
                return AvarageSpeed(this.StartDate, this.EndDate);
            }
            return this.Records.States.Sum(e => e.Speed) / this.Records.States.Count;
        }
        private Double AvarageSpeed(DateTime start, DateTime end)
        {
            var records = this.Records.States.Where(e => e.Time > start && e.Time <= end);
            return records.Sum(e => e.Speed) / records.Count();
        }

        #endregion

        #region GRAPHS

        #endregion

        #region POSITIONS AND STATE

        /// <summary>
        /// Get positions
        /// </summary>
        /// <returns></returns>
        public List<PositionStats> getPositions()
        {
            if (this.StartDate != null && this.EndDate != null)
            {
                return getPositions(this.StartDate, this.EndDate);
            }
            return this.Records.States
                .OrderBy(e => e.Time)
                .Select<State, PositionStats>((e) => { return new PositionStats(e.Position); })
                .ToList();
        }
        private List<PositionStats> getPositions(DateTime start, DateTime end)
        {
            return this.Records.States
                .Where(e => e.Time >= start && e.Time <= end)
                .OrderBy(e => e.Time)
                .Select<State, PositionStats>((e) => { return new PositionStats(e.Position); })
                .ToList();
        }

        /// <summary>
        /// Get state
        /// </summary>
        /// <returns></returns>
        public List<State> getStates()
        {
            if (this.StartDate != null && this.EndDate != null)
            {
                return getStates(this.StartDate, this.EndDate);
            }
            return this.Records.States
                .OrderBy(e => e.Time)
                .ToList();
        }
        private List<State> getStates(DateTime start, DateTime end)
        {
            return this.Records.States
                .Where(e => e.Time >= start && e.Time <= end)
                .OrderBy(e => e.Time)
                .ToList();
        }

        #endregion

        #region KMLS

        /// <summary>
        /// Get kml engine load
        /// </summary>
        /// <returns></returns>
        public KmlFile getKmlFileFor_EngineLoad()
        {
            //positions
            CoordinateCollection positions = new CoordinateCollection();
            List<Placemark> placemarks = new List<Placemark>();

            //records
            var states = this.getStates();
            foreach (var state in states)
            {
                StringBuilder name = new StringBuilder();
                name.Append(state.EngineLoad.ToString());
                name.Append("%");
                createRecord(positions, placemarks, state, name.ToString());
            }

            //create file
            return this.getKmlFile("Engine power", positions, placemarks);
        }

        /// <summary>
        /// Get kml engine reference torque
        /// </summary>
        /// <returns></returns>
        public KmlFile getKmlFileFor_EngineReferenceTorque()
        {
            //positions
            CoordinateCollection positions = new CoordinateCollection();
            List<Placemark> placemarks = new List<Placemark>();

            //records
            var states = this.getStates();
            foreach (var state in states)
            {
                StringBuilder name = new StringBuilder();
                name.Append(state.EngineReferenceTorque);
                name.Append("Nm");
                createRecord(positions, placemarks, state, name.ToString());
            }

            //create file
            return this.getKmlFile("Engine reference torque", positions, placemarks);
        }

        /// <summary>
        /// Get kml speed
        /// </summary>
        /// <returns></returns>
        public KmlFile getKmlFileFor_Speed()
        {
            //positions
            CoordinateCollection positions = new CoordinateCollection();
            List<Placemark> placemarks = new List<Placemark>();

            //records
            var states = this.getStates();
            foreach (var state in states)
            {
                StringBuilder name = new StringBuilder();
                name.Append(state.Speed);
                name.Append("km/h");
                createRecord(positions, placemarks, state, name.ToString());
            }

            //create file
            return this.getKmlFile("Speed", positions, placemarks);
        }

        /// <summary>
        /// Get kml rpm
        /// </summary>
        /// <returns></returns>
        public KmlFile getKmlFileFor_Rpm()
        {
            //positions
            CoordinateCollection positions = new CoordinateCollection();
            List<Placemark> placemarks = new List<Placemark>();

            //records
            var states = this.getStates();
            foreach (var state in states)
            {
                StringBuilder name = new StringBuilder();
                name.Append(state.Rpm);
                name.Append("rpm");
                createRecord(positions, placemarks, state, name.ToString());
            }

            //create file
            return this.getKmlFile("Rpm", positions, placemarks);
        }

        /// <summary>
        /// Get kml rpm
        /// </summary>
        /// <returns></returns>
        public KmlFile getKmlFileFor_EngineTemperature()
        {
            //positions
            CoordinateCollection positions = new CoordinateCollection();
            List<Placemark> placemarks = new List<Placemark>();

            //records
            var states = this.getStates();
            foreach (var state in states)
            {
                StringBuilder name = new StringBuilder();
                name.Append(state.Temperature);
                name.Append(" C");
                createRecord(positions, placemarks, state, name.ToString());
            }

            //create file
            return this.getKmlFile("Temperature", positions, placemarks);
        }

        /// <summary>
        /// Get kml flow rate
        /// </summary>
        /// <returns></returns>
        public KmlFile getKmlFileFor_FlowRate()
        {
            //positions
            CoordinateCollection positions = new CoordinateCollection();
            List<Placemark> placemarks = new List<Placemark>();

            //records
            var states = this.getStates();
            foreach (var state in states)
            {
                StringBuilder name = new StringBuilder();
                name.Append(state.MaxAirFlowRate);
                name.Append("grams/sec");
                createRecord(positions, placemarks, state, name.ToString());
            }

            //create file
            return this.getKmlFile("Maximum air flow rate", positions, placemarks);
        }

        /// <summary>
        /// Get kml Throttle positions
        /// </summary>
        /// <returns></returns>
        public KmlFile getKmlFileFor_ThrottlePosition()
        {
            //positions
            CoordinateCollection positions = new CoordinateCollection();
            List<Placemark> placemarks = new List<Placemark>();

            //records
            var states = this.getStates();
            foreach (var state in states)
            {
                StringBuilder name = new StringBuilder();
                name.Append(state.ThrottlePosition);
                name.Append("%");
                createRecord(positions, placemarks, state, name.ToString());
            }

            //create file
            return this.getKmlFile("Throttle position", positions, placemarks);
        }

        /// <summary>
        /// Get kml uptimr
        /// </summary>
        /// <returns></returns>
        public KmlFile getKmlFileFor_Uptime()
        {
            //positions
            CoordinateCollection positions = new CoordinateCollection();
            List<Placemark> placemarks = new List<Placemark>();

            //records
            var states = this.getStates();
            foreach (var state in states)
            {
                StringBuilder name = new StringBuilder();
                name.Append(state.Uptime);
                name.Append("sec");
                createRecord(positions, placemarks, state, name.ToString());
            }

            //create file
            return this.getKmlFile("Engine uptime", positions, placemarks);
        }

        #endregion






        #region PRIVATE

        /// <summary>
        /// Generate kml file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="positions"></param>
        /// <param name="placemarks"></param>
        /// <returns></returns>
        private KmlFile getKmlFile(String name, CoordinateCollection positions, List<Placemark> placemarks)
        {
            //create line string
            LineString path = new LineString();
            path.Tessellate = false;
            path.Coordinates = positions;

            //create placemark
            Placemark placemark = new Placemark();
            placemark.Geometry = path;
            placemark.Name = name;

            //create fodler with placemarks
            Folder folder = new Folder();
            folder.AddFeature(placemark);
            foreach (var mark in placemarks)
            {
                folder.AddFeature(mark);
            }

            return KmlFile.Create(folder, false);
        }


        /// <summary>
        /// Create one record
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="state"></param>
        /// <param name="name"></param>
        private void createRecord(CoordinateCollection positions, List<Placemark> placemarks, State state, String name)
        {
            //position
            var position = new PositionStats(state.Position);

            //create placemark
            Point point = new Point();
            point.Coordinate = new Vector(position.Latitude, position.Longitude);

            Placemark placemark = new Placemark();
            placemark.Geometry = point;
            placemark.Name = name;

            positions.Add(new Vector(position.Latitude, position.Longitude));
            placemarks.Add(placemark);
        }

        #endregion
    }
}
