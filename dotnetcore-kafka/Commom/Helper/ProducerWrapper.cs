using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Commom.Helper
{
    public class ProducerWrapper
    {
        private string _topicName;
        private ProducerConfig _config;
        private static readonly Random rand = new Random();

        public ProducerWrapper(ProducerConfig config, string topicName)
        {
            _topicName = topicName;
            _config = config;
          
        }
        public async Task writeMessage(string message){
            
            using (var producer = new ProducerBuilder<string, string>(_config).Build())
            {
              
                var dr = await producer.ProduceAsync(
                    _topicName,
                    new Message<string, string>
                        { Key = rand.Next(int.MaxValue).ToString(), Value = message });
                    
                Console.WriteLine($"KAFKA => identify {dr.Key} timestamp {dr.Timestamp.UtcDateTime} Delivered '{dr.Value}'  to '{dr.TopicPartitionOffset}'");
                
            }
          
            return;
        }
    }
}