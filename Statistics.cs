﻿using CoPilot.Core.Data;
using CoPilot.Statistics.Data;
using CoPilot.Statistics.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace CoPilot.Statistics
{
    /**
     * Support for KML files documentations and more:
     * https://sharpkml.codeplex.com/wikipage?title=Getting%20Started&referringTitle=Documentation
     * 
     */

    public class Statistics
    {
        #region PRIVATE

        /// <summary>
        /// Records
        /// </summary>
        private Records records;
        private Records Records 
        { 
            get 
            {
                return records;
            }
            set
            {
                records = value;
            }
        }

        #endregion

        /// <summary>
        /// Statistics
        /// </summary>
        /// <param name="stream"></param>
        public Statistics(Stream stream)
        {
            this.Records = Records.Load(stream);
        }

        /// <summary>
        /// Statistics
        /// </summary>
        /// <param name="stream"></param>
        public Statistics(Records records)
        {
            this.Records = records;
        }

        #region FUEL, REPAIRS

        /// <summary>
        /// Fuel stats
        /// </summary>
        /// <returns></returns>
        public FuelStats getFuelStats()
        {
            return new FuelStats(this.records);
        }

        /// <summary>
        /// Repairs stats
        /// </summary>
        /// <returns></returns>
        public RepairsStats getRepairStats()
        {
            return new RepairsStats(this.records);
        }

        #endregion

        #region STATES

        /// <summary>
        /// State stats
        /// </summary>
        /// <returns></returns>
        public StateStats getStateStats()
        {
            return new StateStats(this.records, DateTime.MinValue, DateTime.MaxValue);
        }

        /// <summary>
        /// State stats by date
        /// </summary>
        /// <returns></returns>
        public StateStats getStateStats(DateTime start, DateTime end)
        {
            return new StateStats(this.records, start, end);
        }

        #endregion

        #region ROUTES

        /// <summary>
        /// Routes
        /// </summary>
        /// <returns></returns>
        public List<RouteStats> getRoutes()
        {
            StateStats stats = new StateStats(this.records, DateTime.MinValue, DateTime.MaxValue);
            List<State> all = stats.getStates().OrderBy(e => e.Time).ToList();
            List<RouteStats> routes = new List<RouteStats>();

            if (all.Count == 0) 
            {
                return routes;
            }

            DateTime start = all.First().Time;
            DateTime tripDate = start;
            List<State> tripStates = new List<State>();

            for (var i = 0; i < all.Count; i++ )
            {
                //state
                var state = all[i];
                //check
                if (start < state.Time.Subtract(TimeSpan.FromMinutes(1)))
                {
                    //add route
                    routes.Add(new RouteStats(this, tripDate, tripStates));
                    //reset
                    tripStates = new List<State>();
                    tripDate = state.Time;
                }
                tripStates.Add(state);

                //update date
                start = state.Time;
            }

            //clear wrong routes
            for (var i = routes.Count - 1; i >= 0; i--)
            {
                if (routes[i].getStates().Count < 10 || routes[i].TraveledDistance < 0.2)
                {
                    routes.RemoveAt(i);
                }
            }

            return routes;
        }

        #endregion



        #region VIDEOS

        /// <summary>
        /// Get videos
        /// </summary>
        /// <returns></returns>
        public List<VideoStats> getVideos()
        {
            return this.Records.Videos
                .Select<Video, VideoStats>((video) => { return new VideoStats(this.Records, video); })
                .ToList();
        }

        /// <summary>
        /// Get videos for date
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<VideoStats> getVideos(DateTime start, DateTime end)
        {
            return this.Records.Videos
                .Where(e => e.Time > start && e.Time <= end)
                .Select<Video, VideoStats>((video) => { return new VideoStats(this.Records, video); })
                .ToList();
        }

        #endregion

        #region PICTURES

        /// <summary>
        /// Get pictures
        /// </summary>
        /// <returns></returns>
        public List<PictureStats> getPictures()
        {
            return this.Records.Pictures
                .Select<Picture, PictureStats>((picture) => { return new PictureStats(this.Records, picture); })
                .ToList();
        }

        /// <summary>
        /// Get pictures for date
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<PictureStats> getPictures(DateTime start, DateTime end)
        {
            return this.Records.Pictures
                .Where(e => e.Time > start && e.Time <= end)
                .Select<Picture, PictureStats>((picture) => { return new PictureStats(this.Records, picture); })
                .ToList();
        }

        #endregion

        #region FILLS

        /// <summary>
        /// Get fills
        /// </summary>
        /// <returns></returns>
        public List<FillStats> getFills()
        {
            return this.Records.Fills
                .Select<Fill, FillStats>((fill) => { return new FillStats(this.Records, fill); })
                .ToList();
        }

        /// <summary>
        /// Get pictures for date
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<FillStats> getFills(DateTime start, DateTime end)
        {
            return this.Records.Fills
                .Where(e => e.Date > start && e.Date <= end)
                .Select<Fill, FillStats>((fill) => { return new FillStats(this.Records, fill); })
                .ToList();
        }

        #endregion

        #region REPAIRS

        /// <summary>
        /// Get repairs
        /// </summary>
        /// <returns></returns>
        public List<RepairStats> getRepairs()
        {
            return this.Records.Repairs
                .Select<Repair, RepairStats>((repair) => { return new RepairStats(this.Records, repair); })
                .ToList();
        }

        /// <summary>
        /// Get repairs for date
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<RepairStats> getRepairs(DateTime start, DateTime end)
        {
            return this.Records.Repairs
                .Where(e => e.Date > start && e.Date <= end)
                .Select<Repair, RepairStats>((repair) => { return new RepairStats(this.Records, repair); })
                .ToList();
        }

        #endregion

        #region CIRCUTS

        /// <summary>
        /// Get fills
        /// </summary>
        /// <returns></returns>
        public List<CircleStats> getCircuits()
        {
            return this.Records.Times.Circuits
                .Select<Circuit, CircleStats>((circle) => { return new CircleStats(this.Records, circle); })
                .ToList();
        }

        /// <summary>
        /// Get fills
        /// </summary>
        /// <returns></returns>
        public List<CircleStats> getCircuits(DateTime start, DateTime end)
        {
            return this.Records.Times.Circuits
                .Where(e => e.Start > start && e.Start <= end)
                .Select<Circuit, CircleStats>((circle) => { return new CircleStats(this.Records, circle); })
                .ToList();
        }

        /// <summary>
        /// Get grouped circuits
        /// </summary>
        /// <returns></returns>
        public List<CircuitGroup> getGroupedCircuits()
        {
            ObservableCollection<Circuit> circuits = this.Records.Times.Circuits;
            return Statistics.GroupedCircuits(circuits);
        }

        /// <summary>
        /// GroupedCircuits
        /// </summary>
        /// <param name="circuits"></param>
        /// <returns></returns>
        public static List<CircuitGroup> GroupedCircuits(ObservableCollection<Circuit> circuits)
        {
            Dictionary<string, CircuitGroup> groups = new Dictionary<string, CircuitGroup>();

            for (var i = 0; i < circuits.Count; i++)
            {
                CircuitGroup group = null;
                var circuit = circuits[i];
                var exists = groups.ContainsKey(circuit.Id);
                //no exists
                if (!exists)
                {
                    group = new CircuitGroup();
                    //create
                    group.Laps = new List<double>();
                    group.States = new List<State>();
                    //fill
                    group.Name = circuit.Name;
                    group.States.AddRange(circuit.States);
                    group.Circuit = circuit;
                    //add
                    groups.Add(circuit.Id, group);
                }
                //get
                group = groups[circuit.Id];
                //find fastest circle
                if (group.Circuit.Laps.Min() > circuit.Laps.Min())
                {
                    group.Circuit = circuit;
                }
                //add laps
                group.Laps.AddRange(circuit.Laps);
            }

            //return
            return groups.Values.ToList();
        }

        #endregion


        #region Others

        /// <summary>
        /// Get distance
        /// </summary>
        /// <returns></returns>
        public Distance getDistanceSetting()
        {
            return this.Records.Distance;
        }

        /// <summary>
        /// Get unit
        /// </summary>
        /// <returns></returns>
        public Unit getUnitSetting()
        {
            return this.Records.Unit;
        }

        #endregion
    }
}
