namespace ProductNotification.Domain.Entities
{
    public class Notification : Base
    {
        public int ClienteId { get; set; }
        public int ProductId { get; set; }
        public string Email { get; set; }

        public override bool Valido()
        {
            throw new System.NotImplementedException();
        }
    }
}
