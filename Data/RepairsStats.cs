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
    public class RepairsStats
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
        public RepairsStats(Records records)
        {
            this.Records = records;
        }

        #region DATA

        /// <summary>
        /// Paid for repairs
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public Double PaidForRepairs(Currency currency)
        {
            return this.Records.Repairs
                .Sum((repair) => { return repair.Price.Currency == currency ? repair.Price.Value : RateExchange.GetExchangeRateFor(repair.Price.Currency, currency) * repair.Price.Value; });
        }

        #endregion
    }
}
