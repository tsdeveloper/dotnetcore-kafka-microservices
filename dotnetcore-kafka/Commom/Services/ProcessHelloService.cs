using System;
using System.Threading;
using System.Threading.Tasks;
using Commom.Helper;
using Commom.Model;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Commom.Services
{
    public class ProcessHelloService : BackgroundService
    {
        private readonly ConsumerConfig consumerConfig;
        private readonly ProducerConfig producerConfig;
        public ProcessHelloService(ConsumerConfig consumerConfig, ProducerConfig producerConfig)
        {
            this.producerConfig = producerConfig;
            this.consumerConfig = consumerConfig;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("HelloWorldProcessing Service Started");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumerHelper = new ConsumerWrapper(consumerConfig, "hello-requests");
                string helloWorldRequest = consumerHelper.readMessage();
                
                var helloWorld = new HelloWorldRequest();;
                //Deserilaize 
                var result = JsonConvert.DeserializeObject<HelloWorldRequest>(helloWorldRequest);

                if (result != null)
                {
                    helloWorld = result;
                    //TODO:: Process helloWorld
                    Console.WriteLine($"Info: HelloWorldHandler => Processing the helloWorld for {helloWorld.Message}");
                    
                }
             
                
                //Write to ReadyToShip Queue

                var producerWrapper = new ProducerWrapper(producerConfig,"readytomessage");
                await producerWrapper.writeMessage(JsonConvert.SerializeObject(helloWorld));
            }
        }
        
    }
}