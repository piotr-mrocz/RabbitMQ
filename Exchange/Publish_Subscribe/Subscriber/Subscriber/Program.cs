using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// Consumer tworzy kolejkę w chwili powstania. Dlatego wiadomości wysłane przez Producenta zanim
// zostanie stworzona kolejka przepadają.

Console.Title = "Consumer 1";

var factory = new ConnectionFactory() { HostName = "localhost" };

using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.ExchangeDeclare(
            exchange: "PublishSubcribe",
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            arguments: null);

        var queueName = channel.QueueDeclare().QueueName;

        channel.QueueBind(
            queue: queueName,
            exchange: "PublishSubcribe",
            routingKey: string.Empty);

        Console.WriteLine("Czekam na wiadomości...");

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"Otrzymana wiadomość: {message}");
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        Console.WriteLine("Wciśnij jakikolwiek klawisz aby zakończyć program");
        Console.ReadKey();
    }
}