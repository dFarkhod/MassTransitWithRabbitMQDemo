namespace MassTransitDemo.Common
{
    public record MessageDto
    {
        public Guid Id { get; set; }
        public string Sender { get; set; }
        public string Content { get; set; }
    }

}