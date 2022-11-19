using Producent;
using RabbitMQ.Client;
using System.Text;

Console.Title = "Producent";

var factory = new ConnectionFactory() { HostName = "localhost" };

using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.ExchangeDeclare(
            exchange: "Routing",
            type: ExchangeType.Direct);

        var isFinished = false;

        do
        {
            Console.Write("Wpisz wiadomość: ");
            var message = Console.ReadLine();

            if (string.IsNullOrEmpty(message))
                continue;

            Console.WriteLine();
            Console.WriteLine("Wybierz typ: ");
            Console.WriteLine("1 - Email");
            Console.WriteLine("2 - SMS");
            Console.WriteLine("3 - Wszystkie");

            var userRoutingKey = int.Parse(Console.ReadKey().KeyChar.ToString());

            if (userRoutingKey == 1 || userRoutingKey == 2 || userRoutingKey == 3)
            {
                var key = EnumHelper
                    .GetEnumDescription((RoutingKeyEnum)userRoutingKey);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: "Routing",
                    routingKey: key,
                    basicProperties: null,
                    body: body);

                Console.WriteLine($"Wysłana wiadomość: {message}");
                Console.WriteLine($"Typ: {key}");
            }

            Console.WriteLine();
            Console.WriteLine($"Wciśniej klawisz {ConsoleKey.Escape}, aby opuścić program");
            Console.WriteLine();

            var readKey = Console.ReadKey();

            if (readKey.Key == ConsoleKey.Escape)
                isFinished = true;

        } while (!isFinished);
    }
}