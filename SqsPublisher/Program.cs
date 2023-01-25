using Amazon.SQS;
using SqsPublisher;
using Amazon.SQS.Model;
using System.Text.Json;

// Load Environment Variables from DotNetEnv Package

DotNetEnv.Env.TraversePath().Load();

var _accesskey = System.Environment.GetEnvironmentVariable("ACCESS_KEY");
var _secretkey = System.Environment.GetEnvironmentVariable("SECRET_KEY");

// Initialise new AWS SQS Client

var sqsClient = new AmazonSQSClient(_accesskey, _secretkey, Amazon.RegionEndpoint.EUWest1);

var customer = new CustomerCreated
{
	Id = Guid.NewGuid(),
	Email = "77markjohnston77@gmail.com",
	FullName = "Mark Johnston",
	DateOfBirth = new DateTime(1981, 3, 3),
	GithubUsername = "markj0hnst0n"
};

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("serverless-project-customers");

var sendMessageRequest = new SendMessageRequest
{
	QueueUrl = queueUrlResponse.QueueUrl,
	MessageBody = JsonSerializer.Serialize(customer)
};

var response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine(response);