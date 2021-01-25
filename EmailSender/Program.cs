using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmailSender
{
    class Program
    {
        static async Task Main(string[] args)
        {
           HubConnection hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:58572/messagehub/").Build();
            await hubConnection.StartAsync();

            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri("	amqps://vecwiffm:FAuYNbEWzFA64wSw7Kc7_nP7bDiPw1jZ@orangutan.rmq.cloudamqp.com/vecwiffm");
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.QueueDeclare("messagequeue", true, false, false);

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume("messagequeue", true, consumer);

            consumer.Received += async (s, e) =>
            {
                string alreadySeriliazedData = Encoding.UTF8.GetString(e.Body.Span);
                UserInfo user = JsonSerializer.Deserialize<UserInfo>(alreadySeriliazedData);

                EmailSenderClass.Send(user.Email, user.Message);

                Console.WriteLine($"{user.Email} sent");
                await hubConnection.InvokeAsync("SendMessageAsync", $"{user.Email} sent mail");

            };
            Console.Read();
        }
    }
}
