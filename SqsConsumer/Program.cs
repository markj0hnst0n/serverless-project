using Amazon.SQS;
using SqsPublisher;
using Amazon.SQS.Model;
using System.Text.Json;
using System.Threading.Tasks;

// Load Environment Variables from DotNetEnv Package

DotNetEnv.Env.TraversePath().Load();

var _accesskey = System.Environment.GetEnvironmentVariable("ACCESS_KEY");
var _secretkey = System.Environment.GetEnvironmentVariable("SECRET_KEY");

// Initialise new AWS SQS Client

var cts = new CancellationTokenSource();
var sqsClient = new AmazonSQSClient(_accesskey, _secretkey, Amazon.RegionEndpoint.EUWest1);

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("serverless-project-customers");

var receiveMessageRequest = new ReceiveMessageRequest
{
	QueueUrl = queueUrlResponse.QueueUrl,
	AttributeNames = new List<string> { "All" },
	MessageAttributeNames = new List<string> { "All" }
};

while (!cts.IsCancellationRequested)
{
	var response = await sqsClient.ReceiveMessageAsync(receiveMessageRequest, cts.Token);

	foreach (var message in response.Messages)
	{
		Console.WriteLine($"Message Id: {message.MessageId}");
		Console.WriteLine($"Message Body: {message.Body}");

		await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
	}

	await Task.Delay(3000);
}