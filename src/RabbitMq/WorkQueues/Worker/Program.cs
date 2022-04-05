using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "task_queue",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += async (sender, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(" [x] Received {0}", message);

    var dots = message.Split('.').Length - 1;
    await Task.Delay(dots * 1000);

    Console.WriteLine(" [x] Done");

    if (sender is EventingBasicConsumer basicConsumer)
    {
        basicConsumer.Model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

    }
};
channel.BasicConsume(queue: "task_queue", autoAck: false, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
Console.WriteLine("Bye");
