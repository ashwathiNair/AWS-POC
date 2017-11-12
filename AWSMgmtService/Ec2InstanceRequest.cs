using System.Collections.Generic;

namespace AWSMgmtService
{
    public class Ec2InstanceRequest
    {
        public string AmiId { get; set; }

        public string KeyPairName { get; set; }

        public List<string> Groups { get; set; }

        public string InstanceType { get; set; }
    }
}