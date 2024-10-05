using System.Runtime.Serialization;

namespace AgileActors.AggregationApp.Types.DTOs
{
    [DataContract]
    public class AggregatedData
    {
        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; } = "";

        [DataMember(Name = "Data")]
        public List<DataModel> Data { get; set; } = new List<DataModel>();

        [DataMember(Name = "Errors")]
        public List<Exception> Errors { get; set; } = new List<Exception>();
    }

    [DataContract]
    public class DataModel
    {
        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; } = "";

        [DataMember(Name = "Data")]
        public string Data { get; set; } = "";
    }

}