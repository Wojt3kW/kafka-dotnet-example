﻿using Confluent.Kafka;

public class Program
{
    public static async Task Main(string[] args)
    {
        var config = new ProducerConfig { BootstrapServers = "localhost:9093" };
        const string topicName = "topic-1p";

        using (var producer = new ProducerBuilder<string, string>(config).Build())
        {
            Console.WriteLine("\n-----------------------------------------------------------------------");
            Console.WriteLine($"Producer {producer.Name} producing on topic {topicName}.");
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("To create a kafka message with UTF-8 encoded key and value:");
            Console.WriteLine("> key value<Enter>");
            Console.WriteLine("To create a kafka message with a null key and UTF-8 encoded value:");
            Console.WriteLine("> value<enter>");
            Console.WriteLine("Ctrl-C to quit.\n");

            var cancelled = false;
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cancelled = true;
            };

            while (!cancelled)
            {
                Console.Write("> ");

                string? text;
                try
                {
                    text = Console.ReadLine();
                }
                catch (IOException)
                {
                    // IO exception is thrown when ConsoleCancelEventArgs.Cancel == true.
                    break;
                }

                if (text is null)
                {
                    // Console returned null before 
                    // the CancelKeyPress was treated
                    break;
                }

                string? key = null;
                string? val = text;

                // split line if both key and value specified.
                int index = text.IndexOf(" ");
                if (index != -1)
                {
                    key = text.Substring(0, index);
                    val = text.Substring(index + 1);
                }

                try
                {
                    // Note: Awaiting the asynchronous produce request below prevents flow of execution
                    // from proceeding until the acknowledgment from the broker is received (at the 
                    // expense of low throughput).
                    var deliveryReport = await producer.ProduceAsync(
                        topicName, new Message<string, string> { Key = key, Value = val });

                    Console.WriteLine($"delivered to: {deliveryReport.TopicPartitionOffset}");
                }
                catch (ProduceException<string, string> e)
                {
                    Console.WriteLine($"failed to deliver message: {e.Message} [{e.Error.Code}]");
                }
            }

            // Since we are producing synchronously, at this point there will be no messages
            // in-flight and no delivery reports waiting to be acknowledged, so there is no
            // need to call producer.Flush before disposing the producer.
        }
    }
}