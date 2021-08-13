using Newtonsoft.Json;

namespace CarDiaryX.Domain.Integration
{
    public class RootInformation
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        public string RawData { get; set; }
    }

    public class Data
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("ts_id")]
        public long TsId { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("model_year")]
        public int? ModelYear { get; set; }

        [JsonProperty("fuel_type")]
        public string FuelType { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("car_type")]
        public string CarType { get; set; }
    }
}
