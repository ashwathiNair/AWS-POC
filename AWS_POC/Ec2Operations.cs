using Amazon.EC2;
using Amazon.EC2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS_POC
{
    public class AWS_POC
    {
        public void LaunchEC2InstanceInClassic()
        {
            string amiID = "ami-9fa343e7";
            string keyPairName = "spatil7891-PC-USWest";

            List<string> groups = new List<string>() { "sg-f133078b" };
            var launchRequest = new RunInstancesRequest()
            {
                ImageId = amiID,
                InstanceType = "t2.micro",
                MinCount = 1,
                MaxCount = 1,
                KeyName = keyPairName,
                SecurityGroupIds = groups
            };


            var ec2Client = new AmazonEC2Client();
            var launchResponse = ec2Client.RunInstances(launchRequest);
            var instances = launchResponse.Reservation.Instances;
            var instanceIds = new List<string>();
            foreach (Instance item in instances)
            {
                instanceIds.Add(item.InstanceId);
                Console.WriteLine();
                Console.WriteLine("New instance: " + item.InstanceId);
                Console.WriteLine("Instance state: " + item.State.Name);
            }
        }

        public void LaunchEC2InstanceInVPC()
        {

        }

        public void StartEC2Instance()
        {



        }

        public void StopEC2Instance()
        {
            

        }
    }
}
