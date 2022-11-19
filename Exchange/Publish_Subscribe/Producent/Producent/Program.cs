using RabbitMQ.Client;
using System.Text;

Console.Title = "Producent";

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

        var isFinished = false;

        do
        {
            Console.Write("Wpisz wiadomość: ");
            var message = Console.ReadLine();

            if (string.IsNullOrEmpty(message))
                continue;

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "PublishSubcribe",
                routingKey: string.Empty,
                basicProperties: null,
                body: body);

            Console.Write($"Wysłana wiadomość: {message}");
            Console.WriteLine($"Wciśniej klawisz {ConsoleKey.Escape}, aby opuścić program");
            Console.WriteLine();

            var readKey = Console.ReadKey();

            if (readKey.Key == ConsoleKey.Escape)
                isFinished = true;

        } while (!isFinished);
    }
}