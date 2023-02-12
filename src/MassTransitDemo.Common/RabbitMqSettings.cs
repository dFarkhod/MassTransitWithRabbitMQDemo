namespace MassTransitDemo.Common
{
    public class RabbitMqSettings
    {
        public string Host { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseSSL { get; set; }
        public string SSLThumbprint { get; set; }
    }
}
