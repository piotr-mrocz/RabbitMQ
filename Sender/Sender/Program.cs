using RabbitMQ.Client;
using System.Runtime.CompilerServices;
using System.Text;

Console.Title = "Sender";

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(
            queue: "FirstRabbitMQQueue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        while (true)
        {
            Console.Write("Just wirte some message: ");
            Console.WriteLine("Write nothing to exit.");

            var message = Console.ReadLine();

            if (string.IsNullOrEmpty(message))
                break;

            var bytes = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "FirstRabbitMQQueue",
                basicProperties: null,
                bytes);

            Console.WriteLine();
            Console.WriteLine("Sender sent message");
            Console.WriteLine();
        }
    }
}