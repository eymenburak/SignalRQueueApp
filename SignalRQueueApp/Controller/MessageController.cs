using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using SignalRQueueApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SignalRQueueApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromForm] UserInfo info)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri("	amqps://vecwiffm:FAuYNbEWzFA64wSw7Kc7_nP7bDiPw1jZ@orangutan.rmq.cloudamqp.com/vecwiffm");
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.QueueDeclare("messagequeue", true, false, false);

            string userSeriliazedData =  JsonSerializer.Serialize(info);

            byte[] data = Encoding.UTF8.GetBytes(userSeriliazedData);
            channel.BasicPublish("", "messagequeue", body:data);
            return Ok();
        }
    }
}
