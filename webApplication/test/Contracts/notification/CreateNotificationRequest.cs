namespace webApplication.Contracts.notification
{
    public class CreateNotificationRequest
    {
        public int userid { get; set; }

        public string note { get; set; } = null!;
    }
}