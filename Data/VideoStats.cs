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
    public class VideoStats
    {
        #region PRIVATE

        /// <summary>
        /// Records
        /// </summary>
        private Records Records { get; set; }

        /// <summary>
        /// Video
        /// </summary>
        public Video Video { get; set; }

        #endregion

        /// <summary>
        /// Video stats
        /// </summary>
        /// <param name="records"></param>
        /// <param name="video"></param>
        public VideoStats(Records records, Video video)
        {
            this.Video = video;
            this.Records = records;
        }

        /// <summary>
        /// Get positions
        /// </summary>
        /// <returns></returns>
        public List<PositionStats> getPositions()
        {
            var video = this.Video;
            var start = video.Time;
            var end = video.Time.Add(video.Duration);

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
            var video = this.Video;
            var start = video.Time;
            var end = video.Time.Add(video.Duration);
            return new StateStats(this.Records, start, end);
        }

        /// <summary>
        /// Get kml for video
        /// </summary>
        /// <returns></returns>
        public KmlFile getKmlFile()
        {
            //create path
            List<PositionStats> videoPositions = this.getPositions();
            CoordinateCollection positions = new CoordinateCollection();
            foreach (var position in videoPositions)
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
            placemark.Name = this.Video.Time.ToString();

            //create fodler with placemark
            Folder folder = new Folder();
            folder.Name = "Videos";
            folder.AddFeature(placemark);

            return KmlFile.Create(folder, false);
        }
    }
}
