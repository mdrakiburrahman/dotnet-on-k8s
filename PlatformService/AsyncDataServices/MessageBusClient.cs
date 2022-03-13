using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            // Setup a Connection Factory
            var factory = new ConnectionFactory() { 
                HostName = _configuration["RabbitMQHost"], 
                Port = int.Parse(_configuration["RabbitMQPort"]) };
            try
            {
                // Try to setup connection to RabbitMQ
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                // Setup Exchange setup paradigm with RabbitMQ
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                // Subscribe to a shutdown event so we can gracefully shut down our app
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to RabbitMQ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }
        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            // Turn our Dto into a JSON string
            var message = JsonSerializer.Serialize(platformPublishedDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine($"--> RabbitMQ connection is open - sending message...");

                // Send the message
                SendMessage(message);
            }
            else
            {
                Console.WriteLine($"--> RabbitMQ connection is closed - not sending.");
            }
        }
        
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);   
            _channel.BasicPublish(exchange: "trigger",
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($"--> We have sent: {message}");
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"--> RabbitMQ connection shutdown: {e.ReplyText}");
        }
        public void Dispose()
        {
            Console.WriteLine("--> Disposing RabbitMQ connection");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}