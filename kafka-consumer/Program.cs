using Confluent.Kafka;

public class Program
{
    public static void Main()
    {
        var conf = new ConsumerConfig
        {
            ClientId = "kafka-demo-consumer",
            GroupId = "kafka-demo-consumer-group",
            BootstrapServers = "localhost:9093",
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };

        const string topicName = "topic-1p";

        using var consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
        consumer.Subscribe(topicName);

        CancellationTokenSource cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        try
        {
            while (true)
            {
                try
                {
                    var result = consumer.Consume(cts.Token);
                    Console.WriteLine($"Consumed message '{result.Message.Value}' at: '{result.TopicPartitionOffset}'.");

                    consumer.Commit();
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occured: {e.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
}