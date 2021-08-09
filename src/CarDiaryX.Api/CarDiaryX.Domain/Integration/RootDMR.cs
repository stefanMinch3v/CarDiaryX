using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarDiaryX.Domain.Integration
{
    public class RootDMR
    {
        [JsonProperty("next_inspection_date")]
        public string NextInspectionDate { get; set; } // string (dd-mm-yyyy)

        [JsonProperty("taxes")]
        public Taxes Taxes { get; set; }

        public string RawData { get; set; }

        public DateTime? GetNextGreenTaxDate
        {
            get
            {
                if (this.Taxes is null)
                {
                    return null;
                }

                return this.Taxes.History
                    .Where(h => DateTime.TryParse(h?.To, out _))
                    .Select(h => DateTime.Parse(h.To))
                    .OrderByDescending(h => h)
                    .FirstOrDefault();
            }
        }

        public DateTime? GetNextInspectionDate
        {
            get
            {
                var success = DateTime.TryParse(this.NextInspectionDate, out var result);

                if (!success)
                {
                    return null;
                }

                return result;
            }
        }
    }

    public class Taxes
    {
        [JsonProperty("history")]
        public List<History> History { get; set; }
    }

    public class History
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
    }
}
