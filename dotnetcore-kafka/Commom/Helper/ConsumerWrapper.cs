using System;
using Confluent.Kafka;

namespace Commom.Helper
{
    public class ConsumerWrapper
    {
        private string _topicName;
        private ConsumerConfig _consumerConfig;
        
        private static readonly Random rand = new Random();
        public ConsumerWrapper(ConsumerConfig config,string topicName)
        {
            _topicName = topicName;
            _consumerConfig = config;
            _consumer = new ConsumerBuilder<string,string>(_consumerConfig).Build();
            _consumer.Subscribe(topicName);
        }

        public IConsumer<string, string> _consumer { get; set; }

        public string readMessage(){
            var consumeResult = _consumer.Consume();
            
            if (consumeResult.Message != null)
                return consumeResult.Message.Value;

            return string.Empty;
        }
    }
}