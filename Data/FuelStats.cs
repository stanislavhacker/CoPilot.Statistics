using CoPilot.Core.Data;
using CoPilot.Core.Utils;
using CoPilot.Statistics.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoPilot.Statistics.Data
{
    public class FuelStats
    {
        #region PRIVATE

        /// <summary>
        /// Records
        /// </summary>
        private Records Records { get; set; }

        #endregion

        /// <summary>
        /// Fuel stats
        /// </summary>
        /// <param name="records"></param>
        /// <param name="video"></param>
        public FuelStats(Records records)
        {
            this.Records = records;
        }

        #region DATA

        /// <summary>
        /// Consumption
        /// </summary>
        /// <returns></returns>
        public Double AverageConsumption(Distance distance)
        {
            DistanceExchange.CurrentDistance = distance;
            return this.Records.AverageConsumption();
        }

        /// <summary>
        /// Paid for fuel
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public Double PaidForFuel(Currency currency)
        {
            return this.Records.Fills
                .Sum((fill) => { return fill.Price.Currency == currency ? fill.Price.Value : RateExchange.GetExchangeRateFor(fill.Price.Currency, currency) * fill.Price.Value; });
        }

        /// <summary>
        /// Total refueled
        /// </summary>
        /// <returns></returns>
        public double TotalRefueled()
        {
            return this.Records.Fills
                .Sum((fill) => { return fill.Refueled; });
        }

        #endregion

        #region GRAPHS

        /// <summary>
        /// Trend fuel prices
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public GraphData_DateTime TrendFuelPrices(Currency currency, Unit unit)
        {
            //graph data
            var rate = UnitExchange.GetExchangeUnitFor(unit, Unit.Liters);
            var data = new GraphData_DateTime();
            data.LegendX = "Date";
            data.LegendY = currency.ToString();

            foreach (var fill in this.Records.Fills)
            {
                var price = fill.UnitPrice.Currency == currency ? fill.UnitPrice.Value : RateExchange.GetExchangeRateFor(fill.UnitPrice.Currency, currency) * fill.UnitPrice.Value;
                data.X.Add(fill.Date);
                data.Y.Add(Math.Round(fill.UnitPrice.Value * rate, 2));
            }

            return data;
        }

        /// <summary>
        /// Trend units per refill
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public GraphData_DateTime TrendUnitsPerRefill(Currency currency, Unit unit)
        {
            //graph data
            var rate = UnitExchange.GetExchangeUnitFor(unit, Unit.Liters);
            var data = new GraphData_DateTime();
            data.LegendX = "Date";
            data.LegendY = currency.ToString();

            foreach (var fill in this.Records.Fills)
            {
                data.X.Add(fill.Date);
                data.Y.Add(Math.Round(fill.Refueled / rate, 2));
            }

            return data;
        }

        #endregion

    }
}
