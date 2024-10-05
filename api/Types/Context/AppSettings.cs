using System.Runtime.Serialization;

namespace AgileActors.AggregationApp.Types.Context
{
    [DataContract]
    public class AppSettings
    {
        [DataMember(Name = "AllowedHosts")]
        public string AllowedHosts { get; set; } = "";

        [DataMember(Name = "ConnectionStrings")]
        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();

        [DataMember(Name = "ExternalApiSettings")]
        public ExternalApiSettings ExternalApiSettings { get; set; } = new ExternalApiSettings();
    }

    [DataContract]
    public class ConnectionStrings
    {
        [DataMember(Name = "MySqlConnection")]
        public string IdentityConnection { get; set; } = "";
    }

    [DataContract]
    public class ExternalApiSettings
    {
        [DataMember(Name = "ExternalApiSettingsList")]
        public List<ExternalApis> ExternalApiSettingsList { get; set; } = new List<ExternalApis>();
    }

    [DataContract]
    public class ExternalApis
    {
        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; } = "";

        [DataMember(Name = "BaseUrl")]
        public string BaseUrl { get; set; } = "";

        [DataMember(Name = "Headers")]
        public ExternalApisHeaders Headers { get; set; } = new ExternalApisHeaders();
    }

    [DataContract]
    public class ExternalApisHeaders
    {
        [DataMember(Name = "Authorization")]
        public string Authorization { get; set; } = "";

        [DataMember(Name = "ApiKey")]
        public string ApiKey { get; set; } = "";

        [DataMember(Name = "Accept")]
        public string Accept { get; set; } = "";
    }

}