using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AWSMgmtService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        public List<ServiceDetails> GetServiceDetails()
        {
            var response = GetServiceOutput();
            return response;
        }

        public bool TerminateInstance(string instanceId)
        {
            var deleteRequest = new TerminateInstancesRequest()
            {
                InstanceIds = new List<string>() {instanceId}
            };
            var ec2Client = new AmazonEC2Client();
            var deleteResponse = ec2Client.TerminateInstances(deleteRequest);
            return deleteResponse != null && deleteResponse.TerminatingInstances.Count > 0;
        }

        public bool StartInstance(string instanceId)
        {
            var ec2Client = new AmazonEC2Client();
            var instanceIds = new List<String>() { instanceId };
            var resp = ec2Client.StartInstances(new StartInstancesRequest(instanceIds));
            return resp != null;
        }

        public bool StopInstance(string instanceId)
        {
            var ec2Client = new AmazonEC2Client();
            var instanceIds = new List<String>() { instanceId };
            var resp = ec2Client.StopInstances(new StopInstancesRequest(instanceIds));
            return resp != null;
        }

        public List<string> LaunchEc2Instance(Ec2InstanceRequest request)
        {
            string amiID = request.AmiId;
            string keyPairName = request.KeyPairName;

            List<string> groups = request.Groups;
            var launchRequest = new RunInstancesRequest()
            {
                ImageId = amiID,
                InstanceType = request.InstanceType,
                MinCount = 1,
                MaxCount = 1,
                KeyName = keyPairName,
                SecurityGroupIds = groups
            };
            var ec2Client = new AmazonEC2Client();
            var launchResponse = ec2Client.RunInstances(launchRequest);
            var instances = launchResponse.Reservation.Instances;
            return instances.Select(instance => instance.InstanceId).ToList();
        }

        private List<ServiceDetails> GetServiceOutput()
        {
            List<ServiceDetails> serviceDetailslist = new List<ServiceDetails>();
            IAmazonEC2 ec2 = new AmazonEC2Client();
            DescribeInstancesRequest ec2Request = new DescribeInstancesRequest();

            try
            {
                DescribeInstancesResponse ec2Response = ec2.DescribeInstances(ec2Request);
                foreach (var reservation in ec2Response.Reservations)
                {
                    foreach (var instance in reservation.Instances)
                    {
                        ServiceDetails serviceDetails = new ServiceDetails
                        {
                            InstanceName = instance.KeyName,
                            InstanceType = instance.InstanceType.Value,
                            Status = instance.State.Name.Value
                        };
                        serviceDetailslist.Add(serviceDetails);
                    }
                }
            }
            catch (AmazonEC2Exception ex)
            {
                throw ex;
            }
            return serviceDetailslist;
        }
    }
}