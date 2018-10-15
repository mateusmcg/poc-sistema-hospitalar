using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Amazon.Runtime;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace receptor_dados_paciente
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"./configs/appsettings.json", true, true)
                .AddEnvironmentVariables();
                
            Configuration = builder.Build();

            string IotEndPoint = Configuration["AWS:Iot:EndPoint"];

            var CaCert = X509Certificate.CreateFromCertFile(Configuration["AWS:Iot:CaCertPath"]);
            var clientCert = new X509Certificate2(Configuration["AWS:Iot:ClientCertPath"], Configuration["AWS:Iot:ClientCertPass"]);

            string clientID = Guid.NewGuid().ToString();

            var IotClient = new MqttClient(IotEndPoint, int.Parse(Configuration["AWS:Iot:BrokerPort"]), true, CaCert, clientCert, MqttSslProtocols.TLSv1_2);
            IotClient.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            IotClient.MqttMsgSubscribed += Client_MqttMsgSubscribed;

            IotClient.Connect(clientID);
            Console.WriteLine("Connected");
            IotClient.Subscribe(new string[] { Configuration["AWS:Iot:Topic"] }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

            while (true)
            {
                //keeping the main thread alive for the event call backs
            }
        }

        private static void Client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Console.WriteLine("Message subscribed");
        }

        private async static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            try {
                var mensagem = System.Text.Encoding.UTF8.GetString(e.Message);
                Console.WriteLine("Message Received is: " + mensagem);

                var credenciais = new BasicAWSCredentials(Configuration["AWS:DynamoDb:accessKey"], Configuration["AWS:DynamoDb:secretKey"]);
                var client = new AmazonDynamoDBClient(credenciais, RegionEndpoint.SAEast1);
                var context = new DynamoDBContext(client);
                
                var paciente = JsonConvert.DeserializeObject<Paciente>(mensagem);

                // Save that object into our DynamoDB
                await context.SaveAsync<Paciente>(paciente);
            }
            catch(Exception ex) {
                Console.WriteLine(string.Format("[ERROR]: {0} \n\n {1}", ex.Message, ex.StackTrace));
            }
        }
    }
}
