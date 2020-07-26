using System;
using System.Threading;
using System.Threading.Tasks;
using Commom.Helper;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            var consumerConfig = new ConsumerConfig
            {
                GroupId = "csharp-consumer",
                BootstrapServers = "localhost:9092",
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000,

            };


            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ProducerConfig>(producerConfig)
                .AddSingleton<ConsumerConfig>(consumerConfig)
                .BuildServiceProvider();
            
            int index = 1;
            
            while (true)
            {
                Thread.Sleep(5000);
                var helloWorldRequest = new
                {
                    id = index++,
                    message = "hello"
                };
                //Serialize 
                string serializedHelloWorld = JsonConvert.SerializeObject(helloWorldRequest);

                Console.WriteLine("========");
                Console.WriteLine("Info: HelloWorldConsole => Post => Recieved a new purchase helloWorld:");
                Console.WriteLine(serializedHelloWorld);
                Console.WriteLine("=========");

                var producer = new ProducerWrapper(producerConfig, "hello-requests");
                await producer.writeMessage(serializedHelloWorld);    
            }
            

        }
    }
}