using System.Runtime.Serialization;

namespace AgileActors.AggregationApp.Types.DTOs
{
    [DataContract]
    public class AggregatedData
    {
        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; } = "";

        [DataMember(Name = "Data")]
        public object? Data { get; set; }
    }

}