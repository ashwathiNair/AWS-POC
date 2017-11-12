using System.Runtime.Serialization;

namespace AWSMgmtService
{
    [DataContract]
    public class ServiceDetails
    {
        [DataMember]
        public string InstanceType { get; set; }

        [DataMember]
        public string InstanceName { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Action { get; set; }
    }
}