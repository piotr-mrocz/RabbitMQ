using Consumer1;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.Title = "Consumer 1 - Email";

var factory = new ConnectionFactory() { HostName = "localhost" };

using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.ExchangeDeclare(
           exchange: "Routing",
           type: ExchangeType.Direct);

        var queueName = channel.QueueDeclare().QueueName;

        channel.QueueBind(
            queue: queueName,
            exchange: "Routing",
            routingKey: RoutingKeyEnum.Email.ToString());

        channel.QueueBind(
            queue: queueName,
            exchange: "Routing",
            routingKey: RoutingKeyEnum.Wszystkie.ToString());

        Console.WriteLine("Czekam na wiadomości...");

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = ea.RoutingKey;

            Console.WriteLine($"Typ: {routingKey}");
            Console.WriteLine($"Otrzymana wiadomość: {message}");
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        Console.WriteLine("Wciśnij jakikolwiek klawisz aby zakończyć program");
        Console.ReadKey();
    }
}