﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


var connectionFactory = new ConnectionFactory();
connectionFactory.HostName = "localhost";
string exchangeName = "headers-exchange";
using var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare(exchangeName, durable: true, type: ExchangeType.Headers);

var queueName = channel.QueueDeclare().QueueName;
Dictionary<string, object> headers = new Dictionary<string, object>();

headers.Add("format", "pdf");
headers.Add("shape", "a41");
headers.Add("x-match", "any");//all olursa header daki bilgilerin eşleşmesi gerekiyor any olursa herhangi biri eşleşirse kabul ediyor.
channel.QueueBind(queue: queueName,exchangeName,String.Empty,headers);

channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(queueName, false, consumer: consumer);
Console.WriteLine("loglar dinleniyor...");
consumer.Received += (render, argument) =>
{
    string message = Encoding.UTF8.GetString(argument.Body.ToArray());
    Console.WriteLine(message);
    channel.BasicAck(deliveryTag: argument.DeliveryTag, false);

};
Console.ReadLine();