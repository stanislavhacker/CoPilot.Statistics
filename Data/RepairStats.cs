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
    public class RepairStats
    {
        #region PRIVATE

        /// <summary>
        /// Records
        /// </summary>
        private Records Records { get; set; }

        /// <summary>
        /// Repair
        /// </summary>
        public Repair Repair { get; set; }

        #endregion

        /// <summary>
        /// Repair stats
        /// </summary>
        /// <param name="records"></param>
        /// <param name="repair"></param>
        public RepairStats(Records records, Repair repair)
        {
            this.Repair = repair;
            this.Records = records;
        }

        /// <summary>
        /// Get positions
        /// </summary>
        /// <returns></returns>
        public List<PositionStats> getPositions()
        {
            var repair = this.Repair;
            var start = repair.Date.Subtract(System.TimeSpan.FromSeconds(2));
            var end = start.Add(System.TimeSpan.FromSeconds(4));

            return this.Records.States
                .Where(e => e.Time >= start && e.Time <= end)
                .OrderBy(e => e.Time)
                .Select<State, PositionStats>((e) => { return new PositionStats(e.Position); })
                .ToList();
        }

        /// <summary>
        /// States
        /// </summary>
        /// <returns></returns>
        public StateStats getStates()
        {
            var repair = this.Repair;
            var start = repair.Date.Subtract(System.TimeSpan.FromSeconds(2));
            var end = start.Add(System.TimeSpan.FromSeconds(4));
            return new StateStats(this.Records, start, end);
        }

        /// <summary>
        /// Get kml for repair
        /// </summary>
        /// <returns></returns>
        public KmlFile getKmlFile()
        {
            //create path
            List<PositionStats> repairPositions = this.getPositions();
            CoordinateCollection positions = new CoordinateCollection();
            foreach (var position in positions)
            {
                positions.Add(new Vector(position.Latitude, position.Longitude));
            }

            //create line string
            LineString path = new LineString();
            path.Tessellate = false;
            path.Coordinates = positions;

            //create placemark
            Placemark placemark = new Placemark();
            placemark.Geometry = path;
            placemark.Name = this.Repair.ServiceName;
            //description
            placemark.Description = new Description();
            placemark.Description.Text = this.Repair.Description;

            //create fodler with placemark
            Folder folder = new Folder();
            folder.Name = "Repairs";
            folder.AddFeature(placemark);

            return KmlFile.Create(folder, false);
        }
    }
}
