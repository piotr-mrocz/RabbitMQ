using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.Title = "Receive";

var factory = new ConnectionFactory() { HostName = "localhost" };

using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "FirstRabbitMQQueue",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    Console.WriteLine("Waiting for messages.");

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine("Received message: {0}", message);
    };
    channel.BasicConsume(queue: "FirstRabbitMQQueue",
                         autoAck: true,
                         consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}