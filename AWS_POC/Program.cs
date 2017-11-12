using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Amazon.S3;
using Amazon.S3.Model;

namespace AWS_POC
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Write(GetServiceOutput());
            //var newlyLaunchedInstanceIds = LaunchEC2InstanceInClassic();
            //StopEC2Instance(newlyLaunchedInstanceIds);
            //StopEC2Instance();
            //StartEC2Instance();
            Console.Write(GetServiceOutput());
            Console.Read();
        }

        public static string GetServiceOutput()
        {
            StringBuilder sb = new StringBuilder(1024);
            using (StringWriter sr = new StringWriter(sb))
            {
                sr.WriteLine("===========================================");
                sr.WriteLine("Welcome to the AWS .NET SDK!");
                sr.WriteLine("===========================================");

                // Print the number of Amazon EC2 instances.
                IAmazonEC2 ec2 = new AmazonEC2Client();
                DescribeInstancesRequest ec2Request = new DescribeInstancesRequest();

                try
                {
                    DescribeInstancesResponse ec2Response = ec2.DescribeInstances(ec2Request);
                    int numInstances = 0;
                    numInstances = ec2Response.Reservations.Count;
                    sr.WriteLine(string.Format("You have {0} Amazon EC2 instance(s) running in the {1} region.",
                                               numInstances, ConfigurationManager.AppSettings["AWSRegion"]));
                }
                catch (AmazonEC2Exception ex)
                {
                    if (ex.ErrorCode != null && ex.ErrorCode.Equals("AuthFailure"))
                    {
                        sr.WriteLine("The account you are using is not signed up for Amazon EC2.");
                        sr.WriteLine("You can sign up for Amazon EC2 at http://aws.amazon.com/ec2");
                    }
                    else
                    {
                        sr.WriteLine("Caught Exception: " + ex.Message);
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("Error Type: " + ex.ErrorType);
                        sr.WriteLine("Request ID: " + ex.RequestId);
                    }
                }
                sr.WriteLine();

                // Print the number of Amazon SimpleDB domains.
                IAmazonSimpleDB sdb = new AmazonSimpleDBClient();
                ListDomainsRequest sdbRequest = new ListDomainsRequest();

                try
                {
                    ListDomainsResponse sdbResponse = sdb.ListDomains(sdbRequest);

                    int numDomains = 0;
                    numDomains = sdbResponse.DomainNames.Count;
                    sr.WriteLine(string.Format("You have {0} Amazon SimpleDB domain(s) in the {1} region.",
                                               numDomains, ConfigurationManager.AppSettings["AWSRegion"]));
                }
                catch (AmazonSimpleDBException ex)
                {
                    if (ex.ErrorCode != null && ex.ErrorCode.Equals("AuthFailure"))
                    {
                        sr.WriteLine("The account you are using is not signed up for Amazon SimpleDB.");
                        sr.WriteLine("You can sign up for Amazon SimpleDB at http://aws.amazon.com/simpledb");
                    }
                    else
                    {
                        sr.WriteLine("Caught Exception: " + ex.Message);
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("Error Type: " + ex.ErrorType);
                        sr.WriteLine("Request ID: " + ex.RequestId);
                    }
                }
                sr.WriteLine();

                // Print the number of Amazon S3 Buckets.
                IAmazonS3 s3Client = new AmazonS3Client();

                try
                {
                    ListBucketsResponse response = s3Client.ListBuckets();
                    int numBuckets = 0;
                    if (response.Buckets != null &&
                        response.Buckets.Count > 0)
                    {
                        numBuckets = response.Buckets.Count;
                    }
                    sr.WriteLine("You have " + numBuckets + " Amazon S3 bucket(s).");
                }
                catch (AmazonS3Exception ex)
                {
                    if (ex.ErrorCode != null && (ex.ErrorCode.Equals("InvalidAccessKeyId") ||
                        ex.ErrorCode.Equals("InvalidSecurity")))
                    {
                        sr.WriteLine("Please check the provided AWS Credentials.");
                        sr.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                    }
                    else
                    {
                        sr.WriteLine("Caught Exception: " + ex.Message);
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("Request ID: " + ex.RequestId);
                    }
                }
                sr.WriteLine("Press any key to continue...");
            }

            
            return sb.ToString();
        }

        public static List<string> LaunchEC2InstanceInClassic()
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
            return instanceIds;
        }

        public static void StopEC2Instance(List<string> instanceIds)
        {
            var ec2Client = new AmazonEC2Client();
            ec2Client.StopInstances(new StopInstancesRequest(instanceIds));

        }

        public static void StopEC2Instance()
        {
            var ec2Client = new AmazonEC2Client();
            var instanceIds = new List<String>() { "i-0b96f247b4837b1c5"};
            ec2Client.StopInstances(new StopInstancesRequest(instanceIds));

        }

        public static void StartEC2Instance()
        {
            var ec2Client = new AmazonEC2Client();
            var instanceIds = new List<String>() { "i-0903c81d523c013d6" };
            ec2Client.StartInstances(new StartInstancesRequest(instanceIds));

        }

        public static void TerminateEc2Instance()
        {
            var deleteRequest = new TerminateInstancesRequest()
            {
                InstanceIds = new List<string>() { "i-0903c81d523c013d6" }
            };
            var ec2Client = new AmazonEC2Client();
            var deleteResponse = ec2Client.TerminateInstances(deleteRequest);
            foreach (InstanceStateChange item in deleteResponse.TerminatingInstances)
            {
                Console.WriteLine();
                Console.WriteLine("Terminated instance: " + item.InstanceId);
                Console.WriteLine("Instance state: " + item.CurrentState.Name);
            }
        }
    }
}