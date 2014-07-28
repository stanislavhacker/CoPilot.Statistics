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
    public class PictureStats
    {
        #region PRIVATE

        /// <summary>
        /// Records
        /// </summary>
        private Records Records { get; set; }

        /// <summary>
        /// Picture
        /// </summary>
        public Picture Picture { get; set; }

        #endregion

        /// <summary>
        /// Picture stats
        /// </summary>
        /// <param name="records"></param>
        /// <param name="video"></param>
        public PictureStats(Records records, Picture picture)
        {
            this.Picture = picture;
            this.Records = records;
        }

        /// <summary>
        /// Get positions
        /// </summary>
        /// <returns></returns>
        public List<PositionStats> getPositions()
        {
            var picture = this.Picture;
            var start = picture.Time.Subtract(System.TimeSpan.FromSeconds(2));
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
            var picture = this.Picture;
            var start = picture.Time.Subtract(System.TimeSpan.FromSeconds(2));
            var end = start.Add(System.TimeSpan.FromSeconds(4));
            return new StateStats(this.Records, start, end);
        }

        /// <summary>
        /// Get kml for picture
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
            placemark.Name = this.Picture.Time.ToString();

            //create fodler with placemark
            Folder folder = new Folder();
            folder.Name = "Pictures";
            folder.AddFeature(placemark);

            return KmlFile.Create(folder, false);
        }
    }
}
